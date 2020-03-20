namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Level-Parameter in der Erweiterung "INTEREST" anzugeben. (RFC 6715)
    /// </summary>
    public enum InterestLevel
    {
        // ACHTUNG: Wenn die Enum erweitert wird, muss
        // InterestLevelConverter angepasst werden!

        /// <summary>
        /// hoch
        /// </summary>
        High,

        /// <summary>
        /// mittel
        /// </summary>
        Medium,

        /// <summary>
        /// gering
        /// </summary>
        Low
    }
}
