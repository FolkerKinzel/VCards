using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK.VCards.Intls.Encodings.QuotedPrintable
{
    /// <summary>
    /// Benannte Konstanten, um die Quoted-Printable-Kodierung zu steuern.
    /// </summary>
    public enum EncodingPreference
    {
        /// <summary>
        /// Um bestmögliche Lesbarkeit für Menschen zu gewährleisten, werden alle erlaubten Zeichen unmaskiert gelassen,
        /// ebenso Zeilenwechselzeichen.
        /// </summary>
        Readability,

        /// <summary>
        /// Entspricht Readability; Zeilenwechselzeichen werden aber maskiert.
        /// </summary>
        ReadabilityWithoutHardLineBreaks,

        /// <summary>
        /// Um höchstmögliche Datenintegrität und Kompatibilität zu gewährleisten werden neben den Zeilenwechselzeichen
        /// auch die Zeichen !"#$@[\]^`{|}~ maskiert.
        /// </summary>
        Compatibility
    }
}
