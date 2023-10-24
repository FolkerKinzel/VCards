using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class AddressTypesCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="addressType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="addressType">The <see cref="AddressTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
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
