using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class AddressTypesCollector
{
    /// <summary>
    /// Sammelt die Namen der in <paramref name="addressType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="addressType"><see cref="AddressTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum sammeln.</param>
    internal static void CollectValueStrings(AddressTypes? addressType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!addressType.HasValue)
        {
            return;
        }

        AddressTypes value = addressType.Value & AddressTypesConverter.DEFINED_ADDRESS_TYPES_VALUES;

        for (int i = AddressTypesConverter.AddressTypesMinBit; i <= AddressTypesConverter.AddressTypesMaxBit; i++)
        {
            AddressTypes flag = (AddressTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
