using System.Globalization;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal abstract class VcfSerializer : IDisposable
{
    private readonly TextWriter _writer;

    internal ParameterSerializer ParameterSerializer { get; }

    [NotNull]
    protected VCard? VCardToSerialize { get; private set; }

    internal StringBuilder Builder { get; } = new();

    internal StringBuilder Worker { get; } = new();

    internal abstract VCdVersion Version { get; }

    internal VcfOptions Options { get; }

    [NotNull]
    internal string? PropertyKey { get; private set; }

    internal bool IsPref { get; private set; }

    internal bool IgnoreEmptyItems { get; }

    internal ITimeZoneIDConverter? TimeZoneConverter { get; }


    protected VcfSerializer(TextWriter writer, VcfOptions options, ParameterSerializer parameterSerializer, ITimeZoneIDConverter? tzConverter)
    {
        this.Options = options;

        // Save this for performance:
        this.IgnoreEmptyItems = !options.IsSet(VcfOptions.WriteEmptyProperties);

        this.ParameterSerializer = parameterSerializer;
        this._writer = writer;
        this.TimeZoneConverter = tzConverter;
        writer.NewLine = VCard.NewLine;
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_SpouseName = "X-KADDRESSBOOK-X-SpouseName";

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_Anniversary = "X-KADDRESSBOOK-X-Anniversary";
  
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal const string X_KADDRESSBOOK_X_IMAddress = "X-KADDRESSBOOK-X-IMAddress";


    protected void BuildPrefProperty<T>(string propertyKey,
                                       IEnumerable<T?> serializables,
                                       Func<T, bool>? filter = null) where T : VCardProperty
    {
        Debug.Assert(serializables != null);

        VCardProperty? pref = filter is null ? serializables.PrefOrNullIntl(IgnoreEmptyItems)
                                             : serializables.PrefOrNullIntl(filter, IgnoreEmptyItems);

        if (pref != null)
        {
            BuildProperty(propertyKey, pref);
        }
    }

    protected void BuildFirstProperty<T>(string propertyKey,
                                         IEnumerable<T?> serializables,
                                         Func<T, bool>? filter = null) where T : VCardProperty
    {
        Debug.Assert(serializables != null);

        VCardProperty? first = filter is null ? serializables.FirstOrNullIntl(IgnoreEmptyItems)
                                              : serializables.FirstOrNullIntl(filter, IgnoreEmptyItems);

        if (first != null)
        {
            BuildProperty(propertyKey, first);
        }
    }

    protected virtual void BuildPropertyCollection(string propertyKey, IEnumerable<VCardProperty?> serializables)
    {
        Debug.Assert(serializables != null);

        bool first = true;

        foreach (VCardProperty prop in serializables.OrderByPrefIntl(IgnoreEmptyItems))
        {
            BuildProperty(propertyKey, prop, first && prop.Parameters.Preference < 100);
            first = false;
        }
    }

    //protected IEnumerable<T> FilterSerializables<T>(IEnumerable<T?> properties)
    //    where T : VCardProperty
    //{
    //    foreach (var prop in properties)
    //    {
    //        if(IsPropertyWithData(prop, Options))
    //        {
    //            yield return prop;
    //        }
    //    }

    //    static bool IsPropertyWithData([NotNullWhen(true)] VCardProperty? x, VcfOptions options)
    //       => x != null && (!x.IsEmpty || options.HasFlag(VcfOptions.WriteEmptyProperties));
    //}



    internal static VcfSerializer GetSerializer(TextWriter writer, VCdVersion version, VcfOptions options, ITimeZoneIDConverter? tzConverter)
    {
        return version switch
        {
            VCdVersion.V2_1 => new Vcf_2_1Serializer(writer, options, tzConverter),
            VCdVersion.V3_0 => new Vcf_3_0Serializer(writer, options, tzConverter),
            VCdVersion.V4_0 => new Vcf_4_0Serializer(writer, options),
            _ => throw new ArgumentException(Res.UndefinedEnumValue, nameof(version))
        };
    }


    internal void Serialize(VCard vCard)
    {
        Debug.Assert(vCard != null);

        VCardToSerialize = vCard;
        ReplenishRequiredProperties();

        _ = Builder.Clear();
        _writer.WriteLine("BEGIN:VCARD");
        _writer.Write(VCard.PropKeys.VERSION);
        _writer.Write(':');
        _writer.WriteLine(VersionString);

        AppendProperties();

        _writer.WriteLine("END:VCARD");
    }

    protected abstract void ReplenishRequiredProperties();

    protected abstract string VersionString { get; }

    private void AppendProperties()
    {
        foreach (KeyValuePair<VCdProp, object> kvp in 
            ((IEnumerable<KeyValuePair<VCdProp, object>>)VCardToSerialize).OrderBy(x => x.Key))
        {
            switch (kvp.Key)
            {
                case VCdProp.Profile:
                    AppendProfile((ProfileProperty)kvp.Value);
                    break;
                case VCdProp.Kind:
                    AppendKind((KindProperty)kvp.Value);
                    break;
                case VCdProp.Mailer:
                    AppendMailer((TextProperty)kvp.Value);
                    break;
                case VCdProp.ProdID:
                    AppendProdID((TextProperty)kvp.Value);
                    break;
                case VCdProp.TimeStamp:
                    AppendLastRevision((TimeStampProperty)kvp.Value);
                    break;
                case VCdProp.UniqueIdentifier:
                    AppendUniqueIdentifier((UuidProperty)kvp.Value);
                    break;
                case VCdProp.Categories:
                    AppendCategories((IEnumerable<StringCollectionProperty?>)kvp.Value);
                    break;
                case VCdProp.TimeZones:
                    AppendTimeZones((IEnumerable<TimeZoneProperty?>)kvp.Value);
                    break;
                case VCdProp.GeoCoordinates:
                    AppendGeoCoordinates((IEnumerable<GeoProperty?>)kvp.Value);
                    break;
                case VCdProp.Access:
                    AppendAccess((AccessProperty)kvp.Value);
                    break;
                case VCdProp.Sources:
                    AppendSources((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.DirectoryName:
                    AppendDirectoryName((TextProperty)kvp.Value);
                    break;
                case VCdProp.DisplayNames:
                    AppendDisplayNames((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.NameViews:
                    AppendNameViews((IEnumerable<NameProperty?>)kvp.Value);
                    break;
                case VCdProp.GenderViews:
                    AppendGenderViews((IEnumerable<GenderProperty?>)kvp.Value);
                    break;
                case VCdProp.NickNames:
                    AppendNickNames((IEnumerable<StringCollectionProperty?>)kvp.Value);
                    break;
                case VCdProp.Titles:
                    AppendTitles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Roles:
                    AppendRoles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Organizations:
                    AppendOrganizations((IEnumerable<OrganizationProperty?>)kvp.Value);
                    break;
                case VCdProp.BirthDayViews:
                    AppendBirthDayViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case VCdProp.BirthPlaceViews:
                    AppendBirthPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.AnniversaryViews:
                    AppendAnniversaryViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case VCdProp.DeathDateViews:
                    AppendDeathDateViews((IEnumerable<DateAndOrTimeProperty?>)kvp.Value);
                    break;
                case VCdProp.DeathPlaceViews:
                    AppendDeathPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Addresses:
                    AppendAddresses((IEnumerable<AddressProperty?>)kvp.Value);
                    break;
                case VCdProp.PhoneNumbers:
                    AppendPhoneNumbers((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.EmailAddresses:
                    AppendEmailAddresses((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.URLs:
                    AppendURLs((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.InstantMessengerHandles:
                    AppendInstantMessengerHandles((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Keys:
                    AppendKeys((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case VCdProp.CalendarAddresses:
                    AppendCalendarAddresses((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.CalendarUserAddresses:
                    AppendCalendarUserAddresses((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.FreeOrBusyUrls:
                    AppendFreeBusyUrls((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Relations:
                    AppendRelations((IEnumerable<RelationProperty?>)kvp.Value);
                    break;
                case VCdProp.Members:
                    AppendMembers((IEnumerable<RelationProperty?>)kvp.Value);
                    break;
                case VCdProp.OrgDirectories:
                    AppendOrgDirectories((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Expertises:
                    AppendExpertises((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Interests:
                    AppendInterests((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Hobbies:
                    AppendHobbies((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Languages:
                    AppendLanguages((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.Notes:
                    AppendNotes((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.XmlProperties:
                    AppendXmlProperties((IEnumerable<XmlProperty?>)kvp.Value);
                    break;
                case VCdProp.Logos:
                    AppendLogos((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case VCdProp.Photos:
                    AppendPhotos((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case VCdProp.Sounds:
                    AppendSounds((IEnumerable<DataProperty?>)kvp.Value);
                    break;
                case VCdProp.PropertyIDMappings:
                    AppendPropertyIDMappings((IEnumerable<PropertyIDMappingProperty?>)kvp.Value);
                    break;
                case VCdProp.NonStandard:
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
            AppendLineFolding();
        }

        _writer.WriteLine(Builder);
    }


    protected void BuildXImpps(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value != null);

        if (Options.HasFlag(VcfOptions.WriteXExtensions))
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

        if (Options.HasFlag(VcfOptions.WriteKAddressbookExtensions))
        {
            TextProperty? prop = value.PrefOrNullIntl(IgnoreEmptyItems);
                                 
            if (prop != null)
            {
                BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_IMAddress, prop, prop.Parameters.Preference < 100);
            }
        }
    }


    protected virtual void AppendLineFolding()
    {
        int counter = 0;

        Encoding enc = Encoding.UTF8;
        char[] arr = new char[1];

        // nach einem Softlinebreak muss noch mindestens 1 Zeichen
        // folgen:
        for (int i = 0; i < Builder.Length - 1; i++)
        {
            char c = Builder[i];


            arr[0] = c;
            counter += enc.GetByteCount(arr);

            if (counter < VCard.MAX_BYTES_PER_LINE)
            {
                continue;
            }
            else if (counter > VCard.MAX_BYTES_PER_LINE)
            {
                i--; // ein Zeichen zurück
            }


            _ = Builder.Insert(++i, VCard.NewLine);
            i += VCard.NewLine.Length;
            _ = Builder.Insert(i, ' ');
            counter = 1; // um das Leerzeichen vorschieben

        }
    }


    #region Append

    [ExcludeFromCodeCoverage]
    protected virtual void AppendAccess(AccessProperty value) { }


    [ExcludeFromCodeCoverage]
    protected virtual void AppendAddresses(IEnumerable<AddressProperty?> value) { }
    

    protected virtual void AppendAnniversaryViews(IEnumerable<DateAndOrTimeProperty?> value)
    {
        Debug.Assert(value != null);

        if (value.WhereNotEmpty()
                 .FirstOrDefault(static x => x is DateOnlyProperty) is DateOnlyProperty pref)
        {
            if (Options.HasFlag(VcfOptions.WriteXExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.X_ANNIVERSARY, pref.Group);
            }

            if (Options.HasFlag(VcfOptions.WriteEvolutionExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY, pref.Group);
            }

            if (Options.HasFlag(VcfOptions.WriteKAddressbookExtensions))
            {
                BuildAnniversary(VcfSerializer.X_KADDRESSBOOK_X_Anniversary, pref.Group);
            }

            if (Options.HasFlag(VcfOptions.WriteWabExtensions))
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
    protected virtual void AppendEmailAddresses(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendExpertises(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendFreeBusyUrls(IEnumerable<TextProperty?> value) { }


    protected virtual void AppendGenderViews(IEnumerable<GenderProperty?> value)
    {
        Debug.Assert(value != null);

        if (value.FirstOrDefault(x => x?.Value?.Gender != null) is GenderProperty pref)
        {
            Gender sex = pref.Value.Gender!.Value;

            if (sex != Gender.Male && sex != Gender.Female)
            {
                return;
            }

            if (Options.HasFlag(VcfOptions.WriteXExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == Gender.Male ? "Male" : "Female", pref.Group);

                BuildProperty(propKey, xGender);
            }


            if (Options.HasFlag(VcfOptions.WriteWabExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_WAB_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == Gender.Male ? "2" : "1", pref.Group);

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
        Debug.Assert(value != null);

        if (!this.Options.HasFlag(VcfOptions.WriteNonStandardProperties))
        {
            return;
        }

        foreach (NonStandardProperty? nonStandardProp in value)
        {
            if (nonStandardProp is null)
            {
                continue;
            }

            BuildProperty(nonStandardProp.PropertyKey,
                          nonStandardProp,
                          nonStandardProp.Parameters.Preference == 1);
        }
    }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendNotes(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendNickNames(IEnumerable<StringCollectionProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendOrganizations(IEnumerable<OrganizationProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendOrgDirectories(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendPhoneNumbers(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendPhotos(IEnumerable<DataProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendProdID(TextProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendProfile(ProfileProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendPropertyIDMappings(IEnumerable<PropertyIDMappingProperty?> value) { }

    protected virtual void AppendRelations(IEnumerable<RelationProperty?> value)
    {
        RelationProperty? agent = value.PrefOrNullIntl(static x => x.Parameters.Relation.IsSet(RelationTypes.Agent),
                                                       IgnoreEmptyItems);

        if (agent != null)
        {
            BuildProperty(VCard.PropKeys.AGENT, agent);
        }

        RelationProperty? spouse = value.PrefOrNullIntl(static x => x.Parameters.Relation.IsSet(RelationTypes.Spouse), 
                                                        IgnoreEmptyItems);
                   
        if (spouse != null)
        {
            if (spouse is RelationVCardProperty vCardProp)
            {
                spouse = ConvertToRelationTextProperty(vCardProp);
            }

            if (spouse is RelationTextProperty)
            {
                if (Options.HasFlag(VcfOptions.WriteXExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.X_SPOUSE, spouse);
                }

                if (Options.HasFlag(VcfOptions.WriteKAddressbookExtensions))
                {
                    BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_SpouseName, spouse);
                }

                if (Options.HasFlag(VcfOptions.WriteEvolutionExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE, spouse);
                }

                if (Options.HasFlag(VcfOptions.WriteWabExtensions))
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

                if (vcdName != null)
                {
                    name = vcdName.ToDisplayName();
                }
                else
                {
                    return null;
                }
            }

            Debug.Assert(name != null);
            return RelationProperty.FromText(name, 
                                             vcardProp.Parameters.Relation ?? RelationTypes.Spouse,
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
    protected virtual void AppendUniqueIdentifier(UuidProperty value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendURLs(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendXmlProperties(IEnumerable<XmlProperty?> value) { }

    internal abstract void AppendBase64EncodedData(byte[]? data);


    public void Dispose() => _writer.Dispose();

    #endregion

}
