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
        /// <c>TEXT</c>: Für Menschen lesbarer Text. (3,4)
        /// </summary>
        Text = 1,

        /// <summary>
        /// <c>URI</c>: Uri (3,4)
        /// </summary>
        Uri = 1 << 1,

        /// <summary>
        /// <c>DATE</c>: Datum (3,4)
        /// </summary>
        /// <remarks>
        /// <code>
        /// yyyy[mmdd]
        /// yyyy-mm    // diese Variante selten
        /// --mm[dd]   // diese Variante selten
        /// ---dd
        /// yyyy-mm-dd
        /// </code>
        ///</remarks>
        Date = 1 << 2,

        /// <summary>
        /// <c>TIME</c>: Zeitangabe (3,4)
        /// </summary>
        /// <remarks>
        /// <code>
        /// hh[mm[ss]][+/-hh[mm]]
        /// -mm[ss][+/-hh[mm]]
        /// --ss[+/-hh[mm]]
        /// hh[mm[ss]][Z] // "Z" bedeutet UTC
        /// -mm[ss][Z]
        /// --ss[Z]
        /// </code>
        /// </remarks>
        Time = 1 << 3,


        /// <summary>
        /// <c>DATE-TIME</c>: Kombination aus Datums- und Zeitangabe (3,4)
        /// </summary>
        /// <remarks>
        /// <code>
        /// yyyymmddThh[mm[ss]][+/-hh[mm]]
        /// --mmddThh[mm[ss]][+/-hh[mm]]
        /// ---ddThh[mm[ss]][+/-hh[mm]]
        /// yyyymmddThh[mm[ss]][Z]        // "Z" bedeutet UTC
        /// --mmddThh[mm[ss]][Z]
        /// ---ddThh[mm[ss]][Z]
        /// </code>
        /// </remarks>
        DateTime = 1 << 4,


        /// <summary>
        /// <c>DATE-AND-OR-TIME</c>: Date, Time oder DateTime. (4)
        /// </summary>
        /// <remarks>
        /// A stand-alone TIME value is always preceded by a "T".
        /// </remarks>
        DateAndOrTime = 1 << 5,

        /// <summary>
        /// <c>TIMESTAMP</c>: Zeitstempel (4)
        /// </summary>
        /// <remarks>
        /// <code>
        /// yyyymmddThhmmss[+/-hh[mm]]
        /// yyyymmddThhmmss[Z]          // "Z" bedeutet UTC
        /// </code>
        /// </remarks>
        Timestamp = 1 << 6,


        /// <summary>
        /// <c>BOOLEAN</c>: Boolscher Wert (3,4)
        /// </summary>
        /// <remarks>
        /// TRUE / FALSE (nicht case-sensitiv)
        /// </remarks>
        Boolean = 1 << 7,

        /// <summary>
        /// <c>INTEGER</c>: Ganzzahliger Typ mit Vorzeichen (3,4)
        /// </summary>
        /// <remarks>
        /// [sign] 1*DIGIT
        /// </remarks>
        Integer = 1 << 8,

        /// <summary>
        /// <c>FLOAT</c>: Fließkommazahl mit Vorzeichen (3,4)
        /// </summary>
        /// <remarks>
        /// [sign] 1*DIGIT ["." 1*DIGIT]
        /// </remarks>
        Float = 1 << 9,

        /// <summary>
        /// <c>UTC-OFFSET</c>: Offset von der Standardzeit (3,4)
        /// </summary>
        /// <remarks>
        /// [+/-hh[mm]]
        /// </remarks>
        UtcOffset = 1 << 10,

        /// <summary>
        /// <c>LANGUAGE-TAG</c>: Sprachangabe nach RFC 5646 (4)
        /// </summary>
        /// <remarks>
        /// z.B. de-DE
        /// </remarks>
        LanguageTag = 1 << 11,


        /// <summary>
        /// <c>BINARY</c>: Base64-codierte binäre Daten (3)
        /// </summary>
        Binary = 1 << 12,


        /// <summary>
        /// <c>PHONE-NUMBER</c>: Telefonnummer (3)
        /// </summary>
        PhoneNumber = 1 << 13,


        /// <summary>
        /// <c>VCARD</c>: Eingebettete vCard (3)
        /// </summary>
        VCard = 1 << 14
    }
}