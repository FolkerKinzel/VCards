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
    /// <summary>Encodes a <see cref="string"/> into Quoted-Printable. </summary>
    /// <param name="value">The <see cref="string"/> to encode or <c>null</c>.</param>
    /// <param name="firstLineOffset">Length of the current line in the text before
    /// <paramref name="value"/> starts. (Used to compute the correct line wrapping.)</param>
    /// <returns><paramref name="value"/> Quoted-Printable encoded. If 
    /// <paramref name="value"/> is <c>null</c>, <see cref="string.Empty"/> is 
    /// returned.</returns>
    /// <remarks>If <see cref="Environment.NewLine"/> is not "\r\n" on the executing 
    /// platform, the <see cref="string"/> will be adjusted automatically.</remarks>
    internal static string Encode(
        string? value,
        int firstLineOffset)
    {
        Debug.Assert(firstLineOffset >= 0);
        Debug.Assert(MAX_ROWLENGTH >= MIN_ROWLENGTH);

        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        value = NormalizeLineBreaksOnUnixSystems(value);

        StringBuilder sb = ProcessCoding(Encoding.UTF8.GetBytes(value));

        EncodeLastCharIfItIsWhiteSpace(sb);
        MakeSoftLineBreaks(firstLineOffset, sb);
        return sb.ToString();

        ////////////////////////////////////////

        [ExcludeFromCodeCoverage]
        static string NormalizeLineBreaksOnUnixSystems(string value)
        {
            if (!StringComparer.Ordinal.Equals(Environment.NewLine, NEW_LINE))
            {
                value = value.Replace(Environment.NewLine, NEW_LINE, StringComparison.Ordinal);
            }

            return value;
        }

        static void EncodeLastCharIfItIsWhiteSpace(StringBuilder sb)
        {
            int sbLast = sb.Length - 1;
            char c = sb[sbLast];

            if (char.IsWhiteSpace(c))
            {
                sb.Length = sbLast;
                _ = sb.Append('=').Append(((byte)c).ToString("X02", CultureInfo.InvariantCulture));
            }
        }
    }

    private static void MakeSoftLineBreaks(int firstLineOffset, StringBuilder sb)
    {
        for (int lineLength = firstLineOffset >= MAX_ROWLENGTH - MIN_ROWLENGTH
                ? InsertSoftlineBreak(sb, 0) + MAX_ROWLENGTH
                : Math.Max(MAX_ROWLENGTH - firstLineOffset, MIN_ROWLENGTH);
            lineLength < sb.Length; // at least 1 Char after the last soft-linebreak
            lineLength += MAX_ROWLENGTH)
        {
            int lastCharIndex = lineLength - 2;

            for (int backwardCounter = 1; backwardCounter <= ENCODED_CHAR_LENGTH; backwardCounter++)
            {
                char lastChar = sb[lastCharIndex];

                if (!char.IsWhiteSpace(lastChar) && lastChar != '=')
                {
                    lineLength = InsertSoftlineBreak(sb, lastCharIndex + 1);
                    break;
                }
                else if (backwardCounter == ENCODED_CHAR_LENGTH)
                {
                    // TAB or SPACE
                    lastCharIndex = EncodeLastCharInLine(sb, lastChar, lastCharIndex);
                    lineLength = InsertSoftlineBreak(sb, lastCharIndex + 1);
                }
                else
                {
                    --lastCharIndex;
                }
            }//for
        }//for (sb)

        /////////////////////////////////////////////////////////////////////////////////////

        static int EncodeLastCharInLine(StringBuilder sb, char lastChar, int lastCharIndex)
        {
            _ = sb.Remove(lastCharIndex, 1)
                  .Insert(lastCharIndex, '=')
                  .Insert(lastCharIndex + 1, ((byte)lastChar).ToString("X02", CultureInfo.InvariantCulture));

            return lastCharIndex - 1 + ENCODED_CHAR_LENGTH;
        }

        static int InsertSoftlineBreak(StringBuilder sb, int softlineBreakIndex)
        {
            _ = sb.Insert(softlineBreakIndex, SOFT_LINEBREAK);
            return softlineBreakIndex + SOFT_LINEBREAK_LENGTH;
        }
    }

    private static StringBuilder ProcessCoding(byte[] data)
    {
        var sb = new StringBuilder();

        var span = data.AsSpan();

        foreach (byte bt in span)
        {
            _ = HasToBeQuoted(bt) ? sb.Append('=').Append(bt.ToString("X02", CultureInfo.InvariantCulture))
                                  : sb.Append((char)bt);
        }
        return sb;

        ///////////////////////////////////

        static bool HasToBeQuoted(byte bt)
        {
            return bt != (byte)'\t' && (bt > 126 || bt == (byte)'=' || bt < 32 || bt == (byte)'\r' || bt == (byte)'\n');
        }
    }

    #endregion

    #region Decode

    /// <summary>Decodes a Quoted-Printable encoded <see cref="string"/>.</summary>
    /// <param name="qpEncoded"> The Quoted-Printable encoded <see cref="string"/>.</param>
    /// <param name="textEncoding">The <see cref="Encoding"/> that had been used to encode 
    /// <paramref name="qpEncoded"/>, or <c>null</c> to choose <see cref="Encoding.UTF8"/>.</param>
    /// <returns><paramref name="qpEncoded"/> decoded. If <paramref name="qpEncoded"/> is <c>null</c>,
    /// <see cref="string.Empty" /> is returned.</returns>
    public static string Decode(
        string? qpEncoded,
        Encoding? textEncoding)
    {
        if (string.IsNullOrEmpty(qpEncoded))
        {
            return "";
        }

        byte[] bytes = DecodeData(qpEncoded);

        textEncoding ??= Encoding.UTF8;
        string s = textEncoding.GetString(bytes);

        s = NormalizeLineBreaksOnUnixSystems(s);

        return s;

        ///////////////////////////////////////////////////////////////////////

        [ExcludeFromCodeCoverage]
        static string NormalizeLineBreaksOnUnixSystems(string s)
        {
            //Anpassen des NewLine-Zeichens für Unix-Systeme
            if (Environment.NewLine != NEW_LINE) // notwendig, wenn Hard-Linebreaks codiert waren
            {
                s = s.Replace(NEW_LINE, Environment.NewLine, StringComparison.Ordinal);
            }

            return s;
        }
    }

    /// <summary>Decodes any data that are Quoted-Printable encoded.</summary>
    /// <param name="qpEncoded">A <see cref="string"/> that represents 
    /// Quoted-Printable encoded data, or <c>null</c>.</param>
    /// <returns>A <see cref="byte"/> array that contains the data
    /// from <paramref name="qpEncoded"/>. If <paramref name="qpEncoded"/> is
    /// <c>null</c>, an empty <see cref="byte"/> array is returned.</returns>
    public static byte[] DecodeData(
        string? qpEncoded)
    {
        if (string.IsNullOrEmpty(qpEncoded))
        {
            return [];
        }

        //Ausbessern illegaler Soft - Line - Breaks, die auf Unix - Systemen entstanden sein könnten.
        //qpCoded = qpCoded.Replace("=\n", "=\r\n");

        using var reader = new StringReader(qpEncoded);

        var sb = new StringBuilder(qpEncoded.Length);

        string? zeile;
        while (null != (zeile = reader.ReadLine()))
        {
            int last = zeile.Length - 1;

            //if (last == -1)
            //{
            //    continue; // unerlaubte Leerzeile
            //}

            sb.Append(zeile);
            last = sb.Length - 1;

            //Soft-Line-Break entfernen
            if (sb[last] == '=')
            {
                _ = sb.Remove(last, 1).TrimEnd();
            }
            else
            {
                _ = sb.TrimEnd();

                //Hard-Line-Break wiederherstellen
                sb.AppendLine();
            }
        }

        //letzten Hard-Line-Break wieder entfernen
        sb.TrimEnd();

        var bytes = new byte[qpEncoded.Length];

        Span<char> charr = stackalloc char[2];

        int j = 0;

        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] == '=')
            {
                if (i > sb.Length - 3)
                {
                    break; // abgeschnittener String
                }
                else
                {
                    charr[0] = sb[++i];
                    charr[1] = sb[++i];
                    bytes[j++] = HexToByte(charr);
                }
            }
            else
            {
                bytes[j++] = (byte)sb[i];
            }
        }

        Array.Resize(ref bytes, j);
        return bytes;


        static byte HexToByte(ReadOnlySpan<char> charr)
        {
            try
            {
#if NETSTANDARD2_0 || NET462
                return (byte)(Uri.FromHex(charr[1]) + Uri.FromHex(charr[0]) * 16);
#else
                return byte.Parse(charr, NumberStyles.AllowHexSpecifier);
#endif
            }
            catch (Exception)
            {
                return (byte)'?';
            }
        }
    }

    #endregion
}//class
