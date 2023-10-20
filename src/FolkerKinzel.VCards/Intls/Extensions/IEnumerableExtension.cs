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

    //internal static IEnumerable<T> HasToBeSerialized<T>(this IEnumerable<T?> values, VcfOptions options)
    //    where T : VCardProperty
    //    => values.Where(x => x != null && (!x.IsEmpty || options.HasFlag(VcfOptions.WriteEmptyProperties)))!;

    internal static IEnumerable<T> OrderByPreference<T>(this IEnumerable<T> values) 
        where T: VCardProperty
        => values.OrderBy(static x => x.Parameters.Preference);

    //private static bool IsPropertyWithData([NotNullWhen(true)] VCardProperty? x, VcfOptions options)
    //        => x != null && (!x.IsEmpty || options.HasFlag(VcfOptions.WriteEmptyProperties));

}
