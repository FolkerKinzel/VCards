using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class PropertyClassTypesCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="propertyClassType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="propertyClassType">The <see cref="PropertyClassTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void CollectValueStrings(PropertyClassTypes? propertyClassType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!propertyClassType.HasValue)
        {
            return;
        }

        PropertyClassTypes value = propertyClassType.Value;

        if (value.HasFlag(PropertyClassTypes.Home))
        {
            list.Add(ParameterSection.TypeValue.HOME);
        }

        if (value.HasFlag(PropertyClassTypes.Work))
        {
            list.Add(ParameterSection.TypeValue.WORK);
        }
    }
}
