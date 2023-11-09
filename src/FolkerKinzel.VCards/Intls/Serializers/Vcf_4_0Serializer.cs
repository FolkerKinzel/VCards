using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_4_0Serializer : VcfSerializer
{
    internal Vcf_4_0Serializer(TextWriter writer, VcfOptions options) 
        : base(writer, options, new ParameterSerializer4_0(options), null) { }

    internal override VCdVersion Version => VCdVersion.V4_0;

    protected override string VersionString => "4.0";

    protected override void ReplenishRequiredProperties()
    {
        if ((VCardToSerialize.Members?.Any(x => x != null && (!x.IsEmpty || !IgnoreEmptyItems)) ?? false)
            && (VCardToSerialize.Kind?.Value != Kind.Group))
        {
            VCardToSerialize.Kind = new KindProperty(Kind.Group);
        }

        if (VCardToSerialize.DisplayNames is null)
        {
            VCardToSerialize.DisplayNames = Array.Empty<TextProperty?>();
        }
    }

    protected override void BuildPropertyCollection(string propertyKey, IEnumerable<VCardProperty?> props)
    {
        Debug.Assert(props != null);

        foreach (VCardProperty? prop in props)
        {
            if (prop is null)
            {
                continue;
            }

            BuildProperty(propertyKey, prop);
        }
    }

    private void BuildPropertyViews(string propertyKey, IEnumerable<VCardProperty?> props)
    {
        SetAltID(props);
        BuildPropertyCollection(propertyKey, props);

        /////////////////////////////////////////////////////////////////////////////////

        static void SetAltID(IEnumerable<VCardProperty?> props)
        {
            Debug.Assert(props != null);

            if (props.WhereNotNull().Take(2).Count() <= 1)
            {
                return;
            }

            string altID = props.FirstOrDefault(
                static x => x?.Parameters.AltID != null)?.Parameters.AltID ?? "1";

            foreach (VCardProperty? prop in props)
            {
                if (prop != null)
                {
                    prop.Parameters.AltID = altID;
                }
            }
        }
    }
    #region Append

    protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.ADR, value);

    protected override void AppendAnniversaryViews(IEnumerable<DateAndOrTimeProperty?> value)
        => BuildPropertyViews(VCard.PropKeys.ANNIVERSARY, value);

    protected override void AppendBirthDayViews(IEnumerable<DateAndOrTimeProperty?> value)
        => BuildPropertyViews(VCard.PropKeys.BDAY, value);

    protected override void AppendBirthPlaceViews(IEnumerable<TextProperty?> value)
    {
        if (!Options.HasFlag(VcfOptions.WriteRfc6474Extensions))
        {
            return;
        }

        BuildPropertyViews(VCard.PropKeys.NonStandard.BIRTHPLACE, value);
    }

    protected override void AppendCalendarAddresses(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CALURI, value);

    protected override void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CALADRURI, value);

    protected override void AppendCategories(IEnumerable<StringCollectionProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CATEGORIES, value);

    protected override void AppendDeathDateViews(IEnumerable<DateAndOrTimeProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6474Extensions))
        {
            return;
        }

        BuildPropertyViews(VCard.PropKeys.NonStandard.DEATHDATE, value);
    }

    protected override void AppendDeathPlaceViews(IEnumerable<TextProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6474Extensions))
        {
            return;
        }

        BuildPropertyViews(VCard.PropKeys.NonStandard.DEATHPLACE, value);
    }

    protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value != null);

        IEnumerable<TextProperty> displNames = value.OrderByPrefIntl(IgnoreEmptyItems);

        if (!displNames.Any())
        {
            var name = VCardToSerialize.NameViews?.FirstOrNullIntl(IgnoreEmptyItems);

            if (name is not null)
            {
                var tProp = new TextProperty(name.ToDisplayName(), name.Group);
                tProp.Parameters.Assign(name.Parameters);
                displNames = tProp;
            }
            else
            {
                displNames = new TextProperty(IgnoreEmptyItems ? "?" : null);
            }
        }
        
        BuildPropertyCollection(VCard.PropKeys.FN, displNames);
    }

    protected override void AppendEMails(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.EMAIL, value);

    protected override void AppendExpertises(IEnumerable<TextProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
        {
            return;
        }

        BuildPropertyCollection(VCard.PropKeys.NonStandard.EXPERTISE, value);
    }

    protected override void AppendFreeBusyUrls(IEnumerable<TextProperty?> value) 
        => BuildPropertyCollection(VCard.PropKeys.FBURL, value);

    protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        => BuildPropertyViews(VCard.PropKeys.GENDER, value);

    protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.GEO, value);

    protected override void AppendHobbies(IEnumerable<TextProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
        {
            return;
        }

        BuildPropertyCollection(VCard.PropKeys.NonStandard.HOBBY, value);
    }

    protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value != null);

        foreach (TextProperty? prop in value)
        {
            if (prop is null)
            {
                continue;
            }

            if (prop.Parameters.InstantMessengerType.IsSet(Impp.Personal))
            {
                prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PCl.Home);
            }

            if (prop.Parameters.InstantMessengerType.IsSet(Impp.Business))
            {
                prop.Parameters.PropertyClass = prop.Parameters.PropertyClass.Set(PCl.Work);
            }

            BuildProperty(VCard.PropKeys.IMPP, prop);
        }
    }

    protected override void AppendInterests(IEnumerable<TextProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
        {
            return;
        }

        BuildPropertyCollection(VCard.PropKeys.NonStandard.INTEREST, value);
    }

    protected override void AppendKind(KindProperty value)
        => BuildProperty(VCard.PropKeys.KIND, value);

    protected override void AppendKeys(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.KEY, value);

    protected override void AppendLanguages(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.LANG, value);

    protected override void AppendLastRevision(TimeStampProperty value) 
        => BuildProperty(VCard.PropKeys.REV, value);

    protected override void AppendLogos(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.LOGO,
                                   value.Where(static x => x is EmbeddedBytesProperty 
                                                             or ReferencedDataProperty));

    protected override void AppendMembers(IEnumerable<RelationProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.MEMBER, value);

    protected override void AppendNameViews(IEnumerable<NameProperty?> value)
        => BuildPropertyViews(VCard.PropKeys.N, value);

    protected override void AppendNickNames(IEnumerable<StringCollectionProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.NICKNAME, value);

    protected override void AppendNotes(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.NOTE, value);

    protected override void AppendOrganizations(IEnumerable<OrgProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.ORG, value);

    protected override void AppendOrgDirectories(IEnumerable<TextProperty?> value)
    {
        if (!Options.IsSet(VcfOptions.WriteRfc6715Extensions))
        {
            return;
        }

        BuildPropertyCollection(VCard.PropKeys.NonStandard.ORG_DIRECTORY, value);
    }

    protected override void AppendPhones(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TEL, value);

    protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.PHOTO, 
                                   value.Where(static x => x is EmbeddedBytesProperty 
                                                             or ReferencedDataProperty));

    protected override void AppendProdID(TextProperty value)
        => BuildProperty(VCard.PropKeys.PRODID, value);

    protected override void AppendPropertyIDMappings(IEnumerable<PropertyIDMappingProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CLIENTPIDMAP, value);

    protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.RELATED, value);

    protected override void AppendRoles(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.ROLE, value);

    protected override void AppendSounds(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.SOUND, 
                                   value.Where(static x => x is EmbeddedBytesProperty 
                                                             or ReferencedDataProperty));
    protected override void AppendSources(IEnumerable<TextProperty?> value) 
        => BuildPropertyCollection(VCard.PropKeys.SOURCE, value);

    protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TZ, value);

    protected override void AppendTitles(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TITLE, value);

    protected override void AppendUniqueIdentifier(UuidProperty value)
        => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.URL, value);

    protected override void AppendXmlProperties(IEnumerable<XmlProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.XML, value);

    #endregion

    [ExcludeFromCodeCoverage]
    internal override void AppendBase64EncodedData(byte[]? data) 
        => throw new NotImplementedException();
}
