using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class RelationTypesCollector
{
    /// <summary>
    /// Sammelt die Namen der in <paramref name="relationType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="relationType"><see cref="RelationTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum sammeln.</param>
    internal static void CollectValueStrings(RelationTypes? relationType, List<string> list)
    {
        Debug.Assert(list != null);

        if (!relationType.HasValue)
        {
            return;
        }

        RelationTypes value = relationType.Value & RelationTypesConverter.DEFINED_RELATION_TYPES_VALUES;

        for (int i = RelationTypesConverter.RELATION_TYPES_MIN_BIT; i <= RelationTypesConverter.RELATION_TYPES_MAX_BIT; i++)
        {
            RelationTypes flag = (RelationTypes)(1 << i);

            if (value.HasFlag(flag))
            {
                list.Add(flag.ToVcfString());
            }
        }
    }
}
