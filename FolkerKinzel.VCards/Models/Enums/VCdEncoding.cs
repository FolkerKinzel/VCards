namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um die Kodierung des Inhalts einer vCard-Property anzugeben.
    /// </summary>
    public enum VCdEncoding
    {
        /// <summary>
        /// Base64
        /// </summary>
        Base64,

        /// <summary>
        /// Quoted Printable (nur vCard 2.1)
        /// </summary>
        QuotedPrintable,

        /// <summary>
        /// 8-Bit-Zeichensatz (nur vCard 2.1)
        /// </summary>
        Ansi,

        /// <summary>
        /// 7-Bit-Zeichensatz (nur vCard 2.1)
        /// </summary>
        Ascii
    }
}