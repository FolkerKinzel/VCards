using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class ImppTypesCollector
{
    /// <summary />
    /// <param name="imppType" />
    /// <param name="list" />
    internal static void CollectValueStrings(ImppTypes? imppType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!imppType.HasValue)
        {
            return;
        }

        ImppTypes value = imppType.Value & ImppTypesConverter.DEFINED_IMPP_TYPES_VALUES;

        for (int i = ImppTypesConverter.IMPP_TYPES_MIN_BIT; i <= ImppTypesConverter.IMPP_TYPES_MAX_BIT; i++)
        {
            ImppTypes flag = (ImppTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
