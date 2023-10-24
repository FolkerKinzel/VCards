using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Named constants to specify the parameter <see cref="ParameterSection.Expertise" /> 
/// in the property <see cref="VCard.Expertises">VCard.Expertises</see>. <c>(RFC 6715)</c>
/// </summary>
public enum ExpertiseLevel
{
    // CAUTION: If the enum is expanded, ExpertiseLevelConverter
    // must be adjusted!

    /// <summary>Beginner</summary>
    Beginner,

    /// <summary>Average</summary>
    Average,

    /// <summary>Expert</summary>
    Expert
}
