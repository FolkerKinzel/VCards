using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK.VCards.Intls.Encodings.QuotedPrintable
{
    /// <summary>
    /// Benannte Konstanten, um einzustellen, wie in einer QuotedPrintable-Dekodierung mit Whitespace an Zeilenenden umgegangen wird.
    /// </summary>
    internal enum TrimBehavior
    {
        /// <summary>
        /// Whitespace an Zeilenenden wird automatisch entfernt. Dieses Verhalten entspricht dem Standard.
        /// </summary>
        RemoveTrailingWhiteSpace,

        /// <summary>
        /// Whitespace an Zeilenenden wird ignoriert. Dieses Verhalten entspricht nicht dem offiziellen QuotedPrintable-Standard.
        /// </summary>
        DoNotRemoveTrailingWhiteSpace
    }
}
