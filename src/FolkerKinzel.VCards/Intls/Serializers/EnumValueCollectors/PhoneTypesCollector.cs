using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class PhoneTypesCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="telType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="telType">The <see cref="PhoneTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
    internal static void CollectValueStrings(PhoneTypes? telType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!telType.HasValue)
        {
            return;
        }

        PhoneTypes value = telType.Value & PhoneTypesConverter.DEFINED_TEL_TYPES_VALUES;

        for (int i = PhoneTypesConverter.TEL_TYPES_MIN_BIT; i <= PhoneTypesConverter.TEL_TYPES_MAX_BIT; i++)
        {
            PhoneTypes flag = (PhoneTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
