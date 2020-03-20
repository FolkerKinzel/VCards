using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um auf Eigenschaften eines <see cref="VCard"/>-Objekts zuzugreifen.
    /// </summary>
    public enum VCdProp
    {
        /// <summary>
        /// PROFILE Legt fest, dass die VCF-Datei eine vCard ist. (3)
        /// </summary>
        Profile,

        /// <summary>
        /// KIND Art des Objekts, das die vCard beschreibt: Eine Person, eine Organisation oder 
        /// eine Gruppe. (4)
        /// </summary>
        Kind,


        /// <summary>
        /// REV Zeitstempel der letzten Aktualisierung der vCard. (2,3,4)
        /// </summary>
        LastRevision,


        /// <summary>
        /// UID UUID, die eine persistente, global eindeutige Kennung des verbundenen Objekts darstellt. (2,3,4)
        /// </summary>
        UniqueIdentifier,

        /// <summary>
        /// CATEGORIES Liste von Eigenschaften, die das Objekt der vCard beschreiben. (3, 4)
        /// </summary>
        Categories,

        /// <summary>
        /// TZ Zeitzone(n) des vCard Objekts. (2,3,4)
        /// </summary>
        TimeZones,

        /// <summary>
        /// GEO Längen- und Breitengrad(e). (2,3,4)
        /// </summary>
        GeoCoordinates,

        /// <summary>
        /// CLASS Sensibilität der in der vCard enthaltenen Daten. (Nur vCard 3.0).
        /// </summary>
        Access,

        /// <summary>
        /// SOURCE URLs, die verwendet werden können, um die neueste Version dieser vCard zu 
        /// erhalten. (3,4)
        /// </summary>
        Sources,


        /// <summary>
        /// NAME
        /// Anzeigbarer Name der SOURCE-Eigenschaft (3)
        /// </summary>
        DirectoryName,


        /// <summary>
        /// MAILER Art des genutzten E-Mail-Programms. (2,3)
        /// </summary>
        Mailer,

        /// <summary>
        /// PRODID Kennung für das Produkt, mit dem die VCF-Datei erstellt wurde. (3,4)
        /// </summary>
        /// <remarks>
        /// Implementations SHOULD use a method such as that
        /// specified for Formal Public Identifiers in [ISO9070] or for
        /// Universal Resource Names in [RFC3406] to ensure that the text
        /// value is unique.
        /// </remarks>
        ProdID,


        /// <summary>
        /// FN Formatierte Zeichenfolge mit dem/den vollständigen Namen des vCard-Objekts. (2,3,4)
        /// </summary>
        DisplayNames,

        /// <summary>
        /// N Name der Person oder Organisation, die die vCard repräsentiert. (2,3,4)
        /// </summary>
        NameViews,

        /// <summary>
        /// GENDER Geschlecht (4)
        /// </summary>
        GenderViews,

        /// <summary>
        /// NICKNAME Ein oder mehrere Alternativnamen für das Objekt, das von der vCard repräsentiert wird. (3,4)
        /// </summary>
        NickNames,

        /// <summary>
        /// TITLE Angabe der Stellenbezeichnung, funktionellen Stellung oder Funktion der mit dem vCard-Objekt 
        /// verbundenen Person innerhalb einer Organisation, z.B. “Vizepräsident”. (2,3,4)
        /// </summary>
        Titles,

        /// <summary>
        /// ROLE Rolle, Beruf oder Wirtschaftskategorie des vCard-Objekts innerhalb 
        /// einer Organisation, z.B. "rechte Hand des Chefs". (2,3,4)
        /// </summary>
        Roles,

        /// <summary>
        /// ORG Name und gegebenenfalls Einheit(en) der Organisation, der das vCard-Objekt zugeordnet ist. (2,3,4)
        /// </summary>
        Organizations,

        /// <summary>
        /// BDAY Geburtstag (2,3,4)
        /// </summary>
        BirthDayViews,

        /// <summary>
        /// BIRTHPLACE Geburtsort (Erweiterung RFC 6474)
        /// </summary>
        BirthPlaceViews,

        /// <summary>
        /// ANNIVERSARY Jahrestag (gemeint ist i. A. der Hochzeitstag) der Person. (4)
        /// </summary>
        AnniversaryViews,

        /// <summary>
        /// DEATHDATE Todestag (Erweiterung RFC 6474)
        /// </summary>
        DeathDateViews,

        /// <summary>
        /// DEATHPLACE Sterbeort (Erweiterung RFC 6474)
        /// </summary>
        DeathPlaceViews,


        /// <summary>
        /// ADR Adressen (2,3,4)
        /// </summary>
        Addresses,

        /// <summary>
        /// TEL Telefonnummern (2,3,4)
        /// </summary>
        PhoneNumbers,

        /// <summary>
        /// EMAIL E-Mail-Adressen  (2,3,4)
        /// </summary>
        EmailAddresses,

        /// <summary>
        /// URL URLs, die die Person repräsentieren. (Webseiten, Blogs, Social-Media-Seiten) (2,3,4)
        /// </summary>
        URLs,

        /// <summary>
        /// IMPP Liste von Instant-Messenger-Handles. (3,4)
        /// </summary>
        InstantMessengerHandles,

        /// <summary>
        /// KEY Öffentliche Schlüssel, die dem vCard-Objekt zugeordnet sind. (2,3,4)
        /// </summary>
        /// <remarks>
        /// Es kann zu einer externen URL verwiesen werden, 
        /// Klartext angegeben oder ein Base64-kodierter Textblock in die vCard eingebettet werden.
        /// </remarks>
        Keys,


        /// <summary>
        /// CALURI URLs zum Kalender der Person. (4)
        /// </summary>
        CalendarAddresses,


        /// <summary>
        /// CALADRURI URLs für das Senden einer Terminanforderung an die Person oder Organisation. (4)
        /// </summary>
        CalendarUserAddresses,

        /// <summary>
        /// RELATED Andere Einheiten, zu der die Person Verbindung hat. (4)
        /// </summary>
        /// <remarks>Zulässige Werte sind:
        /// <list type="bullet">
        /// <item>eine „mailto:“-URL, die eine E-Mail-Adresse enthält</item>
        /// <item>eine UUID, die auf die eigene vCard des Mitglieds verweist</item>
        /// </list>
        /// </remarks>
        Relations,

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
        Members,


        /// <summary>
        /// ORG-DIRECTORY URIs, die die Arbeitsplätze der Person repräsentieren. Damit können Informationen 
        /// über Mitarbeiter der Person eingeholt werden. (Erweiterung RFC 6715)
        /// </summary>
        OrgDirectories,


        /// <summary>
        /// EXPERTISE Fachgebiete, über die die Person Kenntnisse hat (Erweiterung RFC 6715)
        /// </summary>
        Expertises,

        /// <summary>
        /// INTEREST Freizeitbeschäftigungen, für die sich die Person interessiert, an denen sie 
        /// aber nicht zwangsläufig teilnimmt. (Erweiterung RFC 6715)
        /// </summary>
        Interests,

        /// <summary>
        /// HOBBY Freizeitbeschäftigungen, denen die Person nachgeht (Erweiterung RFC 6715)
        /// </summary>
        Hobbies,

        /// <summary>
        /// LANG Sprachen, die die Person spricht. (4)
        /// </summary>
        Languages,

        /// <summary>
        /// NOTE Kommentar(e) (2,3,4)
        /// </summary>
        Notes,

        /// <summary>
        /// XML Beliebige zusätzliche XML-Daten (4)
        /// </summary>
        XmlProperties,


        /// <summary>
        /// LOGO Logo(s) der Organisation, mit der die Person in Beziehung steht, der die vCard gehört. (2,3,4)
        /// </summary>
        Logos,

        /// <summary>
        /// PHOTO Bild(er) oder Fotografie(n) der mit der vCard verbundenen Person. (2,3,4)
        /// </summary>
        /// <remarks> Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock 
        /// in die vCard eingebettet werden.</remarks>
        Photos,

        /// <summary>
        /// SOUND Gibt standardmäßig die Aussprache der FN-Eigenschaft des vCard-Objekts an, 
        /// wenn diese Eigenschaft nicht mit anderen Eigenschaften verknüpft ist. (2,3,4)
        /// </summary>
        /// <remarks> Es kann auf eine externe URL verwiesen oder ein Base64-kodierter Textblock 
        /// in die vCard eingebettet werden.</remarks>
        Sounds,


        /// <summary>
        /// CLIENTPIDMAP Mappings für Property-IDs (PID) (4)
        /// </summary>
        PropertyIDMappings,

        /// <summary>
        /// vCard-Properties, die nicht dem Standard entsprechen.
        /// </summary>
        NonStandardProperties


    }
}
