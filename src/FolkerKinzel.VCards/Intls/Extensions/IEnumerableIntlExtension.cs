using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IEnumerableIntlExtension
{
    public static IEnumerable<IGrouping<string?, VCardProperty>> GroupByVCardGroup(
        this IEnumerable<VCardProperty?> values)
        => values.OfType<VCardProperty>()
                 .GroupBy(static x => x.Group, StringComparer.OrdinalIgnoreCase);

#if NET462
    internal static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> sources, TSource value)
        => sources.Concat(Enumerable.Repeat(value, 1));
#endif

    internal static void SetIndexesIntl(this IEnumerable<VCardProperty?> values, bool skipEmptyItems)
    {
        int idx = 1;

        foreach (VCardProperty? item in values.Distinct())
        {
            if (item is not null)
            {
                item.Parameters.Index = (!skipEmptyItems || !item.IsEmpty) ? idx++ : null;
            }
        }
    }

    internal static void UnsetIndexesIntl(this IEnumerable<VCardProperty?> values)
    {
        foreach (VCardProperty? item in values)
        {
            if (item is not null)
            {
                item.Parameters.Index = null;
            }
        }
    }

    internal static bool IsSingle([NotNullWhen(true)] this IEnumerable<VCardProperty?>? values, bool ignoreEmptyItems)
        => ignoreEmptyItems ? values?.WhereNotEmpty().Take(2).Count() == 1
                            : values?.OfType<VCardProperty>().Take(2).Count() == 1;

    internal static IEnumerable<TSource> WhereNotNullAnd<TSource>(
        this IEnumerable<TSource?> values, Func<TSource, bool> filter) where TSource : VCardProperty
    {
        foreach (TSource? item in values)
        {
            if (item is not null && filter(item))
            {
                yield return item;
            }
        }
    }

    internal static IEnumerable<TSource> WhereNotEmpty<TSource>(
        this IEnumerable<TSource?> values) where TSource : VCardProperty
        => values.Where(static x => x is not null && !x.IsEmpty)!;

    internal static IEnumerable<TSource> WhereNotEmptyAnd<TSource>(
        this IEnumerable<TSource?> values, Func<TSource, bool> filter) where TSource : VCardProperty
    {
        foreach (TSource? item in values)
        {
            if (item is not null && !item.IsEmpty && filter(item))
            {
                yield return item;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IEnumerable<TSource> OrderByPrefIntl<TSource>(
        this IEnumerable<TSource?> values, bool discardEmptyItems) where TSource : VCardProperty
        => OrderByIntl(values, GetPreference, discardEmptyItems);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IEnumerable<TSource> OrderByIndexIntl<TSource>(
        this IEnumerable<TSource?> values, bool discardEmptyItems) where TSource : VCardProperty
        => OrderByIntl(values, GetIndex, discardEmptyItems);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TSource? PrefOrNullIntl<TSource>(
        this IEnumerable<TSource?> values, bool ignoreEmptyItems) where TSource : VCardProperty
        => ItemOrNullIntl(values, GetPreference, ignoreEmptyItems);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TSource? PrefOrNullIntl<TSource>(
        this IEnumerable<TSource?> values,
        Func<TSource, bool> filter,
        bool ignoreEmptyItems) where TSource : VCardProperty
        => ItemOrNullIntl(values, GetPreference, filter, ignoreEmptyItems);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TSource? FirstOrNullIntl<TSource>(
        this IEnumerable<TSource?> values, bool ignoreEmptyItems) where TSource : VCardProperty
        => ItemOrNullIntl(values, GetIndex, ignoreEmptyItems);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TSource? FirstOrNullIntl<TSource>(
        this IEnumerable<TSource?> values,
        Func<TSource, bool> filter,
        bool ignoreEmptyItems) where TSource : VCardProperty
        => ItemOrNullIntl(values, GetIndex, filter, ignoreEmptyItems);

    private static IEnumerable<TSource> OrderByIntl<TSource>(
        this IEnumerable<TSource?> values, 
        Func<TSource, int> sortingCriterion,
        bool discardEmptyItems) where TSource : VCardProperty
        => discardEmptyItems ? values.WhereNotEmpty().OrderBy(sortingCriterion)
                             : values.OfType<TSource>().OrderBy(sortingCriterion);

    private static TSource? ItemOrNullIntl<TSource>(
        IEnumerable<TSource?> values,
        Func<TSource, int> sortingCriterion,
        bool ignoreEmptyItems) where TSource : VCardProperty
    => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(sortingCriterion).FirstOrDefault()
                        : values.OfType<TSource>().OrderBy(sortingCriterion).FirstOrDefault();

    private static TSource? ItemOrNullIntl<TSource>(
        IEnumerable<TSource?> values,
        Func<TSource, int> sortingCriterion,
        Func<TSource, bool> filter,
        bool ignoreEmptyItems) where TSource : VCardProperty
         => ignoreEmptyItems ? values.WhereNotEmptyAnd(filter).OrderBy(sortingCriterion).FirstOrDefault()
                             : values.WhereNotNullAnd(filter).OrderBy(sortingCriterion).FirstOrDefault();

    private static int GetPreference(VCardProperty prop) => prop.Parameters.Preference;

    private static int GetIndex(VCardProperty prop) => prop.Parameters.Index ?? int.MaxValue;
}
