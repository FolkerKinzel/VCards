using FolkerKinzel.MimeTypes;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class UriConverter
{
    private const string HTM = ".htm";

    internal static bool TryConvertToAbsoluteUri(string? value, [NotNullWhen(true)] out Uri? uri)
    {
        uri = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        value = value.Trim();

        if (Uri.TryCreate(value, UriKind.Absolute, out uri))
        {
            return true;
        }

        if (value.StartsWith("http"))
        {
            return false;
        }

        return TryConvertToAbsoluteUri("http://" + value, out uri);
    }

    internal static bool IsContentId(this Uri value)
    {
        Debug.Assert(value is not null);
        Debug.Assert(value.IsAbsoluteUri);

        return value.Scheme.StartsWith("cid", StringComparison.OrdinalIgnoreCase);
    }

    internal static string GetFileTypeExtensionFromUri(Uri? uri)
        => uri is null
            ? MimeCache.DefaultFileTypeExtension
            : uri.IsAbsoluteUri 
                ? ParseFileTypeExtFromAbsoluteUri(uri)
                : ParseFileTypeExtFromRelativeUri(uri);
    

    private static string ParseFileTypeExtFromAbsoluteUri(Uri uri)
    {
        Debug.Assert(uri.IsAbsoluteUri);
        string[] segments = uri.Segments;
        Debug.Assert(segments.Length > 0);
        string segment = segments[segments.Length - 1];

        if (segment == "/")
        {
            // Points to a directory
            // We guess that the server will return *.htm
            return HTM;
        }

        // Path.GetExtension can throw an ArgumentException in NETSTANDARD2_0 and NET462
        // if segments would contain one of the characters defined in Path.GetInvalidPathChars().
        // I think this can never happen here because segment comes from Uri.AbsoluteUri
        // and has all non-URI characters URL-encoded.
        string ext = Path.GetExtension(segment);
        return ext.StartsWith('.') ? ext : MimeCache.DefaultFileTypeExtension;
    }

    private static string ParseFileTypeExtFromRelativeUri(Uri uri)
    {
        string originalString = uri.OriginalString;

        if (originalString.EndsWith('/') || !originalString.Contains('/'))
        {
            // Points to a directory
            // We guess that the server will return *.htm
            return HTM;
        }

        // Escape the OriginalString because
        // Path.GetExtension can throw an ArgumentException in NETSTANDARD2_0 and NET462
        // if segments would contain one of the characters defined in Path.GetInvalidPathChars().
#if NETSTANDARD2_0 || NET462
            originalString = Uri.EscapeDataString(originalString);
#endif
        string extRel = Path.GetExtension(originalString);
        return extRel.StartsWith('.') ? extRel : MimeCache.DefaultFileTypeExtension;
    }
}
