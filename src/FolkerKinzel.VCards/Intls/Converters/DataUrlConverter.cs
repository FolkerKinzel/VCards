using FolkerKinzel.DataUrls;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models;
using OneOf;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class DataUrlConverter
{
    internal static DataProperty ParseDataUrl(VcfRow vcfRow, ref DataUrlInfo dataUrlInfo)
    {
        ReadOnlyMemory<char> mime = dataUrlInfo.MimeType;
        bool maskedBase64DataUrl = UnMaskMimeType(ref mime);

        string mediaType = MimeTypeInfo.TryParse(mime, out MimeTypeInfo mimeTypeInfo)
                           ? mimeTypeInfo.ToString()
                           : MimeString.OctetStream;

        vcfRow.Parameters.MediaType = mediaType;

        if (maskedBase64DataUrl)
        {
            // If the "data" URL is masked and contains the text ;base64\, dataUrlInfo has to be parsed again.
            // (Otherwise the Base64 encoded data would be treated as URL-encoded.)

            int length = 13 + mediaType.Length + dataUrlInfo.Data.Length;

            using ArrayPoolHelper.SharedArray<char> shared = ArrayPoolHelper.Rent<char>(length);
            Memory<char> mem = shared.Array;
            CopyDataUrl(mem.Span, mediaType, dataUrlInfo.Data);
            _ = DataUrl.TryParse(mem.Slice(0, length), out dataUrlInfo);
            return FromDataUrlInfo(vcfRow, in dataUrlInfo);
        }

        return FromDataUrlInfo(vcfRow, in dataUrlInfo);
    }

    private static bool UnMaskMimeType(ref ReadOnlyMemory<char> mime)
    {
        bool maskedBase64DataUrl = false;
        int trimEndLength = 0;
        ReadOnlySpan<char> span = mime.Span;

        if (span.EndsWith(@";base64\")) // masked comma
        {
            maskedBase64DataUrl = true;

            trimEndLength += 8;
            span = span.Slice(0, span.Length - 8);

            if (span.EndsWith('\\')) // masked semicolon
            {
                trimEndLength++;
                span = span.Slice(0, span.Length - 1);
            }

            // The MIME type may have parameters, separated by masked semicolons:
            mime = span.Contains('\\') ? span.UnMaskValue(VCdVersion.V4_0).AsMemory()
                                       : mime.Slice(0, mime.Length - trimEndLength);
        }

        return maskedBase64DataUrl;
    }

    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters",
    Justification = "Not localizable")]
    private static void CopyDataUrl(Span<char> span, string mediaType, ReadOnlySpan<char> data)
    {
        "data:".AsSpan().CopyTo(span);
        span = span.Slice(5);
        mediaType.AsSpan().CopyTo(span);
        span = span.Slice(mediaType.Length);
        ";base64,".AsSpan().CopyTo(span);
        span = span.Slice(8);
        data.CopyTo(span);
    }

    private static DataProperty FromDataUrlInfo(VcfRow vcfRow, in DataUrlInfo dataUrlInfo)
    {
        return dataUrlInfo.TryGetEmbeddedData(out OneOf<string, byte[]> data)
                ? data.Match<DataProperty>
                   (
                    s => new EmbeddedTextProperty(new TextProperty(s, vcfRow)),
                    b => new EmbeddedBytesProperty(b, vcfRow.Group, vcfRow.Parameters)
                    )
                : DataProperty.FromText(vcfRow.Value.ToString(), dataUrlInfo.MimeType.ToString(), vcfRow.Group);
    }
}