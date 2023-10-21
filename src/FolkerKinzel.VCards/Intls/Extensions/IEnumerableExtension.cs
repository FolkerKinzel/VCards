using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IEnumerableExtension
{
    internal static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> values)
        => values.Where(static x => x != null)!;

    internal static IEnumerable<TSource> WhereNotNullAnd<TSource>(this IEnumerable<TSource?> values,
                                                      Func<TSource, bool> filter) where TSource : VCardProperty
        => values.Where(x => x != null && filter(x))!;

    internal static IEnumerable<TSource> WhereNotEmpty<TSource>(this IEnumerable<TSource?> values) where TSource : VCardProperty
        => values.Where(static x => x != null && !x.IsEmpty)!;

    internal static IEnumerable<TSource> WhereNotEmptyAnd<TSource>(this IEnumerable<TSource?> values,
                                                       Func<TSource, bool> filter) where TSource : VCardProperty
        => values.Where(x => x != null && !x.IsEmpty && filter(x))!;

    internal static IEnumerable<TSource> OrderByPrefIntl<TSource>(this IEnumerable<TSource?> values,
                                                      bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetPreference)
                            : values.WhereNotNull().OrderBy(GetPreference);

    internal static IEnumerable<TSource> OrderByIndexIntl<TSource>(this IEnumerable<TSource?> values,
                                                       bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetIndex)
                            : values.WhereNotNull().OrderBy(GetIndex);

    public static TSource? PrefOrNullIntl<TSource>(this IEnumerable<TSource?> values,
                                       bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetPreference).FirstOrDefault()
                            : values.WhereNotNull().OrderBy(GetPreference).FirstOrDefault();

    public static TSource? PrefOrNullIntl<TSource>(this IEnumerable<TSource?> values,
                                       Func<TSource, bool> filter,
                                       bool ignoreEmptyItems)  where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmptyAnd(filter).OrderBy(GetPreference).FirstOrDefault()
                            : values.WhereNotNullAnd(filter).OrderBy(GetPreference).FirstOrDefault();

    public static TSource? FirstOrNullIntl<TSource>(this IEnumerable<TSource?> values,
                                        bool ignoreEmptyItems) where TSource : VCardProperty
        => ignoreEmptyItems ? values.WhereNotEmpty().OrderBy(GetIndex).FirstOrDefault()
                            : values.WhereNotNull().OrderBy(GetIndex).FirstOrDefault();

    public static TSource? FirstOrNullIntl<TSource>(this IEnumerable<TSource?> values,
                                        Func<TSource, bool> filter,
                                        bool ignoreEmptyItems) where TSource : VCardProperty
         => ignoreEmptyItems ? values.WhereNotEmptyAnd(filter).OrderBy(GetIndex).FirstOrDefault()
                             : values.WhereNotNullAnd(filter).OrderBy(GetIndex).FirstOrDefault();


    private static int GetPreference(VCardProperty prop) => prop.Parameters.Preference;

    private static int GetIndex(VCardProperty prop) => prop.Parameters.Index ?? int.MaxValue;
}
