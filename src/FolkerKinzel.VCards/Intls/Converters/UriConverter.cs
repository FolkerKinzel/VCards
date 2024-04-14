using System.Net;
using FolkerKinzel.MimeTypes;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class UriConverter
{
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
        //value = value.ReplaceWhiteSpaceWith(ReadOnlySpan<char>.Empty);
        return TryConvertToAbsoluteUri("http://" + value, out uri);
    }

    internal static bool IsContentId(this Uri value)
    {
        Debug.Assert(value is not null);
        Debug.Assert(value.IsAbsoluteUri);

        return value.Scheme.StartsWith("cid", StringComparison.OrdinalIgnoreCase);
    }

    internal static string GetFileTypeExtensionFromUri(Uri? uri)
    {
        if (uri is null)
        {
            return MimeCache.DefaultFileTypeExtension;
        }

        if (!uri.IsAbsoluteUri)
        {
            return Uri.TryCreate(new Uri("http://a"), uri, out uri) ? GetFileTypeExtensionFromUri(uri)
                                                                    : MimeCache.DefaultFileTypeExtension;
        }

        Debug.Assert(uri.IsAbsoluteUri);
        string[] segments = uri.Segments;
        Debug.Assert(segments.Length > 0);
        string segment = segments[segments.Length - 1];

        if (segment == "/")
        {
            return ".htm";
        }

#if NETSTANDARD2_0 || NET461
        // On Windows this is never true but NETSTANDARD2_0 can
        // be used on several platforms
        if (segment.ContainsAny(Path.GetInvalidPathChars()))
        {
            return MimeCache.DefaultFileTypeExtension;
        }
#endif
        string ext = Path.GetExtension(segment);
        return ext.StartsWith('.') ? ext : MimeCache.DefaultFileTypeExtension;
    }
}
