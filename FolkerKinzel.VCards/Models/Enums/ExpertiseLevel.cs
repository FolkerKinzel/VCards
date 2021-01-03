using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Parameter <see cref="ParameterSection.ExpertiseLevel"/> in der 
    /// Eigenschaft <see cref="VCard.Expertises">VCard.Expertises</see> anzugeben. <c>(RFC 6715)</c>
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
