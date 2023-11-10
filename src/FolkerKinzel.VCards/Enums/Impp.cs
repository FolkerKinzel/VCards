using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to specify the type of an instant messenger handle
/// in vCard&#160;3.0. The constants can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the <see
/// cref="ImppExtension" /> class. 
/// </note>
/// </remarks>
[Flags]
public enum Impp
{
    // CAUTION: If the enum is expanded, ImppConverter and
    // EnumValueCollector must be adjusted!

    /// <summary>Personal</summary>
    Personal = 1,

    /// <summary>Business</summary>
    Business = 2,

    /// <summary>Mobile</summary>
    Mobile = 4
}
