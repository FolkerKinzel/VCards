using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class StringArrayConverter
{
    internal static string[] ToStringArray(IEnumerable<string?> coll)
    {
        string?[] arr = coll.ToArray();
        bool containsWS = false;

        foreach (string? str in arr.AsSpan())
        {
            if(string.IsNullOrEmpty(str))
            {
                containsWS = true;
                break;
            }
        }

        return (containsWS ? arr.Where(x => !string.IsNullOrEmpty(x)).ToArray() : arr)!;
    }

    internal static string[] ToStringArray(string? s)
         => string.IsNullOrEmpty(s) ? [] : [ s ];
}
