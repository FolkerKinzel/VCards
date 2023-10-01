using System.Net;
using FolkerKinzel.MimeTypes;

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

    internal static bool IsContentId(Uri value)
    {
        Debug.Assert(value != null);
        Debug.Assert(value.IsAbsoluteUri);

        return value.Scheme.StartsWith("cid", StringComparison.OrdinalIgnoreCase);
    }


    internal static string GetFileTypeExtensionFromUri(Uri? uri)
    {
        if (uri == null)
        {
            return MimeCache.DefaultFileTypeExtension;
        }
        
        if(!uri.IsAbsoluteUri)
        {
            return Uri.TryCreate(new Uri("http://a"), uri, out uri) ? GetFileTypeExtensionFromUri(uri)
                                                                    : MimeCache.DefaultFileTypeExtension;
        }

        Debug.Assert(uri.IsAbsoluteUri);
        string[] segments = uri.Segments;
        Debug.Assert(segments.Length > 0);
        string segment = segments[segments.Length - 1];

        if(segment == "/")
        {
            return ".htm";
        }

#if NETSTANDARD2_0 || NET461
        if(segment.ContainsAny(Path.GetInvalidPathChars()))
        {
            return MimeCache.DefaultFileTypeExtension;
        }
#endif
        string ext = Path.GetExtension(segment);
        return ext.StartsWith('.') ? ext : MimeCache.DefaultFileTypeExtension;
    }
}
