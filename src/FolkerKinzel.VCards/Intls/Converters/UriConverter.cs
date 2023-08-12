namespace FolkerKinzel.VCards.Intls.Converters;

internal static class UriConverter
{
    internal static Uri? ToAbsoluteUri(string? value)
    {
        if(value == null)
        {
            return null;
        }

        if(Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return uri;
        }

        if(value.StartsWith("http"))
        {
            return null;
        }
        value = value.ReplaceWhiteSpaceWith(ReadOnlySpan<char>.Empty);
        return ToAbsoluteUri("http://" + value);
    }
}
