using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal sealed class Vcf_3_0Serializer : VcfSerializer
{
    private NameBuilder? _nameBuilder;

    internal NameBuilder NameBuilder
    {
        get
        {
            this._nameBuilder ??= NameBuilder.Create();
            return _nameBuilder;
        }
    }

    internal Vcf_3_0Serializer(TextWriter writer,
                               Opts options,
                               ITimeZoneIDConverter? tzConverter)
        : base(writer, options, new ParameterSerializer3_0(options), tzConverter) { }

    internal override VCdVersion Version => VCdVersion.V3_0;

    protected override string VersionString => "3.0";

    protected override void ReplenishRequiredProperties()
    {
        if (VCardToSerialize.NameViews is null)
        {
            VCardToSerialize.NameViews = [];
        }

        if (VCardToSerialize.DisplayNames is null)
        {
            VCardToSerialize.DisplayNames = [];
        }
    }

    protected override void SetPropertyIDs()
    {
        // Do nothing
    }

    protected override void SetIndexes()
    {
        // Do nothing
    }

    protected override void AppendAccess(AccessProperty value)
        => BuildProperty(VCard.PropKeys.CLASS, value, false);

    protected override void AppendAddresses(IEnumerable<AddressProperty?> value)
    {
        Debug.Assert(value is not null);

        bool first = true;

        foreach (AddressProperty? prop in value.OrderByPrefIntl(IgnoreEmptyItems))
        {
            bool isPref = first && prop.Parameters.Preference < 100;

            // AddressProperty.IsEmpty returns false if only
            // AddressProperty.Parameters.Label or
            // AddressProperty.Parameters.GeoPosition or
            // AddressProperty.Parameters.TimeZone is not null:
            if (!prop!.Value.IsEmpty || !IgnoreEmptyItems)
            {
                BuildProperty(VCard.PropKeys.ADR, prop, isPref);
            }

            PreserveLabel(prop, isPref);

            if (first)
            {
                PreserveGeoCoordinate(prop);
                PreserveTimeZoneID(prop);
            }

            first = false;
        }
    }

    protected override void AppendAnniversaryViews(IEnumerable<DateAndOrTimeProperty?> value)
        => base.AppendAnniversaryViews(value);

    protected override void AppendBirthDayViews(IEnumerable<DateAndOrTimeProperty?> value)
        => BuildFirstProperty(VCard.PropKeys.BDAY,
                              value,
                              static x => x is DateOnlyProperty or DateTimeOffsetProperty);

    protected override void AppendCalendarAccessUri(IEnumerable<TextProperty?> value)
    {
        if(Options.HasFlag(Opts.WriteRfc2739Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc2739.CAPURI, value);
        }
    }

    protected override void AppendCalendarAddresses(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc2739Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc2739.CALURI, value);
        }
    }

    protected override void AppendCalendarUserAddresses(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc2739Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc2739.CALADRURI, value);
        }
    }

    protected override void AppendFreeBusyUrls(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteRfc2739Extensions))
        {
            BuildPropertyCollection(VCard.PropKeys.Rfc2739.FBURL, value);
        }
    }

    protected override void AppendCategories(IEnumerable<StringCollectionProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.CATEGORIES, value);

    protected override void AppendDirectoryName(TextProperty value)
        => BuildProperty(VCard.PropKeys.NAME, value);

    protected override void AppendDisplayNames(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value is not null);

        TextProperty? displayName = value.PrefOrNullIntl(IgnoreEmptyItems);

        if (displayName is null)
        {
            Debug.Assert(VCardToSerialize.NameViews is not null);
            NameProperty? name = VCardToSerialize.NameViews.FirstOrNullIntl(IgnoreEmptyItems);

            if (name is not null)
            {
                displayName = new TextProperty(NameFormatter.Default.ToDisplayName(name, this.VCardToSerialize), name.Group);
                displayName.Parameters.Assign(name.Parameters);
            }
        }

        BuildProperty(VCard.PropKeys.FN,
                      displayName ?? new TextProperty(IgnoreEmptyItems ? "?" : null));
    }

    protected override void AppendEMails(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.EMAIL, value);

    protected override void AppendGenderViews(IEnumerable<GenderProperty?> value)
        => base.AppendGenderViews(value);

    protected override void AppendGeoCoordinates(IEnumerable<GeoProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.GEO, value);

    protected override void AppendInstantMessengerHandles(IEnumerable<TextProperty?> value)
    {
        Debug.Assert(value is not null);

        if (Options.HasFlag(Opts.WriteImppExtension))
        {
            bool first = true;

            foreach (TextProperty prop in value.OrderByPrefIntl(IgnoreEmptyItems))
            {
                ParameterSection parameters = prop.Parameters;

                if (parameters.PropertyClass.IsSet(PCl.Home))
                {
                    parameters.InstantMessengerType =
                        parameters.InstantMessengerType.Set(Impp.Personal);
                }

                if (parameters.PropertyClass.IsSet(PCl.Work))
                {
                    parameters.InstantMessengerType =
                        parameters.InstantMessengerType.Set(Impp.Business);
                }

                BuildProperty(VCard.PropKeys.IMPP,
                              prop,
                              first && parameters.Preference < 100);
                first = false;
            }
        }

        BuildXImpps(value);
    }

    protected override void AppendKeys(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.KEY,
                             value,
                             static x => x is EmbeddedBytesProperty or EmbeddedTextProperty);

    protected override void AppendLastRevision(TimeStampProperty value)
        => BuildProperty(VCard.PropKeys.REV, value);

    protected override void AppendLogos(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.LOGO,
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendMailer(TextProperty value)
        => BuildProperty(VCard.PropKeys.MAILER, value);

    protected override void AppendNameViews(IEnumerable<NameProperty?> value)
    {
        Debug.Assert(value is not null);

        NameProperty name = value.FirstOrNullIntl(IgnoreEmptyItems)
                            ?? (IgnoreEmptyItems
                                ? new NameProperty(NameBuilder.Clear().AddFamilyName("?"))
                                : new NameProperty(NameBuilder.Clear()));

        BuildProperty(VCard.PropKeys.N, name);

        string? sortString = name.Parameters.SortAs?.FirstOrDefault();

        if (sortString is not null)
        {
            var sortStringProp = new TextProperty(sortString, name.Group);
            sortStringProp.Parameters.Language = name.Parameters.Language;
            BuildProperty(VCard.PropKeys.SORT_STRING, sortStringProp);
        }
    }

    protected override void AppendNickNames(IEnumerable<StringCollectionProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.NICKNAME, value);

    protected override void AppendNotes(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.NOTE, value);

    protected override void AppendOrganizations(IEnumerable<OrgProperty?> value)
    {
        OrgProperty? pref = value.PrefOrNullIntl(IgnoreEmptyItems);

        if (pref is null) 
        { 
            return; 
        }

        BuildProperty(VCard.PropKeys.ORG, pref);

        string? sortString = pref.Parameters.SortAs?.FirstOrDefault();

        if (sortString is not null)
        {
            // Required property
            Debug.Assert(VCardToSerialize.NameViews is not null);

            if (VCardToSerialize.NameViews!
                                .FirstOrNullIntl(IgnoreEmptyItems)?
                                .Parameters.SortAs?.Any() ?? false)
            { 
                return; 
            }

            var sortStringProp = new TextProperty(sortString, pref.Group);
            sortStringProp.Parameters.Language = pref.Parameters.Language;
            BuildProperty(VCard.PropKeys.SORT_STRING, sortStringProp);
        }
    }

    protected override void AppendPhones(IEnumerable<TextProperty?> value)
        => BuildPropertyCollection(VCard.PropKeys.TEL, value);

    protected override void AppendPhotos(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.PHOTO,
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendProdID(TextProperty value)
        => BuildProperty(VCard.PropKeys.PRODID, value);

    protected override void AppendProfile(ProfileProperty value)
        => BuildProperty(VCard.PropKeys.PROFILE, value);

    protected override void AppendRelations(IEnumerable<RelationProperty?> value)
        => base.AppendRelations(value);

    protected override void AppendRoles(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.ROLE, value);

    protected override void AppendSocialMediaProfiles(IEnumerable<TextProperty?> value)
    {
        if (Options.HasFlag(Opts.WriteXExtensions))
        {
            BuildPropertyCollection(VCard.PropKeys.NonStandard.X_SOCIALPROFILE, value);
        }
    }

    protected override void AppendSounds(IEnumerable<DataProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.SOUND,
                             value,
                             static x => x is EmbeddedBytesProperty or ReferencedDataProperty);

    protected override void AppendSources(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.SOURCE, value);

    protected override void AppendTimeZones(IEnumerable<TimeZoneProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.TZ, value);

    protected override void AppendTitles(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.TITLE, value);

    protected override void AppendUniqueIdentifier(IDProperty value)
        => BuildProperty(VCard.PropKeys.UID, value);

    protected override void AppendURLs(IEnumerable<TextProperty?> value)
        => BuildPrefProperty(VCard.PropKeys.URL, value);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendBase64EncodedData(byte[]? data)
        => _ = Builder.AppendBase64(data);


    private void PreserveLabel(AddressProperty prop, bool isPref)
    {
        string? label = prop.Parameters.Label;

        if (label is not null)
        {
            var labelProp = new TextProperty(label, prop.Group);
            labelProp.Parameters.Assign(prop.Parameters);
            BuildProperty(VCard.PropKeys.LABEL, labelProp, isPref);
        }
    }

    private void PreserveGeoCoordinate(AddressProperty prop)
    {
        GeoCoordinate? geo = prop.Parameters.GeoPosition;

        if (geo is not null)
        {
            GeoProperty? geoProp = VCardToSerialize
                                      .GeoCoordinates?
                                      .PrefOrNullIntl(IgnoreEmptyItems);

            if (geoProp is null)
            {
                BuildProperty(VCard.PropKeys.GEO,
                              new GeoProperty(geo, prop.Group),
                              false);
            }
        }
    }

    private void PreserveTimeZoneID(AddressProperty prop)
    {
        TimeZoneID? tz = prop.Parameters.TimeZone;

        if (tz is not null)
        {
            TimeZoneProperty? tzProp = VCardToSerialize
                                            .TimeZones?
                                            .PrefOrNullIntl(IgnoreEmptyItems);

            if (tzProp is null)
            {
                BuildProperty(VCard.PropKeys.TZ,
                              new TimeZoneProperty(tz, prop.Group),
                              false);
            }
        }
    }
}
