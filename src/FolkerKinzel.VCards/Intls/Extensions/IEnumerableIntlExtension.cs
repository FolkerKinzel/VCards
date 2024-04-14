using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IEnumerableIntlExtension
{
    public static IEnumerable<IGrouping<string?, VCardProperty>> GroupByVCardGroup(
        this IEnumerable<VCardProperty?> values)
        => values.WhereNotNull()
                 .GroupBy(static x => x.Group, StringComparer.OrdinalIgnoreCase);

#if NET461
    internal static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> sources, TSource value)
        => sources.Concat(Enumerable.Repeat(value, 1));
#endif

    internal static void SetIndexesIntl<TSource>(this IEnumerable<TSource?> values, bool skipEmptyItems)
        where TSource : VCardProperty
    {
        int idx = 1;

        foreach (var item in values.Distinct())
        {
            if (item is not null)
            {
                item.Parameters.Index = (!skipEmptyItems || !item.IsEmpty) ? idx++ : null;
            }
        }
    }

    internal static void UnsetIndexesIntl<TSource>(this IEnumerable<TSource?> values)
        where TSource : VCardProperty
    {
        foreach (var item in values)
        {
            if (item is not null)
            {
                item.Parameters.Index = null;
            }
        }
    }

    internal static bool IsSingle([NotNullWhen(true)] this IEnumerable<VCardProperty?>? values, bool ignoreEmptyItems)
        => ignoreEmptyItems ? values?.WhereNotEmpty().Take(2).Count() == 1
                            : values?.WhereNotNull().Take(2).Count() == 1;

    internal static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> values)
        => values.Where(static x => x is not null)!;

    internal static IEnumerable<TSource> WhereNotNullAnd<TSource>(
        this IEnumerable<TSource?> values, Func<TSource, bool> filter) where TSource : VCardProperty
    {
        foreach (var item in values)
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
        foreach (var item in values)
        {
            if (item is not null && !item.IsEmpty && filter(item))
            {
                yield return item;
            }
        }
    }

    internal static IEnumerable<TSource> OrderByPrefIntl<TSource>(
        this IEnumerable<TSource?> values, bool discardEmptyItems) where TSource : VCardProperty
        => discardEmptyItems ? values.WhereNotEmpty().OrderBy(GetPreference)
                             : values.WhereNotNull().OrderBy(GetPreference);

    internal static IEnumerable<TSource> OrderByIndexIntl<TSource>(
        this IEnumerable<TSource?> values, bool discardEmptyItems) where TSource : VCardProperty
        => discardEmptyItems ? values.WhereNotEmpty().OrderBy(GetIndex)
                             : values.WhereNotNull().OrderBy(GetIndex);

    internal static TSource? PrefOrNullIntl<TSource>(
        this IEnumerable<TSource?> values, bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetPreference).FirstOrDefault()
                            : values.WhereNotNull().OrderBy(GetPreference).FirstOrDefault();

    internal static TSource? PrefOrNullIntl<TSource>(
        this IEnumerable<TSource?> values,
        Func<TSource, bool> filter,
        bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmptyAnd(filter).OrderBy(GetPreference).FirstOrDefault()
                            : values.WhereNotNullAnd(filter).OrderBy(GetPreference).FirstOrDefault();

    internal static TSource? FirstOrNullIntl<TSource>(
        this IEnumerable<TSource?> values, bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetIndex).FirstOrDefault()
                            : values.WhereNotNull().OrderBy(GetIndex).FirstOrDefault();

    internal static TSource? FirstOrNullIntl<TSource>(
        this IEnumerable<TSource?> values,
        Func<TSource, bool> filter,
        bool ignoreEmptyItems) where TSource : VCardProperty
         => ignoreEmptyItems ? values.WhereNotEmptyAnd(filter).OrderBy(GetIndex).FirstOrDefault()
                             : values.WhereNotNullAnd(filter).OrderBy(GetIndex).FirstOrDefault();



    private static int GetPreference(VCardProperty prop) => prop.Parameters.Preference;

    private static int GetIndex(VCardProperty prop) => prop.Parameters.Index ?? int.MaxValue;
}
