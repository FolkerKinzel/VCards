using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;


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
                    sb.Remove(sbLast, 1);
                    sb.Append('=');
                    sb.Append(((byte)c).ToString("X02", CultureInfo.InvariantCulture));
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

                _ = sb.Remove(lastCharIndex, 1);
                _ = sb.Insert(lastCharIndex, '=')
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
                if (HasToBeQuoted(bt))
                {
                    _ = sb.Append('=');
                    _ = sb.Append(bt.ToString("X02", CultureInfo.InvariantCulture));
                }
                else
                {
                    _ = sb.Append((char)bt);
                }
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
        /// <remarks>Wenn der übergebene String <c>null</c> oder Empty ist, wird string.Empty zurückgegeben.</remarks>
        /// <param name="qpCoded">Der codierte String. Wird <c>null</c> übergeben, wird ein Leerstring zurückgegeben.</param>
        /// <param name="textEncoding">Die Textencodierung, der der codierte String entspricht. Als Standard wird Encoding.UTF-8 angenommen.
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

            // Zeilen splitten
#if NET40
            string[] zeilen = qpCoded.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
#else
            string[] zeilen = qpCoded.Split(Environment.NewLine, StringSplitOptions.None);

#endif

            for (int i = 0; i < zeilen.Length; i++)
            {

                int last = zeilen[i].Length - 1;

                if (last == -1)
                {
                    continue; // unerlaubte Leerzeile
                }

                //Soft-Line-Break entfernen
                if (zeilen[i][last] == '=')
                {
                    zeilen[i] = zeilen[i].Remove(last).TrimEnd(null);
                }
                else
                {
                    zeilen[i] = zeilen[i].TrimEnd(null);

                    //Hard-Line-Break wiederherstellen
                    if (i < zeilen.Length - 1)
                    {
                        zeilen[i] += Environment.NewLine;
                    }
                }
            }

            var bytes = new List<byte>(qpCoded.Length);

            var sb = new StringBuilder();
            foreach (string? zeile in zeilen)
            {
                sb.Append(zeile);
            }


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
                        bytes.Add(HexToByte(new char[] { sb[++i], sb[++i] }));
                    }
                }
                else
                {
                    bytes.Add((byte)sb[i]);
                }
            }

            return bytes.ToArray();


            static byte HexToByte(char[] charr)
            {
                string s = new string(charr);

                try
                {
                    return Convert.ToByte(s, 16);
                }
                catch (Exception)
                {
                    return (byte)'?';
                }
            }
        }

        #endregion
    }//class

}//namespace
