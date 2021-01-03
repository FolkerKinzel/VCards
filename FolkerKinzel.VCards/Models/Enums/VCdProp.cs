using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um auf Eigenschaften eines <see cref="VCard"/>-Objekts zuzugreifen.
    /// </summary>
    public enum VCdProp
    {
        /// <summary>
        /// <c>PROFILE</c>: Legt fest, dass die VCF-Datei eine vCard ist. <c>(3)</c>
        /// </summary>
        Profile,

        /// <summary>
        /// <c>KIND</c>: Art des Objekts, das die vCard beschreibt. <c>(4)</c>
        /// </summary>
        Kind,


        /// <summary>
        /// <c>REV</c>: Zeitstempel der letzten Aktualisierung der vCard. <c>(2,3,4)</c>
        /// </summary>
        LastRevision,


        /// <summary>
        /// <c>UID</c>: UUID, die eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. <c>(2,3,4)</c>
        /// </summary>
        UniqueIdentifier,

        /// <summary>
        /// <c>CATEGORIES</c>: Liste von Eigenschaften, die das Objekt der vCard beschreiben. <c>(3,4)</c>
        /// </summary>
        Categories,

        /// <summary>
        /// <c>TZ</c>: Zeitzone(n) des vCard Objekts. <c>(2,3,4)</c>
        /// </summary>
        TimeZones,

        /// <summary>
        /// <c>GEO</c>: Längen- und Breitengrad(e). <c>(2,3,4)</c>
        /// </summary>
        GeoCoordinates,

        /// <summary>
        /// <c>CLASS</c>: Sensibilität der in der vCard enthaltenen Daten. <c>(3)</c>
        /// </summary>
        Access,

        /// <summary>
        /// <c>SOURCE</c>: URLs, die verwendet werden können, um die neueste Version dieser vCard zu 
        /// erhalten. <c>(3,4)</c>
        /// </summary>
        Sources,


        /// <summary>
        /// <c>NAME</c>: 
        /// Anzeigbarer Name der <see cref="Sources"/>-Eigenschaft. <c>(3)</c>
        /// </summary>
        DirectoryName,


        /// <summary>
        /// <c>MAILER</c>: Art des genutzten E-Mail-Programms. <c>(2,3)</c>
        /// </summary>
        Mailer,

        /// <summary>
        /// <c>PRODID</c>: Kennung für das Produkt, mit dem die VCF-Datei erstellt wurde. <c>(3,4)</c>
        /// </summary>
        ProdID,


        /// <summary>
        /// <c>FN</c>: Formatierte Zeichenfolge mit dem/den vollständigen Namen des vCard-Objekts. <c>(2,3,4)</c>
        /// </summary>
        DisplayNames,

        /// <summary>
        /// <c>N</c>: Name der Person oder Organisation, die die vCard repräsentiert. <c>(2,3,4)</c>
        /// </summary>
        NameViews,

        /// <summary>
        /// <c>GENDER</c>: Geschlecht <c>(4)</c>
        /// </summary>
        GenderViews,

        /// <summary>
        /// <c>NICKNAME</c>: Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. <c>(3,4)</c>
        /// </summary>
        NickNames,

        /// <summary>
        /// <c>TITLE</c>: Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt 
        /// verbundenen Person innerhalb einer Organisation, z.B. “Vizepräsident”. <c>(2,3,4)</c>
        /// </summary>
        Titles,

        /// <summary>
        /// <c>ROLE</c>: Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb 
        /// einer Organisation, z.B. "rechte Hand des Chefs". <c>(2,3,4)</c>
        /// </summary>
        Roles,

        /// <summary>
        /// <c>ORG</c>: Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. <c>(2,3,4)</c>
        /// </summary>
        Organizations,

        /// <summary>
        /// <c>BDAY</c>: Geburtstag <c>(2,3,4)</c>
        /// </summary>
        BirthDayViews,

        /// <summary>
        /// <c>BIRTHPLACE</c>: Geburtsort <c>(RFC 6474)</c>
        /// </summary>
        BirthPlaceViews,

        /// <summary>
        /// <c>ANNIVERSARY</c>: Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person. <c>(4)</c>
        /// </summary>
        AnniversaryViews,

        /// <summary>
        /// <c>DEATHDATE</c>: Todestag <c>(RFC 6474)</c>
        /// </summary>
        DeathDateViews,

        /// <summary>
        /// <c>DEATHPLACE</c>: Sterbeort <c>(RFC 6474)</c>
        /// </summary>
        DeathPlaceViews,


        /// <summary>
        /// <c>ADR</c>: Adressen <c>(2,3,4)</c>
        /// </summary>
        Addresses,

        /// <summary>
        /// <c>TEL</c>: Telefonnummern <c>(2,3,4)</c>
        /// </summary>
        PhoneNumbers,

        /// <summary>
        /// <c>EMAIL</c>: E-Mail-Adressen  <c>(2,3,4)</c>
        /// </summary>
        EmailAddresses,

        /// <summary>
        /// <c>URL</c>: URLs, die die Person repräsentieren (Webseiten, Blogs, Social-Media-Seiten). <c>(2,3,4)</c>
        /// </summary>
        URLs,

        /// <summary>
        /// <c>IMPP</c>: Liste von Instant-Messenger-Handles. <c>(3,4)</c>
        /// </summary>
        InstantMessengerHandles,

        /// <summary>
        /// <c>KEY</c>: Öffentliche Schlüssel, die dem vCard-Objekt zugeordnet sind. <c>(2,3,4)</c>
        /// </summary>
        Keys,


        /// <summary>
        /// <c>CALURI</c>: URLs zum Kalender der Person. <c>(4)</c>
        /// </summary>
        CalendarAddresses,


        /// <summary>
        /// <c>CALADRURI</c>: URLs für das Senden einer Terminanforderung an die Person oder Organisation. <c>(4)</c>
        /// </summary>
        CalendarUserAddresses,

        /// <summary>
        /// <c>RELATED</c>: Andere Einheiten, zu der die Person Verbindung hat. <c>(4)</c>
        /// </summary>
        Relations,

        /// <summary>
        /// <c>MEMBER</c>: Definiert das Objekt, das die vCard repräsentiert, als Teil einer Gruppe. 
        /// Um diese Eigenschaft verwenden zu können, muss die <see cref="VCard.Kind"/>-Eigenschaft auf
        /// <see cref="VCdKind.Group"/> gesetzt werden. <c>(4)</c>
        /// </summary>
        Members,


        /// <summary>
        /// <c>ORG-DIRECTORY</c>: URIs, die die Arbeitsplätze der Person repräsentieren. Damit können Informationen 
        /// über Mitarbeiter der Person eingeholt werden. <c>(RFC 6715)</c>
        /// </summary>
        OrgDirectories,


        /// <summary>
        /// <c>EXPERTISE</c>: Fachgebiete, über die die Person Kenntnisse hat. <c>(RFC 6715)</c>
        /// </summary>
        Expertises,

        /// <summary>
        /// <c>INTEREST</c>: Freizeitbeschäftigungen, für die sich die Person interessiert, an denen sie 
        /// aber nicht zwangsläufig teilnimmt. <c>(RFC 6715)</c>
        /// </summary>
        Interests,

        /// <summary>
        /// <c>HOBBY</c>: Freizeitbeschäftigungen, denen die Person nachgeht. <c>(RFC 6715)</c>
        /// </summary>
        Hobbies,

        /// <summary>
        /// <c>LANG</c>: Sprachen, die die Person spricht. <c>(4)</c>
        /// </summary>
        Languages,

        /// <summary>
        /// <c>NOTE</c>: Kommentar(e) <c>(2,3,4)</c>
        /// </summary>
        Notes,

        /// <summary>
        /// <c>XML</c>: Beliebige zusätzliche XML-Daten. <c>(4)</c>
        /// </summary>
        XmlProperties,


        /// <summary>
        /// <c>LOGO</c>: Logo(s) der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. <c>(2,3,4)</c>
        /// </summary>
        Logos,

        /// <summary>
        /// <c>PHOTO</c>: Bild(er) oder Fotografie(n) der mit der vCard verbundenen Person. <c>(2,3,4)</c>
        /// </summary>
        Photos,

        /// <summary>
        /// <c>SOUND</c>: Demonstriert die Aussprache der <see cref="VCard.DisplayNames"/>-Eigenschaft des <see cref="VCard"/>-Objekts. <c>(2,3,4)</c>
        /// </summary>
        Sounds,


        /// <summary>
        /// <c>CLIENTPIDMAP</c>: Mappings für <see cref="PropertyID"/>s. Wird verwendet,
        /// um verschiedene Bearbeitungsstände derselben vCard zu synchronisieren. <c>(4)</c>
        /// </summary>
        PropertyIDMappings,

        /// <summary>
        /// vCard-Properties, die nicht dem Standard entsprechen.
        /// </summary>
        NonStandardProperties


    }
}
