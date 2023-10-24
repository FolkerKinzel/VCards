using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class ImppTypesCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="imppType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="imppType">The <see cref="ImppTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
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
