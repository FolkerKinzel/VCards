using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Enums;

    /// <summary>Named constants to specify the type of an instant messenger handle
    /// in vCard 3.0. The constants can be combined.</summary>
    /// <remarks>
    /// <note type="tip">
    /// Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus der <see
    /// cref="ImppTypesExtension" />-Klasse.
    /// </note>
    /// </remarks>
[Flags]
public enum ImppTypes
{
    // ACHTUNG: Wenn die Enum erweitert wird, müssen 
    // AddressTypesConverter und AddressTypesCollector angepasst werden!

    /// <summary>Personal</summary>
    Personal = 1,

    /// <summary>Business</summary>
    Business = 2,

    /// <summary>Mobile</summary>
    Mobile = 4
}
