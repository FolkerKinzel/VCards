namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class ReadOnlySpanExtension
{
    internal static bool ContainsUtcOffset(this ReadOnlySpan<char> span) => span.TrimStart('-').ContainsAny("+-".AsSpan());

}
