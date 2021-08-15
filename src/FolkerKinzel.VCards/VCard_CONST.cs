using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Bezeichner müssen ein korrektes Suffix aufweisen", Justification = "<Ausstehend>")]
    public sealed partial class VCard
    {
        /// <summary>
        /// In VCF-Dateien verwendeter Zeilenwechsel ("\r\n").
        /// </summary>
        public const string NewLine = "\r\n";

        internal const int MAX_BYTES_PER_LINE = 75;
        internal const string DEFAULT_CHARSET = "UTF-8";
        internal const VCdVersion DEFAULT_VERSION = VCdVersion.V3_0;

        private const int DESERIALIZER_QUEUE_INITIAL_CAPACITY = 64;

        internal static class PropKeys
        {
            /// <summary>
            /// Strukturierte Darstellung der physischen 
            /// Anschrift des vCard-Objekts. (2,3,4)
            /// </summary>
            internal const string ADR = "ADR";

            /// <summary>
            /// Informationen über eine andere Person, die im Namen des vCard-Objekts handeln soll. 
            /// Typischerweise ist das ein Vertreter, Assistent oder Sekretär. Hier kann entweder 
            /// eine URL oder eine eingebettete vCard angegeben werden. (2,3,4)
            /// </summary>
            internal const string AGENT = "AGENT";

            /// <summary>
            /// Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person. (4)
            /// </summary>
            internal const string ANNIVERSARY = "ANNIVERSARY";

            /// <summary>
            /// Geburtsdatum der mit der vCard verbundenen Person. (2,3,4)
            /// </summary>
            internal const string BDAY = "BDAY";

            /// <summary>
            /// Eine URL für das Senden einer Terminanforderung zur Verwendung des 
            /// Kalenders der Person. (4)
            /// </summary>
            internal const string CALADRURI = "CALADRURI";

            /// <summary>
            /// Eine URL zum Kalender der Person. (4)
            /// </summary>
            internal const string CALURI = "CALURI";

            /// <summary>
            /// Liste von Eigenschaften, die das Objekt der vCard beschreiben. (3, 4)
            /// </summary>
            internal const string CATEGORIES = "CATEGORIES";

            /// <summary>
            /// Sensibilität der in der vCard enthaltenen Daten. (3)
            /// </summary>
            internal const string CLASS = "CLASS";

            /// <summary>
            /// Mappings für Property-IDs (4)
            /// </summary>
            internal const string CLIENTPIDMAP = "CLIENTPIDMAP";

            /// <summary>
            /// E-Mail-Adresse zur Kommunikation mit dem vCard-Objekt. (2,3,4)
            /// </summary>
            internal const string EMAIL = "EMAIL";

            /// <summary>
            /// URL, die beschreibt, ob auf dem Kalender der Person „frei“ oder „besetzt“ angezeigt wird. (4)
            /// </summary>
            internal const string FBURL = "FBURL";

            /// <summary>
            /// Formatierte Zeichenfolge mit dem vollständigen Namen des vCard-Objekts. (2,3,4)
            /// </summary>
            internal const string FN = "FN";

            /// <summary>
            /// Geschlecht der Person. (4)
            /// </summary>
            internal const string GENDER = "GENDER";

            /// <summary>
            /// Längen- und Breitengrad. (2,3,4)
            /// </summary>
            internal const string GEO = "GEO";

            /// <summary>
            /// Definiert ein Instant-Messenger-Handle. (3,4)
            /// </summary>
            internal const string IMPP = "IMPP";

            /// <summary>
            /// Öffentlicher Schlüssel, der dem vCard-Objekt zugeordnet ist. (2,3,4)
            /// </summary>
            /// <remarks>Es kann zu einer externen URL verwiesen werden, Klartext angegeben werden
            /// oder ein Base64-kodierter Textblock in die vCard eingebettet werden.</remarks>
            internal const string KEY = "KEY";

            /// <summary>
            /// Art des Objekts, das die vCard beschreibt. (4)
            /// </summary>
            /// <remarks> Eine Person (individual), eine Organisation (organization) oder eine Gruppe (group).</remarks>
            internal const string KIND = "KIND";

            /// <summary>
            /// Stellt den eigentlichen Text dar, der auf einem physischen Adressetikett zur Adressierung 
            /// an das mit der vCard verbundene Objekt vorhanden ist (ähnlich der ADR-Eigenschaft). (2,3)
            /// </summary>
            internal const string LABEL = "LABEL";

            /// <summary>
            /// Sprache, die die Person spricht. (4)
            /// </summary>
            internal const string LANG = "LANG";

            /// <summary>
            /// Logo der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. (2,3,4)
            /// <remarks>Es kann 
            /// auf eine externe URL verwiesen oder ein Base64-kodierter Textblock in die vCard eingebettet 
            /// werden.</remarks>
            /// </summary>
            internal const string LOGO = "LOGO";

            /// <summary>
            /// Art des genutzten E-Mail-Programms. (2,3)
            /// </summary>
            internal const string MAILER = "MAILER";

            /// <summary>
            /// Definiert das Objekt, das die vCard repräsentiert, als Teil einer Gruppe. (4)
            /// </summary>
            /// <remarks>
            /// <para>Zulässige Werte sind:</para>
            /// <list type="bullet">
            /// <item>eine „mailto:“-URL, die eine E-Mail-Adresse enthält</item>
            /// <item>eine UUID, die auf die eigene vCard des Mitglieds verweist</item>
            /// </list>
            /// <para>Um diese Eigenschaft verwenden zu können, muss die KIND-Eigenschaft 
            /// auf „group“ gesetzt werden.</para>
            /// </remarks>
            internal const string MEMBER = "MEMBER";

            /// <summary>
            /// Strukturierte Darstellung von Namen der Person, Ort oder Sache, der das vCard-Objekt 
            /// zugeordnet ist. (2,3,4)
            /// </summary>
            internal const string N = "N";

            /// <summary>
            /// Textdarstellung der SOURCE-Eigenschaft. (3)
            /// </summary>
            internal const string NAME = "NAME";

            /// <summary>
            /// Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. (3,4)
            /// </summary>
            internal const string NICKNAME = "NICKNAME";

            /// <summary>
            /// Kommentar (2,3,4)
            /// </summary>
            internal const string NOTE = "NOTE";

            /// <summary>
            /// Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. (2,3,4)
            /// </summary>
            /// <remarks>Diese Eigenschaft basiert auf den Attributen „Organization Name“ und „Organization Unit“ 
            /// des X.520-Standards.</remarks>
            internal const string ORG = "ORG";

            /// <summary>
            /// Bild oder Fotografie der mit der vCard verbundenen Person. (2,3,4)
            /// </summary>
            /// <remarks>Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock in die vCard 
            /// eingebettet werden.</remarks>
            internal const string PHOTO = "PHOTO";

            /// <summary>
            /// Kennung für das Produkt, mit dem das vCard-Objekt erstellt wurde. (3,4)
            /// </summary>
            internal const string PRODID = "PRODID";

            /// <summary>
            /// Legt fest, dass die vCard eine vCard ist. (3)
            /// </summary>
            internal const string PROFILE = "PROFILE";

            /// <summary>
            /// Andere Einheit, zu der die Person Verbindung hat. (4)
            /// </summary>
            /// <remarks>
            /// <para>Zulässige Werte sind:</para>
            /// <list type="bullet">
            /// <item>eine „mailto:“-URL, die eine E-Mail-Adresse enthält</item>
            /// <item>eine UUID, die auf die eigene vCard des Mitglieds verweist</item>
            /// </list>
            /// </remarks>
            internal const string RELATED = "RELATED";

            /// <summary>
            /// Zeitstempel der letzten Aktualisierung der vCard. (2,3,4)
            /// </summary>
            internal const string REV = "REV";

            /// <summary>
            /// Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb einer Organisation. (2,3,4)
            /// </summary>
            internal const string ROLE = "ROLE";

            /// <summary>
            /// Zeichenkette, die die Sortierreihenfolge der vCard in Anwendungen beschreibt. (3)
            /// </summary>
            internal const string SORT_STRING = "SORT-STRING";

            /// <summary>
            /// Gibt standardmäßig die Aussprache der FN-Eigenschaft des vCard-Objekts an, wenn diese Eigenschaft 
            /// nicht mit anderen Eigenschaften verknüpft ist. (2,3,4)</summary>
            /// <remarks>Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock in die vCard 
            /// eingebettet werden.</remarks>
            internal const string SOUND = "SOUND";

            /// <summary>
            /// URL, die verwendet werden kann, um die neueste Version dieser vCard zu erhalten. (3,4)
            /// </summary>
            internal const string SOURCE = "SOURCE";

            /// <summary>
            /// Normalform einer numerischen Zeichenkette für eine Telefonnummer zur telefonischen Kommunikation 
            /// mit dem vCard-Objekt. (2,3,4)
            /// </summary>
            internal const string TEL = "TEL";


            /// <summary>
            /// Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt 
            /// verbundenen Person innerhalb einer Organisation. (2,3,4)	
            /// </summary>
            internal const string TITLE = "TITLE";


            /// <summary>
            /// Zeitzone des vCard Objekts. (2,3,4)
            /// </summary>
            internal const string TZ = "TZ";

            /// <summary>
            /// UUID, die eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. (2,3,4)
            /// </summary>
            internal const string UID = "UID";

            /// <summary>
            /// URL zu einer Website, die die Person repräsentiert. (2,3,4)
            /// </summary>
            internal const string URL = "URL";

            /// <summary>
            /// Version der vCard-Spezifikation. (2,3,4)
            /// </summary>
            /// <remarks>In den Versionen 3.0 und 4.0 muss diese auf die BEGIN-Eigenschaft folgen.</remarks>
            internal const string VERSION = "VERSION";


            /// <summary>
            /// Beliebige mit der vCard verbundene XML-Daten. (4)
            /// </summary>
            internal const string XML = "XML";



            internal static class NonStandard
            {

                /// <summary>
                /// Fachgebiet, über dessen Kenntnis die Person verfügt. RFC 6715
                /// </summary>
                internal const string EXPERTISE = "EXPERTISE";

                /// <summary>
                /// Freizeitbeschäftigung, der die Person nachgeht. RFC 6715
                /// </summary>
                internal const string HOBBY = "HOBBY";

                /// <summary>
                /// Freizeitbeschäftigung, für die sich die Person interessiert, an der sie aber nicht 
                /// zwangsläufig teilnimmt. RFC 6715
                /// </summary>
                internal const string INTEREST = "INTEREST";

                /// <summary>
                /// Geburtsort der Person RFC 6474
                /// </summary>
                internal const string BIRTHPLACE = "BIRTHPLACE";

                /// <summary>
                /// Sterbedatum der Person RFC 6474
                /// </summary>
                internal const string DEATHDATE = "DEATHDATE";

                /// <summary>
                /// Sterbeort der Person RFC 6474
                /// </summary>
                internal const string DEATHPLACE = "DEATHPLACE";

                /// <summary>
                /// URI, der den Arbeitsplatz der Person repräsentiert; damit können Informationen über 
                /// Mitarbeiter der Person eingeholt werden. RFC 6715
                /// </summary>
                internal const string ORG_DIRECTORY = "ORG-DIRECTORY";

                /// <summary>
                /// Geschlecht (Male oder Female)
                /// </summary>
                internal const string X_GENDER = "X-GENDER";

                /// <summary>
                /// Geschlecht (value 1 == weiblich 2 == männlich) [Outlook Express]
                /// </summary>
                internal const string X_WAB_GENDER = "X-WAB-GENDER";

                /// <summary>
                /// Hochzeitstag oder beliebiges Jubiläum (zusätzlich zu BDAY, Geburtstag).
                /// </summary>
                /// <remarks>YYYY-MM-DD</remarks>
                internal const string X_ANNIVERSARY = "X-ANNIVERSARY";

                /// <summary>
                /// Hochzeitstag. [Outlook Express]
                /// </summary>
                /// <remarks>YYYYMMDD</remarks>
                internal const string X_WAB_WEDDING_ANNIVERSARY = "X-WAB-WEDDING_ANNIVERSARY";

                /// <summary>
                /// Name des Ehepartners
                /// </summary>
                internal const string X_SPOUSE = "X-SPOUSE";

                /// <summary>
                /// Name des Ehepartners [Outlook Express]
                /// </summary>
                internal const string X_WAB_SPOUSE_NAME = "X-WAB-SPOUSE_NAME";

                /// <summary>
                /// Assistenzname (anstelle von AGENT)
                /// </summary>
                internal const string X_ASSISTANT = "X-ASSISTANT";


                internal static class InstantMessenger
                {
                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_AIM = "X-AIM";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_ICQ = "X-ICQ";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_GOOGLE_TALK = "X-GOOGLE-TALK";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_GTALK = "X-GTALK";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_JABBER = "X-JABBER";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_MSN = "X-MSN";

                    internal const string X_YAHOO = "X-YAHOO";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_TWITTER = "X-TWITTER";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_SKYPE = "X-SKYPE";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_SKYPE_USERNAME = "X-SKYPE-USERNAME";

                    /// <summary>
                    /// Kontaktinformationen für Instant Messaging (IM); TYPE-Parameter wie für TEL
                    /// </summary>
                    internal const string X_GADUGADU = "X-GADUGADU";

                    /// <summary>
                    /// GroupWise-Adresse
                    /// </summary>
                    internal const string X_GROUPWISE = "X-GROUPWISE";

                    /// <summary>
                    /// IM-Adresse im VCF-Attachment von Microsoft Outlook (bei Rechtsklick auf den 
                    /// Kontakt-Eintrag → Vollständigen Kontakt senden → Im Internetformat)
                    /// </summary>
                    internal const string X_MS_IMADDRESS = "X-MS-IMADDRESS";

                    /// <summary>
                    /// IM-Adresse 
                    /// </summary>
                    internal const string X_KADDRESSBOOK_X_IMADDRESS = "X-KADDRESSBOOK-X-IMADDRESS";
                }


                internal static class KAddressbook
                {
                    /// <summary>
                    /// Name des Ehepartners
                    /// </summary>
                    internal const string X_KADDRESSBOOK_X_SPOUSENAME = "X-KADDRESSBOOK-X-SPOUSENAME";

                    /// <summary>
                    /// beliebiges Jubiläum (zusätzlich zu BDAY, Geburtstag) 
                    /// </summary>
                    internal const string X_KADDRESSBOOK_X_ANNIVERSARY = "X-KADDRESSBOOK-X-ANNIVERSARY";

                    /// <summary>
                    /// Assistenzname (anstelle von AGENT) 
                    /// </summary>
                    internal const string X_KADDRESSBOOK_X_ASSISTANTSNAME = "X-KADDRESSBOOK-X-ASSISTANTSNAME";
                }

                internal static class Evolution
                {
                    /// <summary>
                    /// Name des Ehepartners
                    /// </summary>
                    internal const string X_EVOLUTION_SPOUSE = "X-EVOLUTION-SPOUSE";

                    /// <summary>
                    /// beliebiges Jubiläum (zusätzlich zu BDAY, Geburtstag) 
                    /// </summary>
                    internal const string X_EVOLUTION_ANNIVERSARY = "X-EVOLUTION-ANNIVERSARY";

                    /// <summary>
                    /// Assistenzname (anstelle von AGENT) 
                    /// </summary>
                    internal const string X_EVOLUTION_ASSISTANT = "X-EVOLUTION-ASSISTANT";
                }
            }

        }
    }
}
