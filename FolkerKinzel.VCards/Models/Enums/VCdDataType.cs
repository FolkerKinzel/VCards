namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Datentyp des Inhalts einer vCard-Property anzugeben.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Bezeichner enthält Typnamen", Justification = "<Ausstehend>")]
    public enum VCdDataType
    {
        // ACHTUNG: Wenn die Enum erweitert wird, muss
        // VCdDataTypeConverter angepasst werden!
        //
        // Die Wertzuweisung als Flags ist nötig, um inkompatible Werte leichter auszufiltern!

        /// <summary>
        /// <c>TEXT</c>: Für Menschen lesbarer Text. <c>(3,4)</c>
        /// </summary>
        Text = 1,

        /// <summary>
        /// <c>URI</c>: Uri <c>(3,4)</c>
        /// </summary>
        Uri = 1 << 1,

        /// <summary>
        /// <c>DATE</c>: Datum <c>(3,4)</c>
        /// </summary>
        Date = 1 << 2,
        // yyyy[mmdd]
        // yyyy-mm    // diese Variante selten
        // --mm[dd]   // diese Variante selten
        // ---dd
        // yyyy-mm-dd


        /// <summary>
        /// <c>TIME</c>: Zeitangabe <c>(3,4)</c>
        /// </summary>
        Time = 1 << 3,
        // hh[mm[ss]][+/-hh[mm]]
        // -mm[ss][+/-hh[mm]]
        // --ss[+/-hh[mm]]
        // hh[mm[ss]][Z] // "Z" bedeutet UTC
        // -mm[ss][Z]
        // --ss[Z]


        /// <summary>
        /// <c>DATE-TIME</c>: Kombination aus Datums- und Zeitangabe <c>(3,4)</c>
        /// </summary>
        DateTime = 1 << 4,
        // yyyymmddThh[mm[ss]][+/-hh[mm]]
        // --mmddThh[mm[ss]][+/-hh[mm]]
        // ---ddThh[mm[ss]][+/-hh[mm]]
        // yyyymmddThh[mm[ss]][Z]        // "Z" bedeutet UTC
        // --mmddThh[mm[ss]][Z]
        // ---ddThh[mm[ss]][Z]

        /// <summary>
        /// <c>DATE-AND-OR-TIME</c>: Date, Time oder DateTime. <c>(4)</c>
        /// </summary>
        DateAndOrTime = 1 << 5,
        // A stand-alone TIME value is always preceded by a "T".

        /// <summary>
        /// <c>TIMESTAMP</c>: Zeitstempel <c>(4)</c>
        /// </summary>
        Timestamp = 1 << 6,
        // yyyymmddThhmmss[+/-hh[mm]]
        // yyyymmddThhmmss[Z]          // "Z" bedeutet UTC


        /// <summary>
        /// <c>BOOLEAN</c>: Boolscher Wert <c>(3,4)</c>
        /// </summary>
        Boolean = 1 << 7, // TRUE / FALSE (nicht case-sensitiv)

        /// <summary>
        /// <c>INTEGER</c>: Ganzzahliger Typ mit Vorzeichen <c>(3,4)</c>
        /// </summary>
        Integer = 1 << 8, // [sign] 1*DIGIT

        /// <summary>
        /// <c>FLOAT</c>: Fließkommazahl mit Vorzeichen <c>(3,4)</c>
        /// </summary>
        Float = 1 << 9, // [sign] 1*DIGIT ["." 1*DIGIT]

        /// <summary>
        /// <c>UTC-OFFSET</c>: Offset von der Standardzeit <c>(3,4)</c>
        /// </summary>
        UtcOffset = 1 << 10, // [+/-hh[mm]]

        /// <summary>
        /// <c>LANGUAGE-TAG</c>: Sprachangabe nach RFC 5646 <c>(4)</c>
        /// </summary>
        LanguageTag = 1 << 11, // z.B. de-DE


        /// <summary>
        /// <c>BINARY</c>: Base64-codierte binäre Daten <c>(3)</c>
        /// </summary>
        Binary = 1 << 12,


        /// <summary>
        /// <c>PHONE-NUMBER</c>: Telefonnummer <c>(3)</c>
        /// </summary>
        PhoneNumber = 1 << 13,


        /// <summary>
        /// <c>VCARD</c>: Eingebettete vCard <c>(3)</c>
        /// </summary>
        VCard = 1 << 14
    }
}