using FolkerKinzel.VCards.Resources;
using System;

namespace FolkerKinzel.VCards.Models.Enums
{
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

        /// <summary>
        /// 7-Bit-Zeichensatz (Default in vCard 2.1) <c>(2)</c>
        /// </summary>
        [Obsolete("Ascii (7-Bit) is the standard encoding in vCard 2.1 and does not have to be indicated. Therefore this enum value will be removed in the next major version.")]
        Ascii
    }
}