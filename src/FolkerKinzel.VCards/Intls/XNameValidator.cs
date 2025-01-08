namespace FolkerKinzel.VCards.Intls;

internal static class XNameValidator
{
    internal static bool IsXName(string? xName)
    {
        ReadOnlySpan<char> span = xName.AsSpan();
        return span.Length >= 3 &&
               span.StartsWith("X-", StringComparison.OrdinalIgnoreCase) &&
               !span.Contains(' ');
    }
}