using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal abstract class VcfSerializer
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

    internal ITimeZoneIDConverter? TimeZoneConverter { get; }


    protected VcfSerializer(TextWriter writer, VcfOptions options, ParameterSerializer parameterSerializer, ITimeZoneIDConverter? tzConverter)
    {
        this.Options = options;
        this.ParameterSerializer = parameterSerializer;
        this._writer = writer;
        this.TimeZoneConverter = tzConverter;
        writer.NewLine = VCard.NewLine;
    }


    /// <summary>
    /// Name des Ehepartners
    /// </summary>
    internal const string X_KADDRESSBOOK_X_SpouseName = "X-KADDRESSBOOK-X-SpouseName";

    /// <summary>
    /// beliebiges Jubiläum (zusätzlich zu BDAY, Geburtstag) 
    /// </summary>
    internal const string X_KADDRESSBOOK_X_Anniversary = "X-KADDRESSBOOK-X-Anniversary";

    ///// <summary>
    ///// Assistenzname (anstelle von AGENT) 
    ///// </summary>
    //internal const string X_KADDRESSBOOK_X_AssistantsName = "X-KADDRESSBOOK-X-AssistantsName";

    /// <summary>
    /// Instant-Messenger-Adresse
    /// </summary>
    internal const string X_KADDRESSBOOK_X_IMAddress = "X-KADDRESSBOOK-X-IMAddress";


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
        //var builder = Builder;

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
        foreach (KeyValuePair<VCdProp, object> kvp in ((IEnumerable<KeyValuePair<VCdProp, object>>)VCardToSerialize).OrderBy(x => x.Key))
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
                    AppendBirthDayViews((IEnumerable<DateTimeProperty?>)kvp.Value);
                    break;
                case VCdProp.BirthPlaceViews:
                    AppendBirthPlaceViews((IEnumerable<TextProperty?>)kvp.Value);
                    break;
                case VCdProp.AnniversaryViews:
                    AppendAnniversaryViews((IEnumerable<DateTimeProperty?>)kvp.Value);
                    break;
                case VCdProp.DeathDateViews:
                    AppendDeathDateViews((IEnumerable<DateTimeProperty?>)kvp.Value);
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
                case VCdProp.NonStandardProperties:
                    AppendNonStandardProperties((IEnumerable<NonStandardProperty?>)kvp.Value);
                    break;
                default:
                    break;
            }//switch
        }//foreach
    }


    protected void BuildProperty(string propertyKey, VCardProperty prop, bool isPref = false)
    {
        if (prop.IsEmpty && !Options.IsSet(VcfOptions.WriteEmptyProperties))
        {
            return;
        }

        PropertyKey = propertyKey;
        //PropertyStartIndex = Builder.Length;
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

        if (Options.IsSet(VcfOptions.WriteXExtensions))
        {
            TextProperty[] arr = value.Where(x => x?.Value != null).OrderBy(x => x!.Parameters.Preference).ToArray()!;

            for (int i = 0; i < arr.Length; i++)
            {
                TextProperty prop = arr[i];

                if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Personal))
                {
                    prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Home);
                }

                if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Business))
                {
                    prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PropertyClassTypes.Work);
                }

                if (prop.Parameters.InstantMessengerType.IsSet(ImppTypes.Mobile))
                {
                    prop.Parameters.TelephoneType = prop.Parameters.TelephoneType.Set(TelTypes.PCS);
                }

                string val = prop.Value!;

                if (val.StartsWith("aim:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_AIM, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("gg:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GADUGADU, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("gtalk:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GTALK, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("com.google.hangouts:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("icq:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_ICQ, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("xmpp:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_JABBER, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("msnim:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MSN, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("sip:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("skype:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_SKYPE, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("twitter:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_TWITTER, prop, i == 0 && prop.Parameters.Preference < 100);
                }
                else if (val.StartsWith("ymsgr:", StringComparison.OrdinalIgnoreCase))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.InstantMessenger.X_YAHOO, prop, i == 0 && prop.Parameters.Preference < 100);
                }
            }
        }

        if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
        {
            TextProperty? prop = value.Where(x => x?.Value != null).OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

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


    protected virtual void AppendAnniversaryViews(IEnumerable<DateTimeProperty?> value)
    {
        Debug.Assert(value != null);


        if (value.FirstOrDefault(x => x != null && x is DateTimeOffsetProperty) is DateTimeOffsetProperty pref
            && (!pref.IsEmpty || Options.IsSet(VcfOptions.WriteEmptyProperties)))
        {
            if (Options.IsSet(VcfOptions.WriteXExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.X_ANNIVERSARY, pref.Group);
            }

            if (Options.IsSet(VcfOptions.WriteEvolutionExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY, pref.Group);
            }

            if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
            {
                BuildAnniversary(VcfSerializer.X_KADDRESSBOOK_X_Anniversary, pref.Group);
            }

            if (Options.IsSet(VcfOptions.WriteWabExtensions))
            {
                BuildAnniversary(VCard.PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY, pref.Group);
            }

            void BuildAnniversary(string propKey, string? group)
            {
                DateTimeOffset dto = pref.Value ?? DateTimeOffset.MinValue;

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
    protected virtual void AppendBirthDayViews(IEnumerable<DateTimeProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendBirthPlaceViews(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCalendarAddresses(IEnumerable<TextProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendCategories(IEnumerable<StringCollectionProperty?> value) { }

    [ExcludeFromCodeCoverage]
    protected virtual void AppendDeathDateViews(IEnumerable<DateTimeProperty?> value) { }

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

        if (value.FirstOrDefault(x => x?.Value?.Sex != null) is GenderProperty pref)
        {
            VCdSex sex = pref.Value.Sex!.Value;

            if (sex != VCdSex.Male && sex != VCdSex.Female)
            {
                return;
            }

            if (Options.IsSet(VcfOptions.WriteXExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == VCdSex.Male ? "Male" : "Female", pref.Group);

                BuildProperty(propKey, xGender);
            }


            if (Options.IsSet(VcfOptions.WriteWabExtensions))
            {
                string propKey = VCard.PropKeys.NonStandard.X_WAB_GENDER;

                var xGender = new NonStandardProperty(
                    propKey,
                    sex == VCdSex.Male ? "2" : "1", pref.Group);

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

        if (!this.Options.IsSet(VcfOptions.WriteNonStandardProperties))
        {
            return;
        }

        foreach (NonStandardProperty? nonStandardProp in value)
        {
            if (nonStandardProp is null)
            {
                continue;
            }

            BuildProperty(nonStandardProp.PropertyKey, nonStandardProp, nonStandardProp.Parameters.Preference == 1);
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
        RelationProperty? agent = Options.IsSet(VcfOptions.WriteEmptyProperties)
            ? value.Where(x => x != null && x.Parameters.RelationType.IsSet(RelationTypes.Agent))
                   .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

            : value.Where(x => x != null && !x.IsEmpty && x.Parameters.RelationType.IsSet(RelationTypes.Agent))
                   .OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

        if (agent != null)
        {
            BuildProperty(VCard.PropKeys.AGENT, agent);
        }


        RelationProperty? spouse = Options.IsSet(VcfOptions.WriteEmptyProperties)
            ? value.Where(x => x != null && x.Parameters.RelationType.IsSet(RelationTypes.Spouse))
                   .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()

            : value.Where(x => x != null && !x.IsEmpty && x.Parameters.RelationType.IsSet(RelationTypes.Spouse))
                   .OrderBy(x => x!.Parameters.Preference).FirstOrDefault();

        if (spouse != null)
        {
            if (spouse is RelationVCardProperty vCardProp)
            {
                spouse = ConvertToRelationTextProperty(vCardProp);
            }

            if (spouse is RelationTextProperty)
            {

                if (Options.IsSet(VcfOptions.WriteXExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.X_SPOUSE, spouse);
                }

                if (Options.IsSet(VcfOptions.WriteKAddressbookExtensions))
                {
                    BuildProperty(VcfSerializer.X_KADDRESSBOOK_X_SpouseName, spouse);
                }

                if (Options.IsSet(VcfOptions.WriteEvolutionExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE, spouse);
                }

                if (Options.IsSet(VcfOptions.WriteWabExtensions))
                {
                    BuildProperty(VCard.PropKeys.NonStandard.X_WAB_SPOUSE_NAME, spouse);
                }
            }
        }


        static RelationTextProperty ConvertToRelationTextProperty(RelationVCardProperty vcardProp)
        {
            string? name = vcardProp.Value?.DisplayNames?.Where(x => x != null && !x.IsEmpty)
                .OrderBy(x => x!.Parameters.Preference).FirstOrDefault()?.Value;

            if (name is null)
            {
                Models.PropertyParts.Name? vcdName = vcardProp.Value?.NameViews?.Where(x => x != null && !x.IsEmpty).FirstOrDefault()?.Value;

                if (vcdName != null)
                {
                    name = $"{vcdName.FirstName} {vcdName.MiddleName}".Trim() + " " + vcdName.LastName;
                    name = name.Trim();
                }
            }


            return new RelationTextProperty(name, vcardProp.Parameters.RelationType ?? RelationTypes.Spouse, vcardProp.Group);
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

    #endregion

}
