namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringArrayExtension
{
    internal static bool ContainsData(this string?[]? arr)
    {
        foreach (string? str in arr.AsSpan())
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
        }

        return false;
    }
}
