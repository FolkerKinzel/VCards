using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Enums;

/// <summary>
/// Named constants to specify the parameter <see cref="ParameterSection.Interest"
/// /> in the properties <see cref="VCard.Hobbies">VCard.Hobbies</see> and <see
/// cref="VCard.Interests">VCard.Interests</see>. <c>(RFC 6715)</c>
/// </summary>
public enum Interest
{
    // CAUTION: If the enum is expanded, InterestConverter
    // must be adjusted!

    /// <summary>High</summary>
    High,

    /// <summary>Medium</summary>
    Medium,

    /// <summary>Low</summary>
    Low
}
