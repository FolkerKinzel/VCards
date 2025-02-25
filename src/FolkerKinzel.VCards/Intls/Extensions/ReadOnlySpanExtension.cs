using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class ReadOnlySpanExtension
{
    /// <summary> Unmasks a masked text value contained in <paramref name="value"/> according to
    /// vCard 2.1, and decodes its Quoted-Printable encoding.</summary>
    /// <param name="value">The span to unmask and decode.</param>
    /// <param name="charSet">The Charset to use for Quoted-Printable decoding.</param>
    internal static string UnMaskAndDecodeValue(this ReadOnlySpan<char> value, string? charSet)
    {
        if (value.Length > Const.STACKALLOC_CHAR_THRESHOLD)
        {
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(value.Length);
            return DoUnMasking(value, buf.Array.AsSpan(), charSet);
        }
        else
        {
            Span<char> bufSpan = stackalloc char[value.Length];
            return DoUnMasking(value, bufSpan, charSet);
        }

        static string DoUnMasking(ReadOnlySpan<char> valueSpan, Span<char> bufSpan, string? charSet)
        {
            return QuotedPrintable.Decode(bufSpan.Slice(0, UnMask(valueSpan, bufSpan)),
                                          TextEncodingConverter.GetEncoding(charSet)); // null-check not needed

            ///////////////////////////////////////////////////////////

            static int UnMask(ReadOnlySpan<char> value, Span<char> outBuf)
            {
                int idxOfBackSlash = value.IndexOf('\\');

                if (idxOfBackSlash == -1)
                {
                    value.CopyTo(outBuf);
                    return value.Length;
                }

                value.Slice(0, idxOfBackSlash).CopyTo(outBuf);

                return idxOfBackSlash + UnMask21(value.Slice(idxOfBackSlash), outBuf.Slice(idxOfBackSlash));
            }
        }
    }

    internal static string UnMaskValue(this ReadOnlySpan<char> value, VCdVersion version)
    {
        int idxOfBackSlash = value.IndexOf('\\');

        if (idxOfBackSlash == -1)
        {
            return value.ToString();
        }

        int processedLength = value.Length - idxOfBackSlash;

        if (processedLength > Const.STACKALLOC_CHAR_THRESHOLD)
        {
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(processedLength);
            Span<char> bufSpan = buf.Array.AsSpan();
            return CreateUnMaskedString(version, value, idxOfBackSlash, bufSpan);
        }
        else
        {
            Span<char> bufSpan = stackalloc char[processedLength];
            return CreateUnMaskedString(version, value, idxOfBackSlash, bufSpan);
        }

        static string CreateUnMaskedString(VCdVersion version, ReadOnlySpan<char> sourceSpan, int idxOfBackSlash, Span<char> bufSpan)
        {
            ReadOnlySpan<char> processedSpan = sourceSpan.Slice(idxOfBackSlash);

            int outputLength = version switch
            {
                VCdVersion.V2_1 => UnMask21(processedSpan, bufSpan),
                VCdVersion.V3_0 => UnMask30(processedSpan, bufSpan),
                _ => UnMask40(processedSpan, bufSpan)
            };

            bufSpan = bufSpan.Slice(0, outputLength);

            return idxOfBackSlash == 0 ? bufSpan.ToString()
                                       : StaticStringMethod.Concat(sourceSpan.Slice(0, idxOfBackSlash), bufSpan);
        }
    }

    private static int UnMask21(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        ReadOnlySpan<char> newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\')
            {
                switch (source[i + 1])
                {
                    // @"\n" see RFC 2425, 8.3
                    case 'n':
                    case 'N':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case ';':
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];
        return length;
    }

    private static int UnMask30(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        ReadOnlySpan<char> newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\')
            {
                switch (source[i + 1])
                {
                    case 'n':
                    case 'N':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case ',':
                    case ';':
                    case ':':
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];
        return length;
    }

    private static int UnMask40(ReadOnlySpan<char> source, Span<char> destination)
    {
        int lastSourceIdx = source.Length - 1;
        int length = 0;

        ReadOnlySpan<char> newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = source[i];

            if (c == '\\')
            {
                switch (source[i + 1])
                {
                    case 'n':
                    case 'N':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case ',':
                    case ';':
                        continue;
                    case '\\':
                        destination[length++] = c;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = source[lastSourceIdx];

        return length;
    }

    /// <summary>
    /// Unmasks a parameter value and makes a special treatment if <paramref name="paramVal"/>
    /// is the value of a <c>LABEL</c> parameter.
    /// </summary>
    /// <param name="paramVal">The parameter value to unmask.</param>
    /// <param name="isLabel"><c>true</c> if <paramref name="paramVal"/> is the value of a <c>LABEL</c>
    /// parameter.</param>
    /// <returns>The unmasked content of <paramref name="paramVal"/>. [Double-]quotes will not be removed.</returns>
    internal static string UnMaskParameterValue(this ReadOnlySpan<char> paramVal, bool isLabel)
    {
        int idxOfEscapeChar = paramVal.IndexOf('^');

        if (isLabel)
        {
            int idxOfBackSlash = paramVal.IndexOf('\\');

            if (idxOfBackSlash != -1)
            {
                idxOfEscapeChar = idxOfEscapeChar == -1
                                    ? idxOfBackSlash
                                    : Math.Min(idxOfBackSlash, idxOfEscapeChar);
            }
        }

        if (idxOfEscapeChar == -1)
        {
            return paramVal.ToString();
        }

        int processedLength = paramVal.Length - idxOfEscapeChar;

        if (processedLength > Const.STACKALLOC_CHAR_THRESHOLD)
        {
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(processedLength);
            Span<char> bufSpan = buf.Array.AsSpan();
            return CreateUnMaskedString(paramVal, idxOfEscapeChar, bufSpan, isLabel);
        }
        else
        {
            Span<char> bufSpan = stackalloc char[processedLength];
            return CreateUnMaskedString(paramVal, idxOfEscapeChar, bufSpan, isLabel);
        }

        static string CreateUnMaskedString(ReadOnlySpan<char> sourceSpan,
                                           int idxOfEscapeChar,
                                           Span<char> bufSpan,
                                           bool isLabel)
        {
            ReadOnlySpan<char> processedSpan = sourceSpan.Slice(idxOfEscapeChar);

            bufSpan = bufSpan.Slice(0, UnMaskParameterValue40(processedSpan, bufSpan, isLabel));

            return idxOfEscapeChar == 0 ? bufSpan.ToString()
                                        : StaticStringMethod.Concat(sourceSpan.Slice(0, idxOfEscapeChar), bufSpan);
        }
    }

    /// <summary>
    /// Unmasks a parameter value according to RFC 6868 and makes a special treatment if <paramref name="paramVal"/>
    /// is the value of a <c>LABEL</c> parameter.
    /// </summary>
    /// <param name="paramVal">The parameter value to unmask.</param>
    /// <param name="destination">The buffer to write the result.</param>
    /// <param name="isLabel"><c>true</c> if <paramref name="paramVal"/> is the value of a <c>LABEL</c>
    /// parameter.</param>
    /// <returns>The length of the content in <paramref name="destination"/>.</returns>
    /// <remarks>
    /// RFC 6350 6.3.1 states that newlines in LABEL parameters "are encoded
    /// as \n, as they are for property values". For compatibility reasons this is accepted in
    /// <c>LABEL</c> parameters.
    /// </remarks>
    private static int UnMaskParameterValue40(ReadOnlySpan<char> paramVal, Span<char> destination, bool isLabel)
    {
        int lastSourceIdx = paramVal.Length - 1;
        int length = 0;

        ReadOnlySpan<char> newLineSpan = Environment.NewLine.AsSpan();

        for (int i = 0; i < lastSourceIdx; i++)
        {
            char c = paramVal[i];

            if (isLabel && c == '\\')
            {
                switch (paramVal[i + 1])
                {
                    case 'n':
                    case 'N': // allowed in property values RFC 6350 3.4
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    default:
                        break;
                }
            }

            if (c == '^')
            {
                switch (paramVal[i + 1])
                {
                    case 'n':
                        newLineSpan.TryCopyTo(destination.Slice(length));
                        length += newLineSpan.Length;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case '\'':
                        destination[length++] = '\"';
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    case '^':
                        destination[length++] = c;
                        i++;
                        if (i == lastSourceIdx)
                        {
                            return length;
                        }
                        continue;
                    default:
                        break;
                }
            }

            destination[length++] = c;
        }

        destination[length++] = paramVal[lastSourceIdx];

        return length;
    }
}
