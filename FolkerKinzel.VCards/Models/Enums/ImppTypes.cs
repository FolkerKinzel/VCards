using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um in vCard 3.0 die Art einer Instant-Messenger-Adresse anzugeben. Die Konstanten können
    /// kombiniert werden.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.ImppTypesExtension"/>.</note>
    [Flags]
    public enum ImppTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen 
        // AddressTypesConverter und AddressTypesCollector angepasst werden!

        /// <summary>
        /// privat
        /// </summary>
        Personal = 1,

        /// <summary>
        /// geschäftlich
        /// </summary>
        Business = 2,

        /// <summary>
        /// mobil
        /// </summary>
        Mobile = 4
    }
}
