using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FolkerKinzel.VCards
{
    /// <summary>
    /// Kapselt die in einer vCard enthaltenen Informationen.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    /// <example>
    /// <note type="note">Der leichteren Lesbarkeit wegen, wurde in dem Beispiel auf Ausnahmebehandlung verzichtet.</note>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
#if NET40
        internal static string[] EmptyStringArray = new string[0];
#endif

        //private static readonly Regex _vCardBegin =
        //    new Regex(@"\ABEGIN[ \t]*:[ \t]*VCARD[ \t]*\z",
        //        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        //private static readonly Regex _vCardEnd =
        //    new Regex(@"\AEND[ \t]*:[ \t]*VCARD[ \t]*\z",
        //        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);


        private readonly Dictionary<VCdProp, object> _propDic = new Dictionary<VCdProp, object>();

        [return: MaybeNull]
        private T Get<T>(VCdProp prop) => _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;


        private void Set(VCdProp prop, object? value)
        {
            if (value is null)
            {
                _ = _propDic.Remove(prop);
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
        public IEnumerator<KeyValuePair<VCdProp, object>> GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();


        /// <summary>
        /// <c>VERSION</c>: Version der vCard-Spezifikation. <c>(2,3,4)</c>
        /// </summary>
        public VCdVersion Version
        {
            get; private set;
        }


        /// <summary>
        /// <c>CLASS</c>: Sensibilität der in der vCard enthaltenen Daten. <c>(3)</c>
        /// </summary>
        public AccessProperty? Access
        {
            get => Get<AccessProperty?>(VCdProp.Access);
            set => Set(VCdProp.Access, value);
        }


        /// <summary>
        /// <c>ADR</c>: Adressen <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<AddressProperty?>? Addresses
        {
            get => Get<IList<AddressProperty?>?>(VCdProp.Addresses);
            set => Set(VCdProp.Addresses, value);
        }


        /// <summary>
        /// <c>ANNIVERSARY</c>: Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person. <c>(4)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur dann zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? AnniversaryViews
        {
            get => Get<IEnumerable<DateTimeProperty?>?>(VCdProp.AnniversaryViews);
            set => Set(VCdProp.AnniversaryViews, value);
        }


        /// <summary>
        /// <c>BDAY</c>: Geburtstag <c>(2,3,4)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig, und nur dann, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? BirthDayViews
        {
            get => Get<IEnumerable<DateTimeProperty?>?>(VCdProp.BirthDayViews);
            set => Set(VCdProp.BirthDayViews, value);
        }


        /// <summary>
        /// <c>BIRTHPLACE</c>: Geburtsort <c>(4 - RFC 6474)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur dann zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<TextProperty?>? BirthPlaceViews
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.BirthPlaceViews);
            set => Set(VCdProp.BirthPlaceViews, value);
        }



        /// <summary>
        /// <c>CALURI</c>: URLs zum Kalender der Person. <c>(4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? CalendarAddresses
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.CalendarAddresses);
            set => Set(VCdProp.CalendarAddresses, value);
        }



        /// <summary>
        /// <c>CALADRURI</c>: URLs für das Senden einer Terminanforderung an die Person oder Organisation. <c>(4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? CalendarUserAddresses
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.CalendarUserAddresses);
            set => Set(VCdProp.CalendarUserAddresses, value);
        }


        /// <summary>
        /// <c>CATEGORIES</c>: Liste(n) von Eigenschaften, die das Objekt der vCard beschreiben. <c>(3,4)</c>
        /// </summary>
        public IEnumerable<StringCollectionProperty?>? Categories
        {
            get => Get<IEnumerable<StringCollectionProperty?>?>(VCdProp.Categories);
            set => Set(VCdProp.Categories, value);
        }


        /// <summary>
        /// <c>DEATHDATE</c>: Todestag <c>(4 - RFC 6474)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur dann zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<DateTimeProperty?>? DeathDateViews
        {
            get => Get<IEnumerable<DateTimeProperty?>?>(VCdProp.DeathDateViews);
            set => Set(VCdProp.DeathDateViews, value);
        }


        /// <summary>
        /// <c>DEATHPLACE</c>: Sterbeort <c>(4 - RFC 6474)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur dann zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<TextProperty?>? DeathPlaceViews
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.DeathPlaceViews);
            set => Set(VCdProp.DeathPlaceViews, value);
        }


        /// <summary>
        /// <c>NAME</c>:
        /// Anzeigbarer Name der <see cref="Sources"/>-Eigenschaft. <c>(3)</c>
        /// </summary>
        public TextProperty? DirectoryName
        {
            get => Get<TextProperty?>(VCdProp.DirectoryName);
            set => Set(VCdProp.DirectoryName, value);
        }




        /// <summary>
        /// <c>FN</c>: Formatierte Zeichenfolge mit dem/den vollständigen Namen des vCard-Objekts. <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? DisplayNames
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.DisplayNames);
            set => Set(VCdProp.DisplayNames, value);
        }



        /// <summary>
        /// <c>EMAIL</c>: E-Mail-Adressen  <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? EmailAddresses
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.EmailAddresses);
            set => Set(VCdProp.EmailAddresses, value);
        }


        /// <summary>
        /// <c>EXPERTISE</c>: Fachgebiete, über die die Person Kenntnisse hat. <c>(RFC 6715)</c>
        /// </summary>
        /// <remarks>
        /// Geben Sie den Grad der Fachkenntnis im Parameter <see cref="ParameterSection.ExpertiseLevel"/> an!
        /// </remarks>
        public IEnumerable<TextProperty?>? Expertises
        {
            get => Get<IEnumerable<TextProperty>>(VCdProp.Expertises);
            set => Set(VCdProp.Expertises, value);
        }

        /// <summary>
        /// <c>GENDER</c>: Geschlecht <c>(4)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur dann zulässig, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<GenderProperty?>? GenderViews
        {
            get => Get<IEnumerable<GenderProperty?>?>(VCdProp.GenderViews);
            set => Set(VCdProp.GenderViews, value);
        }


        /// <summary>
        /// <c>GEO</c>: Längen- und Breitengrad(e). <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<GeoProperty?>? GeoCoordinates
        {
            get => Get<IEnumerable<GeoProperty?>?>(VCdProp.GeoCoordinates);
            set => Set(VCdProp.GeoCoordinates, value);
        }



        /// <summary>
        /// <c>HOBBY</c>: Freizeitbeschäftigungen, denen die Person nachgeht. <c>(4 - RFC 6715)</c>
        /// </summary>
        /// <remarks>
        /// Legen Sie den Grad des Interesses im Parameter <see cref="ParameterSection.InterestLevel"/> fest!
        /// </remarks>
        public IEnumerable<TextProperty?>? Hobbies
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Hobbies);
            set => Set(VCdProp.Hobbies, value);
        }


        /// <summary>
        /// <c>IMPP</c>: Liste von Instant-Messenger-Handles. <c>(3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? InstantMessengerHandles
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.InstantMessengerHandles);
            set => Set(VCdProp.InstantMessengerHandles, value);
        }


        /// <summary>
        /// <c>INTEREST</c>: Freizeitbeschäftigungen, für die sich die Person interessiert, an denen sie 
        /// aber nicht zwangsläufig teilnimmt. <c>(4 - RFC 6715)</c>
        /// </summary>
        /// <remarks>
        /// Legen Sie den Grad des Interesses im Parameter <see cref="ParameterSection.InterestLevel"/> fest!
        /// </remarks>
        public IEnumerable<TextProperty?>? Interests
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Interests);
            set => Set(VCdProp.Interests, value);
        }


        /// <summary>
        /// <c>KEY</c>: Öffentliche Schlüssel, die dem vCard-Objekt zugeordnet sind. <c>(2,3,4)</c>
        /// </summary>
        /// <value>
        /// Es kann zu einer externen URL verwiesen werden, 
        /// Klartext angegeben oder ein Base64-kodierter Textblock in die vCard eingebettet werden.
        /// </value>
        public IEnumerable<DataProperty?>? Keys
        {
            get => Get<IEnumerable<DataProperty?>?>(VCdProp.Keys);
            set => Set(VCdProp.Keys, value);
        }


        /// <summary>
        /// <c>KIND</c>: Art des Objekts, das die vCard beschreibt. <c>(4)</c>
        /// </summary>
        public KindProperty? Kind
        {
            get => Get<KindProperty?>(VCdProp.Kind);
            set => Set(VCdProp.Kind, value);
        }


        /// <summary>
        /// <c>LANG</c>: Sprachen, die die Person spricht. <c>(4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? Languages
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Languages);
            set => Set(VCdProp.Languages, value);
        }


        /// <summary>
        /// <c>REV</c>: Zeitstempel der letzten Aktualisierung der vCard. <c>(2,3,4)</c>
        /// </summary>
        [Obsolete("Use \"TimeStamp\" instead!", VCardProperty.OBSOLETE_AS_ERROR)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeStampProperty? LastRevision
        {
            get => TimeStamp;
            set => TimeStamp = value;
        }

        /// <summary>
        /// <c>REV</c>: Zeitstempel der letzten Aktualisierung der vCard. <c>(2,3,4)</c>
        /// </summary>
        public TimeStampProperty? TimeStamp
        {
            get => Get<TimeStampProperty?>(VCdProp.TimeStamp);
            set => Set(VCdProp.TimeStamp, value);
        }


        /// <summary>
        /// <c>LOGO</c>: Logo(s) der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. <c>(2,3,4)</c>
        /// </summary>
        /// <value>
        /// Es kann zu einer externen URL verwiesen oder ein Base64-kodierter Textblock in die vCard eingebettet werden.
        /// </value>
        public IEnumerable<DataProperty?>? Logos
        {
            get => Get<IEnumerable<DataProperty?>?>(VCdProp.Logos);
            set => Set(VCdProp.Logos, value);
        }



        /// <summary>
        /// <c>MAILER</c>: Art des genutzten E-Mail-Programms. <c>(2,3)</c>
        /// </summary>
        public TextProperty? Mailer
        {
            get => Get<TextProperty?>(VCdProp.Mailer);
            set => Set(VCdProp.Mailer, value);
        }


        /// <summary>
        /// <c>MEMBER</c>: Definiert das Objekt, das die vCard repräsentiert, als Teil einer Gruppe. 
        /// Um diese Eigenschaft verwenden zu können, muss die <see cref="VCard.Kind"/>-Eigenschaft auf
        /// <see cref="VCdKind.Group"/> gesetzt werden. <c>(4)</c>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Der Standard erlaubt nur URI-Werte. Daten, die nicht in einen <see cref="Uri"/> konvertiert werden können,
        /// werden zwar gelesen und als <see cref="RelationTextProperty"/> eingefügt. Der Inhalt einer 
        /// <see cref="RelationTextProperty"/> wird aber nur dann in die VCF-Datei geschrieben, wenn er in einen
        /// <see cref="Uri"/> kovertiert werden kann.
        /// </para>
        /// <para>
        /// Der Inhalt einer <see cref="RelationUuidProperty"/> wird als <see cref="Uri"/> geschrieben (urn:uuid:...).
        /// Die von <see cref="RelationVCardProperty"/>-Objekten referenzierten <see cref="VCard"/>-Objekte, werden 
        /// beim Schreiben als separate vCards an die VCF-Datei angehängt und der Wert ihrer <see cref="VCard.UniqueIdentifier"/>-Eigenschaft
        /// wird in die "Mutter"-<see cref="VCard"/> als uuid-URI geschrieben. Wenn die <see cref="VCard.UniqueIdentifier"/>-Eigenschaft
        /// der eingefügten <see cref="VCard"/>s nicht gesetzt ist, wird ihnen automatisch eine neue <see cref="Guid"/>
        /// zugewiesen.
        /// </para>
        /// </remarks>
        public IEnumerable<RelationProperty?>? Members
        {
            get => Get<IEnumerable<RelationProperty?>?>(VCdProp.Members);
            set => Set(VCdProp.Members, value);
        }


        /// <summary>
        /// <c>N</c>: Name der Person oder Organisation, die die vCard repräsentiert. <c>(2,3,4)</c>
        /// </summary>
        /// <remarks>
        /// Mehrere Instanzen sind nur in vCard 4.0 zulässig, und nur dann, wenn sie
        /// denselben <see cref="ParameterSection.AltID"/>-Parameter haben. Das kann z.B.
        /// sinnvoll sein, wenn die Property in verschiedenen Sprachen dargestellt ist.
        /// </remarks>
        public IEnumerable<NameProperty?>? NameViews
        {
            get => Get<IEnumerable<NameProperty?>?>(VCdProp.NameViews);
            set => Set(VCdProp.NameViews, value);
        }


        /// <summary>
        /// <c>NICKNAME</c>: Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. <c>(3,4)</c>
        /// </summary>
        public IEnumerable<StringCollectionProperty?>? NickNames
        {
            get => Get<IEnumerable<StringCollectionProperty?>?>(VCdProp.NickNames);
            set => Set(VCdProp.NickNames, value);
        }



        /// <summary>
        /// vCard-Properties, die nicht dem Standard entsprechen.
        /// </summary>
        /// <remarks>
        /// <para><see cref="NonStandardProperties"/> speichert alle vCard-Properties, die beim Parsen nicht
        /// ausgewertet werden konnten. Um den Inhalt von <see cref="NonStandardProperties"/> in eine vCard
        /// zu schreiben, muss das Flag <see cref="VcfOptions.WriteNonStandardProperties">VcfOptions.WriteNonStandardProperties</see>
        /// explizit gesetzt werden.</para>
        /// <para>Einige <see cref="NonStandardProperty"/>-Objekte werden der zu schreibenden vCard automatisch
        /// hinzugefügt, wenn es kein Standard-Äquivalent dafür gibt. Sie können dieses Verhalten mit <see cref="VcfOptions"/>
        /// steuern. Es wird deshalb nicht empfohlen, der Eigenschaft <see cref="NonStandardProperties"/> Instanzen dieser
        /// automatisch hinzuzufügenden <see cref="NonStandardProperty"/>-Objekte zu übergeben.</para>
        /// <para>Es handelt sich um folgende vCard-Properties:</para>
        /// <list type="bullet">
        /// <item><c>X-AIM</c></item>
        /// <item><c>X-ANNIVERSARY</c></item>
        /// <item><c>X-ASSISTANT</c></item>
        /// <item><c>X-EVOLUTION-SPOUSE</c></item>
        /// <item><c>X-EVOLUTION-ANNIVERSARY</c></item>
        /// <item><c>X-EVOLUTION-ASSISTANT</c></item>
        /// <item><c>X-GADUGADU</c></item>
        /// <item><c>X-GENDER</c></item>
        /// <item><c>X-GOOGLE-TALK</c></item>
        /// <item><c>X-GROUPWISE</c></item>
        /// <item><c>X-GTALK</c></item>
        /// <item><c>X-ICQ</c></item>
        /// <item><c>X-JABBER</c></item>
        /// <item><c>X-KADDRESSBOOK-X-ANNIVERSARY</c></item>
        /// <item><c>X-KADDRESSBOOK-X-ASSISTANTSNAME</c></item>
        /// <item><c>X-KADDRESSBOOK-X-IMADDRESS</c></item>
        /// <item><c>X-KADDRESSBOOK-X-SPOUSENAME</c></item>
        /// <item><c>X-MS-IMADDRESS</c></item>
        /// <item><c>X-MSN</c></item>
        /// <item><c>X-SKYPE</c></item>
        /// <item><c>X-SKYPE-USERNAME</c></item>
        /// <item><c>X-SPOUSE</c></item>
        /// <item><c>X-TWITTER</c></item>
        /// <item><c>X-WAB-GENDER</c></item>
        /// <item><c>X-WAB-WEDDING_ANNIVERSARY</c></item>
        /// <item><c>X-WAB-SPOUSE_NAME</c></item>
        /// <item><c>X-YAHOO</c></item>
        /// </list>
        /// </remarks>
        public IEnumerable<NonStandardProperty?>? NonStandardProperties
        {
            get => Get<IEnumerable<NonStandardProperty?>?>(VCdProp.NonStandardProperties);
            set => Set(VCdProp.NonStandardProperties, value);
        }


        /// <summary>
        /// <c>NOTE</c>: Kommentar(e) <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? Notes
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Notes);
            set => Set(VCdProp.Notes, value);
        }


        /// <summary>
        /// <c>ORG</c>: Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<OrganizationProperty?>? Organizations
        {
            get => Get<IList<OrganizationProperty?>?>(VCdProp.Organizations);
            set => Set(VCdProp.Organizations, value);
        }



        /// <summary>
        /// <c>ORG-DIRECTORY</c>: URIs, die die Arbeitsplätze der Person repräsentieren. Damit können Informationen 
        /// über Mitarbeiter der Person eingeholt werden. <c>(RFC 6715)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? OrgDirectories
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.OrgDirectories);
            set => Set(VCdProp.OrgDirectories, value);
        }


        /// <summary>
        /// <c>TEL</c>: Telefonnummern <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? PhoneNumbers
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.PhoneNumbers);
            set => Set(VCdProp.PhoneNumbers, value);
        }


        /// <summary>
        /// <c>PHOTO</c>: Bild(er) oder Fotografie(n) der mit der vCard verbundenen Person. <c>(2,3,4)</c>
        /// </summary>
        /// <value> Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock 
        /// in die vCard eingebettet werden.</value>
        public IEnumerable<DataProperty?>? Photos
        {
            get => Get<IEnumerable<DataProperty>>(VCdProp.Photos);
            set => Set(VCdProp.Photos, value);
        }


        /// <summary>
        /// <c>PRODID</c>: Kennung für das Produkt, mit dem das vCard-Objekt erstellt wurde. <c>(3,4)</c>
        /// </summary>
        /// <value>Der Name sollte weltweit eindeutig sein. Er sollte deshalb der Spezifikation für
        /// Formal Public Identifiers [ISO9070] oder Universal Resource Names in [RFC3406] entsprechen.</value>
        public TextProperty? ProdID
        {
            get => Get<TextProperty?>(VCdProp.ProdID);
            set => Set(VCdProp.ProdID, value);
        }

        /// <summary>
        /// <c>PROFILE</c>: Legt fest, dass die VCF-Datei eine vCard ist. <c>(3)</c>
        /// </summary>
        public ProfileProperty? Profile
        {
            get => Get<ProfileProperty?>(VCdProp.Profile);
            set => Set(VCdProp.Profile, value);
        }

        /// <summary>
        /// <c>CLIENTPIDMAP</c>: Mappings für <see cref="PropertyID"/>s. Wird verwendet,
        /// um verschiedene Bearbeitungsstände derselben vCard zu synchronisieren. <c>(4)</c>
        /// </summary>
        public IEnumerable<PropertyIDMappingProperty?>? PropertyIDMappings
        {
            get => Get<IEnumerable<PropertyIDMappingProperty?>?>(VCdProp.PropertyIDMappings);
            set => Set(VCdProp.PropertyIDMappings, value);
        }


        /// <summary>
        /// <c>RELATED</c>: Andere Einheiten, zu der die Person Verbindung hat. <c>(4)</c>
        /// </summary>
        public IEnumerable<RelationProperty?>? Relations
        {
            get => Get<IEnumerable<RelationProperty?>?>(VCdProp.Relations);
            set => Set(VCdProp.Relations, value);
        }


        /// <summary>
        /// <c>ROLE</c>: Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb 
        /// einer Organisation, z.B. "rechte Hand des Chefs". <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? Roles
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Roles);
            set => Set(VCdProp.Roles, value);
        }


        /// <summary>
        /// <c>SOUND</c>: Demonstriert die Aussprache der <see cref="DisplayNames"/>-Eigenschaft des <see cref="VCard"/>-Objekts. <c>(2,3,4)</c>
        /// </summary>
        /// <value>
        /// Es kann zu einer externen URL verwiesen oder ein Base64-kodierter Textblock in die vCard eingebettet werden.
        /// </value>
        public IEnumerable<DataProperty?>? Sounds
        {
            get => Get<IEnumerable<DataProperty?>?>(VCdProp.Sounds);
            set => Set(VCdProp.Sounds, value);
        }


        /// <summary>
        /// <c>SOURCE</c>: URLs, die verwendet werden können, um die neueste Version dieser vCard zu 
        /// erhalten. <c>(3,4)</c>
        /// </summary>
        /// <remarks>vCard 3.0 erlaubt nur eine Instanz dieser Property.</remarks>
        public IEnumerable<TextProperty?>? Sources
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Sources);
            set => Set(VCdProp.Sources, value);
        }


        /// <summary>
        /// <c>TZ</c>: Zeitzone(n) des vCard Objekts. <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TimeZoneProperty?>? TimeZones
        {
            get => Get<IEnumerable<TimeZoneProperty?>?>(VCdProp.TimeZones);
            set => Set(VCdProp.TimeZones, value);
        }


        /// <summary>
        /// <c>TITLE</c>: Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt 
        /// verbundenen Person innerhalb einer Organisation, z.B. “Vizepräsident”. <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? Titles
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.Titles);
            set => Set(VCdProp.Titles, value);
        }


        /// <summary>
        /// <c>UID</c>: UUID, die eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. <c>(2,3,4)</c>
        /// </summary>
        /// <value>
        /// Obwohl der Standard beliebige Strings zur Identifizierung erlaubt, werden hier nur 
        /// UUIDs unterstützt.
        /// </value>
        public UuidProperty? UniqueIdentifier
        {
            get => Get<UuidProperty?>(VCdProp.UniqueIdentifier);
            set => Set(VCdProp.UniqueIdentifier, value);
        }



        /// <summary>
        /// <c>URL</c>: URLs, die die Person repräsentieren (Webseiten, Blogs, Social-Media-Seiten). <c>(2,3,4)</c>
        /// </summary>
        public IEnumerable<TextProperty?>? URLs
        {
            get => Get<IEnumerable<TextProperty?>?>(VCdProp.URLs);
            set => Set(VCdProp.URLs, value);
        }


        /// <summary>
        /// <c>XML</c>: Beliebige zusätzliche XML-Daten. <c>(4)</c>
        /// </summary>
        public IEnumerable<XmlProperty?>? XmlProperties
        {
            get => Get<IEnumerable<XmlProperty?>?>(VCdProp.XmlProperties);
            set => Set(VCdProp.XmlProperties, value);
        }



    }
}