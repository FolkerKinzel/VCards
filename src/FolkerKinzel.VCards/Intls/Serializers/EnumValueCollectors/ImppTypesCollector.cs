using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class ImppTypesCollector
{
    /// <summary>
    /// Sammelt die Namen der in <paramref name="imppType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="imppType"><see cref="ImppTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum Sammeln.</param>
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
