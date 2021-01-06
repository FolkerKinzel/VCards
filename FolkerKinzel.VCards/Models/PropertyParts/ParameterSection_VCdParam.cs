namespace FolkerKinzel.VCards.Models.PropertyParts
{
    public partial class ParameterSection
    {
        private enum VCdParam
        {
            /// <summary>
            /// <c>CONTEXT</c>: Gibt den Kontext der Daten an, z.B. <c>VCARD</c> oder <c>LDAP</c>. <c>(3)</c>
            /// </summary>
            Context,

            /// <summary>
            /// <c>TYPE</c>: Klassifiziert eine <see cref="VCardProperty"/> als dienstlich und / oder privat. <c>(2,3,4)</c>
            /// </summary>
            PropertyClass,

            /// <summary>
            /// <c>TYPE</c>: Bestimmt in einer <see cref="RelationProperty"/> (<c>RELATED</c>) die Art der Beziehung zu einer Person. <c>(4)</c>
            /// </summary>
            RelationType,

            /// <summary>
            /// <c>TYPE</c>: Beschreibt die Art einer Adresse. <c>(2,3)</c>
            /// </summary>
            AddressType,

            /// <summary>
            /// <c>TYPE</c>: Beschreibt die Art einer E-Mail. <c>(2,3)</c>
            /// </summary>
            EmailType,


            /// <summary>
            /// <c>TYPE</c>: Beschreibt die Art einer Telefonnummer. <c>(2,3,4)</c>
            /// </summary>
            TelephoneType,


            /// <summary>
            /// <c>LEVEL</c>: Grad der Sachkenntnis einer Person. (Für vCard-Property <c>EXPERTISE</c>). <c>(4 - RFC 6715)</c>
            /// </summary>
            ExpertiseLevel,

            /// <summary>
            /// <c>TYPE</c>: Nähere Beschreibung einer Instant-Messenger-Adresse. <c>(3 - RFC 4770)</c>
            /// </summary>
            InstantMessengerType,

            /// <summary>
            /// <c>LEVEL</c>: Grad des Interesses einer Person für eine Sache (Für die vCard-Properties <c>HOBBY</c> und <c>INTEREST</c>.) <c>(4 - RFC 6715)</c>
            /// </summary>
            InterestLevel,


            /// <summary>
            /// <c>LABEL</c>: Gibt die formatierte Textdarstellung einer Adresse an. <c>([2],[3],4)</c>
            /// </summary>
            Label,


            /// <summary>
            /// <c>PREF</c> oder <c>TYPE=PREF</c>: Drückt die Beliebtheit einer Property aus. <c>(2,3,4)</c>
            /// </summary>
            Preference,


            /// <summary>
            /// <c>CHARSET</c>: Gibt den Zeichensatz an, der für die Property verwendet wurde. <c>(2)</c>
            /// </summary>
            Charset,

            /// <summary>
            /// <c>ENCODING</c>: Gibt die Encodierung der <see cref="VCardProperty"/> an. <c>(2,3)</c>
            /// </summary>
            Encoding,

            /// <summary>
            /// <c>LANGUAGE</c>: Sprache der <see cref="VCardProperty"/>. <c>(2,3,4)</c>
            /// </summary>
            Language,


            /// <summary>
            /// <c>VALUE</c>: Gibt an, welchem der vom vCard-Standard vordefinierten Datentypen der Inhalt
            /// der vCard-Property entspricht. <c>(3,4)</c>
            /// </summary>
            DataType,

            /// <summary>
            /// <c>VALUE</c>: Gibt an, wo sich der eigentiche Inhalt der Property befindet. <c>(2)</c>
            /// </summary>
            ContentLocation,

            /// <summary>
            /// <c>MEDIATYPE</c>: Gibt bei URIs den MIME-Typ der Daten an, auf den der URI verweist (z.B. <c>text/plain</c>). <c>(4)</c>
            /// </summary>
            MediaType,

            /// <summary>
            /// <c>GEO</c>: Geografische Position. <c>(4)</c>
            /// </summary>
            GeographicalPosition,


            /// <summary>
            /// <c>TZ</c>: Zeitzone <c>(4)</c>
            /// </summary>
            TimeZone,

            /// <summary>
            /// <c>CALSCALE</c>: Gibt die Art des Kalenders an, der für Datumsangaben verwendet wird. <c>(4)</c>
            /// </summary>
            Calendar,


            /// <summary>
            /// <c>SORT-AS</c>:&#160;<see cref="string"/>s (case-sensitiv!), die die Sortierreihenfolge festlegen. (Maximal so viele, wie Felder der 
            /// zusammengesetzten Property!) <c>([3],4)</c>
            /// </summary>
            SortAs,


            /// <summary>
            /// Nicht standardisierte Attribute. (2,3,4)
            /// </summary>
            NonStandard,


            /// <summary>
            /// <c>PID</c>: Property-ID zur Identifizierung einer bestimmten vCard-Property unter verschiedenen Instanzen mit demselben Bezeichner. <c>(4)</c>
            /// </summary>
            PropertyIDs,


            /// <summary>
            /// <c>ALTID</c>: Ein gemeinsamer Bezeichner, der zu erkennen gibt, dass mehrere Instanzen derselben Property dasselbe 
            /// darstellen (z.B. in unterschiedlichen Sprachen). <c>(4)</c>
            /// </summary>
            AltID,


            /// <summary>
            /// <c>INDEX</c>: 1-basierter Index einer Property, wenn mehrere Instanzen derselben Property möglich sind. <c>(4 - RFC 6715)</c>
            /// </summary>
            Index
        }
    }
}
