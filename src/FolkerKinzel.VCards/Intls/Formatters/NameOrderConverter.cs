namespace FolkerKinzel.VCards.Intls.Formatters;

internal static class NameOrderConverter
{
    internal static NameOrder ParseIetfLanguageTag(string? languageTag)
    {
        ReadOnlySpan<char> span = languageTag.AsSpan();
        const StringComparison comp = StringComparison.Ordinal;

        return span.Length == 2 || (span.Length >= 3 && span[2] == '-')
                ? span.StartsWith("es", comp) ? NameOrder.Spanish
                     : span.StartsWith("vi", comp) ? NameOrder.Vietnamese
                     : span.StartsWith("zh", comp) ||
                       span.StartsWith("ko", comp) ||
                       span.StartsWith("hr", comp) ||
                       span.StartsWith("sr", comp) ||
                       span.StartsWith("bs", comp) ||
                       span.StartsWith("hu", comp)
                       ? NameOrder.Hungarian
                       : NameOrder.Default
                : NameOrder.Default;
    }
}
