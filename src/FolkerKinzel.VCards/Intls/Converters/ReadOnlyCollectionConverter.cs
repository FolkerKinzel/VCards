using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ReadOnlyCollectionConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Style", "IDE0300:Simplify collection initialization",
        Justification = "Performance: Initializer initializes a new List.")]
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(string? s)
        => string.IsNullOrWhiteSpace(s) ? ReadOnlyStringCollection.Empty : new ReadOnlyCollection<string>(new string[] { s });

    internal static ReadOnlyCollection<string> ToReadOnlyCollection(IEnumerable<string?>? coll)
    {
        return coll is null || !coll.Any(x => !string.IsNullOrWhiteSpace(x))
            ? ReadOnlyStringCollection.Empty
            : coll.All(x => !string.IsNullOrWhiteSpace(x))
                  ? coll is ReadOnlyCollection<string> roc
                          ? roc
                          : coll is IList<string> list
                                 ? new ReadOnlyCollection<string>(list)
                                 : new ReadOnlyCollection<string>(coll.ToArray()!)
                  : new ReadOnlyCollection<string>(coll.Where(x => !string.IsNullOrWhiteSpace(x))
                                                       .ToArray()!);
    }

    internal static string[]? ToReadOnlyList(string?[]? coll)
    {
        if (coll is null)
        {
            return null;
        }

        bool containsWS = false;

        foreach (string? s in coll.AsSpan())
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                containsWS = true;
                break;
            }
        }

        return (containsWS ? coll.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : coll)!;
    }

    internal static string[]? ToReadOnlyList(string? s)
         => string.IsNullOrWhiteSpace(s) ? null : [ s ];
}
