using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class TelTypesCollector
{
    /// <summary />
    /// <param name="telType" />
    /// <param name="list" />
    internal static void CollectValueStrings(TelTypes? telType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!telType.HasValue)
        {
            return;
        }

        TelTypes value = telType.Value & TelTypesConverter.DEFINED_TEL_TYPES_VALUES;

        for (int i = TelTypesConverter.TEL_TYPES_MIN_BIT; i <= TelTypesConverter.TEL_TYPES_MAX_BIT; i++)
        {
            TelTypes flag = (TelTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
