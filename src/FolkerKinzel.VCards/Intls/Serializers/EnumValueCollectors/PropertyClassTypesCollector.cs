using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class PropertyClassTypesCollector
{
    /// <summary>
    /// Sammelt die Namen der in <paramref name="propertyClassType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="propertyClassType"><see cref="PropertyClassTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum sammeln.</param>
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
