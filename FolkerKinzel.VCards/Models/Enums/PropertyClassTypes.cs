using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Geltungsbereich einer vCard-Property zu klassifizieren.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.PropertyClassTypesExtensions"/>.</note>
    [Flags]
    public enum PropertyClassTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen
        // VCardPropertyParameters.ParseTypeValue(string typeValue, string propertyKey)
        // und PropertyClassTypesCollector angepasst werden!

        /// <summary>
        /// HOME privat
        /// </summary>
        Home = 1,


        /// <summary>
        /// WORK dienstlich
        /// </summary>
        Work = 2
    }
}
