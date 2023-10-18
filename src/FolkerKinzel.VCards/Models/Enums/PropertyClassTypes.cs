using FolkerKinzel.VCards.Extensions;

namespace FolkerKinzel.VCards.Models.Enums;

    /// <summary>Named constants to classify the scope of a vCard property. The constants
    /// can be combined.</summary>
    /// <remarks>
    /// <note type="tip">
    /// Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus <see
    /// cref="PropertyClassTypesExtension" />.
    /// </note>
    /// </remarks>
[Flags]
public enum PropertyClassTypes
{
    // ACHTUNG: Wenn die Enum erweitert wird, müssen
    // VCardPropertyParameters.ParseTypeValue(string typeValue, string propertyKey)
    // und PropertyClassTypesCollector angepasst werden!

    /// <summary> <c>HOME</c>: Implies that the property is related to an individual's
    /// personal life.</summary>
    Home = 1,


    /// <summary> <c>WORK</c>: Implies that the property is related to an individual's
    /// work place.</summary>
    Work = 2
}
