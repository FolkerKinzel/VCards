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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0008:Expliziten Typ verwenden", Justification = "<Ausstehend>")]
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
                        {
                            Kind = new KindProperty(vcfRow);
                            break;
                        }
                    case PropKeys.TEL:
                        {
                            var telephones = (List<TextProperty?>?)PhoneNumbers ?? new List<TextProperty?>();
                            PhoneNumbers = telephones;

                            telephones.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.EMAIL:
                        {
                            var emails = (List<TextProperty?>?)this.EmailAddresses ?? new List<TextProperty?>();
                            this.EmailAddresses = emails;

                            emails.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.N:  //LastName, FirstName, MiddleName, Prefix, Suffix
                        {
                            var names = (List<NameProperty?>?)this.NameViews ?? new List<NameProperty?>();
                            this.NameViews = names;

                            names.Add(new NameProperty(vcfRow, this.Version));
                            break;
                        }

                    case PropKeys.FN:
                        {
                            var displayNames = (List<TextProperty?>?)DisplayNames ?? new List<TextProperty?>();
                            DisplayNames = displayNames;

                            displayNames.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.BDAY:
                        {
                            var birthDays = (List<DateTimeProperty?>?)this.BirthDayViews ?? new List<DateTimeProperty?>();
                            BirthDayViews = birthDays;

                            birthDays.Add(DateTimeProperty.Create(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.ADR: // PoBox, ExtendedAddress, Street, Locality, Region, PostalCode, Country
                        {
                            var addresses = (List<AddressProperty?>?)this.Addresses ?? new List<AddressProperty?>();
                            Addresses = addresses;

                            addresses.Add(new AddressProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.LABEL:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else
                        {
                            vcfRow.DecodeQuotedPrintable();

                            if (Version != VCdVersion.V2_1)
                            {
                                vcfRow.UnMask(Version);
                            }

                            var addresses = (List<AddressProperty?>?)this.Addresses ?? new List<AddressProperty?>();
                            Addresses = addresses;

                            if (addresses.Count != 0)
                            {
                                addresses[0]!.Parameters.Label = vcfRow.Value;
                            }
                            else
                            {
                                addresses.Add(new AddressProperty());
                                addresses[0]!.Parameters.Label = vcfRow.Value;
                            }
                        }
                        break;
                    case PropKeys.REV:
                        TimeStamp = new TimeStampProperty(vcfRow);
                        break;
                    case PropKeys.CALURI:
                        {
                            var urls = (List<TextProperty?>?)CalendarAddresses ?? new List<TextProperty?>();
                            CalendarAddresses = urls;

                            urls.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.CALADRURI:
                        {
                            var urls = (List<TextProperty?>?)CalendarUserAddresses ?? new List<TextProperty?>();
                            CalendarUserAddresses = urls;

                            urls.Add(new TextProperty(vcfRow, this.Version));

                            break;
                        }
                    case PropKeys.TITLE:
                        {
                            var titles = (List<TextProperty?>?)Titles ?? new List<TextProperty?>();
                            Titles = titles;

                            titles.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.ROLE:
                        {
                            var roles = (List<TextProperty?>?)Roles ?? new List<TextProperty?>();
                            Roles = roles;

                            roles.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NOTE:
                        {
                            var notes = (List<TextProperty?>?)Notes ?? new List<TextProperty?>();
                            Notes = notes;

                            notes.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.URL:
                        {
                            var urls = (List<TextProperty?>?)URLs ?? new List<TextProperty?>();
                            URLs = urls;

                            urls.Add(new TextProperty(vcfRow, Version));
                            break;
                        }
                    case PropKeys.UID:
                        try
                        {
                            UniqueIdentifier = new UuidProperty(vcfRow);
                        }
                        catch { }
                        break;
                    case PropKeys.ORG:
                        {
                            var org = (List<OrganizationProperty?>?)Organizations ?? new List<OrganizationProperty?>();
                            Organizations = org;

                            org.Add(new OrganizationProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.GEO:
                        {
                            var geo = (List<GeoProperty?>?)GeoCoordinates ?? new List<GeoProperty?>();
                            GeoCoordinates = geo;

                            geo.Add(new GeoProperty(vcfRow));
                            break;
                        }

                    case PropKeys.NICKNAME:
                        {
                            var nickNames = (List<StringCollectionProperty?>?)NickNames ?? new List<StringCollectionProperty?>();
                            NickNames = nickNames;

                            nickNames.Add(new StringCollectionProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.CATEGORIES:
                        {
                            var categories = (List<StringCollectionProperty?>?)Categories ?? new List<StringCollectionProperty?>();
                            Categories = categories;

                            categories.Add(new StringCollectionProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.SOUND:
                        {
                            var sounds = (List<DataProperty?>?)Sounds ?? new List<DataProperty?>();
                            Sounds = sounds;

                            sounds.Add(new DataProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.PHOTO:
                        {
                            var photos = (List<DataProperty?>?)Photos ?? new List<DataProperty?>();
                            Photos = photos;

                            photos.Add(new DataProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.LOGO:
                        {
                            var logos = (List<DataProperty?>?)Logos ?? new List<DataProperty?>();
                            Logos = logos;

                            logos.Add(new DataProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.KEY:
                        {
                            var keys = (List<DataProperty?>?)Keys ?? new List<DataProperty?>();
                            Keys = keys;

                            keys.Add(new DataProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.SORT_STRING: // nur vCard 3.0: dort keine zusammengesetzte Property
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
                                    var names = new List<NameProperty>();
                                    this.NameViews = names;

                                    var name = new NameProperty();
                                    name.Parameters.SortAs = new string?[] { textProp.Value };
                                    names.Add(name);
                                }
                            }
                        }
                        break;

                    case PropKeys.SOURCE:
                        {
                            var sources = (List<TextProperty?>?)Sources ?? new List<TextProperty?>();
                            Sources = sources;

                            sources.Add(new TextProperty(vcfRow, this.Version));

                            break;
                        }

                    case PropKeys.ANNIVERSARY:
                        {
                            var anniversaries = (List<DateTimeProperty?>?)this.AnniversaryViews ?? new List<DateTimeProperty?>();
                            this.AnniversaryViews = anniversaries;

                            anniversaries.Add(DateTimeProperty.Create(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.X_ANNIVERSARY:
                    case PropKeys.NonStandard.Evolution.X_EVOLUTION_ANNIVERSARY:
                    case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ANNIVERSARY:
                    case PropKeys.NonStandard.X_WAB_WEDDING_ANNIVERSARY:
                        {
                            if (vcfRowsParsed < vcfRowsToParse)
                            {
                                queue.Enqueue(vcfRow);
                            }
                            else if (AnniversaryViews is null)
                            {
                                var anniversaries = new List<DateTimeProperty?>();
                                this.AnniversaryViews = anniversaries;

                                anniversaries.Add(DateTimeProperty.Create(vcfRow, this.Version));
                            }

                            break;
                        }

                    case PropKeys.GENDER:
                        {
                            var genders = (List<GenderProperty?>?)this.GenderViews ?? new List<GenderProperty?>();
                            this.GenderViews = genders;

                            genders.Add(new GenderProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.X_GENDER:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (GenderViews is null && vcfRow.Value != null)
                        {
                            var genders = new List<GenderProperty?>();
                            this.GenderViews = genders;

                            if (vcfRow.Value.StartsWith("F", true, CultureInfo.InvariantCulture))
                            {
                                genders.Add(new GenderProperty(VCdSex.Female));
                            }
                            else
                            {
                                genders.Add(new GenderProperty(VCdSex.Male));
                            }
                        }
                        break;
                    case PropKeys.NonStandard.X_WAB_GENDER:
                        if (vcfRowsParsed < vcfRowsToParse)
                        {
                            queue.Enqueue(vcfRow);
                        }
                        else if (GenderViews is null && vcfRow.Value != null)
                        {
                            var genders = new List<GenderProperty?>();
                            this.GenderViews = genders;

                            if (
#if NET40
                                vcfRow.Value.Contains("1"))
#else
                                vcfRow.Value.Contains('1', StringComparison.Ordinal))
#endif
                            {
                                genders.Add(new GenderProperty(VCdSex.Female));
                            }
                            else
                            {
                                genders.Add(new GenderProperty(VCdSex.Male));
                            }
                        }
                        break;
                    case PropKeys.IMPP:
                        {
                            var impps = (List<TextProperty?>?)InstantMessengerHandles ?? new List<TextProperty?>();
                            InstantMessengerHandles = impps;

                            impps.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
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
                        {
                            if (vcfRowsParsed < vcfRowsToParse)
                            {
                                queue.Enqueue(vcfRow);
                            }
                            else
                            {
                                var textProp = new TextProperty(vcfRow, this.Version);

                                if (InstantMessengerHandles?.All(x => x!.Value != textProp.Value) ?? true)
                                {
                                    var impps = (List<TextProperty?>?)InstantMessengerHandles ?? new List<TextProperty?>();
                                    InstantMessengerHandles = impps;

                                    impps.Add(textProp);

                                    textProp.Parameters.Assign(vcfRow.Parameters); // die meisten diese Properties enthalten
                                                                                   // Telefon-Type-Informationen.
                                }
                            }

                            break;
                        }
                    case PropKeys.LANG:
                        {
                            var languages = (List<TextProperty?>?)Languages ?? new List<TextProperty?>();
                            Languages = languages;

                            languages.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.MAILER:
                        Mailer = new TextProperty(vcfRow, this.Version);
                        break;
                    case PropKeys.TZ:
                        {
                            var tz = (List<TimeZoneProperty?>?)TimeZones ?? new List<TimeZoneProperty?>();
                            TimeZones = tz;

                            tz.Add(new TimeZoneProperty(vcfRow));
                            break;
                        }
                    case PropKeys.CLASS:
                        Access = new AccessProperty(vcfRow);
                        break;


                    case PropKeys.MEMBER:
                        {
                            var relations = (List<RelationProperty?>?)Members ?? new List<RelationProperty?>();
                            Members = relations;

                            relations.Add(RelationProperty.Parse(vcfRow, this.Version));
                            break;
                        }


                    case PropKeys.RELATED:
                        {
                            var relations = (List<RelationProperty?>?)Relations ?? new List<RelationProperty?>();
                            Relations = relations;

                            relations.Add(RelationProperty.Parse(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.Evolution.X_EVOLUTION_SPOUSE:
                    case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_SPOUSENAME:
                    case PropKeys.NonStandard.X_SPOUSE:
                    case PropKeys.NonStandard.X_WAB_SPOUSE_NAME:
                        {
                            if (vcfRowsParsed < vcfRowsToParse)
                            {
                                queue.Enqueue(vcfRow);
                            }
                            else if (Relations?.All(x => x!.Parameters.RelationType != RelationTypes.Spouse) ?? true)
                            {
                                var relations = (List<RelationProperty?>?)Relations ?? new List<RelationProperty?>();
                                Relations = relations;

                                vcfRow.DecodeQuotedPrintable(); // RelationProperty gehört zu ´vCard 4.0
                                vcfRow.Parameters.DataType = VCdDataType.Text; // führt dazu, dass eine RelationTextProperty erzeugt wird
                                vcfRow.Parameters.RelationType = RelationTypes.Spouse;
                                relations.Add(RelationProperty.Parse(vcfRow, this.Version));
                            }

                            break;
                        }
                    case PropKeys.NonStandard.X_ASSISTANT:
                    case PropKeys.NonStandard.Evolution.X_EVOLUTION_ASSISTANT:
                    case PropKeys.NonStandard.KAddressbook.X_KADDRESSBOOK_X_ASSISTANTSNAME:
                        {
                            if (vcfRowsParsed < vcfRowsToParse)
                            {
                                queue.Enqueue(vcfRow);
                            }
                            else if (Relations?.All(x => !x!.Parameters.RelationType.IsSet(RelationTypes.Agent)) ?? true)
                            {
                                var relations = (List<RelationProperty?>?)Relations ?? new List<RelationProperty?>();
                                Relations = relations;

                                vcfRow.DecodeQuotedPrintable();
                                vcfRow.Parameters.DataType ??= VCdDataType.Text;
                                vcfRow.Parameters.RelationType = RelationTypes.Agent;
                                relations.Add(RelationProperty.Parse(vcfRow, this.Version));
                            }

                            break;
                        }
                    case PropKeys.AGENT:
                        {
                            var relations = (List<RelationProperty?>?)Relations ?? new List<RelationProperty?>();
                            Relations = relations;

                            if (vcfRow.Value is null)
                            {
                                relations.Add(new RelationTextProperty(
                                    null, RelationTypes.Agent, vcfRow.Group));
                            }
                            else
                            {
                                if (vcfRow.Value.StartsWith("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
                                {
                                    relations.Add(new RelationVCardProperty(
                                        VCard.ParseNestedVcard(vcfRow.Value, info, this.Version),
                                        RelationTypes.Agent,
                                        vcfRow.Group));
                                }
                                else
                                {
                                    vcfRow.DecodeQuotedPrintable();
                                    vcfRow.Parameters.DataType ??= VCdDataType.Text;
                                    vcfRow.Parameters.RelationType = RelationTypes.Agent;
                                    relations.Add(RelationProperty.Parse(vcfRow, this.Version));
                                }
                            }

                            break;
                        }

                    case PropKeys.PROFILE:
                        this.Profile = new ProfileProperty(vcfRow, this.Version);
                        break;

                    case PropKeys.XML:
                        {
                            var xmlProps = (List<XmlProperty?>?)XmlProperties ?? new List<XmlProperty?>();
                            XmlProperties = xmlProps;

                            xmlProps.Add(new XmlProperty(vcfRow));
                            break;
                        }

                    case PropKeys.CLIENTPIDMAP:
                        {
                            var pidMappings = (List<PropertyIDMappingProperty?>?)PropertyIDMappings ?? new List<PropertyIDMappingProperty?>();
                            PropertyIDMappings = pidMappings;

                            try
                            {
                                pidMappings.Add(new PropertyIDMappingProperty(vcfRow));
                            }
                            catch { }

                            break;

                        }

                    case PropKeys.PRODID:
                        ProdID = new TextProperty(vcfRow, this.Version);
                        break;

                    case PropKeys.NAME:
                        {
                            this.DirectoryName = new TextProperty(vcfRow, this.Version);
                            break;
                        }

                    // Erweiterungen:
                    case PropKeys.NonStandard.DEATHDATE:
                        {
                            var deathDates = (List<DateTimeProperty?>?)this.DeathDateViews ?? new List<DateTimeProperty?>();
                            this.DeathDateViews = deathDates;

                            deathDates.Add(DateTimeProperty.Create(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.BIRTHPLACE:
                        {
                            var birthPlaces = (List<TextProperty?>?)this.BirthPlaceViews ?? new List<TextProperty?>();
                            this.BirthPlaceViews = birthPlaces;

                            birthPlaces.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.DEATHPLACE:
                        {
                            var deathPlaces = (List<TextProperty?>?)this.DeathPlaceViews ?? new List<TextProperty?>();
                            this.DeathPlaceViews = deathPlaces;

                            deathPlaces.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.EXPERTISE:
                        {
                            var expertises = (List<TextProperty?>?)Expertises ?? new List<TextProperty?>();
                            Expertises = expertises;

                            expertises.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.INTEREST:
                        {
                            var interests = (List<TextProperty?>?)Interests ?? new List<TextProperty?>();
                            Interests = interests;

                            interests.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.HOBBY:
                        {
                            var hobbies = (List<TextProperty?>?)Hobbies ?? new List<TextProperty?>();
                            Hobbies = hobbies;

                            hobbies.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    case PropKeys.NonStandard.ORG_DIRECTORY:
                        {
                            var orgDirectories = (List<TextProperty?>?)OrgDirectories ?? new List<TextProperty?>();
                            OrgDirectories = orgDirectories;

                            orgDirectories.Add(new TextProperty(vcfRow, this.Version));
                            break;
                        }
                    default:
                        {
                            var extensions = (List<NonStandardProperty?>?)NonStandardProperties ?? new List<NonStandardProperty?>();
                            NonStandardProperties = extensions;

                            extensions.Add(new NonStandardProperty(vcfRow));
                            break;
                        }
                };//switch

                vcfRowsParsed++;
            }//foreach


        }//ctor


    }
}
