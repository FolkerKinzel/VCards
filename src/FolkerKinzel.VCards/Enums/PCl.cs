using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to classify the scope of a vCard property. The constants
/// can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the <see
/// cref="PClExtension" /> class. 
/// </note>
/// </remarks>
[Flags]
public enum PCl
{
    // CAUTION: If the enum is expanded,
    // ParameterSection.ParseTypeParameter(string, string) and
    // EnumValueCollector must be adjusted!

    /// <summary> <c>HOME</c>: Implies that the property is related to an individual's
    /// personal life.</summary>
    Home = 1,

    /// <summary> <c>WORK</c>: Implies that the property is related to an individual's
    /// work place.</summary>
    Work = 2
}
