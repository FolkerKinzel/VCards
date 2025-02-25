﻿using FolkerKinzel.DataUrls;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class DataUrlConverter
{
    internal static RawData ToRawData(ref DataUrlInfo dataUrlInfo)
    {
        ReadOnlyMemory<char> mime = dataUrlInfo.MimeType;

        bool masked = UnMaskMimeType(ref mime, out bool base64Encoded);

        if (!masked)
        {
            return RawData.FromDataUrlInfo(in dataUrlInfo);
        }

        // If the "data" URL is masked dataUrlInfo has to be parsed again:

        const int defaultLength = 6; // data:,
        const int base64EncodedLength = 13; // data:;base64,

        int length = (base64Encoded ? base64EncodedLength : defaultLength) + mime.Length + dataUrlInfo.Data.Length;

        using ArrayPoolHelper.SharedArray<char> shared = ArrayPoolHelper.Rent<char>(length);
        Memory<char> mem = shared.Array;
        CopyUnmaskedDataUrl(mem.Span, mime.Span, dataUrlInfo.Data, base64Encoded);

        _ = DataUrl.TryParse(mem.Slice(0, length), out dataUrlInfo);
        return RawData.FromDataUrlInfo(in dataUrlInfo);
    }

    private static bool UnMaskMimeType(ref ReadOnlyMemory<char> mime, out bool isBase64)
    {
        isBase64 = false;
        ReadOnlySpan<char> span = mime.Span;

        if (!span.EndsWith('\\')) // masked comma
        {
            return false; // unmasked
        }

        span = span.Slice(0, span.Length - 1);

        int trimEndLength = 1;

        if (span.EndsWith(@";base64"))
        {
            isBase64 = true;
            trimEndLength += 7;
            span = span.Slice(0, span.Length - 7);
        }

        if (span.EndsWith('\\')) // masked semicolon
        {
            trimEndLength++;
            span = span.Slice(0, span.Length - 1);
        }

        // The MIME type may have parameters, separated by masked semicolons:
        mime = span.Contains('\\') ? span.UnMaskValue(VCdVersion.V4_0).AsMemory()
                                   : mime.Slice(0, mime.Length - trimEndLength);

        return true;
    }

    private static void CopyUnmaskedDataUrl(Span<char> span, ReadOnlySpan<char> mime, ReadOnlySpan<char> data, bool base64Encoded)
    {
        if (base64Encoded)
        {
            CopyBase64DataUrl(span, mime, data);
        }
        else
        {
            CopyUrlEncodedDataUrl(span, mime, data);
        }

        static void CopyBase64DataUrl(Span<char> span, ReadOnlySpan<char> mediaType, ReadOnlySpan<char> data)
        {
            "data:".AsSpan().CopyTo(span);
            span = span.Slice(5);
            mediaType.CopyTo(span);
            span = span.Slice(mediaType.Length);
            ";base64,".AsSpan().CopyTo(span);
            span = span.Slice(8);
            data.CopyTo(span);
        }

        static void CopyUrlEncodedDataUrl(Span<char> span, ReadOnlySpan<char> mediaType, ReadOnlySpan<char> data)
        {
            "data:".AsSpan().CopyTo(span);
            span = span.Slice(5);
            mediaType.CopyTo(span);
            span = span.Slice(mediaType.Length);
            span[0] = ',';
            span = span.Slice(1);
            data.CopyTo(span);
        }
    }
}