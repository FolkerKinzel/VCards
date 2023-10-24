using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Globalization;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>Initializes a new <see cref="VCard" /> object.</summary>
    public VCard() { }

    /// <summary>Copy ctor.</summary>
    /// <param name="vCard">The <see cref="VCard"/> instance to clone.</param>
    private VCard(VCard vCard)
    {
        Version = vCard.Version;

        Func<ICloneable?, object?> cloner = Cloned;

        foreach (KeyValuePair<VCdProp, object> kvp in vCard._propDic)
        {
            Set(kvp.Key, kvp.Value switch
            {
                XmlProperty xmlProp => xmlProp.Clone(),
                IEnumerable<XmlProperty?> xmlPropEnumerable => xmlPropEnumerable.Select(cloner).Cast<XmlProperty?>().ToArray(),
                ProfileProperty profProp => profProp.Clone(),
                TextProperty txtProp => txtProp.Clone(),
                IEnumerable<TextProperty?> txtPropEnumerable => txtPropEnumerable.Select(cloner).Cast<TextProperty?>().ToArray(),
                DateAndOrTimeProperty dtTimeProp => dtTimeProp.Clone(),
                IEnumerable<DateAndOrTimeProperty?> dtTimePropEnumerable => dtTimePropEnumerable.Select(cloner).Cast<DateAndOrTimeProperty?>().ToArray(),
                AddressProperty adrProp => adrProp.Clone(),
                IEnumerable<AddressProperty?> adrPropEnumerable => adrPropEnumerable.Select(cloner).Cast<AddressProperty?>().ToArray(),
                NameProperty nameProp => nameProp.Clone(),
                IEnumerable<NameProperty?> namePropEnumerable => namePropEnumerable.Select(cloner).Cast<NameProperty?>().ToArray(),
                RelationProperty relProp => relProp.Clone(),
                IEnumerable<RelationProperty?> relPropEnumerable => relPropEnumerable.Select(cloner).Cast<RelationProperty?>().ToArray(),
                OrganizationProperty orgProp => orgProp.Clone(),
                IEnumerable<OrganizationProperty?> orgPropEnumerable => orgPropEnumerable.Select(cloner).Cast<OrganizationProperty?>().ToArray(),
                StringCollectionProperty strCollProp => strCollProp.Clone(),
                IEnumerable<StringCollectionProperty?> strCollPropEnumerable => strCollPropEnumerable.Select(cloner).Cast<StringCollectionProperty?>().ToArray(),
                GenderProperty sexProp => sexProp.Clone(),
                IEnumerable<GenderProperty?> sexPropEnumerable => sexPropEnumerable.Select(cloner).Cast<GenderProperty?>().ToArray(),
                GeoProperty geoProp => geoProp.Clone(),
                IEnumerable<GeoProperty?> geoPropEnumerable => geoPropEnumerable.Select(cloner).Cast<GeoProperty?>().ToArray(),
                DataProperty dataProp => dataProp.Clone(),
                IEnumerable<DataProperty?> dataPropEnumerable => dataPropEnumerable.Select(cloner).Cast<DataProperty?>().ToArray(),
                NonStandardProperty nStdProp => nStdProp.Clone(),
                IEnumerable<NonStandardProperty?> nStdPropEnumerable => nStdPropEnumerable.Select(cloner).Cast<NonStandardProperty?>().ToArray(),
                PropertyIDMappingProperty pidMapProp => pidMapProp.Clone(),
                IEnumerable<PropertyIDMappingProperty?> pidMapPropEnumerable => pidMapPropEnumerable.Select(cloner).Cast<PropertyIDMappingProperty?>().ToArray(),
                TimeZoneProperty tzProp => tzProp.Clone(),
                IEnumerable<TimeZoneProperty?> tzPropEnumerable => tzPropEnumerable.Select(cloner).Cast<TimeZoneProperty?>().ToArray(),

                ICloneable cloneable => cloneable.Clone(), // AccessProperty, KindProperty, TimeStampProperty, UuidProperty
                _ => kvp.Value
            });
        }

        /////////////////////////////////////////////////
        static object? Cloned(ICloneable? x) => x?.Clone();
    }


    /// <summary>Initializes a <see cref="VCard" /> object from a queue of <see cref="VcfRow"
    /// /> objects.</summary>
    /// <param name="queue" />
    /// <param name="info" />
    /// <param name="versionHint" />
    private VCard(Queue<VcfRow> queue, VcfDeserializationInfo info, VCdVersion versionHint)
    {
        Debug.Assert(queue != null);
        Debug.Assert(info.Builder != null);
        Debug.Assert(queue.All(x => x != null));

        this.Version = versionHint;

        StringBuilder builder = info.Builder;

        int vcfRowsToParse = queue.Count;
        int vcfRowsParsed = 0;

        while (queue.Count != 0)
        {
            VcfRow? vcfRow = queue.Dequeue();

            switch (vcfRow.Key)
            {
                case PropKeys.VERSION:
                    this.Version = VCdVersionConverter.Parse(vcfRow.Value);
                    break;
                case PropKeys.KIND:
                    Kind = new KindProperty(vcfRow);
                    break;
                case PropKeys.TEL:
                    Phones = new TextProperty(vcfRow, this.Version).GetAssignment(Phones);
                    break;
                case PropKeys.EMAIL:
                    this.EMails = new TextProperty(vcfRow, this.Version).GetAssignment(EMails);
                    break;
                case PropKeys.N:  //LastName, FirstName, MiddleName, Prefix, Suffix
                    this.NameViews = new NameProperty(vcfRow, this.Version).GetAssignment(NameViews);
                    break;
                case PropKeys.FN:
                    DisplayNames = new TextProperty(vcfRow, this.Version).GetAssignment(DisplayNames);
                    break;
                case PropKeys.BDAY:
                    BirthDayViews = DateAndOrTimeProperty.Parse(vcfRow, this.Version).GetAssignment(BirthDayViews);
                    break;
                case PropKeys.ADR: // PostOfficeBox, ExtendedAddress, Street, Locality, Region, PostalCode, Country
                    Addresses = new AddressProperty(vcfRow, this.Version).GetAssignment(Addresses);
                    break;
                case PropKeys.LABEL:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else
                    {
                        AssignLabelToAddress(vcfRow);
                    }
                    break;
                case PropKeys.REV:
                    TimeStamp = new TimeStampProperty(vcfRow);
                    break;
                case PropKeys.CALURI:
                    CalendarAddresses = new TextProperty(vcfRow, this.Version).GetAssignment(CalendarAddresses);
                    break;
                case PropKeys.CALADRURI:
                    CalendarUserAddresses = new TextProperty(vcfRow, this.Version).GetAssignment(CalendarUserAddresses);
                    break;
                case PropKeys.FBURL:
                    FreeOrBusyUrls = new TextProperty(vcfRow, this.Version).GetAssignment(FreeOrBusyUrls);
                    break;
                case PropKeys.TITLE:
                    Titles = new TextProperty(vcfRow, this.Version).GetAssignment(Titles);
                    break;
                case PropKeys.ROLE:
                    Roles = new TextProperty(vcfRow, this.Version).GetAssignment(Roles);
                    break;
                case PropKeys.NOTE:
                    Notes = new TextProperty(vcfRow, this.Version).GetAssignment(Notes);
                    break;
                case PropKeys.URL:
                    URLs = new TextProperty(vcfRow, Version).GetAssignment(URLs);
                    break;
                case PropKeys.UID:
                    try
                    {
                        UniqueIdentifier = new UuidProperty(vcfRow);
                    }
                    catch { }
                    break;
                case PropKeys.ORG:
                    Organizations = new OrganizationProperty(vcfRow, this.Version).GetAssignment(Organizations);
                    break;
                case PropKeys.GEO:
                    GeoCoordinates = new GeoProperty(vcfRow).GetAssignment(GeoCoordinates);
                    break;
                case PropKeys.NICKNAME:
                    NickNames = new StringCollectionProperty(vcfRow, this.Version).GetAssignment(NickNames);
                    break;
                case PropKeys.CATEGORIES:
                    Categories = new StringCollectionProperty(vcfRow, this.Version).GetAssignment(Categories);
                    break;
                case PropKeys.SOUND:
                    Sounds = DataProperty.Parse(vcfRow, this.Version).GetAssignment(Sounds);
                    break;
                case PropKeys.PHOTO:
                    Photos = DataProperty.Parse(vcfRow, this.Version).GetAssignment(Photos);
                    break;
                case PropKeys.LOGO:
                    Logos = DataProperty.Parse(vcfRow, this.Version).GetAssignment(Logos);
                    break;
                case PropKeys.KEY:
                    Keys = DataProperty.Parse(vcfRow, this.Version).GetAssignment(Keys);
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
                            if (NameViews != null)
                            {
                                NameViews.First()!.Parameters.SortAs = new string?[] { textProp.Value };
                            }
                            else
                            {
                                var name = new NameProperty();
                                name.Parameters.SortAs = new string?[] { textProp.Value };
                                NameViews = name;
                            }
                        }
                    }
                    break;
                case PropKeys.SOURCE:
                    Sources = new TextProperty(vcfRow, this.Version).GetAssignment(Sources);
                    break;
                case PropKeys.ANNIVERSARY:
                    this.AnniversaryViews = DateAndOrTimeProperty.Parse(vcfRow, this.Version).GetAssignment(AnniversaryViews);
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
                        this.AnniversaryViews = DateAndOrTimeProperty.Parse(vcfRow, this.Version);
                    }

                    break;
                case PropKeys.GENDER:
                    this.GenderViews = new GenderProperty(vcfRow, this.Version).GetAssignment(GenderViews);
                    break;
                case PropKeys.NonStandard.X_GENDER:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (GenderViews is null && vcfRow.Value != null)
                    {
                        GenderViews = vcfRow.Value.StartsWith("F", true, CultureInfo.InvariantCulture)
                            ? new GenderProperty(Models.Enums.Gender.Female)
                            : new GenderProperty(Models.Enums.Gender.Male);
                    }
                    break;
                case PropKeys.NonStandard.X_WAB_GENDER:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else
                    {
                        GenderViews ??= vcfRow.Value.Contains('1', StringComparison.Ordinal)
                                            ? new GenderProperty(Models.Enums.Gender.Female)
                                            : new GenderProperty(Models.Enums.Gender.Male);
                    }
                    break;
                case PropKeys.IMPP:
                    InstantMessengerHandles = new TextProperty(vcfRow, this.Version).GetAssignment(InstantMessengerHandles);
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

                        if (textProp.Value != null && (InstantMessengerHandles?.All(x => x?.Value != textProp.Value) ?? true))
                        {
                            InstantMessengerHandles = textProp.GetAssignment(InstantMessengerHandles);

                            var para = textProp.Parameters;
                            XMessengerParameterConverter.ConvertToInstantMessengerType(para);
                            AddCopyToPhoneNumbers(textProp, para);
                        }
                    }

                    break;
                case PropKeys.LANG:
                    Languages = new TextProperty(vcfRow, this.Version).GetAssignment(Languages);
                    break;
                case PropKeys.MAILER:
                    Mailer = new TextProperty(vcfRow, this.Version);
                    break;
                case PropKeys.TZ:
                    TimeZones = new TimeZoneProperty(vcfRow, this.Version).GetAssignment(TimeZones);
                    break;
                case PropKeys.CLASS:
                    Access = new AccessProperty(vcfRow);
                    break;
                case PropKeys.MEMBER:
                    Members = RelationProperty.Parse(vcfRow, this.Version).GetAssignment(Members);
                    break;
                case PropKeys.RELATED:
                    Relations = RelationProperty.Parse(vcfRow, this.Version).GetAssignment(Relations);
                    break;
                case PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
                case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_SPOUSENAME:
                case PropKeys.NonStandard.X_SPOUSE:
                case PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (Relations?.All(x => x!.Parameters.Relation != RelationTypes.Spouse) ?? true)
                    {
                        vcfRow.Parameters.DataType = VCdDataType.Text; // führt dazu, dass eine RelationTextProperty erzeugt wird
                        vcfRow.Parameters.Relation = RelationTypes.Spouse;

                        Relations = RelationProperty.Parse(vcfRow, this.Version).GetAssignment(Relations);
                    }

                    break;
                case PropKeys.NonStandard.X_ASSISTANT:
                case PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:
                case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
                    if (vcfRowsParsed < vcfRowsToParse)
                    {
                        queue.Enqueue(vcfRow);
                    }
                    else if (Relations?.All(x => !x!.Parameters.Relation.IsSet(RelationTypes.Agent)) ?? true)
                    {
                        vcfRow.Parameters.DataType ??= VCdDataType.Text;
                        vcfRow.Parameters.Relation = RelationTypes.Agent;

                        Relations = RelationProperty.Parse(vcfRow, this.Version).GetAssignment(Relations);
                    }

                    break;
                case PropKeys.AGENT:
                    if (string.IsNullOrWhiteSpace(vcfRow.Value))
                    {
                        Relations = RelationProperty.FromText(null, RelationTypes.Agent, vcfRow.Group)
                                                    .GetAssignment(Relations);
                    }
                    else
                    {
                        if (vcfRow.Value.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                        {
                            var nested = VCard.ParseNestedVcard(vcfRow.Value, info, this.Version);
                            Relations = nested is null ? RelationProperty.FromText(vcfRow.Value, RelationTypes.Agent, vcfRow.Group)
                                                       // use the ctor directly because nested can't be a circular
                                                       // reference and therefore don't neeed to be cloned:
                                                       : new RelationVCardProperty(nested, RelationTypes.Agent, vcfRow.Group)
                                        .GetAssignment(Relations);
                        }
                        else
                        {
                            vcfRow.Parameters.DataType ??= VCdDataType.Text;
                            vcfRow.Parameters.Relation = RelationTypes.Agent;

                            Relations = RelationProperty.Parse(vcfRow, this.Version)
                                                        .GetAssignment(Relations);
                        }
                    }
                    break;
                case PropKeys.PROFILE:
                    this.Profile = new ProfileProperty(vcfRow, this.Version);
                    break;
                case PropKeys.XML:
                    XmlProperties = new XmlProperty(vcfRow).GetAssignment(XmlProperties);
                    break;
                case PropKeys.CLIENTPIDMAP:
                    PropertyIDMappingProperty prop;
                    try
                    {
                        prop = new PropertyIDMappingProperty(vcfRow);
                    }
                    catch
                    {
                        break;
                    }

                    PropertyIDMappings = prop.GetAssignment(PropertyIDMappings);

                    break;
                case PropKeys.PRODID:
                    ProdID = new TextProperty(vcfRow, this.Version);
                    break;
                case PropKeys.NAME:
                    this.DirectoryName = new TextProperty(vcfRow, this.Version);
                    break;

                // Erweiterungen:
                case PropKeys.NonStandard.DEATHDATE:
                    this.DeathDateViews =
                        DateAndOrTimeProperty.Parse(vcfRow, this.Version).GetAssignment(DeathDateViews);
                    break;
                case PropKeys.NonStandard.BIRTHPLACE:
                    this.BirthPlaceViews =
                        new TextProperty(vcfRow, this.Version).GetAssignment(BirthPlaceViews);
                    break;
                case PropKeys.NonStandard.DEATHPLACE:
                    this.DeathPlaceViews =
                        new TextProperty(vcfRow, this.Version).GetAssignment(DeathPlaceViews);
                    break;
                case PropKeys.NonStandard.EXPERTISE:
                    Expertises = new TextProperty(vcfRow, this.Version).GetAssignment(Expertises);
                    break;
                case PropKeys.NonStandard.INTEREST:
                    Interests = new TextProperty(vcfRow, this.Version).GetAssignment(Interests);
                    break;
                case PropKeys.NonStandard.HOBBY:
                    Hobbies = new TextProperty(vcfRow, this.Version).GetAssignment(Hobbies);
                    break;
                case PropKeys.NonStandard.ORG_DIRECTORY:
                    OrgDirectories = new TextProperty(vcfRow, this.Version).GetAssignment(OrgDirectories);
                    break;
                default:
                    NonStandard = new NonStandardProperty(vcfRow).GetAssignment(NonStandard);
                    break;
            };//switch

            vcfRowsParsed++;
        }//foreach

    }//ctor

    private void AddCopyToPhoneNumbers(TextProperty textProp, ParameterSection para)
    {
        if ((para.PhoneType.IsSet(PhoneTypes.Voice) ||
                                        para.PhoneType.IsSet(PhoneTypes.Video)) &&
                                        (!Phones?.Any(x => x?.Value == textProp.Value) ?? true))
        {
            Phones = textProp.GetAssignment(Phones);
        }
    }

    private void AssignLabelToAddress(VcfRow labelRow)
    {
        labelRow.UnMask(Version);

        IEnumerable<AddressProperty?>? addresses = Addresses;

        if (addresses is null)
        {
            Addresses = CreateEmptyAddressPropertyWithLabel(labelRow);
            return;
        }

        if (labelRow.Parameters.PropertyClass.HasValue && labelRow.Parameters.AddressType.HasValue)
        {
            AddressProperty? address = addresses
                .Where(x => x!.Parameters.Label is null &&
                            x.Parameters.PropertyClass.IsSet(labelRow.Parameters.PropertyClass.Value) &&
                            x.Parameters.AddressType.IsSet(labelRow.Parameters.AddressType.Value) &&
                            x.Parameters.Preference == labelRow.Parameters.Preference)
                .FirstOrDefault();

            if (address != null)
            {
                Assign(labelRow, address);
                return;
            }
        }
        else if (labelRow.Parameters.PropertyClass.HasValue)
        {
            AddressProperty? address = addresses
                .Where(x => x!.Parameters.Label is null &&
                            x.Parameters.PropertyClass.IsSet(labelRow.Parameters.PropertyClass.Value) &&
                            x.Parameters.Preference == labelRow.Parameters.Preference)
                .FirstOrDefault();

            if (address != null)
            {
                Assign(labelRow, address);
                return;
            }
        }
        else if (labelRow.Parameters.AddressType.HasValue)
        {
            AddressProperty? address = addresses
                .Where(x => x!.Parameters.Label is null &&
                            x.Parameters.AddressType.IsSet(labelRow.Parameters.AddressType.Value) &&
                            x.Parameters.Preference == labelRow.Parameters.Preference)
                .FirstOrDefault();

            if (address != null)
            {
                Assign(labelRow, address);
                return;
            }
        }

        Debug.Assert(addresses.Any());
        Debug.Assert(!addresses.Any(x => x is null));

        AddressProperty? addressWithEmptyLabel = addresses
            .Where(x => x!.Parameters.Label is null &&
                        x.Parameters.Preference == labelRow.Parameters.Preference)
            .FirstOrDefault();

        if (addressWithEmptyLabel is null)
        {
            Addresses = CreateEmptyAddressPropertyWithLabel(labelRow).GetAssignment(Addresses);
        }
        else
        {
            Assign(labelRow, addressWithEmptyLabel);
        }


        static AddressProperty CreateEmptyAddressPropertyWithLabel(VcfRow vcfRow)
        {
            var adrProp = new AddressProperty("", null, null, null, appendLabel: false);
            adrProp.Parameters.Assign(vcfRow.Parameters);
            adrProp.Parameters.Label = vcfRow.Value;
            return adrProp;
        }

        static void Assign(VcfRow labelRow, AddressProperty address)
        {
            address.Parameters.Label = labelRow.Value;
            if (address.Parameters.CharSet is null)
            {
                address.Parameters.CharSet = labelRow.Parameters.CharSet;
            }
        }
    }
}
