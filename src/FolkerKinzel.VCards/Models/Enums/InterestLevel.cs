using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um den Parameter <see cref="ParameterSection.InterestLevel"/> in den 
    /// Eigenschaften <see cref="VCard.Hobbies">VCard.Hobbies</see> und <see cref="VCard.Interests">VCard.Interests</see> anzugeben. <c>(RFC 6715)</c>
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
