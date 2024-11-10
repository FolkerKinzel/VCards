using FolkerKinzel.DataUrls;
using System.Security.Cryptography;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_4_0Serializer : VcfSerializer
{
    internal Vcf_4_0Serializer(TextWriter writer, Opts options)
        : base(writer, options, new ParameterSerializer4_0(options), null) { }

    internal override VCdVersion Version => VCdVersion.V4_0;

    protected override string VersionString => "4.0";

    protected override void ReplenishRequiredProperties()
    {
        if ((VCardToSerialize.Members?.Any(x => x is not null && (!x.IsEmpty || !IgnoreEmptyItems)) ?? false)
            && (VCardToSerialize.Kind?.Value != Kind.Group))
        {
            VCardToSerialize.Kind = new KindProperty(Kind.Group);
        }

        if (VCardToSerialize.DisplayNames is null)
        {
            VCardToSerialize.DisplayNames = [];
        }
    }

    protected override void BuildPropertyCollection(string propertyKey, IEnumerable<VCardProperty?> props)
    {
        Debug.Assert(props is not null);

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
            Debug.Assert(props is not null);

            if (props.OfType<VCardProperty>().Take(2).Count() <= 1)
            {
                return;
            }

            string altID = props.FirstOrDefault(
                static x => x?.Parameters.AltID is not null)?.Parameters.AltID ?? "1";

            foreach (VCardProperty? prop in props)
            {
                if (prop is not null)
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
        if (Options.HasFlag(Opts.WriteRfc6474Extensions))
        {
            BuildPropertyViews(VCard.PropKeys.Rfc6474.BIRTHPLACE, value);
        }
    }

    protected override void AppendCalendarAddresses(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.Rfc2739.CALURI, value);

    protected override void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.Rfc2739.CALADRURI, value);

    protected override void AppendCategories(IEnumerable<StringCollectionProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CATEGORIES, value);

    protected override void AppendContactUris(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc8605Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc8605.CONTACT_URI, value);
        }
    }

    protected override void AppendCreated(TimeStampProperty value)
    {
        if (Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            BuildProperty(VCard.PropKeys.Rfc9554.CREATED, value);
        }
    }

    protected override void AppendDeathDateViews(IEnumerable<DateAndOrTimeProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc6474Extensions))
        {
            BuildPropertyViews(VCard.PropKeys.Rfc6474.DEATHDATE, value);
        }
    }

    protected override void AppendDeathPlaceViews(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc6474Extensions))
        {
            BuildPropertyViews(VCard.PropKeys.Rfc6474.DEATHPLACE, value);
        }
    }

    protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value is not null);

        IEnumerable<TextProperty> displNames = value.OrderByPrefIntl(IgnoreEmptyItems);

        if (!displNames.Any())
        {
            NameProperty? name = VCardToSerialize.NameViews?.FirstOrNullIntl(IgnoreEmptyItems);

            if (name is not null)
            {
                var tProp = new TextProperty(NameFormatter.Default.ToDisplayName(name, this.VCardToSerialize), name.Group);
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
        if (Options.HasFlag(Opts.WriteRfc6715Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc6715.EXPERTISE, value);
        }
    }

    protected override void AppendFreeBusyUrls(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.Rfc2739.FBURL, value);

    protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        => BuildPropertyViews(VCard.PropKeys.GENDER, value);

    protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.GEO, value);

    protected override void AppendGramGenders(IEnumerable<GramProperty?> value)
    {
        if(Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc9554.GRAMGENDER, value);
        }
    }

    protected override void AppendHobbies(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc6715Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc6715.HOBBY, value);
        }
    }

    protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value is not null);

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
        if (Options.HasFlag(Opts.WriteRfc6715Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc6715.INTEREST, value);
        }
    }

    protected override void AppendJSContactProps(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc9555Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc9555.JSPROP, value);
        }
    }

    protected override void AppendKind(KindProperty value)
        => BuildProperty(VCard.PropKeys.KIND, value);

    protected override void AppendKeys(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.KEY, value);

    protected override void AppendLanguage(TextProperty value)
    {
        if (Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            BuildProperty(VCard.PropKeys.Rfc9554.LANGUAGE, value);
        }
    }

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
        if (Options.HasFlag(Opts.WriteRfc6715Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc6715.ORG_DIRECTORY, value);
        }
    }

    protected override void AppendPhones(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TEL, value);

    protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.PHOTO,
                                   value.Where(static x => x is EmbeddedBytesProperty
                                                             or ReferencedDataProperty));

    protected override void AppendProdID(TextProperty value)
        => BuildProperty(VCard.PropKeys.PRODID, value);

    protected override void AppendPronouns(IEnumerable<TextProperty?> value)
    {
        if(Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc9554.PRONOUNS, value);
        }
    }

    protected override void AppendVCardClients(IEnumerable<AppIDProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.CLIENTPIDMAP, value);

    protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.RELATED, value);

    protected override void AppendRoles(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.ROLE, value);

    protected override void AppendSocialMediaProfiles(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc9554Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc9554.SOCIALPROFILE, value);
            return;
        }

        if (Options.HasFlag(Opts.WriteXExtensions))
        {
            BuildPropertyCollection(VCard.PropKeys.NonStandard.X_SOCIALPROFILE, value);
        }
    }

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

    protected override void AppendUniqueIdentifier(ContactIDProperty value)
        => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.URL, value);

    protected override void AppendXmlProperties(IEnumerable<XmlProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.XML, value);

    #endregion

    internal override void AppendBase64EncodedData(byte[]? data)
        => Builder.Append(DataUrl.FromBytes(data, this.ParameterSerializer.ParaSection.MediaType));
    // A "data" URL contains a comma and a semicolon and should be masked.
    // But the "Verifier notes" to https://www.rfc-editor.org/errata/eid3845
    // note that "the ABNF does not support escaping for URIs."
    // That's why the "data" URL will remain unmasked.
}
