using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable
{
    /// <summary>
    /// Eine Klasse, die Methoden zum Codieren und 
    /// Encodieren im Quoted-Printable-Format enthält.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    internal static class QuotedPrintableConverter
    {
        internal const string STANDARD_LINEBREAK = "\r\n";

        private const string SOFT_LINEBREAK = "=\r\n";
        private const int SOFT_LINEBREAK_LENGTH = 3;
        private const int ENCODED_CHAR_LENGTH = 3;

        private const int MAX_ROWLENGTH = VCard.MAX_BYTES_PER_LINE; //76;
        private const int MIN_ROWLENGTH = ENCODED_CHAR_LENGTH + 1;


        # region Encode
        /// <summary>
        /// Codiert einen String in Quoted-Printable. Sollte Environment.NewLine auf dem ausführenden System nicht 
        /// "\r\n" sein, wird der String automatisch angepasst.
        /// </summary>
        /// <param name="value">Der String, der codiert werden soll. Ist <c>sb == null</c> wird
        /// ein <see cref="string.Empty">string.Empty</see> zurückgegeben.</param>
        /// <param name="firstLineOffset">Anzahl der nicht-enkodierten Zeichen, die in der ersten Zeile vor dem enkodierten Text kommen.</param>
        /// <returns>Der encodierte String. Wenn der übergebene String <c>null</c> oder Empty ist, wird string.Empty zurückgegeben.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Wird ausgelöst, wenn maxRowLength kleiner als 4 ist.</exception>
        public static string Encode(
            string? value,
            int firstLineOffset)
        {
            Debug.Assert(firstLineOffset >= 0);
            Debug.Assert(MAX_ROWLENGTH >= MIN_ROWLENGTH);

            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (Environment.NewLine != STANDARD_LINEBREAK)
            {
#if NET40
                value = value.Replace(Environment.NewLine, STANDARD_LINEBREAK);
#else
                value = value.Replace(Environment.NewLine, STANDARD_LINEBREAK, StringComparison.Ordinal);
#endif
            }


            //unerlaubte Zeichen codieren
            StringBuilder sb = ProcessCoding(Encoding.UTF8.GetBytes(value));


            EncodeLastCharInLineIfItsWhitespace(sb);
            MakeSoftLineBreaks(firstLineOffset, sb);
            return sb.ToString();


            ////////////////////////////////////////

            static void EncodeLastCharInLineIfItsWhitespace(StringBuilder sb)
            {
                // wenn das letzte Zeichen einer Zeile Whitespace ist, dann codieren
                int sbLast = sb.Length - 1;
                char c = sb[sbLast];

                if (char.IsWhiteSpace(c))
                {
                    _ = sb.Remove(sbLast, 1).Append('=').Append(((byte)c).ToString("X02", CultureInfo.InvariantCulture));
                }
            }
        }


        private static void MakeSoftLineBreaks(int firstLineOffset, StringBuilder sb)
        {
            //firstLineOffset %= MAX_ROWLENGTH;
            //int charsBeforeSoftlineBreak = MAX_ROWLENGTH - 1;

            int firstLineLength = Math.Max(MAX_ROWLENGTH - firstLineOffset, MIN_ROWLENGTH);


            for (int lineLength = firstLineLength;
                lineLength < sb.Length; // mindestens 1 Zeichen nach dem letzten Soft-Linebreak
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
                        // in diesem Fall kann es sich nur um TAB und SPACE handeln
                        lastCharIndex = EncodeLastChar(sb, lastChar, lastCharIndex);
                        lineLength = InsertSoftlineBreak(sb, lastCharIndex + 1);
                    }
                    else
                    {
                        --lastCharIndex;
                    }
                }//for
            }//for (sb)

            /////////////////////////////////////////////////////////////////////////////////////

            static int EncodeLastChar(StringBuilder sb, char lastChar, int lastCharIndex)
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



        private static StringBuilder ProcessCoding(IEnumerable<byte> data)
        {
            var sb = new StringBuilder();

            foreach (byte bt in data)
            {
                _ = HasToBeQuoted(bt) ? sb.Append('=').Append(bt.ToString("X02", CultureInfo.InvariantCulture)) : sb.Append((char)bt);
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
        /// <summary>
        /// Decodiert einen Quoted-Printable codierten String. Zeilenwechselzeichen werden an das auf dem System übliche Zeilenwechselzeichen angepasst.
        /// </summary>
        /// <remarks>Wenn der übergebene String <c>null</c> oder Empty ist, wird <c>null</c> zurückgegeben.</remarks>
        /// <param name="qpCoded">Der codierte String. Wird <c>null</c> übergeben, wird ein Leerstring zurückgegeben.</param>
        /// <param name="textEncoding">Die Textencodierung, der der codierte String entspricht. Als Standard wird <see cref="Encoding.UTF8"/> angenommen.
        /// (Wird auch gewählt, wenn dem Parameter <c>null</c> übergeben wird.)</param>
        /// <returns>Der decodierte String.</returns>
        public static string? Decode(
            string? qpCoded,
            Encoding? textEncoding = null)
        {
            if (string.IsNullOrEmpty(qpCoded))
            {
                return null;
            }

            byte[] bytes = DecodeData(qpCoded);


            if (textEncoding == null)
            {
                textEncoding = Encoding.UTF8;
            }

            string s = textEncoding.GetString(bytes);

            //Anpassen des NewLine-Zeichens für Unix-Systeme
            if (Environment.NewLine != STANDARD_LINEBREAK) // notwendig, wenn Hard-Linebreaks codiert waren
            {
#if NET40
                s = s.Replace(STANDARD_LINEBREAK, Environment.NewLine);
#else
                s = s.Replace(STANDARD_LINEBREAK, Environment.NewLine, StringComparison.Ordinal);
#endif
            }

            return s;
        }


        /// <summary>
        /// Decodiert beliebige Daten, die im Quoted-Printable-Format codiert sind.
        /// </summary>
        /// <remarks>Wenn der übergebene String <c>null</c> oder Empty ist, wird ein Byte-Array 
        /// der Länge 0 zurückgegeben.</remarks>
        /// <param name="qpCoded">Der codierte String. Wird <c>null</c> übergeben, wird ein Byte-Array der Länge 0 zurückgegeben.</param>
        /// <returns>Die decodierten Daten als Byte-Array.</returns>
        public static byte[] DecodeData(
            string? qpCoded)
        {
            if (string.IsNullOrEmpty(qpCoded))
            {
#if NET40
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }

            //Ausbessern illegaler Soft - Line - Breaks, die auf Unix - Systemen entstanden sein könnten.
            //qpCoded = qpCoded.Replace("=\n", "=\r\n");

            using var reader = new StringReader(qpCoded);

            var sb = new StringBuilder(qpCoded.Length);

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

            var bytes = new byte[qpCoded.Length];


#if NET40
            char[] charr = new char[2];
#else
            Span<char> charr = stackalloc char[2];
#endif

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


#if NET40
            static byte HexToByte(char[] charr)
            {
                string s = new(charr);

                try
                {
                    return Convert.ToByte(s, 16);
                }
                catch (Exception)
                {
                    return (byte)'?';
                }
            }
#else
            static byte HexToByte(Span<char> charr)
            {
                try
                {
                    return byte.Parse(charr, NumberStyles.AllowHexSpecifier);
                }
                catch (Exception)
                {
                    return (byte)'?';
                }
            }
#endif
        }

#endregion
    }//class

}//namespace
