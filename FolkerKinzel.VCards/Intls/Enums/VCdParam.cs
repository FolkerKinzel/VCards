using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Model;


namespace FolkerKinzel.VCards.Intls.Enums
{
    internal enum VCdParam
    {
        

        /// <summary>
        /// CONTEXT Gibt den Kontext der Daten an, z.B. "VCARD" oder "LDAP". (3)
        /// </summary>
        /// <remarks>Kommt in der SOURCE-Property von vCard 3.0 zum Einsatz.</remarks>
        Context,

        /// <summary>
        /// TYPE Klassifiziert eine Property als dienstlich und / oder privat. (2,3,4)
        /// </summary>
        PropertyClass,

        /// <summary>
        /// TYPE Bestimmt in der Relations-Property (RELATED) die Art der Beziehung zu einer Person. (4)
        /// </summary>
        RelationType,

        /// <summary>
        /// TYPE Beschreibt die Art einer Adresse. (2,3)
        /// </summary>
        AddressType,

        /// <summary>
        /// TYPE Beschreibt die Art einer E-Mail. Verwenden Sie nur die Konstanten 
        /// <see cref="ParameterSection.E_MAIL_TYPE_SMTP"/> für eine Internet-E-Mail-Adresse (Standard)
        /// oder <see cref="ParameterSection.E_MAIL_TYPE_X400"/> für eine X.400-E-Mail-Adresse. (2,3)
        /// </summary>
        EmailType,


        /// <summary>
        /// TYPE Beschreibt die Art einer Telefonnummer. (2,3,4)
        /// </summary>
        TelephoneType,


        /// <summary>
        /// LEVEL RFC 6715: Used to indicate a level of expertise
        /// attained by the object the vCard represents. (Für Property EXPERTISE). (4 Erweiterung)
        /// </summary>
        ExpertiseLevel,

        /// <summary>
        /// TYPE Nähere Beschreibung einer Instant-Messenger-Adresse. (3 Erweiterung RFC 4770)
        /// </summary>
        InstantMessengerType,

        /// <summary>
        /// LEVEL RFC 6715: Used to indicate a level of hobby or interest
        /// attained by the object the vCard represents. (Für Property INTEREST.) (4 Erweiterung)
        /// </summary>
        InterestLevel,


        /// <summary>
        /// LABEL Gibt die formatierte Textdarstellung einer Adresse an. ([2],[3],4)
        /// </summary>
        /// <remarks>In vCard 2.1 und vCard 3.0 wird der Inhalt als separate LABEL-Property eingefügt.</remarks>
        Label,


        /// <summary>
        /// PREF oder TYPE=PREF Drückt die Beliebtheit einer Property aus (zwischen 1 und 100). 1 bedeutet 
        /// am beliebtesten. Bei Properties, die mehrfach vorkommen, zählt die größte Beliebtheit. (2,3,4)
        /// </summary>
        Preference,


        /// <summary>
        /// CHARSET Gibt den Zeichensatz an, der für die Property verwendet wurde. (2)
        /// </summary>
        Charset,

        /// <summary>
        /// ENCODING Gibt die Encodierung der Property an. (2,3)
        /// </summary>
        /// <remarks>
        /// vCard 3.0 nur "b" für "Base64.
        /// </remarks>
        Encoding,

        /// <summary>
        /// LANGUAGE Sprache der Property. (2,3,4)
        /// </summary>
        Language,


        /// <summary>
        /// VALUE: Gibt an, welchem der vom vCard-Standard vordefinierten Datentypen der Inhalt
        /// der vCard-Property entspricht. (3,4)
        /// </summary>
        DataType,

        /// <summary>
        /// VALUE: Gibt an, wo sich der eigentiche Inhalt der Property befindet. (2)
        /// </summary>
        ContentLocation,

        /// <summary>
        /// MEDIATYPE Gibt bei URIs den MIME-Typ an, auf den die Uri verweist (z.B. text/plain). (4)
        /// </summary>
        MediaType,

        /// <summary>
        /// GEO Geografische Position (4)
        /// </summary>
        GeographicPosition,
        

        /// <summary>
        /// TZ Zeitzone (4)
        /// </summary>
        TimeZone,

        /// <summary>
        /// CALSCALE Gibt die Art des Kalenders an, der für Datumsangaben verwendet wird. (4)
        /// </summary>
        /// <remarks>Der einzige offiziell registrierte Wert ist "GREGORIAN" für den gregorianischen 
        /// Kalender.</remarks>
        Calendar,


        /// <summary>
        /// SORT-AS Ein kommagetrennter <see cref="string"/> (case-sensitiv!) der die Sortierreihenfolge festlegt. ([3],4)
        /// </summary>
        /// <example>
        /// <code>
        /// FN:Rene van der Harten
        /// N;SORT-AS="Harten,Rene":van der Harten;Rene,J.;Sir;R.D.O.N.
        /// </code>
        /// </example>
        /// <remarks>
        /// In vCard 3.0 wird eine separate SORT-STRING - Property eingefügt, in die lediglich der erste <see cref="string"/>
        /// übernommen wird.
        /// </remarks>
        SortAs,


        /// <summary>
        /// Nicht standardisierte Attribute. (2,3,4)
        /// </summary>
        NonStandard,


        /// <summary>
        /// PID Property-ID: Um eine bestimmte Property unter verschiedenen Instanzen derselben Property
        /// zu identifizieren. (4)
        /// </summary>
        PID,


        /// <summary>
        /// ALTID Ein String, der zu erkennen gibt, dass mehrere Instanzen derselben Property dasselbe 
        /// darstellen (z.B. in unterschiedlichen Sprachen). (4)
        /// </summary>
        AltID,


        /// <summary>
        /// INDEX RFC 6715: Used in a multi-valued property to indicate the position of
        /// this value within the set of values. (4 Erweiterung)
        /// </summary>
        Index


    }
}
