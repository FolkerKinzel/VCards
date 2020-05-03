using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um in vCards die Art einer Telefonnummer zu beschreiben. Die Konstanten können kombiniert werden.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.TelTypesExtension"/>.</note>
    [Flags]
    public enum TelTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, müssen TelTypesConverter 
        // und TelTypesCollector angepasst werden!


        /// <summary>
        /// VOICE Telefonnummer für Sprachkommunikation (Default). (2,3,4)
        /// </summary>
        Voice = 1,

        /// <summary>
        /// FAX Fax-Nummer (2,3,4)
        /// </summary>
        Fax = 1 << 1,

        /// <summary>
        /// MSG Indicates a messaging service on the number. (2,3)
        /// </summary>
        Msg = 1 << 2,

        /// <summary>
        /// CELL Handy (2,3,4)
        /// </summary>
        Cell = 1 << 3,

        /// <summary>
        /// PAGER Pager-Nummer (2,3,4)
        /// </summary>
        Pager = 1 << 4,

        /// <summary>
        /// BBS Mailbox (Bulletin board system) (2,3)
        /// </summary>
        BBS = 1 << 5,

        /// <summary>
        /// MODEM Modem (2,3)
        /// </summary>
        Modem = 1 << 6,

        /// <summary>
        /// CAR Autotelefon (2,3)
        /// </summary>
        Car = 1 << 7,

        /// <summary>
        /// ISDN ISDN-Nummer (2,3)
        /// </summary>
        ISDN = 1 << 8,

        /// <summary>
        /// VIDEO Telefonnummer für Videokonferenzen (2,3,4)
        /// </summary>
        Video = 1 << 9,

        /// <summary>
        /// PCS Mobilfunkdienst (Personal communication services) (3)
        /// </summary>
        PCS = 1 << 10, // nur vCard 3.0

        /// <summary>
        /// TEXTPHONE Telefonnummer für Menschen mit Hör- oder 
        /// Sprachstörungen. (4)
        /// </summary>
        TextPhone = 1 << 11,

        /// <summary>
        /// Die Telefonnummer unterstützt Textnachrichten (SMS). (4)
        /// </summary>
        Text = 1 << 12
    }
}