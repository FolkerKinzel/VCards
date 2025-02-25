namespace FolkerKinzel.VCards.Intls.Converters;

internal static class StringArrayConverter
{
    internal static string[] ToStringArray(IEnumerable<string?> coll)
    {
        string?[] arr = coll.ToArray();
        Span<string?> span = arr;

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] is null)
            {
                span[i] = "";
            }
        }

        return arr!;
    }

    internal static string[] ToStringArray(string? s)
         => string.IsNullOrEmpty(s) ? [] : [s];
}
