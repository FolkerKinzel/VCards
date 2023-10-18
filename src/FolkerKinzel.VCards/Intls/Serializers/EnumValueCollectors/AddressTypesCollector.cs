using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class AddressTypesCollector
{
    /// <summary />
    /// <param name="addressType" />
    /// <param name="list" />
    internal static void CollectValueStrings(AddressTypes? addressType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!addressType.HasValue)
        {
            return;
        }

        AddressTypes value = addressType.Value & AddressTypesConverter.DEFINED_ADDRESS_TYPES_VALUES;

        for (int i = AddressTypesConverter.ADDRESS_TYPES_MIN_BIT; i <= AddressTypesConverter.ADDRESS_TYPES_MAX_BIT; i++)
        {
            AddressTypes flag = (AddressTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
