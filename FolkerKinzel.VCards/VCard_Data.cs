using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FolkerKinzel.VCards
{
    /// <summary>
    /// Kapselt die in einer vCard enthaltenen Informationen.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
#if NET40
        internal static string[] EmptyStringArray = new string[0];
#endif

        private static readonly Regex VCardBegin =
            new Regex(@"\ABEGIN[ \t]*:[ \t]*VCARD[ \t]*\z",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex VCardEnd =
            new Regex(@"\AEND[ \t]*:[ \t]*VCARD[ \t]*\z",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);


        private readonly Dictionary<VCdProp, object> _propDic = new Dictionary<VCdProp, object>();

        [return: MaybeNull]
        private T Get<T>(VCdProp prop)
        {
            return _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;
        }


        private void Set(VCdProp prop, object? value)
        {
            if (value is null)
            {
                _propDic.Remove(prop);
            }
            else
            {
                _propDic[prop] = value;
            }
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die <see cref="VCard"/> durchläuft.
        /// </summary>
        /// <returns>Ein Enumerator für die <see cref="VCard"/>.</returns>
        public IEnumerator<KeyValuePair<VCdProp, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();
        }


        /// <summary>
        /// VERSION Version der vCard-Spezifikation. (2,3,4)
        /// </summary>
        public VCdVersion Version { get; private set; }


        /// <summary>
        /// CLASS Sensibilität der in der vCard enthaltenen Daten. (3)
        /// </summary>
        public AccessProperty? Access
        {
            get
            {
                return Get<AccessProperty?>(VCdProp.Access);
            }
            set
            {
                Set(VCdProp.Access, value);
            }
        }


        /// <summary>
        /// ADR Adressen (2,3,4)
        /// </summary>
        public IEnumerable<AddressProperty?>? Addresses
        {
            get
            {
                return Get<IList<AddressProperty?>?>(VCdProp.Addresses);
            }
            set
            {
                Set(VCdProp.Addresses, value);
            }
        }


        /// <summary>
        /// ANNIVERSARY Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person. (4)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? AnniversaryViews
        {
            get
            {
                return Get<IEnumerable<DateTimeProperty?>?>(VCdProp.AnniversaryViews);
            }
            set
            {
                Set(VCdProp.AnniversaryViews, value);
            }
        }


        /// <summary>
        /// BDAY Geburtstag (2,3,4)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? BirthDayViews
        {
            get
            {
                return Get<IEnumerable<DateTimeProperty?>?>(VCdProp.BirthDayViews);
            }
            set
            {
                Set(VCdProp.BirthDayViews, value);
            }
        }

        /// <summary>
        /// BIRTHPLACE Geburtsort (Erweiterung RFC 6474)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<TextProperty?>? BirthPlaceViews
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.BirthPlaceViews);
            }
            set
            {
                Set(VCdProp.BirthPlaceViews, value);
            }
        }



        /// <summary>
        /// CALURI URLs zum Kalender der Person. (4)
        /// </summary>
        public IEnumerable<TextProperty?>? CalendarAddresses
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.CalendarAddresses);
            }
            set
            {
                Set(VCdProp.CalendarAddresses, value);
            }
        }



        /// <summary>
        /// CALADRURI URLs für das Senden einer Terminanforderung an die Person oder Organisation. (4)
        /// </summary>
        public IEnumerable<TextProperty?>? CalendarUserAddresses
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.CalendarUserAddresses);
            }
            set
            {
                Set(VCdProp.CalendarUserAddresses, value);
            }
        }


        /// <summary>
        /// CATEGORIES Liste(n) von Eigenschaften, die das Objekt der vCard beschreiben. (3, 4)
        /// </summary>
        public IEnumerable<StringCollectionProperty?>? Categories
        {
            get
            {
                return Get<IEnumerable<StringCollectionProperty?>?>(VCdProp.Categories);
            }
            set
            {
                Set(VCdProp.Categories, value);
            }
        }


        /// <summary>
        /// DEATHDATE Todestag (Erweiterung RFC 6474)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? DeathDateViews
        {
            get
            {
                return Get<IEnumerable<DateTimeProperty?>?>(VCdProp.DeathDateViews);
            }
            set
            {
                Set(VCdProp.DeathDateViews, value);
            }
        }


        /// <summary>
        /// DEATHPLACE Sterbeort (Erweiterung RFC 6474)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<TextProperty?>? DeathPlaceViews
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.DeathPlaceViews);
            }
            set
            {
                Set(VCdProp.DeathPlaceViews, value);
            }
        }


        /// <summary>
        /// NAME
        /// Anzeigbarer Name der SOURCE-Eigenschaft (3)
        /// </summary>
        public TextProperty? DirectoryName
        {
            get
            {
                return Get<TextProperty?>(VCdProp.DirectoryName);
            }
            set
            {
                Set(VCdProp.DirectoryName, value);
            }
        }




        /// <summary>
        /// FN Formatierte Zeichenfolge mit dem/den vollständigen Namen des vCard-Objekts. (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? DisplayNames
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.DisplayNames);
            }
            set
            {
                Set(VCdProp.DisplayNames, value);
            }
        }



        /// <summary>
        /// EMAIL E-Mail-Adressen  (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? EmailAddresses
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.EmailAddresses);
            }
            set
            {
                Set(VCdProp.EmailAddresses, value);
            }
        }


        /// <summary>
        /// EXPERTISE Fachgebiete, über die die Person Kenntnisse hat (Erweiterung RFC 6715)
        /// </summary>
        public IEnumerable<TextProperty?>? Expertises
        {
            get
            {
                return Get<IEnumerable<TextProperty>>(VCdProp.Expertises);
            }
            set
            {
                Set(VCdProp.Expertises, value);
            }
        }

        /// <summary>
        /// GENDER Geschlecht (4)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<GenderProperty?>? GenderViews
        {
            get
            {
                return Get<IEnumerable<GenderProperty?>?>(VCdProp.GenderViews);
            }
            set
            {
                Set(VCdProp.GenderViews, value);
            }
        }


        /// <summary>
        /// GEO Längen- und Breitengrad(e). (2,3,4)
        /// </summary>
        public IEnumerable<GeoProperty?>? GeoCoordinates
        {
            get
            {
                return Get<IEnumerable<GeoProperty?>?>(VCdProp.GeoCoordinates);
            }
            set
            {
                Set(VCdProp.GeoCoordinates, value);
            }
        }



        /// <summary>
        /// HOBBY Freizeitbeschäftigungen, denen die Person nachgeht (Erweiterung RFC 6715)
        /// </summary>
        public IEnumerable<TextProperty?>? Hobbies
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Hobbies);
            }
            set
            {
                Set(VCdProp.Hobbies, value);
            }
        }


        /// <summary>
        /// IMPP Liste von Instant-Messenger-Handles. (3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? InstantMessengerHandles
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.InstantMessengerHandles);
            }
            set
            {
                Set(VCdProp.InstantMessengerHandles, value);
            }
        }


        /// <summary>
        /// INTEREST Freizeitbeschäftigungen, für die sich die Person interessiert, an denen sie 
        /// aber nicht zwangsläufig teilnimmt. (Erweiterung RFC 6715)
        /// </summary>
        public IEnumerable<TextProperty?>? Interests
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Interests);
            }
            set
            {
                Set(VCdProp.Interests, value);
            }
        }


        /// <summary>
        /// KEY Öffentliche Schlüssel, die dem vCard-Objekt zugeordnet sind. (2,3,4)
        /// </summary>
        /// <remarks>
        /// Es kann zu einer externen URL verwiesen werden, 
        /// Klartext angegeben oder ein Base64-kodierter Textblock in die vCard eingebettet werden.
        /// </remarks>
        public IEnumerable<DataProperty?>? Keys
        {
            get
            {
                return Get<IEnumerable<DataProperty?>?>(VCdProp.Keys);
            }
            set
            {
                Set(VCdProp.Keys, value);
            }
        }


        /// <summary>
        /// KIND Art des Objekts, das die vCard beschreibt: Eine Person, eine Organisation oder 
        /// eine Gruppe. (4)
        /// </summary>
        public KindProperty? Kind
        {
            get
            {
                return Get<KindProperty?>(VCdProp.Kind);
            }
            set
            {
                Set(VCdProp.Kind, value);
            }
        }


        /// <summary>
        /// LANG Sprachen, die die Person spricht. (4)
        /// </summary>
        public IEnumerable<TextProperty?>? Languages
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Languages);
            }
            set
            {
                Set(VCdProp.Languages, value);
            }
        }


        /// <summary>
        /// REV Zeitstempel der letzten Aktualisierung der vCard. (2,3,4)
        /// </summary>
        public TimestampProperty? LastRevision
        {
            get
            {
                return Get<TimestampProperty?>(VCdProp.LastRevision);
            }
            set
            {
                Set(VCdProp.LastRevision, value);
            }
        }


        /// <summary>
        /// LOGO Logo(s) der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. (2,3,4)
        /// </summary>
        public IEnumerable<DataProperty?>? Logos
        {
            get
            {
                return Get<IEnumerable<DataProperty?>?>(VCdProp.Logos);
            }
            set
            {
                Set(VCdProp.Logos, value);
            }
        }



        /// <summary>
        /// MAILER Art des genutzten E-Mail-Programms. (2,3)
        /// </summary>
        public TextProperty? Mailer
        {
            get
            {
                return Get<TextProperty?>(VCdProp.Mailer);
            }
            set
            {
                Set(VCdProp.Mailer, value);
            }
        }


        /// <summary>
        /// MEMBER Definiert das Objekt, das die vCard repräsentiert, als Teil einer Gruppe. 
        /// Um diese Eigenschaft verwenden zu können, muss die <see cref="VCard.Kind"/>-Eigenschaft auf
        /// <see cref="VCdKind.Group"/> gesetzt werden. (4)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Der Standard erlaubt nur Uri-Werte. Daten, die nicht in eine Uri konvertiert werden können,
        /// werden zwar gelesen und als <see cref="RelationTextProperty"/> eingefügt. Der Inhalt einer 
        /// <see cref="RelationTextProperty"/> wird aber nur dann in die VCF-Datei geschrieben, wenn er in eine
        /// <see cref="Uri"/> kovertiert werden kann.
        /// </para>
        /// <para>
        /// Der Inhalt einer <see cref="RelationUuidProperty"/> wird als <see cref="Uri"/> geschrieben (urn:uuid:...).
        /// Die von <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte, werden 
        /// beim Schreiben als separate VCards an die VCF-Datei angehängt und der Wert ihrer <see cref="VCard.UniqueIdentifier"/>-Eigenschaft
        /// wird in die "Mutter"-<see cref="VCard"/> als uuid-Uri geschrieben. Wenn die <see cref="VCard.UniqueIdentifier"/>-Eigenschaft
        /// der eingefügten <see cref="VCard"/>s nicht gesetzt ist, wird ihnen automatisch eine neue <see cref="Guid"/>
        /// zugewiesen.
        /// </para>
        /// </remarks>
        public IEnumerable<RelationProperty?>? Members
        {
            get
            {
                return Get<IEnumerable<RelationProperty?>?>(VCdProp.Members);
            }
            set
            {
                Set(VCdProp.Members, value);
            }
        }


        /// <summary>
        /// N Name der Person oder Organisation, die die vCard repräsentiert. (2,3,4)
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig und müssen dann
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann 
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<NameProperty?>? NameViews
        {
            get
            {
                return Get<IEnumerable<NameProperty?>?>(VCdProp.NameViews);
            }
            set
            {
                Set(VCdProp.NameViews, value);
            }
        }


        /// <summary>
        /// NICKNAME Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. (3,4)
        /// </summary>
        public IEnumerable<StringCollectionProperty?>? NickNames
        {
            get
            {
                return Get<IEnumerable<StringCollectionProperty?>?>(VCdProp.NickNames);
            }
            set
            {
                Set(VCdProp.NickNames, value);
            }
        }



        /// <summary>
        /// vCard-Properties, die nicht dem Standard entsprechen.
        /// </summary>
        public IEnumerable<NonStandardProperty?>? NonStandardProperties
        {
            get
            {
                return Get<IEnumerable<NonStandardProperty?>?>(VCdProp.NonStandardProperties);
            }
            set
            {
                Set(VCdProp.NonStandardProperties, value);
            }
        }


        /// <summary>
        /// NOTE Kommentar(e) (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? Notes
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Notes);
            }
            set
            {
                Set(VCdProp.Notes, value);
            }
        }


        /// <summary>
        /// ORG Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. (2,3,4)
        /// </summary>
        public IEnumerable<OrganizationProperty?>? Organizations
        {
            get
            {
                return Get<IList<OrganizationProperty?>?>(VCdProp.Organizations);
            }
            set
            {
                Set(VCdProp.Organizations, value);
            }
        }



        /// <summary>
        /// ORG-DIRECTORY URIs, die die Arbeitsplätze der Person repräsentieren. Damit können Informationen 
        /// über Mitarbeiter der Person eingeholt werden. (Erweiterung RFC 6715)
        /// </summary>
        public IEnumerable<TextProperty?>? OrgDirectories
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.OrgDirectories);
            }
            set
            {
                Set(VCdProp.OrgDirectories, value);
            }
        }


        /// <summary>
        /// TEL Telefonnummern (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? PhoneNumbers
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.PhoneNumbers);
            }
            set
            {
                Set(VCdProp.PhoneNumbers, value);
            }
        }


        /// <summary>
        /// PHOTO Bild(er) oder Fotografie(n) der mit der vCard verbundenen Person. (2,3,4)
        /// </summary>
        /// <remarks> Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock 
        /// in die vCard eingebettet werden.</remarks>
        public IEnumerable<DataProperty?>? Photos
        {
            get
            {
                return Get<IEnumerable<DataProperty>>(VCdProp.Photos);
            }
            set
            {
                Set(VCdProp.Photos, value);
            }
        }


        /// <summary>
        /// PRODID Kennung für das Produkt, mit dem das vCard-Objekt erstellt wurde. (3,4)
        /// </summary>
        /// <remarks>
        /// Implementations SHOULD use a method such as that
        /// specified for Formal Public Identifiers in [ISO9070] or for
        /// Universal Resource Names in [RFC3406] to ensure that the text
        /// value is unique.
        /// </remarks>
        public TextProperty? ProdID
        {
            get
            {
                return Get<TextProperty?>(VCdProp.ProdID);
            }
            set
            {
                Set(VCdProp.ProdID, value);
            }
        }

        /// <summary>
        /// PROFILE Legt fest, dass die VCF-Datei eine vCard ist. (3)
        /// </summary>
        public ProfileProperty? Profile
        {
            get
            {
                return Get<ProfileProperty?>(VCdProp.Profile);
            }
            set
            {
                Set(VCdProp.Profile, value);
            }
        }

        /// <summary>
        /// CLIENTPIDMAP Mappings für Property-IDs
        /// </summary>
        public IEnumerable<PropertyIDMappingProperty?>? PropertyIDMappings
        {
            get
            {
                return Get<IEnumerable<PropertyIDMappingProperty?>?>(VCdProp.PropertyIDMappings);
            }
            set
            {
                Set(VCdProp.PropertyIDMappings, value);
            }
        }





        /// <summary>
        /// RELATED Andere Einheiten, zu der die Person Verbindung hat. (4)
        /// </summary>
        public IEnumerable<RelationProperty?>? Relations
        {
            get
            {
                return Get<IEnumerable<RelationProperty?>?>(VCdProp.Relations);
            }
            set
            {
                Set(VCdProp.Relations, value);
            }
        }



        /// <summary>
        /// ROLE Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb 
        /// einer Organisation, z.B. "rechte Hand des Chefs". (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? Roles
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Roles);
            }
            set
            {
                Set(VCdProp.Roles, value);
            }
        }



        /// <summary>
        /// SOUND Gibt standardmäßig die Aussprache der FN-Eigenschaft des vCard-Objekts an, 
        /// wenn diese Eigenschaft nicht mit anderen Eigenschaften verknüpft ist. (2,3,4)
        /// </summary>
        public IEnumerable<DataProperty?>? Sounds
        {
            get
            {
                return Get<IEnumerable<DataProperty?>?>(VCdProp.Sounds);
            }
            set
            {
                Set(VCdProp.Sounds, value);
            }
        }


        /// <summary>
        /// SOURCE URLs, die verwendet werden können, um die neueste Version dieser vCard zu 
        /// erhalten. (3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? Sources
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Sources);
            }
            set
            {
                Set(VCdProp.Sources, value);
            }
        }


        /// <summary>
        /// TZ Zeitzone(n) des vCard Objekts. (2,3,4)
        /// </summary>
        public IEnumerable<TimeZoneProperty?>? TimeZones
        {
            get
            {
                return Get<IEnumerable<TimeZoneProperty?>?>(VCdProp.TimeZones);
            }
            set
            {
                Set(VCdProp.TimeZones, value);
            }
        }


        /// <summary>
        /// TITLE Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt 
        /// verbundenen Person innerhalb einer Organisation, z.B. “Vizepräsident”. (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? Titles
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.Titles);
            }
            set
            {
                Set(VCdProp.Titles, value);
            }
        }


        /// <summary>
        /// UID UUID, die eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. (2,3,4)
        /// </summary>
        /// <remarks>
        /// Obwohl der Standard beliebige Strings zur Identifizierung erlaubt werden hier nur 
        /// UUIDs unterstützt.
        /// </remarks>
        public UuidProperty? UniqueIdentifier
        {
            get
            {
                return Get<UuidProperty?>(VCdProp.UniqueIdentifier);
            }
            set
            {
                Set(VCdProp.UniqueIdentifier, value);
            }
        }



        /// <summary>
        /// URL URLs, die die Person repräsentieren. (Webseiten, Blogs, Social-Media-Seiten) (2,3,4)
        /// </summary>
        public IEnumerable<TextProperty?>? URLs
        {
            get
            {
                return Get<IEnumerable<TextProperty?>?>(VCdProp.URLs);
            }
            set
            {
                Set(VCdProp.URLs, value);
            }
        }


        /// <summary>
        /// XML Beliebige zusätzliche XML-Daten (4)
        /// </summary>
        public IEnumerable<XmlProperty?>? XmlProperties
        {
            get
            {
                return Get<IEnumerable<XmlProperty?>?>(VCdProp.XmlProperties);
            }
            set
            {
                Set(VCdProp.XmlProperties, value);
            }
        }



    }
}