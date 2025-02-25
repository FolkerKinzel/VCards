using System.Globalization;

namespace FolkerKinzel.VCards.Intls.Encodings;

/// <summary>Provides methods for encoding and decoding the Quoted-Printable format.</summary>
/// <threadsafety static="true" instance="false" />
internal static class QuotedPrintable
{
    internal const string NEW_LINE = "\r\n";

    private const string SOFT_LINEBREAK = "=\r\n";
    private const int SOFT_LINEBREAK_LENGTH = 3;
    private const int ENCODED_CHAR_LENGTH = 3;

    private const int MAX_ROWLENGTH = VCard.MAX_BYTES_PER_LINE; //76;
    private const int MIN_ROWLENGTH = ENCODED_CHAR_LENGTH + 1;

    #region Encode

    /// <summary>
    /// Appends a read-only character span Quoted-Printable encoded to the end of a 
    /// <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to which the encoded content is added.</param>
    /// <param name="value">The read-only character span to encode.</param>
    /// <param name="firstLineOffset">Length of the current line in the text before
    /// <paramref name="value"/> starts. (Used to compute the correct line wrapping.)</param>
    /// <returns>A reference to <paramref name="builder"/>.</returns>
    /// <remarks>If <see cref="Environment.NewLine"/> is not "\r\n" on the executing 
    /// platform, the <see cref="string"/> will be adjusted automatically.</remarks>
    internal static StringBuilder AppendQuotedPrintable(this StringBuilder builder,
                                                        ReadOnlySpan<char> value,
                                                        int firstLineOffset)
    {
        Debug.Assert(firstLineOffset >= 0);
        Debug.Assert(MAX_ROWLENGTH >= MIN_ROWLENGTH);

        if (value.IsEmpty)
        {
            return builder;
        }

        value = NormalizeLineBreaksOnUnixSystems(value);

        using ArrayPoolHelper.SharedArray<byte> byteBuf = ArrayPoolHelper.Rent<byte>(Encoding.UTF8.GetMaxByteCount(value.Length));
        Span<byte> byteSpan = byteBuf.Array.AsSpan();
#if NETSTANDARD2_0 || NET462
        using ArrayPoolHelper.SharedArray<char> charBuf = ArrayPoolHelper.Rent<char>(value.Length);
        Span<char> charSpan = charBuf.Array.AsSpan();
        _ = value.TryCopyTo(charSpan);
        byteSpan = byteSpan.Slice(0, Encoding.UTF8.GetBytes(charBuf.Array, 0, value.Length, byteBuf.Array, 0));
#else
        byteSpan = byteSpan.Slice(0, Encoding.UTF8.GetBytes(value, byteSpan));
#endif
        AppendEncodedTo(builder, byteSpan, firstLineOffset);
        return builder;

        /////////////////////////////////////////////////////////

        [ExcludeFromCodeCoverage]
        static ReadOnlySpan<char> NormalizeLineBreaksOnUnixSystems(ReadOnlySpan<char> value)
        {
            if (!StringComparer.Ordinal.Equals(Environment.NewLine, NEW_LINE) && value.Contains(Environment.NewLine[0]))
            {
                value = value.ToString().Replace(Environment.NewLine, NEW_LINE, StringComparison.Ordinal).AsSpan();
            }

            return value;
        }

        static void AppendEncodedTo(StringBuilder builder, ReadOnlySpan<byte> source, int firstLineOffset)
        {
            // The last index in bufSpan is reserved for the last Byte in source.
            using ArrayPoolHelper.SharedArray<char> buf = ArrayPoolHelper.Rent<char>(MAX_ROWLENGTH);
            Span<char> bufSpan = buf.Array.AsSpan(0, MAX_ROWLENGTH);

            // Only the last row can use bufSpan completely. All others have to leave
            // one index for the '=' of the soft-linebreak.
            const int usableBufLength = MAX_ROWLENGTH - 1;
            int sourceIdx = 0;

            int usableLength = usableBufLength - firstLineOffset;

            if (usableLength <= 0)
            {
                builder.Append(SOFT_LINEBREAK);
                usableLength = usableBufLength;
            }

            int bufIdx = 0;
Repeat:
            while (bufIdx < usableLength && sourceIdx < source.Length - 1)
            {
                byte bt = source[sourceIdx++];

                if (HasToBeQuoted(bt))
                {
                    bufSpan[bufIdx++] = '=';

                    if (bufIdx < usableLength)
                    {
                        bufSpan[bufIdx++] = GetNibble(bt >> 4);
                    }
                    else
                    {
                        builder.Append(buf.Array, 0, bufIdx);
                        builder.Append(SOFT_LINEBREAK);

                        bufIdx = 0;
                        bufSpan[bufIdx++] = GetNibble(bt >> 4);
                        bufSpan[bufIdx++] = GetNibble(bt & 0xF);

                        usableLength = usableBufLength;
                        continue;
                    }

                    if (bufIdx < usableLength)
                    {
                        bufSpan[bufIdx++] = GetNibble(bt & 0xF);
                    }
                    else
                    {
                        builder.Append(buf.Array, 0, bufIdx);
                        builder.Append(SOFT_LINEBREAK);

                        bufIdx = 0;
                        bufSpan[bufIdx++] = GetNibble(bt & 0xF);

                        usableLength = usableBufLength;
                    }
                }
                else
                {
                    bufSpan[bufIdx++] = (char)bt;
                }
            }//while

            if (sourceIdx < source.Length - 1)
            {
                builder.Append(buf.Array, 0, bufIdx);
                builder.Append(SOFT_LINEBREAK);
                bufIdx = 0;
                usableLength = usableBufLength;
                goto Repeat;
            }

            int lastByte = source[source.Length - 1];

            if (HasToBeQuoted(lastByte) || lastByte == ' ' || lastByte == '\t')
            {
                if (bufIdx == usableLength)
                {
                    builder.Append(buf.Array, 0, bufIdx);
                    builder.Append(SOFT_LINEBREAK);
                    bufIdx = 0;
                    usableLength = usableBufLength;
                }

                bufSpan[bufIdx++] = '=';

                if (bufIdx < usableLength)
                {
                    bufSpan[bufIdx++] = GetNibble(lastByte >> 4);
                }
                else
                {
                    builder.Append(buf.Array, 0, bufIdx);
                    builder.Append(SOFT_LINEBREAK);

                    bufIdx = 0;
                    bufSpan[bufIdx++] = GetNibble(lastByte >> 4);
                }

                bufSpan[bufIdx++] = GetNibble(lastByte & 0xF);
                builder.Append(buf.Array, 0, bufIdx);
            }
            else
            {
                bufSpan[bufIdx++] = (char)lastByte;
                builder.Append(buf.Array, 0, bufIdx);
            }

            ///////////////////////////////////

            static char GetNibble(int nibble) => nibble > 9 ? (char)(55 + nibble) : (char)(48 + nibble);

            static bool HasToBeQuoted(int bt) => bt is not '\t' and (> 126 or '=' or < 32);
        }
    }

    #endregion

    #region Decode

    /// <summary>Decodes a Quoted-Printable encoded read-only character span.</summary>
    /// <param name="data"> The Quoted-Printable encoded read-only character span.</param>
    /// <param name="textEncoding">The <see cref="Encoding"/> that had been used to encode 
    /// <paramref name="data"/>, or <c>null</c> to choose <see cref="Encoding.UTF8"/>.</param>
    /// <returns><paramref name="data"/> decoded. If <paramref name="data"/> is empty,
    /// <see cref="string.Empty" /> is returned.</returns>
    public static string Decode(ReadOnlySpan<char> data, Encoding? textEncoding)
    {
        data = data.TrimEnd(); // remove padding white space at the end

        if (data.IsEmpty)
        {
            return "";
        }

        using ArrayPoolHelper.SharedArray<byte> buf = ArrayPoolHelper.Rent<byte>(data.Length);
        Span<byte> bufSpan = buf.Array.AsSpan();

        int decodedLength = DecodeData(data, bufSpan);

        textEncoding ??= Encoding.UTF8;
        string s = textEncoding.GetString(buf.Array, 0, decodedLength);

        s = NormalizeLineBreaksOnUnixSystems(s);

        return s;

        ///////////////////////////////////////////////////////////////////////

        [ExcludeFromCodeCoverage]
        static string NormalizeLineBreaksOnUnixSystems(string s)
        {
            // Adjust encoded hard-linebreaks for Unix systems
            if (Environment.NewLine != NEW_LINE)
            {
                s = s.Replace(NEW_LINE, Environment.NewLine, StringComparison.Ordinal);
            }

            return s;
        }
    }

    /// <summary>
    /// Decodes any data that are Quoted-Printable encoded.
    /// </summary>
    /// <param name="data">A read-only character span that represents 
    ///  Quoted-Printable encoded data.</param>
    /// <returns>A <see cref="byte"/> array that contains the decoded data
    ///  from <paramref name="data"/>. If <paramref name="data"/> is
    ///  empty, an empty <see cref="byte"/> array is returned.</returns>
    public static byte[] DecodeData(ReadOnlySpan<char> data)
    {
        if (data.IsEmpty)
        {
            return [];
        }
        using ArrayPoolHelper.SharedArray<byte> buf = ArrayPoolHelper.Rent<byte>(data.Length);
        Span<byte> bufSpan = buf.Array.AsSpan();

        return bufSpan.Slice(0, DecodeData(data, bufSpan)).ToArray();
    }

    private static int DecodeData(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        Debug.Assert(chars.Length > 0);

        int byteIdx = 0;

        Span<char> charr = stackalloc char[2];

        bool skipWS = false;
        int byteLengthToParse = 0;

        for (int i = 0; i < chars.Length - 1; i++)
        {
            char c = chars[i];

            if (skipWS)
            {
                if (c == '\n')
                {
                    skipWS = false;
                }
                continue;
            }

            if (c == '=')
            {
                char next = chars[i + 1];

                if (next.IsWhiteSpace()) //  =\r\n
                {
                    i++;
                    skipWS = true;
                    continue;
                }

                byteLengthToParse = 2;

                if (next == '=') // ==\r\n0A
                {
                    i++;
                    skipWS = true;
                }

                continue;
            }

            if (byteLengthToParse == 2)
            {
                byteLengthToParse--;
                charr[0] = c;
                continue;
            }

            if (byteLengthToParse == 1)
            {
                byteLengthToParse--;
                charr[1] = c;
                bytes[byteIdx++] = HexToByte(charr);
                continue;
            }

            bytes[byteIdx++] = (byte)c;
        }

        if (skipWS)
        {
            return byteIdx;
        }

        char lastChar = chars[chars.Length - 1];

        if (byteLengthToParse == 1)
        {
            charr[1] = lastChar;
            bytes[byteIdx++] = HexToByte(charr);
        }
        else
        {
            bytes[byteIdx++] = (byte)lastChar;
        }

        return byteIdx;

        static byte HexToByte(ReadOnlySpan<char> charr)
        {
#if NETSTANDARD2_0 || NET462
            try
            {
                return (byte)(Uri.FromHex(charr[1]) + (Uri.FromHex(charr[0]) << 4));
            }
            catch
            {
                return (byte)'?';
            }
#else
            return byte.TryParse(charr, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out byte result)
                ? result
                : (byte)'?';
#endif
        }
    }

    #endregion
}//class
