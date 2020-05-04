using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um in vCards die Art einer Postanschrift zu beschreiben. Die Konstanten können
    /// kombiniert werden.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.AddressTypesExtension"/>.</note>
    [Flags]
    public enum AddressTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen 
        // AddressTypesConverter und AddressTypesCollector angepasst werden!

        /// <summary>
        /// <c>(DOM)</c>: inländische Adresse
        /// </summary>
        Dom = 1,


        /// <summary>
        /// <c>(INTL)</c>: internationale Adresse
        /// </summary>
        Intl = 2,


        /// <summary>
        /// <c>(POSTAL)</c>: Postanschrift
        /// </summary>
        Postal = 4,


        /// <summary>
        /// <c>(PARCEL)</c>: Anschrift für die Zustellung von Paketen (Lieferadresse)
        /// </summary>
        Parcel = 8


    }
}