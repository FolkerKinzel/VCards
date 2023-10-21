using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IEnumerableExtension
{
    internal static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> values)
        => values.Where(static x => x != null)!;

    internal static IEnumerable<T> WhereNotNullAnd<T>(this IEnumerable<T?> values, Predicate<T> predicate)
        where T : VCardProperty
        => values.Where(x => x != null && predicate(x))!;

    internal static IEnumerable<T> WhereNotEmpty<T>(this IEnumerable<T?> values)
        where T : VCardProperty
        => values.Where(static x => x != null && !x.IsEmpty)!;

    internal static IEnumerable<T> WhereNotEmptyAnd<T>(this IEnumerable<T?> values, Predicate<T> predicate)
        where T : VCardProperty
        => values.Where(x => x != null && !x.IsEmpty && predicate(x))!;

    

    internal static IEnumerable<T> OrderByPref<T>(this IEnumerable<T> values) 
        where T: VCardProperty
        => values.OrderBy(static x => x.Parameters.Preference);

    public static T? PrefOrNullIntl<T>(this IEnumerable<T?> properties, bool ignoreEmptyItems)
        where T : VCardProperty
        => ignoreEmptyItems ? properties.WhereNotEmpty().OrderByPref().FirstOrDefault()
                            : properties.WhereNotNull().OrderByPref().FirstOrDefault();

    public static T? PrefOrNullIntl<T>(this IEnumerable<T?> properties, Predicate<T> predicate, bool ignoreEmptyItems)
        where T : VCardProperty
        => ignoreEmptyItems ? properties.WhereNotEmptyAnd(predicate).OrderByPref().FirstOrDefault()
                            : properties.WhereNotNullAnd(predicate).OrderByPref().FirstOrDefault();

    public static T? FirstOrNullIntl<T>(this IEnumerable<T?> properties, bool ignoreEmptyItems) where T : VCardProperty
        => ignoreEmptyItems ? properties.FirstOrDefault(static x => x != null && !x.IsEmpty)
                            : properties.FirstOrDefault(static x => x != null);

    public static T? FirstOrNullIntl<T>(this IEnumerable<T?> properties, Predicate<T> predicate, bool ignoreEmptyItems) where T : VCardProperty
        => ignoreEmptyItems ? properties.FirstOrDefault(x => x != null && !x.IsEmpty && predicate(x))
                            : properties.FirstOrDefault(x => x != null && predicate(x));

}
