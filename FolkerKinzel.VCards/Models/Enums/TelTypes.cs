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
        /// <c>VOICE</c>: Telefonnummer für Sprachkommunikation (Default). (2,3,4)
        /// </summary>
        Voice = 1,

        /// <summary>
        /// <c>FAX</c>: Fax-Nummer (2,3,4)
        /// </summary>
        Fax = 1 << 1,

        /// <summary>
        /// <c>MSG</c>: Voice Message (z.B. MMS). (2,3)
        /// </summary>
        Msg = 1 << 2,

        /// <summary>
        /// <c>CELL</c>: Handy (2,3,4)
        /// </summary>
        Cell = 1 << 3,

        /// <summary>
        /// <c>PAGER</c>: Pager-Nummer (2,3,4)
        /// </summary>
        Pager = 1 << 4,

        /// <summary>
        /// <c>BBS</c>: Anrufbeantworter (Bulletin board system) (2,3)
        /// </summary>
        BBS = 1 << 5,

        /// <summary>
        /// <c>MODEM</c>: Modem (2,3)
        /// </summary>
        Modem = 1 << 6,

        /// <summary>
        /// <c>CAR</c>: Autotelefon (2,3)
        /// </summary>
        Car = 1 << 7,

        /// <summary>
        /// <c>ISDN</c>: ISDN-Nummer (2,3)
        /// </summary>
        ISDN = 1 << 8,

        /// <summary>
        /// <c>VIDEO</c>: Telefonnummer für Videokonferenzen (2,3,4)
        /// </summary>
        Video = 1 << 9,

        /// <summary>
        /// <c>PCS</c>: Mobilfunkdienst (Personal communication services) (3)
        /// </summary>
        PCS = 1 << 10, // nur vCard 3.0

        /// <summary>
        /// <c>TEXTPHONE</c>: Telefonnummer für Menschen mit Hör- oder 
        /// Sprachstörungen. (4)
        /// </summary>
        TextPhone = 1 << 11,

        /// <summary>
        /// <c>TEXT</c>: Die Telefonnummer unterstützt Textnachrichten (SMS). (4)
        /// </summary>
        Text = 1 << 12
    }
}