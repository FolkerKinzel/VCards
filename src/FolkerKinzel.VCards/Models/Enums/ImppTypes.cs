using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>Named constants to specify the type of an instant messenger handle
/// in vCard 3.0. The constants can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the <see
/// cref="ImppTypesExtension" /> class. 
/// </note>
/// </remarks>
[Flags]
public enum ImppTypes
{
    // CAUTION: If the enum is expanded, ImppTypesConverter and
    // ImppTypesCollector must be adjusted!

    /// <summary>Personal</summary>
    Personal = 1,

    /// <summary>Business</summary>
    Business = 2,

    /// <summary>Mobile</summary>
    Mobile = 4
}
