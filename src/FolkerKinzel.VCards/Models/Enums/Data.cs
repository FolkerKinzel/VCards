namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>Named constants to specify the data type of the value of a vCard property.</summary>
public enum Data
{
    // CAUTION: If the enum is expanded, DataConverter must be adjusted!
    // 
    // The value assignment as flags is necessary to filter out incompatible values more easily.

    /// <summary> <c>TEXT</c>: Values that contain human-readable text. <c>(3,4)</c></summary>
    Text = 1,

    /// <summary> <c>URI</c>: Values that are referenced by a Uniform Resource Identifier
    /// (URI) instead of encoded in-line. <c>(3,4)</c></summary>
    Uri = 1 << 1,

    /// <summary> <c>DATE</c>: A calendar date. <c>(3,4)</c></summary>
    Date = 1 << 2,
    // yyyy[mmdd]
    // yyyy-mm    // diese Variante selten
    // --mm[dd]   // diese Variante selten
    // ---dd
    // yyyy-mm-dd


    /// <summary> <c>TIME</c>: A time of day. <c>(3,4)</c></summary>
    Time = 1 << 3,
    // hh[mm[ss]][+/-hh[mm]]
    // -mm[ss][+/-hh[mm]]
    // --ss[+/-hh[mm]]
    // hh[mm[ss]][Z] // "Z" bedeutet UTC
    // -mm[ss][Z]
    // --ss[Z]


    /// <summary> <c>DATE-TIME</c>: A date and time of day combination. <c>(3,4)</c></summary>
    DateTime = 1 << 4,
    // yyyymmddThh[mm[ss]][+/-hh[mm]]
    // --mmddThh[mm[ss]][+/-hh[mm]]
    // ---ddThh[mm[ss]][+/-hh[mm]]
    // yyyymmddThh[mm[ss]][Z]        // "Z" means UTC
    // --mmddThh[mm[ss]][Z]
    // ---ddThh[mm[ss]][Z]


    /// <summary> <c>DATE-AND-OR-TIME</c>: <see cref="Date" />, <see cref="Time" />,
    /// or <see cref="DateTime" />. <c>(4)</c></summary>
    DateAndOrTime = 1 << 5,
    // A stand-alone TIME value is always preceded by a "T".


    /// <summary> <c>TIMESTAMP</c>: A complete date and time of day combination as specified
    /// in [ISO.8601.2004], Section 4.3.2. <c>(4)</c></summary>
    TimeStamp = 1 << 6,
    // yyyymmddThhmmss[+/-hh[mm]]
    // yyyymmddThhmmss[Z]          // "Z" means UTC


    /// <summary> <c>BOOLEAN</c>: A boolean value. <c>(3,4)</c></summary>
    Boolean = 1 << 7, // TRUE / FALSE (nicht case-sensitiv)

    /// <summary> <c>INTEGER</c>: Signed integer values in decimal format. <c>(3,4)</c></summary>
    Integer = 1 << 8, // [sign] 1*DIGIT

    /// <summary> <c>FLOAT</c>: Real numbers. <c>(3,4)</c></summary>
    Float = 1 << 9, // [sign] 1*DIGIT ["." 1*DIGIT]

    /// <summary> <c>UTC-OFFSET</c>: The value is a signed offset from UTC. <c>(3,4)</c></summary>
    UtcOffset = 1 << 10, // [+/-hh[mm]]

    /// <summary> <c>LANGUAGE-TAG</c>: A single language tag, as defined in RFC 5646.
    /// <c>(4)</c></summary>
    LanguageTag = 1 << 11, // e.g. de-DE

    /// <summary> <c>BINARY</c>: The value is inline, Base64-encoded binary data. <c>(3)</c></summary>
    Binary = 1 << 12,

    /// <summary> <c>PHONE-NUMBER</c>: The value is a telephone number. <c>(3)</c></summary>
    PhoneNumber = 1 << 13,

    /// <summary> <c>VCARD</c>: The value is another embedded vCard. <c>(3)</c></summary>
    VCard = 1 << 14
}
