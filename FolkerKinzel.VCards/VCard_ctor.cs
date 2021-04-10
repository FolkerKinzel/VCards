using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="VCard"/>-Objekt.
        /// </summary>
        public VCard() { }



        /// <summary>
        /// Initialisiert ein <see cref="VCard"/>-Objekt aus einer Queue von <see cref="VcfRow"/>-Objekten.
        /// </summary>
        /// <param name="queue">Eine Queue von <see cref="VcfRow"/>-Objekten, deren Value-Property
        /// den rohen Textinhalt der vCard-Zeile darstellt.</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>-Objekt, das Daten für den Deserialisierungsvorgang zur Verfügung stellt.</param>
        /// <param name="versionHint">Ein Hinweis, welche vCard-Version angenommen wird. (Eingebettete
        /// vCards haben manchmal keinen "VERSION:"-Tag.)</param>
        private VCard(Queue<VcfRow> queue, VCardDeserializationInfo info, VCdVersion versionHint)
        {
            Debug.Assert(queue != null);
            Debug.Assert(info.Builder != null);
            Debug.Assert(queue.All(x => x != null));
            Debug.Assert(queue.All(x => x.Value == null || !string.IsNullOrWhiteSpace(x.Value)));

            this.Version = versionHint;

            StringBuilder builder = info.Builder;

            int vcfRowsToParse = queue.Count;
            int vcfRowsParsed = 0;

            while (queue.Count != 0)
            {
                // vcfRow.Value ist entweder null oder enthält verwertbare Daten
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
                        PhoneNumbers = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), PhoneNumbers);
                        break;
                    case PropKeys.EMAIL:
                        this.EmailAddresses = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), EmailAddresses);
                        break;
                    case PropKeys.N:  //LastName, FirstName, MiddleName, Prefix, Suffix
                        this.NameViews = Assigner.GetAssignment(new NameProperty(vcfRow, this.Version), NameViews);
                        break;
                    case PropKeys.FN:
                        DisplayNames = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), DisplayNames);
                        break;
                    case PropKeys.BDAY:
                        BirthDayViews = Assigner.GetAssignment(DateTimeProperty.Create(vcfRow, this.Version), BirthDayViews);
                        break;
                    case PropKeys.ADR: // PostOfficeBox, ExtendedAddress, Street, Locality, Region, PostalCode, Country
                        Addresses = Assigner.GetAssignment(new AddressProperty(vcfRow, this.Version), Addresses);
                        break;
                    case PropKeys.LABEL:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else
                        {
                            vcfRow.UnMask(Version);

                            IEnumerable<AddressProperty?>? addresses = Addresses;

                            if (addresses is null)
                            {
                                var adrProp = new AddressProperty();
                                adrProp.Parameters.Label = vcfRow.Value;
                                adrProp.Parameters.Assign(vcfRow.Parameters);
                                Addresses = adrProp;
                            }
                            else
                            {
                                if (vcfRow.Parameters.PropertyClass.HasValue && vcfRow.Parameters.AddressType.HasValue)
                                {
                                    AddressProperty? address = addresses.OrderBy(x => x!.Parameters.Preference)
                                        .FirstOrDefault(
                                        x => x!.Parameters.PropertyClass.IsSet(vcfRow.Parameters.PropertyClass.Value) &&
                                             x.Parameters.AddressType.IsSet(vcfRow.Parameters.AddressType.Value));

                                    if (address != null)
                                    {
                                        address.Parameters.Label = vcfRow.Value;
                                        break;
                                    }
                                }
                                else if (vcfRow.Parameters.PropertyClass.HasValue)
                                {
                                    AddressProperty? address = addresses.OrderBy(x => x!.Parameters.Preference)
                                        .FirstOrDefault(
                                        x => x!.Parameters.PropertyClass.IsSet(vcfRow.Parameters.PropertyClass.Value));

                                    if (address != null)
                                    {
                                        address.Parameters.Label = vcfRow.Value;
                                        break;
                                    }
                                }
                                else if (vcfRow.Parameters.AddressType.HasValue)
                                {
                                    AddressProperty? address = addresses.OrderBy(x => x!.Parameters.Preference)
                                        .FirstOrDefault(
                                        x => x!.Parameters.AddressType.IsSet(vcfRow.Parameters.AddressType.Value));

                                    if (address != null)
                                    {
                                        address.Parameters.Label = vcfRow.Value;
                                        break;
                                    }
                                }

                                Debug.Assert(addresses.Any());
                                Debug.Assert(!addresses.Any(x => x is null));

                                AddressProperty first = addresses.OrderBy(x => x!.Parameters.Preference).First()!;
                                first.Parameters.Label = vcfRow.Value;
                            }//else
                        }
                        break;
                    case PropKeys.REV:
                        TimeStamp = new TimeStampProperty(vcfRow);
                        break;
                    case PropKeys.CALURI:
                        CalendarAddresses = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), CalendarAddresses);
                        break;
                    case PropKeys.CALADRURI:
                        CalendarUserAddresses = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), CalendarUserAddresses);
                        break;
                    case PropKeys.FBURL:
                        FreeBusyUrls = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), FreeBusyUrls);
                        break;
                    case PropKeys.TITLE:
                        Titles = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Titles);
                        break;
                    case PropKeys.ROLE:
                        Roles = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Roles);
                        break;
                    case PropKeys.NOTE:
                        Notes = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Notes);
                        break;
                    case PropKeys.URL:
                        URLs = Assigner.GetAssignment(new TextProperty(vcfRow, Version), URLs);
                        break;
                    case PropKeys.UID:
                        try
                        {
                            UniqueIdentifier = new UuidProperty(vcfRow);
                        }
                        catch { }
                        break;
                    case PropKeys.ORG:
                        Organizations = Assigner.GetAssignment(new OrganizationProperty(vcfRow, this.Version), Organizations);
                        break;
                    case PropKeys.GEO:
                        GeoCoordinates = Assigner.GetAssignment(new GeoProperty(vcfRow), GeoCoordinates);
                        break;
                    case PropKeys.NICKNAME:
                        NickNames = Assigner.GetAssignment(new StringCollectionProperty(vcfRow, this.Version), NickNames);
                        break;
                    case PropKeys.CATEGORIES:
                        Categories = Assigner.GetAssignment(new StringCollectionProperty(vcfRow, this.Version), Categories);
                        break;
                    case PropKeys.SOUND:
                        Sounds = Assigner.GetAssignment(new DataProperty(vcfRow, this.Version), Sounds);
                        break;
                    case PropKeys.PHOTO:
                        Photos = Assigner.GetAssignment(new DataProperty(vcfRow, this.Version), Photos);
                        break;
                    case PropKeys.LOGO:
                        Logos = Assigner.GetAssignment(new DataProperty(vcfRow, this.Version), Logos);
                        break;
                    case PropKeys.KEY:
                        Keys = Assigner.GetAssignment(new DataProperty(vcfRow, this.Version), Keys);
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
                        Sources = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Sources);
                        break;
                    case PropKeys.ANNIVERSARY:
                        this.AnniversaryViews = Assigner.GetAssignment(DateTimeProperty.Create(vcfRow, this.Version), AnniversaryViews);
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
                            this.AnniversaryViews = DateTimeProperty.Create(vcfRow, this.Version);
                        }

                        break;
                    case PropKeys.GENDER:
                        this.GenderViews = Assigner.GetAssignment(new GenderProperty(vcfRow, this.Version), GenderViews);
                        break;
                    case PropKeys.NonStandard.X_GENDER:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (GenderViews is null && vcfRow.Value != null)
                        {
                            GenderViews = vcfRow.Value.StartsWith("F", true, CultureInfo.InvariantCulture)
                                ? new GenderProperty(VCdSex.Female)
                                : new GenderProperty(VCdSex.Male);
                        }
                        break;
                    case PropKeys.NonStandard.X_WAB_GENDER:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (GenderViews is null && vcfRow.Value != null)
                        {
                            if (
#if NET40
                                vcfRow.Value.Contains("1"))
#else
                                vcfRow.Value.Contains('1', StringComparison.Ordinal))
#endif
                            {
                                GenderViews = new GenderProperty(VCdSex.Female);
                            }
                            else
                            {
                                GenderViews = new GenderProperty(VCdSex.Male);
                            }
                        }
                        break;
                    case PropKeys.IMPP:
                        InstantMessengerHandles = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), InstantMessengerHandles);
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
                                InstantMessengerHandles = Assigner.GetAssignment(textProp, InstantMessengerHandles);

                                textProp.Parameters.Assign(vcfRow.Parameters); // die meisten diese Properties enthalten
                                                                               // Telefon-Type-Informationen.

                                if ((textProp.Parameters.TelephoneType.IsSet(TelTypes.Voice) ||
                                    textProp.Parameters.TelephoneType.IsSet(TelTypes.Video)) &&
                                    (!PhoneNumbers?.Any(x => x?.Value == textProp.Value) ?? true))
                                {
                                    PhoneNumbers = Assigner.GetAssignment(textProp, PhoneNumbers);
                                }
                            }
                        }

                        break;
                    case PropKeys.LANG:
                        Languages = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Languages);
                        break;
                    case PropKeys.MAILER:
                        Mailer = new TextProperty(vcfRow, this.Version);
                        break;
                    case PropKeys.TZ:
                        TimeZones = Assigner.GetAssignment(new TimeZoneProperty(vcfRow), TimeZones);
                        break;
                    case PropKeys.CLASS:
                        Access = new AccessProperty(vcfRow);
                        break;
                    case PropKeys.MEMBER:
                        Members = Assigner.GetAssignment(RelationProperty.Parse(vcfRow, this.Version), Members);
                        break;
                    case PropKeys.RELATED:
                        Relations = Assigner.GetAssignment(RelationProperty.Parse(vcfRow, this.Version), Relations);
                        break;
                    case PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
                    case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_SPOUSENAME:
                    case PropKeys.NonStandard.X_SPOUSE:
                    case PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (Relations?.All(x => x!.Parameters.RelationType != RelationTypes.Spouse) ?? true)
                        {
                            vcfRow.Parameters.DataType = VCdDataType.Text; // führt dazu, dass eine RelationTextProperty erzeugt wird
                            vcfRow.Parameters.RelationType = RelationTypes.Spouse;

                            Relations = Assigner.GetAssignment(RelationProperty.Parse(vcfRow, this.Version), Relations);
                        }

                        break;
                    case PropKeys.NonStandard.X_ASSISTANT:
                    case PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:
                    case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (Relations?.All(x => !x!.Parameters.RelationType.IsSet(RelationTypes.Agent)) ?? true)
                        {
                            vcfRow.Parameters.DataType ??= VCdDataType.Text;
                            vcfRow.Parameters.RelationType = RelationTypes.Agent;

                            Relations = Assigner.GetAssignment(RelationProperty.Parse(vcfRow, this.Version), Relations);
                        }

                        break;
                    case PropKeys.AGENT:
                        if (vcfRow.Value is null)
                        {
                            Relations = Assigner.GetAssignment
                                (
                                    new RelationTextProperty(null, RelationTypes.Agent, vcfRow.Group),
                                    Relations
                                );
                        }
                        else
                        {
                            if (vcfRow.Value.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                            {
                                Relations = Assigner.GetAssignment
                                    (
                                        new RelationVCardProperty(VCard.ParseNestedVcard(vcfRow.Value, info, this.Version),
                                                                  RelationTypes.Agent,
                                                                  vcfRow.Group),
                                        Relations
                                    );
                            }
                            else
                            {
                                vcfRow.Parameters.DataType ??= VCdDataType.Text;
                                vcfRow.Parameters.RelationType = RelationTypes.Agent;

                                Relations = Assigner.GetAssignment
                                    (
                                        RelationProperty.Parse(vcfRow, this.Version),
                                        Relations
                                    );
                            }
                        }
                        break;
                    case PropKeys.PROFILE:
                        this.Profile = new ProfileProperty(vcfRow, this.Version);
                        break;
                    case PropKeys.XML:
                        XmlProperties = Assigner.GetAssignment(new XmlProperty(vcfRow), XmlProperties);
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

                        PropertyIDMappings = Assigner.GetAssignment(prop, PropertyIDMappings);

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
                            Assigner.GetAssignment(DateTimeProperty.Create(vcfRow, this.Version), DeathDateViews);
                        break;
                    case PropKeys.NonStandard.BIRTHPLACE:
                        this.BirthPlaceViews =
                            Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), BirthPlaceViews);
                        break;
                    case PropKeys.NonStandard.DEATHPLACE:
                        this.DeathPlaceViews =
                            Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), DeathPlaceViews);
                        break;
                    case PropKeys.NonStandard.EXPERTISE:
                        Expertises =
                            Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Expertises);
                        break;
                    case PropKeys.NonStandard.INTEREST:
                        Interests =
                            Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Interests);
                        break;
                    case PropKeys.NonStandard.HOBBY:
                        Hobbies = Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), Hobbies);
                        break;
                    case PropKeys.NonStandard.ORG_DIRECTORY:
                        OrgDirectories =
                            Assigner.GetAssignment(new TextProperty(vcfRow, this.Version), OrgDirectories);
                        break;
                    default:
                        NonStandardProperties =
                            Assigner.GetAssignment(new NonStandardProperty(vcfRow), NonStandardProperties);
                        break;
                };//switch

                vcfRowsParsed++;
            }//foreach

        }//ctor

    }
}
