using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>Initializes a new <see cref="VCard" /> object.</summary>
    /// <param name="setID"><c>true</c> to set the <see cref="VCard.ID"/>
    /// property with a newly created <see cref="IDProperty"/>, otherwise
    /// <c>false</c>.</param>
    /// <param name="setCreated">
    /// <c>true</c> to set the <see cref="VCard.Created"/>
    /// property with a newly created <see cref="TimeStampProperty"/>, otherwise
    /// <c>false</c>.
    /// </param>
    public VCard(bool setID = true, bool setCreated = true)
    {
        if (setID)
        {
            ID = new IDProperty();
        }

        if (setCreated)
        {
            Created = new TimeStampProperty();
        }

        // Should be the last in ctor:
        Sync = new SyncOperation(this);
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="vCard">The <see cref="VCard"/> instance to clone.</param>
    private VCard(VCard vCard)
    {
        Version = vCard.Version;

        foreach (KeyValuePair<Prop, object> kvp in vCard._propDic)
        {
#if !DEBUG
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
#endif
            Set(kvp.Key, kvp.Value switch
            {
                VCardProperty prop => prop.Clone(),
                IEnumerable<XmlProperty?> xmlPropEnumerable => xmlPropEnumerable.Select(Cloned).Cast<XmlProperty?>().ToArray(),
                IEnumerable<TextProperty?> txtPropEnumerable => txtPropEnumerable.Select(Cloned).Cast<TextProperty?>().ToArray(),
                IEnumerable<DateAndOrTimeProperty?> dtTimePropEnumerable => dtTimePropEnumerable.Select(Cloned).Cast<DateAndOrTimeProperty?>().ToArray(),
                IEnumerable<AddressProperty?> adrPropEnumerable => adrPropEnumerable.Select(Cloned).Cast<AddressProperty?>().ToArray(),
                IEnumerable<NameProperty?> namePropEnumerable => namePropEnumerable.Select(Cloned).Cast<NameProperty?>().ToArray(),
                IEnumerable<RelationProperty?> relPropEnumerable => relPropEnumerable.Select(Cloned).Cast<RelationProperty?>().ToArray(),
                IEnumerable<OrgProperty?> orgPropEnumerable => orgPropEnumerable.Select(Cloned).Cast<OrgProperty?>().ToArray(),
                IEnumerable<StringCollectionProperty?> strCollPropEnumerable => strCollPropEnumerable.Select(Cloned).Cast<StringCollectionProperty?>().ToArray(),
                IEnumerable<GenderProperty?> sexPropEnumerable => sexPropEnumerable.Select(Cloned).Cast<GenderProperty?>().ToArray(),
                IEnumerable<GeoProperty?> geoPropEnumerable => geoPropEnumerable.Select(Cloned).Cast<GeoProperty?>().ToArray(),
                IEnumerable<DataProperty?> dataPropEnumerable => dataPropEnumerable.Select(Cloned).Cast<DataProperty?>().ToArray(),
                IEnumerable<NonStandardProperty?> nStdPropEnumerable => nStdPropEnumerable.Select(Cloned).Cast<NonStandardProperty?>().ToArray(),
                IEnumerable<AppIDProperty?> pidMapPropEnumerable => pidMapPropEnumerable.Select(Cloned).Cast<AppIDProperty?>().ToArray(),
                IEnumerable<TimeZoneProperty?> tzPropEnumerable => tzPropEnumerable.Select(Cloned).Cast<TimeZoneProperty?>().ToArray(),
                IEnumerable<GramProperty?> gramPropEnumerable => gramPropEnumerable.Select(Cloned).Cast<GramProperty?>().ToArray(),
#if DEBUG
                _ => throw new NotImplementedException()
#endif
            });
#if !DEBUG
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
#endif
        }//foreach

        Debug.Assert(VCard.IsAppRegistered);

        // Must be the last in ctor
        Sync = new SyncOperation(this);

        /////////////////////////////////////////////////
        static object? Cloned(ICloneable? x) => x?.Clone();
    }


    /// <summary>Initializes a <see cref="VCard" /> object from a queue of <see cref="VcfRow"
    /// /> objects.</summary>
    /// <param name="queue" />
    /// <param name="info" />
    /// <param name="versionHint" />
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class.</exception>
    internal VCard(Queue<VcfRow> queue, VcfDeserializationInfo info, VCdVersion versionHint)
    {
        Debug.Assert(queue is not null);
        Debug.Assert(queue.All(x => x is not null));

        this.Version = versionHint;

        int vcfRowsToParse = queue.Count;
        int vcfRowsParsed = 0;

        List<TextProperty>? labels = null;

        while (queue.Count != 0)
        {
            VcfRow? vcfRow = queue.Dequeue();

            switch (vcfRow.Key)
            {
                case PropKeys.VERSION:
                    this.Version = VCdVersionConverter.Parse(vcfRow.Value.Span);
                    break;
                case PropKeys.KIND:
                    Kind = new KindProperty(vcfRow);
                    break;
                case PropKeys.TEL:
                    Phones = Concat(Phones, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.EMAIL:
                    this.EMails = Concat(EMails, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.N:  //LastName, FirstName, MiddleName, Prefix, Suffix
                    this.NameViews = Concat(NameViews, new NameProperty(vcfRow, this.Version));
                    break;
                case PropKeys.FN:
                    DisplayNames = Concat(DisplayNames, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.BDAY:
                    BirthDayViews = Concat(BirthDayViews, DateAndOrTimeProperty.Parse(vcfRow, this.Version, info));
                    break;
                case PropKeys.ADR: // PostOfficeBox, ExtendedAddress, Street, Locality, Region, PostalCode, Country
                    Addresses = Concat(Addresses, new AddressProperty(vcfRow, this.Version));
                    break;
                case PropKeys.LABEL:
                    labels ??= [];
                    labels.Add(new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.REV:
                    TimeStamp = new TimeStampProperty(vcfRow, info);
                    break;
                case PropKeys.Rfc2739.CAPURI:
                    CalendarAccessUris = Concat(CalendarAccessUris, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc2739.CALURI:
                    CalendarAddresses = Concat(CalendarAddresses, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc2739.CALADRURI:
                    CalendarUserAddresses = Concat(CalendarUserAddresses, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc2739.FBURL:
                    FreeOrBusyUrls = Concat(FreeOrBusyUrls, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.TITLE:
                    Titles = Concat(Titles, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.ROLE:
                    Roles = Concat(Roles, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.NOTE:
                    Notes = Concat(Notes, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.URL:
                    Urls = Concat(Urls, new TextProperty(vcfRow, Version));
                    break;
                case PropKeys.Rfc8605.CONTACT_URI:
                    ContactUris = Concat(ContactUris, new TextProperty(vcfRow, Version));
                    break;
                case PropKeys.UID:
                    ID = new IDProperty(vcfRow);
                    break;
                case PropKeys.ORG:
                    Organizations = Concat(Organizations, new OrgProperty(vcfRow, this.Version));
                    break;
                case PropKeys.GEO:
                    GeoCoordinates = Concat(GeoCoordinates, new GeoProperty(vcfRow));
                    break;
                case PropKeys.NICKNAME:
                    NickNames = Concat(NickNames, new StringCollectionProperty(vcfRow, this.Version));
                    break;
                case PropKeys.CATEGORIES:
                    Categories = Concat(Categories, new StringCollectionProperty(vcfRow, this.Version));
                    break;
                case PropKeys.SOUND:
                    Sounds = Concat(Sounds, DataProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.PHOTO:
                    Photos = Concat(Photos, DataProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.LOGO:
                    Logos = Concat(Logos, DataProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.KEY:
                    Keys = Concat(Keys, DataProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.SORT_STRING: // nur vCard 3.0
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else
                    {
                        var textProp = new TextProperty(vcfRow, this.Version);

                        if (!textProp.IsEmpty)
                        {
                            if (NameViews is not null)
                            {
                                NameViews.FirstOrNullIntl(ignoreEmptyItems: false)!.Parameters.SortAs = [textProp.Value];
                            }
                            // this is not legal: N property is required in VCard 3.0
                            else if (Organizations is not null)
                            {
                                Organizations.PrefOrNullIntl(ignoreEmptyItems: false)!.Parameters.SortAs = [textProp.Value];
                            }
                            else
                            {
                                var name = new NameProperty();
                                name.Parameters.SortAs = [textProp.Value];
                                NameViews = name;
                            }
                        }
                    }
                    break;
                case PropKeys.SOURCE:
                    Sources = Concat(Sources, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.ANNIVERSARY:
                    this.AnniversaryViews = Concat(AnniversaryViews, DateAndOrTimeProperty.Parse(vcfRow, this.Version, info));
                    break;
                case PropKeys.NonStandard.X_ANNIVERSARY:
                case PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY:
                case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ANNIVERSARY:
                case PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (AnniversaryViews is null)
                    {
                        this.AnniversaryViews = DateAndOrTimeProperty.Parse(vcfRow, this.Version, info);
                    }

                    break;
                case PropKeys.GENDER:
                    this.GenderViews = Concat(GenderViews, new GenderProperty(vcfRow, this.Version));
                    break;
                case PropKeys.NonStandard.X_GENDER:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (GenderViews is null)
                    {
                        ReadOnlySpan<char> valSpan = vcfRow.Value.Span.TrimStart();

                        if (!valSpan.IsEmpty)
                        {
                            GenderProperty genderProp = char.ToUpperInvariant(valSpan[0]) == 'F'
                                ? new GenderProperty(Sex.Female, group: vcfRow.Group)
                                : new GenderProperty(Sex.Male, group: vcfRow.Group);
                            genderProp.Parameters.Assign(vcfRow.Parameters);

                            GenderViews = genderProp;
                        }
                    }
                    break;
                case PropKeys.NonStandard.X_WAB_GENDER:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if(GenderViews is null && !vcfRow.Value.IsEmpty)
                    {
                        GenderProperty genderProp = vcfRow.Value.Span.Contains('1')
                                            ? new GenderProperty(Enums.Sex.Female, group: vcfRow.Group)
                                            : new GenderProperty(Enums.Sex.Male, group: vcfRow.Group);
                        genderProp.Parameters.Assign(vcfRow.Parameters);

                        GenderViews = genderProp;
                    }
                    break;
                case PropKeys.IMPP:
                    Messengers =
                        Concat(Messengers, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.NonStandard.InstantMessenger.X_AIM:
                case PropKeys.NonStandard.InstantMessenger.X_GADUGADU:
                case PropKeys.NonStandard.InstantMessenger.X_GOOGLE_TALK:
                case PropKeys.NonStandard.InstantMessenger.X_GROUPWISE:
                case PropKeys.NonStandard.InstantMessenger.X_GTALK:
                case PropKeys.NonStandard.InstantMessenger.X_ICQ:
                case PropKeys.NonStandard.InstantMessenger.X_JABBER:
                case PropKeys.NonStandard.InstantMessenger.X_KADDRESSBOOK_X_IMADDRESS:
                case PropKeys.NonStandard.InstantMessenger.X_MSN:
                case PropKeys.NonStandard.InstantMessenger.X_MS_IMADDRESS:
                case PropKeys.NonStandard.InstantMessenger.X_SKYPE:
                case PropKeys.NonStandard.InstantMessenger.X_SKYPE_USERNAME:
                case PropKeys.NonStandard.InstantMessenger.X_TWITTER:
                case PropKeys.NonStandard.InstantMessenger.X_YAHOO:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else
                    {
                        var textProp = new TextProperty(vcfRow, this.Version);

                        if (textProp.Value is not null &&
                            (Messengers?.All(x => x!.Value != textProp.Value) ?? true))
                        {
                            Messengers = Concat(Messengers, textProp);

                            XMessengerParameterConverter.ConvertToInstantMessengerType(textProp.Parameters);

                            if (vcfRow.Key is PropKeys.NonStandard.InstantMessenger.X_SKYPE or
                               PropKeys.NonStandard.InstantMessenger.X_SKYPE_USERNAME)
                            {
                                textProp.Parameters.PhoneType =
                                    textProp.Parameters.PhoneType.Set(Tel.Voice | Tel.Video);
                            }

                            AddCopyToPhoneNumbers(textProp, textProp.Parameters);
                        }
                    }

                    break;
                case PropKeys.LANG:
                    Languages = Concat(Languages, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.MAILER:
                    Mailer = new TextProperty(vcfRow, this.Version);
                    break;
                case PropKeys.TZ:
                    TimeZones = Concat(TimeZones, new TimeZoneProperty(vcfRow, this.Version));
                    break;
                case PropKeys.CLASS:
                    Access = new AccessProperty(vcfRow);
                    break;
                case PropKeys.MEMBER:
                    Members = Concat(Members, RelationProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.RELATED:
                    Relations = Concat(Relations, RelationProperty.Parse(vcfRow, this.Version));
                    break;
                case PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
                case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_SPOUSENAME:
                case PropKeys.NonStandard.X_SPOUSE:
                case PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (Relations?.All(static x => x!.Parameters.RelationType != Rel.Spouse) ?? true)
                    {
                        vcfRow.Parameters.DataType = Data.Text; // führt dazu, dass eine RelationTextProperty erzeugt wird
                        vcfRow.Parameters.RelationType = Rel.Spouse;

                        Relations = Concat(Relations, RelationProperty.Parse(vcfRow, this.Version));
                    }

                    break;
                case PropKeys.NonStandard.X_ASSISTANT:
                case PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:
                case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (Relations?.All(x => !x!.Parameters.RelationType.IsSet(Rel.Agent)) ?? true)
                    {
                        vcfRow.Parameters.DataType ??= Data.Text;
                        vcfRow.Parameters.RelationType = Rel.Agent;

                        Relations = Concat(Relations, new RelationTextProperty(new TextProperty(vcfRow, this.Version)));
                    }

                    break;
                case PropKeys.AGENT:
                    {
                        vcfRow.Parameters.RelationType = Rel.Agent;
                        ReadOnlySpan<char> valSpan = vcfRow.Value.Span;

                        if (valSpan.IsWhiteSpace())
                        {
                            Relations = Concat(Relations, RelationProperty.FromText(null, Rel.Agent, vcfRow.Group));
                        }
                        else
                        {
                            if (valSpan.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                            {
                                VCard? nested = ParseNestedVcard(vcfRow.Value.Span, info, this.Version);

                                Relations =
                                    Concat(Relations, nested is null
                                                       ? new RelationTextProperty(new TextProperty(vcfRow, Version))

                                                       // use the ctor directly because nested can't be a circular
                                                       // reference and therefore don't neeed to be cloned:
                                                       : new RelationVCardProperty(nested, Rel.Agent, vcfRow.Group));
                            }
                            else
                            {
                                vcfRow.Parameters.DataType ??= Data.Text;
                                Relations = Concat(Relations, RelationProperty.Parse(vcfRow, this.Version));
                            }
                        }
                        break;
                    }
                case PropKeys.PROFILE:
                    this.Profile = new ProfileProperty(vcfRow, this.Version);
                    break;
                case PropKeys.XML:
                    Xmls = Concat(Xmls, new XmlProperty(vcfRow));
                    break;
                case PropKeys.CLIENTPIDMAP:
                    if (AppIDProperty.TryParse(vcfRow, out AppIDProperty? prop))
                    {
                        AppIDs = AppIDs?.Concat(prop) ?? prop;
                    }
                    break;
                case PropKeys.PRODID:
                    ProductID = new TextProperty(vcfRow, this.Version);
                    break;
                case PropKeys.NAME:
                    this.DirectoryName = new TextProperty(vcfRow, this.Version);
                    break;

                // Extensions to the vCard standard:
                case PropKeys.Rfc6474.DEATHDATE:
                    this.DeathDateViews =
                        Concat(DeathDateViews, DateAndOrTimeProperty.Parse(vcfRow, this.Version, info));
                    break;
                case PropKeys.Rfc6474.BIRTHPLACE:
                    this.BirthPlaceViews =
                        Concat(BirthPlaceViews, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc6474.DEATHPLACE:
                    this.DeathPlaceViews =
                        Concat(DeathPlaceViews, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc6715.EXPERTISE:
                    Expertises = Concat(Expertises, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc6715.INTEREST:
                    Interests = Concat(Interests, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc6715.HOBBY:
                    Hobbies = Concat(Hobbies, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc6715.ORG_DIRECTORY:
                    OrgDirectories = Concat(OrgDirectories, new TextProperty(vcfRow, this.Version));
                    break;
                case PropKeys.Rfc9554.CREATED:
                    Created = new TimeStampProperty(vcfRow, info);
                    break;
                case PropKeys.Rfc9554.GRAMGENDER:
                    GramGenders = Concat(GramGenders, new GramProperty(vcfRow));
                    break;
                case PropKeys.Rfc9554.LANGUAGE:
                    Language = new TextProperty(vcfRow, this.Version);
                    break;
                case PropKeys.Rfc9554.PRONOUNS:
                    Pronouns = Concat(Pronouns, new TextProperty(vcfRow, Version));
                    break;
                case PropKeys.Rfc9554.SOCIALPROFILE:
                    SocialMediaProfiles = Concat(SocialMediaProfiles, new TextProperty(vcfRow, Version));
                    break;
                case PropKeys.Rfc9555.JSPROP:
                    JSContactProps = Concat(JSContactProps, new TextProperty(vcfRow, Version));
                    break;
                case PropKeys.NonStandard.X_AB_LABEL:
                    ABLabels = Concat(ABLabels, new TextProperty(vcfRow, Version));
                    break;
                default:
                    Debug.Assert(!vcfRow.Key.Equals(PropKeys.NonStandard.X_SOCIALPROFILE, StringComparison.OrdinalIgnoreCase));
                    NonStandards = Concat(NonStandards, new NonStandardProperty(vcfRow));
                    break;
            };//switch

            vcfRowsParsed++;
        }//foreach


        if (Version is VCdVersion.V2_1 or VCdVersion.V3_0)
        {
            if (labels is not null)
            {
                AssignLabelsToAddresses(labels);
            }

            ConnectTimeZonesWithAddresses();
            ConnectGeoCoordinatesWithAddresses();
        }

        // Must be the last in ctor:
        Sync = new SyncOperation(this);


        static VCard? ParseNestedVcard(ReadOnlySpan<char> content,
                                       VcfDeserializationInfo info,
                                       VCdVersion versionHint)
        {
            // Version 2.1 is not masked:
            string s = versionHint == VCdVersion.V2_1
                ? content.ToString()
                : content.UnMaskValue(versionHint);

            using var reader = new StringReader(s);

            IList<VCard> list = Vcf.DoDeserialize(reader, versionHint);

            return list.FirstOrDefault();
        }
    }//ctor


    private static IEnumerable<TSource?> Concat<TSource>(
        IEnumerable<TSource?>? first, IEnumerable<TSource?> second) where TSource : VCardProperty
    {
        Debug.Assert(second is not null);

        return first?.Concat(second) ?? second;
    }

    private void AssignLabelsToAddresses(List<TextProperty> labels)
    {
        if (labels.Count == 1 && Addresses.IsSingle(ignoreEmptyItems: false))
        {
            Assign(labels[0], Addresses.First()!);
            return;
        }

        IEnumerable<IGrouping<string?, VCardProperty>> groups = (Addresses ?? Enumerable.Empty<VCardProperty?>())
                     .Concat(labels)
                     .GroupByVCardGroup();

        foreach (IGrouping<string?, VCardProperty> group in groups)
        {
            if (group.Key is not null)
            {
                if (group.PrefOrNullIntl(static x => x is TextProperty, ignoreEmptyItems: false)
                     is TextProperty label)
                {
                    if (group.PrefOrNullIntl(static x => x is AddressProperty, ignoreEmptyItems: false)
                        is AddressProperty adrProp)
                    {
                        Assign(label, adrProp);
                    }
                    else
                    {
                        Addresses = Concat(Addresses, CreateEmptyAddressPropertyWithLabel(label));
                    }
                }
            }
            else // group by parameters
            {
                var paraGroup = group.GroupBy(x => new
                {
                    x.Parameters.PropertyClass,
                    x.Parameters.AddressType,
                    x.Parameters.Preference
                });

                foreach (var para in paraGroup)
                {
                    if (para.FirstOrDefault(static x => x is TextProperty)
                        is TextProperty label)
                    {
                        if (para.FirstOrDefault(static x => x is AddressProperty)
                            is AddressProperty adrProp)
                        {
                            Assign(label, adrProp);
                        }
                        else
                        {
                            Addresses = Concat(Addresses, CreateEmptyAddressPropertyWithLabel(label));
                        }
                    }
                }
            }
        }

        static AddressProperty CreateEmptyAddressPropertyWithLabel(TextProperty label)
        {
            var adrProp = new AddressProperty("", null, null, null, autoLabel: false);
            adrProp.Parameters.Assign(label.Parameters);
            adrProp.Parameters.Label = label.Value;
            return adrProp;
        }

        static void Assign(TextProperty labelRow, AddressProperty address)
        {
            address.Parameters.Label = labelRow.Value;
            if (address.Parameters.CharSet is null)
            {
                address.Parameters.CharSet = labelRow.Parameters.CharSet;
            }
        }
    }

    private void ConnectTimeZonesWithAddresses()
    {
        if (TimeZones is null || Addresses is null)
        {
            return;
        }

        IEnumerable<IGrouping<string?, VCardProperty>> groups =
            Addresses.Concat<VCardProperty?>(TimeZones)
                     .GroupByVCardGroup();

        foreach (IGrouping<string?, VCardProperty> group in groups)
        {
            if (group.Key is not null)
            {
                if (group.FirstOrDefault(static x => x is TimeZoneProperty)
                    is TimeZoneProperty tzProp)
                {
                    foreach (VCardProperty prop in group)
                    {
                        if (prop is AddressProperty adrProp)
                        {
                            prop.Parameters.TimeZone = tzProp.Value;
                        }
                    }
                }
            }
            else // Group  is null
            {
                if (group.FirstOrDefault(static x => x is TimeZoneProperty)
                    is TimeZoneProperty tzProp)
                {
                    if (group.PrefOrNull(static x => x is AddressProperty)
                        is AddressProperty adrProp)
                    {
                        adrProp.Parameters.TimeZone = tzProp.Value;
                    }
                }
            }
        }
    }

    private void ConnectGeoCoordinatesWithAddresses()
    {
        if (GeoCoordinates is null || Addresses is null)
        {
            return;
        }

        IEnumerable<IGrouping<string?, VCardProperty>> groups =
            Addresses.Concat<VCardProperty?>(GeoCoordinates)
                     .GroupByVCardGroup();

        foreach (IGrouping<string?, VCardProperty> group in groups)
        {
            if (group.Key is not null)
            {
                if (group.FirstOrDefault(static x => x is GeoProperty)
                    is GeoProperty geoProp)
                {
                    foreach (VCardProperty prop in group)
                    {
                        if (prop is AddressProperty adrProp)
                        {
                            prop.Parameters.GeoPosition = geoProp.Value;
                        }
                    }
                }
            }
            else // Group  is null
            {
                if (group.FirstOrDefault(static x => x is GeoProperty)
                    is GeoProperty geoProp)
                {
                    if (group.PrefOrNull(static x => x is AddressProperty)
                        is AddressProperty adrProp)
                    {
                        adrProp.Parameters.GeoPosition = geoProp.Value;
                    }
                }
            }
        }
    }

    private void AddCopyToPhoneNumbers(TextProperty textProp, ParameterSection para)
    {
        if ((para.PhoneType.IsSet(Tel.Voice) || para.PhoneType.IsSet(Tel.Video)) &&
           (!Phones?.Any(x => x!.Value == textProp.Value) ?? true))
        {
            Phones = Concat(Phones, textProp);
        }
    }
}
