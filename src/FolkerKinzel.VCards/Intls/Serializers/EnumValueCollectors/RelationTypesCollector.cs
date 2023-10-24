using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal static class RelationTypesCollector
{
    /// <summary>Collects the names of the flags set in <paramref name="relationType" /> into <paramref name="list" />. 
    /// </summary>
    /// <param name="relationType">The <see cref="RelationTypes"/> value to parse.</param>
    /// <param name="list">The list to collect in.</param>
    /// <remarks>
    /// <note type="caution">
    /// <paramref name="list" /> is not emptied by the method!
    /// </note>
    /// </remarks>
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
