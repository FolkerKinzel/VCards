using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class StringArrayConverter
{
    internal static string[]? AsNonEmptyStringArray(string?[]? coll)
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

        string[] arr = (containsWS ? coll.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : coll)!;

        return arr.Length == 0 ? null : arr;
    }

    internal static string[]? AsNonEmptyStringArray(string? s)
         => string.IsNullOrWhiteSpace(s) ? null : [ s ];
}
