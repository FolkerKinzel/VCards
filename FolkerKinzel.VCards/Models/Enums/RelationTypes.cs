using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um die Art der Beziehung zu einer Person oder Organisation zu beschreiben.
    /// </summary>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus 
    /// <see cref="Models.Helpers.RelationTypesExtension"/>.</note>
    [Flags]
    public enum RelationTypes
    {

        // ACHTUNG: Wenn die Enum erweitert wird, muss RelationTypesConverter
        // angepasst werden!

        /// <summary>
        /// <c>CONTACT</c>: Jemand, mit dem Sie Kontakt aufnehmen können. Oft symmetrisch.
        /// </summary>
        Contact = 1,

        /// <summary>
        /// <c>ACQUAINTANCE</c>: Jemand, mit dem Sie Grüße ausgetauscht haben und nicht viel mehr (wenn überhaupt) 
        /// - vielleicht ein kurzes Gespräch oder zwei.
        /// Oft symmetrisch.
        /// </summary>
        Acquaintance = 1 << 1,

        /// <summary>
        /// <c>FRIEND</c>: Jemand, mit dem Sie befreundet sind, ein Landsmann, ein Kumpel. Oft symmetrisch.
        /// </summary>
        Friend = 1 << 2,

        /// <summary>
        /// <c>MET</c>: Jemand, den Sie tatsächlich persönlich getroffen haben. Symmetrisch.
        /// </summary>
        Met = 1 << 3,

        /// <summary>
        /// <c>CO-WORKER</c>: Jemand, mit dem eine Person zusammenarbeitet oder der in derselben Organisation arbeitet. Symmetrisch. Normalerweise transitiv.
        /// </summary>
        CoWorker = 1 << 4,

        /// <summary>
        /// <c>COLLEAGUE</c>: Jemand im selben Studien- / Tätigkeitsbereich. Symmetrisch. Oft transitiv.
        /// </summary>
        Colleague = 1 << 5,

        /// <summary>
        /// <c>CO-RESIDENT</c>: Jemand, mit dem Sie in der selben Straße wohnen. Symmetrisch und transitiv.
        /// </summary>
        CoResident = 1 << 6,

        /// <summary>
        /// <c>NEIGHBOR</c>: Nachbar: Jemand, der in der Nähe wohnt, vielleicht im Nachbarhaus oder Tür an Tür. Symmetrisch. Oft transitiv.
        /// </summary>
        Neighbor = 1 << 7,

        /// <summary>
        /// <c>CHILD</c>: Kind: Der genetische Nachwuchs einer Person oder jemand, den eine Person adoptiert hat um und den sie sich kümmert. Inverse ist Elternteil.
        /// </summary>
        Child = 1 << 8,

        /// <summary>
        /// <c>PARENT</c>: Elternteil: Das Gegenteil von <see cref="Child"/>.
        /// </summary>
        Parent = 1 << 9,

        /// <summary>
        /// <c>SIBLING</c>: Geschwister: Jemand, mit dem eine Person ein Elternteil teilt. Symmetrisch. Normalerweise transitiv.
        /// </summary>
        Sibling = 1 << 10,

        /// <summary>
        /// <c>SPOUSE</c>: Ehepartner: Jemand, mit dem Sie verheiratet sind. Symmetrisch. Nicht transitiv.
        /// </summary>
        Spouse = 1 << 11,

        /// <summary>
        /// <c>KIN</c>: Verwandtschaft: Ein Verwandter, jemand, den Sie als Teil Ihrer Großfamilie betrachten. Symmetrisch und typisch transitiv.
        /// </summary>
        Kin = 1 << 12,

        /// <summary>
        /// <c>MUSE</c>: Muse: Jemand, der dir Inspiration bringt. Keine Umkehrung.
        /// </summary>
        Muse = 1 << 13,

        /// <summary>
        /// <c>CRUSH</c>: Jemand, in den du verknallt bist. Keine Umkehrung.
        /// </summary>
        Crush = 1 << 14,

        /// <summary>
        /// <c>DATE</c>: Jemand, mit dem Sie ein Date haben. Symmetrisch. Nicht transitiv.
        /// </summary>
        Date = 1 << 15,

        /// <summary>
        /// <c>SWEETHEART</c>: Jemand, mit dem Sie intim und zumindest etwas verbandelt sind, normalerweise exklusiv. Symmetrisch. Nicht transitiv.
        /// </summary>
        Sweetheart = 1 << 16,

        /// <summary>
        /// <c>ME</c>: Ich: Ein Link zu sich selbst unter einer anderen URL. Anders als alle anderen XFN-Werte. Notwendigerweise 
        /// symmetrisch. Es gibt eine implizite "Ich"-Relation vom Inhalt eines Verzeichnisses zum Verzeichnis selbst.
        /// </summary>
        Me = 1 << 17,

        /// <summary>
        /// <c>AGENT</c>: Person oder Organisation, die manchmal als Helfer der Person oder Organisation, auf die die vCard 
        /// ausgestellt ist, auftritt.
        /// </summary>
        Agent = 1 << 18,

        /// <summary>
        /// <c>EMERGENCY</c>: Kontakt für Notfälle.
        /// </summary>
        Emergency = 1 << 19
    }
}