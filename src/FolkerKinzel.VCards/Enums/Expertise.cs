using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Enums;

/// <summary>
/// Named constants to specify the parameter <see cref="ParameterSection.Expertise" /> 
/// in the property <see cref="VCard.Expertises">VCard.Expertises</see>. <c>(RFC 6715)</c>
/// </summary>
public enum Expertise
{
    // CAUTION: If the enum is expanded, ExpertiseConverter
    // must be adjusted!

    /// <summary>Beginner</summary>
    Beginner,

    /// <summary>Average</summary>
    Average,

    /// <summary>Expert</summary>
    Expert
}
