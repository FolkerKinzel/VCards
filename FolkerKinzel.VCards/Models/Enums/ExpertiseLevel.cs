namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Level-Parameter in der Erweiterung "EXPERTISE" anzugeben. (RFC 6715)
    /// </summary>
    public enum ExpertiseLevel
    {
        // ACHTUNG: Wenn die Enum erweitert wird, muss
        // ExpertiseLevelConverter angepasst werden!

        /// <summary>
        /// Anfänger
        /// </summary>
        Beginner,

        /// <summary>
        /// Fortgeschrittener
        /// </summary>
        Average,

        /// <summary>
        /// Experte
        /// </summary>
        Expert
    }
}
