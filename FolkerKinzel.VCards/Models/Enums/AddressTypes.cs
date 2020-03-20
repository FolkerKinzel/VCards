using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um in vCards die Art einer Postanschrift zu beschreiben. Die Konstanten können
    /// kombiniert werden.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.AddressTypesExtensions"/>.</note>
    [Flags]
    public enum AddressTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen 
        // AddressTypesConverter und AddressTypesCollector angepasst werden!

        /// <summary>
        /// DOM inländische Adresse
        /// </summary>
        Dom = 1,


        /// <summary>
        /// INTL internationale Adresse
        /// </summary>
        Intl = 2,


        /// <summary>
        /// POSTAL Postanschrift
        /// </summary>
        Postal = 4,


        /// <summary>
        /// PARCEL Anschrift für die Zustellung von Paketen (Lieferadresse)
        /// </summary>
        Parcel = 8


    }
}