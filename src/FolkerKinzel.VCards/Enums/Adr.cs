using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Enums;

/// <summary>Named constants to describe the type of a postal address in vCards.
/// The constants can be combined.</summary>
/// <remarks>
/// <note type="tip">
/// When working with the enum use the extension methods from the 
/// <see cref="AdrExtension" /> class.
/// </note>
/// </remarks>
[Flags]
public enum Adr
{
    // CAUTION: If the enum is expanded, AdrConverter and
    // EnumValueCollector must be adjusted!

    /// <summary> <c>DOM</c>: Domestic delivery address</summary>
    Dom = 1,


    /// <summary> <c>INTL</c>: International delivery address</summary>
    Intl = 2,


    /// <summary> <c>POSTAL</c>: Postal delivery address</summary>
    Postal = 4,


    /// <summary> <c>PARCEL</c>: Parcel delivery address</summary>
    Parcel = 8
}
