using System;
using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Geltungsbereich einer vCard-Property zu klassifizieren. Die Konstanten können
    /// kombiniert werden.
    /// </summary>
    /// <remarks>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="PropertyClassTypesExtension"/>.</note>
    /// </remarks>
    [Flags]
    public enum PropertyClassTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen
        // VCardPropertyParameters.ParseTypeValue(string typeValue, string propertyKey)
        // und PropertyClassTypesCollector angepasst werden!

        /// <summary>
        /// <c>HOME</c>: privat
        /// </summary>
        Home = 1,


        /// <summary>
        /// <c>WORK</c>: dienstlich
        /// </summary>
        Work = 2
    }
}
