using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class PropertyClassTypesCollector
{
    /// <summary />
    /// <param name="propertyClassType" />
    /// <param name="list" />
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
