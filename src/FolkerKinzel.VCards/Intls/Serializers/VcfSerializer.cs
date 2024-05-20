using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal abstract class VcfSerializer : IDisposable
{
    private const int BUILDER_INITIAL_CAPACITY = 128;

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_SpouseName = "X-KADDRESSBOOK-X-SpouseName";

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_Anniversary = "X-KADDRESSBOOK-X-Anniversary";

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_IMAddress = "X-KADDRESSBOOK-X-IMAddress";

    private readonly char[] _byteCounterArr = new char[1];
    protected readonly TextWriter _writer;

    protected VcfSerializer(TextWriter writer,
                            Opts options,
                            ParameterSerializer parameterSerializer,
                            ITimeZoneIDConverter? tzConverter)
    {
        this.Options = options;

        // Store this for performance:
        this.IgnoreEmptyItems = !options.IsSet(Opts.WriteEmptyProperties);

        this.ParameterSerializer = parameterSerializer;
        this._writer = writer;
        this.TimeZoneConverter = tzConverter;
        writer.NewLine = VCard.NewLine;
    }

    internal ParameterSerializer ParameterSerializer { get; }

    internal StringBuilder Builder { get; } = new(BUILDER_INITIAL_CAPACITY);

    internal abstract VCdVersion Version { get; }

    internal Opts Options { get; }

    internal bool IgnoreEmptyItems { get; }

    internal ITimeZoneIDConverter? TimeZoneConverter { get; }

    protected abstract string VersionString { get; }

    [NotNull]
    protected VCard? VCardToSerialize { get; private set; }

    [NotNull]
    internal string? PropertyKey { get; private set; }

    internal bool IsPref { get; private set; }

    public void Dispose() => _writer.Dispose();

    protected void BuildPrefProperty<T>(string propertyKey,
                                       IEnumerable<T?> serializables,
                                       Func<T, bool>? filter = null) where T : VCardProperty
    {
        Debug.Assert(serializables is not null);

        VCardProperty? pref = filter is null ? serializables.PrefOrNullIntl(IgnoreEmptyItems)
                                             : serializables.PrefOrNullIntl(filter, IgnoreEmptyItems);

        if (pref is not null)
        {
            BuildProperty(propertyKey, pref);
        }
    }

    protected void BuildFirstProperty<T>(string propertyKey,
                                         IEnumerable<T?> serializables,
                                         Func<T, bool>? filter = null) where T : VCardProperty
    {
        Debug.Assert(serializables is not null);

        VCardProperty? first = filter is null ? serializables.FirstOrNullIntl(IgnoreEmptyItems)
                                              : serializables.FirstOrNullIntl(filter, IgnoreEmptyItems);

        if (first is not null)
        {
            BuildProperty(propertyKey, first);
        }
    }

    protected virtual void BuildPropertyCollection(string propertyKey, IEnumerable<VCardProperty?> serializables)
    {
        Debug.Assert(serializables is not null);

        bool first = true;

        foreach (VCardProperty prop in serializables.OrderByPrefIntl(IgnoreEmptyItems))
        {
            BuildProperty(propertyKey, prop, first && prop.Parameters.Preference < 100);
            first = false;
        }
    }

    internal static VcfSerializer GetSerializer(Stream stream,
                                                bool leaveStreamOpen,
                                                VCdVersion version,
                                                Opts options,
                                                ITimeZoneIDConverter? tzConverter)
    {
        // UTF-8 must be written without BOM, otherwise it cannot be read
        // (vCard 2.1 can use UTF-8 because only ASCII characters are written)
        var encoding = new UTF8Encoding(false);

        StreamWriter writer = leaveStreamOpen
                              ? new StreamWriter(stream, encoding, 1024, true)
                              : new StreamWriter(stream, encoding);

        return version switch
        {
            VCdVersion.V2_1 => new Vcf_2_1Serializer(writer, options, tzConverter),
            VCdVersion.V3_0 => new Vcf_3_0Serializer(writer, options, tzConverter),
            VCdVersion.V4_0 => new Vcf_4_0Serializer(writer, options),
            _ => throw new ArgumentOutOfRangeException(nameof(version))
        };
    }

    internal void Serialize(VCard vCard)
    {
        Debug.Assert(vCard is not null);

        VCardToSerialize = vCard;
        ReplenishRequiredProperties();

        if (Options.HasFlag(Opts.SetPropertyIDs))
        {
            SetPropertyIDs();
        }

        if (Options.HasFlag(Opts.SetIndexes))
        {
            SetIndexes();
        }

        Builder.Clear();
        _writer.WriteLine("BEGIN:VCARD");
        _writer.Write(VCard.PropKeys.VERSION);
        _writer.Write(':');
        _writer.WriteLine(VersionString);

        AppendProperties();

        _writer.WriteLine("END:VCARD");
    }

    protected virtual void SetPropertyIDs() => VCardToSerialize.Sync.SetPropertyIDs();

    protected virtual void SetIndexes()
    {
        foreach (IEnumerable<VCardProperty?> coll in VCardToSerialize.Properties
                                                     .Where(static x => (x.Value is IEnumerable<VCardProperty?>) && (x.Key != Prop.AppIDs))
                                                     .Select(static x => (IEnumerable<VCardProperty?>)x.Value))
        {
            if (coll.IsSingle(IgnoreEmptyItems))
            {
                coll.UnsetIndexesIntl();
            }
            else
            {
                coll.SetIndexesIntl(IgnoreEmptyItems);
            }
        }
    }

    protected abstract void ReplenishRequiredProperties();

    private void AppendProperties()
    {
        foreach (KeyValuePair<Prop, object> kvp in
            VCardToSerialize.Properties.OrderBy(static x => x.Key))
        {
            switch (kvp.Key)
            {
                case Prop.Profile:
                    AppendProfile((ProfileProperty)kvp.Value);
                    break;
                case Prop.Kind:
                    AppendKind((KindProperty)kvp.Value);
                    break;
                case Prop.Mailer:
                    AppendMailer((TextProperty)kvp.Value);
                    break;
                case Prop.ProductID:
                    AppendProdID((TextProperty)kvp.Value);
                    break;
                case Prop.TimeStamp:
                    AppendLastRevision((TimeStampProperty)kvp.Value);
                    break;
                case Prop.ID:
                    AppendUniqueIdentifier((IDProperty)kvp.Value);
                    break;
                case Prop.Categories:
                    AppendCategories((IEnumerable<StringCollectionProperty?>)kvp.Value);
                    break;
                case Prop.TimeZones:
                    AppendTimeZones((IEnumerable<TimeZoneProperty?>)kvp.Value);
                    break;
                case Prop.GeoCoordinates:
                    AppendGeoCoordinates((IEnumerable<GeoProperty?>)kvp.Value);
                    break;
                case Prop.Access:
                    AppendAccess((AccessProperty)kvp.Value);
                    break;
                case Prop.Sources:
                    AppendSources((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.DirectoryName:
                    AppendDirectoryName((TextProperty)kvp.Value);
                    break;
                case Prop.DisplayNames:
                    AppendDisplayNames((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.NameViews:
                    AppendNameViews((IEnumerable<NameProperty?>)kvp.Value);
                    break;
                case Prop.GenderViews:
                    AppendGenderViews((IEnumerable<GenderProperty?>)kvp.Value);
                    break;
                case Prop.NickNames:
                    AppendNickNames((IEnumerable<StringCollectionProperty?>)kvp.Value);
                    break;
                case Prop.Titles:
                    AppendTitles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Roles:
                    AppendRoles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Organizations:
                    AppendOrganizations((IEnumerable<OrgProperty?>)kvp.Value);
                    break;
                case Prop.BirthDayViews:
                    AppendBirthDayViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case Prop.BirthPlaceViews:
                    AppendBirthPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.AnniversaryViews:
                    AppendAnniversaryViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case Prop.DeathDateViews:
                    AppendDeathDateViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case Prop.DeathPlaceViews:
                    AppendDeathPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Addresses:
                    AppendAddresses((IEnumerable<AddressProperty?>)kvp.Value);
                    break;
                case Prop.Phones:
                    AppendPhones((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.EMails:
                    AppendEMails((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Urls:
                    AppendURLs((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Messengers:
                    AppendInstantMessengerHandles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Keys:
                    AppendKeys((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case Prop.CalendarAddresses:
                    AppendCalendarAddresses((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.CalendarUserAddresses:
                    AppendCalendarUserAddresses((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.FreeOrBusyUrls:
                    AppendFreeBusyUrls((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Relations:
                    AppendRelations((IEnumerable<RelationProperty?>)kvp.Value);
                    break;
                case Prop.Members:
                    AppendMembers((IEnumerable<RelationProperty?>)kvp.Value);
                    break;
                case Prop.OrgDirectories:
                    AppendOrgDirectories((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Expertises:
                    AppendExpertises((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Interests:
                    AppendInterests((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Hobbies:
                    AppendHobbies((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Languages:
                    AppendLanguages((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Notes:
                    AppendNotes((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case Prop.Xmls:
                    AppendXmlProperties((IEnumerable<XmlProperty?>)kvp.Value);
                    break;
                case Prop.Logos:
                    AppendLogos((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case Prop.Photos:
                    AppendPhotos((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case Prop.Sounds:
                    AppendSounds((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case Prop.AppIDs:
                    AppendVCardClients((IEnumerable<AppIDProperty?>)kvp.Value);
                    break;
                case Prop.NonStandards:
                    AppendNonStandardProperties((IEnumerable<NonStandardProperty?>)kvp.Value);
                    break;
                default:
                    break;
            }//switch
        }//foreach
    }

    protected void BuildProperty(string propertyKey, VCardProperty prop, bool isPref = false)
    {
        if (prop.IsEmpty && IgnoreEmptyItems)
        {
            return;
        }

        PropertyKey = propertyKey;

        IsPref = isPref;

        if (prop.BuildProperty(this))
        {
            WriteLineFolded();
        }
        else
        {
#if NET8_0_OR_GREATER
            _writer.WriteLine(Builder);
#else
            using var shared = ArrayPoolHelper.Rent<char>(Builder.Length);
            Builder.CopyTo(0, shared.Array, 0, Builder.Length);
            _writer.WriteLine(shared.Array, 0, Builder.Length);
#endif
        }
    }

    protected void BuildXImpps(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value is not null);

        if (Options.HasFlag(Opts.WriteXExtensions))
        {
            bool first = true;

            foreach (TextProperty prop in value.OrderByPrefIntl(IgnoreEmptyItems))
            {
                bool isPref = first && prop.Parameters.Preference < 100;
                first = false;

                XMessengerParameterConverter.ConvertFromInstantMessengerType(prop.Parameters);

                string val = prop.Value!;

                if (val.StartsWith("aim:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_AIM, prop, isPref);
                }
                else if (val.StartsWith("gg:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GADUGADU, prop, isPref);
                }
                else if (val.StartsWith("gtalk:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GTALK, prop, isPref);
                }
                else if (val.StartsWith("com.google.hangouts:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK, prop, isPref);
                }
                else if (val.StartsWith("icq:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_ICQ, prop, isPref);
                }
                else if (val.StartsWith("xmpp:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_JABBER, prop, isPref);
                }
                else if (val.StartsWith("msnim:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MSN, prop, isPref);
                }
                else if (val.StartsWith("sip:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS, prop, isPref);
                }
                else if (val.StartsWith("skype:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_SKYPE, prop, isPref);
                }
                else if (val.StartsWith("twitter:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_TWITTER, prop, isPref);
                }
                else if (val.StartsWith("ymsgr:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_YAHOO, prop, isPref);
                }
            }
        }

        if (Options.HasFlag(Opts.WriteKAddressbookExtensions))
        {
            TextProperty? prop = value.PrefOrNullIntl(IgnoreEmptyItems);

            if (prop is not null)
            {
                BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_IMAddress, prop, prop.Parameters.Preference < 100);
            }
        }
    }

    protected virtual void WriteLineFolded()
    {
        Debug.Assert(Builder.Length > 0);

        using var shared = ArrayPoolHelper.Rent<char>(Builder.Length);
        Builder.CopyTo(0, shared.Array, 0, Builder.Length);
        ReadOnlySpan<char> span = shared.Array.AsSpan(0, Builder.Length);

        int counter = 0;
        int chunkStart = 0;

        // After a soft linebreak at least 1 char must remain:
        for (int i = 0; i < span.Length - 1; i++)
        {
            char c = span[i];

            counter += GetByteCount(c);

            if (counter < VCard.MAX_BYTES_PER_LINE)
            {
                continue;
            }

            if (counter > VCard.MAX_BYTES_PER_LINE)
            {
                i--; // one char back
            }

            _writer.WriteLine(shared.Array, chunkStart, i + 1 - chunkStart);
            _writer.Write(' ');
            
            counter = 1; // line start + ' '
            chunkStart = i + 1;
        }

        _writer.WriteLine(shared.Array, chunkStart, Builder.Length - chunkStart);
    }

    private int GetByteCount(char c)
    {
        _byteCounterArr[0] = c;
        return Encoding.UTF8.GetByteCount(_byteCounterArr);
    }

    #region Append

    [ExcludeFromCodeCoverage]
    protected virtual void AppendAccess(AccessProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendAddresses(IEnumerable<AddressProperty?> value) { }

    protected virtual void AppendAnniversaryViews(IEnumerable<DateAndOrTimeProperty?> value)
    {
        Debug.Assert(value is not null);

        if (value.WhereNotEmpty()
                 .FirstOrDefault(static x => x is DateOnlyProperty) is DateOnlyProperty pref)
        {
            if (Options.HasFlag(Opts.WriteXExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.X_ANNIVERSARY, pref.Group);
            }

            if (Options.HasFlag(Opts.WriteEvolutionExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY, pref.Group);
            }

            if (Options.HasFlag(Opts.WriteKAddressbookExtensions))
            {
                BuildAnniversary(VcfSerializer.X_KADDRESSBOOK_X_Anniversary, pref.Group);
            }

            if (Options.HasFlag(Opts.WriteWabExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY, pref.Group);
            }

            void BuildAnniversary(string propKey, string? group)
            {
                DateOnly dto = pref.Value;

                var xAnniversary = new NonStandardProperty(
                    propKey,
                    pref.IsEmpty ? null : string.Format(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}",
                                  dto.Year, dto.Month, dto.Day),
                    group);

                BuildProperty(propKey, xAnniversary);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendBirthDayViews(IEnumerable<DateAndOrTimeProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendBirthPlaceViews(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCalendarAddresses(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCategories(IEnumerable<StringCollectionProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendDeathDateViews(IEnumerable<DateAndOrTimeProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendDeathPlaceViews(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendDirectoryName(TextProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendDisplayNames(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendEMails(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendExpertises(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendFreeBusyUrls(IEnumerable<TextProperty?> value) { }

    protected virtual void AppendGenderViews(IEnumerable<GenderProperty?> value)
    {
        Debug.Assert(value is not null);

        if (value.FirstOrDefault(x => x?.Value?.Sex is not null) is GenderProperty pref)
        {
            Sex sex = pref.Value.Sex!.Value;

            if (sex != Sex.Male && sex != Sex.Female)
            {
                return;
            }

            if (Options.HasFlag(Opts.WriteXExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == Sex.Male ? "Male" : "Female", pref.Group);

                BuildProperty(propKey, xGender);
            }

            if (Options.HasFlag(Opts.WriteWabExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_WAB_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == Sex.Male ? "2" : "1", pref.Group);

                BuildProperty(propKey, xGender);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendGeoCoordinates(IEnumerable<GeoProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendHobbies(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendInterests(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendKeys(IEnumerable<DataProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendKind(KindProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendLanguages(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendLastRevision(TimeStampProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendLogos(IEnumerable<DataProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendMailer(TextProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendMembers(IEnumerable<RelationProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendNameViews(IEnumerable<NameProperty?> value) { }

    protected void AppendNonStandardProperties(IEnumerable<NonStandardProperty?> value)
    {
        Debug.Assert(value is not null);

        if (!this.Options.HasFlag(Opts.WriteNonStandardProperties))
        {
            return;
        }

        foreach (NonStandardProperty? nonStandardProp in value)
        {
            if (nonStandardProp is null)
            {
                continue;
            }

            BuildProperty(nonStandardProp.XName,
                          nonStandardProp,
                          nonStandardProp.Parameters.Preference == 1);
        }
    }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendNotes(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendNickNames(IEnumerable<StringCollectionProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendOrganizations(IEnumerable<OrgProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendOrgDirectories(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendPhones(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendPhotos(IEnumerable<DataProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendProdID(TextProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendProfile(ProfileProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendVCardClients(IEnumerable<AppIDProperty?> value) { }

    protected virtual void AppendRelations(IEnumerable<RelationProperty?> value)
    {
        RelationProperty? agent = value.PrefOrNullIntl(static x => x.Parameters.RelationType.IsSet(Rel.Agent),
                                                       IgnoreEmptyItems);

        if (agent is not null)
        {
            BuildProperty(VCard.PropKeys.AGENT, agent);
        }

        RelationProperty? spouse = value.PrefOrNullIntl(static x => x.Parameters.RelationType.IsSet(Rel.Spouse),
                                                        IgnoreEmptyItems);

        if (spouse is not null)
        {
            if (spouse is RelationVCardProperty vCardProp)
            {
                spouse = ConvertToRelationTextProperty(vCardProp);
            }

            if (spouse is RelationTextProperty)
            {
                if (Options.HasFlag(Opts.WriteXExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.X_SPOUSE, spouse);
                }

                if (Options.HasFlag(Opts.WriteKAddressbookExtensions))
                {
                    BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_SpouseName, spouse);
                }

                if (Options.HasFlag(Opts.WriteEvolutionExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE, spouse);
                }

                if (Options.HasFlag(Opts.WriteWabExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME, spouse);
                }
            }
        }

        static RelationProperty? ConvertToRelationTextProperty(RelationVCardProperty vcardProp)
        {
            string? name = vcardProp.Value?.DisplayNames?.PrefOrNullIntl(ignoreEmptyItems: true)?.Value;

            if (name is null)
            {
                NameProperty? vcdName = vcardProp.Value?.NameViews?.FirstOrNullIntl(ignoreEmptyItems: true);

                if (vcdName is not null)
                {
                    name = vcdName.ToDisplayName();
                }
                else
                {
                    return null;
                }
            }

            Debug.Assert(name is not null);
            return RelationProperty.FromText(name,
                                             vcardProp.Parameters.RelationType ?? Rel.Spouse,
                                             vcardProp.Group);
        }
    }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendRoles(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendSounds(IEnumerable<DataProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendSources(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendTimeZones(IEnumerable<TimeZoneProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendTitles(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendUniqueIdentifier(IDProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendURLs(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendXmlProperties(IEnumerable<XmlProperty?> value) { }

    internal abstract void AppendBase64EncodedData(byte[]? data);

    #endregion
}
