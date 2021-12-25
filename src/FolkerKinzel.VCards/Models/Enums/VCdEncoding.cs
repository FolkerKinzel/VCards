namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Benannte Konstanten, um die Kodierung des Inhalts einer vCard-Property anzugeben.
/// </summary>
public enum VCdEncoding
{
    /// <summary>
    /// <c>B</c>, <c>BASE64</c>: Base64 <c>(2,3)</c>
    /// </summary>
    Base64,

    /// <summary>
    /// <c>QUOTED-PRINTABLE</c>: Quoted Printable <c>(2)</c>
    /// </summary>
    QuotedPrintable,

    /// <summary>
    /// <c>8BIT</c>: 8-Bit-Zeichensatz <c>(2)</c>
    /// </summary>
    Ansi,


    // Ascii (7-Bit) is the standard encoding in vCard 2.1 and does not have to be indicated.
    // Ascii
}
