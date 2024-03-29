using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Named constants to specify the parameter <see cref="ParameterSection.Interest"
/// /> in the properties <see cref="VCard.Hobbies">VCard.Hobbies</see> and <see
/// cref="VCard.Interests">VCard.Interests</see>. <c>(RFC 6715)</c>
/// </summary>
public enum InterestLevel
{
    // CAUTION: If the enum is expanded, InterestLevelConverter
    // must be adjusted!

    /// <summary>High</summary>
    High,

    /// <summary>Medium</summary>
    Medium,

    /// <summary>Low</summary>
    Low
}
