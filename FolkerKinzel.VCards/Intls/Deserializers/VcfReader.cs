using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal class VcfReader : IEnumerable<VcfRow>
    {
        private static readonly Regex _vCardBegin =
            new Regex(@"\ABEGIN[ \t]*:[ \t]*VCARD[ \t]*\z",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex _vCardEnd =
            new Regex(@"\AEND[ \t]*:[ \t]*VCARD[ \t]*\z",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private readonly TextReader _reader;
        private readonly VCardDeserializationInfo _info;
        private readonly VCdVersion _versionHint;


        internal VcfReader(TextReader reader, VCardDeserializationInfo info, VCdVersion versionHint = VCdVersion.V2_1)
        {
            this._reader = reader;
            this._info = info;
            this.EOF = false;
            this._versionHint = versionHint;
        }

        public bool EOF { get; private set; }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public IEnumerator<VcfRow> GetEnumerator()
        {
            // Die TextReader.ReadLine()-Methode normalisiert die Zeilenwechselzeichen!

            Debug.WriteLine("");

            string s;

            if (EOF)
            {
                yield break;
            }

            do //findet den Anfang der vCard
            {
                s = _reader.ReadLine();

                if (s is null) //Dateiende
                {
                    this.EOF = true;
                    yield break;
                }

                Debug.WriteLine(s);

            } while (!_vCardBegin.IsMatch(s));


            _ = _info.Builder.Clear(); // nötig, wenn die vcf-Datei mehrere vCards enthält


            bool isFirstLine = true;
            bool isVcard_2_1 = _versionHint == VCdVersion.V2_1;

            do
            {
                s = _reader.ReadLine();

                if (s is null) // Dateiende: Sollte END:VCARD fehlen, wird die vCard nicht gelesen.
                {
                    EOF = true;
                    yield break;
                }

                Debug.WriteLine(s);

                if (isFirstLine && isVcard_2_1 && s.Length != 0)
                {
                    isFirstLine = false;
                    isVcard_2_1 = GetIsVcard_2_1(s);
                }


                if (s.Length != 0 && char.IsWhiteSpace(s[0])) //vCard-Wrapping (Dies kann kein "BEGIN:VCARD" und kein "END:VCARD" sein.)
                {
                    Debug.WriteLine("  == vCard Line-Wrapping detected ==");

                    int insertPosition = _info.Builder.Length;
                    _ = _info.Builder.Append(s);

                    if (!isVcard_2_1)
                    {
                        _ = _info.Builder.Remove(insertPosition, 1); //automatisch eingefügtes Leerzeichen wieder entfernen
                    }
                    continue;
                }
                else if (isVcard_2_1 && _info.Builder.Length != 0)
                {
                    VcfRow? tmpRow = CreateVcfRow(out string tmp);

                    if (tmpRow is null)
                    {
                        _ = _info.Builder.Append(s);
                        continue;
                    }


                    if (tmpRow.Parameters.Encoding == VCdEncoding.QuotedPrintable && tmp[tmp.Length - 1] == '=') // QuotedPrintable Soft-Linebreak (Dies kann kein "BEGIN:VCARD" und kein "END:VCARD" sein.)
                    {
                        Debug.WriteLine("  == QuotedPrintable Soft-Linebreak detected ==");

                        _ = _info.Builder.Append(tmp);

                        if (ConcatQuotedPrintableSoftLineBreak(s))
                        {
                            VcfRow? vcfRow = CreateVcfRow(out _);

                            if (vcfRow != null)
                            {
                                yield return vcfRow;
                            }

                            s = "";
                            continue;
                        }
                        else
                        {
                            yield break;
                        }
                    }
                    else if (tmpRow.Parameters.Encoding == VCdEncoding.Base64)
                    {
                        Debug.WriteLine("  == vCard 2.1 Base64 detected ==");

                        if (s.Length == 0) // einzeiliges Base64
                        {
                            yield return tmpRow;
                            continue;
                        }
                        else
                        {
                            _ = _info.Builder.Append(tmp);
                            if (ConcatVcard2_1Base64(s))
                            {
                                VcfRow? vcfRow = CreateVcfRow(out _);

                                if (vcfRow != null)
                                {
                                    yield return vcfRow;
                                }

                                s = "";
                                continue;
                            }
                            else
                            {
                                yield break;
                            }
                        }
                    }
                    else
                    {
                        yield return tmpRow;
                    }

                    _ = _info.Builder.Append(s);
                }
                else
                {
                    if (_info.Builder.Length != 0)
                    {
                        if (_vCardBegin.IsMatch(s)) //eingebettete VCard 2.1. AGENT-vCard:
                        {
                            Debug.WriteLine("  == Embedded VCARD 2.1 vCard detected ==");

                            ConcatNestedVcard(s);

                            if (EOF)
                            {
                                yield break;
                            }

                            s = string.Empty; //damit die äußere Schleife nicht endet

                            VcfRow? vcfRow = CreateVcfRow(out _);

                            if (vcfRow != null)
                            {
                                yield return vcfRow;
                            }
                        }
                        else
                        {
                            // s stellt den Beginn einer neuen VcfRow dar. Deshalb enthält
                            // builder bereits eine vollständige VcfRow, die erzeugt werden muss,
                            // bevor s in builder geladen werden kann:

                            VcfRow? vcfRow = CreateVcfRow(out _);

                            if (vcfRow != null)
                            {
                                yield return vcfRow;
                            }
                        }
                    } //if
                    _ = _info.Builder.Append(s);
                }//else

            } while (!_vCardEnd.IsMatch(s));

            yield break;
        }



        private static bool GetIsVcard_2_1(string s)
        {
#if NET40
            if (s.StartsWith("VERSION", StringComparison.OrdinalIgnoreCase) && !s.Contains("2.1"))
#else
            if (s.StartsWith("VERSION", StringComparison.OrdinalIgnoreCase) && !s.Contains("2.1", StringComparison.Ordinal))
#endif
            {
                Debug.WriteLine("  == No vCard 2.1 detected ==");
                return false;
            }

            return true;
        }


        private bool ConcatQuotedPrintableSoftLineBreak(string s)
        {
            _ = _info.Builder.AppendLine().Append(s);

            while (s.Length == 0 || s[s.Length - 1] == '=')
            {
                s = _reader.ReadLine();

                if (s is null)
                {
                    EOF = true;
                    return false;
                }

                Debug.WriteLine(s);

                if (s.Length == 0)
                {
                    continue;
                }

                if (_vCardEnd.IsMatch(s))
                {
                    return false;
                }

                _ = _info.Builder.AppendLine().Append(s);
            }

            return true;
        }

        private bool ConcatVcard2_1Base64(string s)
        {
            while (s.Length != 0) // Leerzeile beendet Base64
            {
                _ = _info.Builder.Append(s);

                s = _reader.ReadLine();

                if (s is null) // EOF
                {
                    EOF = true;
                    return false;
                }

                Debug.WriteLine(s);

                if (_vCardEnd.IsMatch(s))
                {
                    return false;
                }
            }

            return true;
        }


        private void ConcatNestedVcard(string s)
        {
            _ = _info.Builder.Append(s);

            do
            {
                s = _reader.ReadLine();

                if (s is null)
                {
                    EOF = true;
                    return;
                }

                Debug.WriteLine(s);

                if (s.Length == 0)
                {
                    continue;
                }

                _ = _info.Builder.Append(VCard.NewLine);
                _ = _info.Builder.Append(s);
            }
            while (!_vCardEnd.IsMatch(s));
        }



        private VcfRow? CreateVcfRow(out string toParse)
        {
            toParse = _info.Builder.ToString();
            var vcfRow = VcfRow.Parse(toParse, _info);
            _ = _info.Builder.Clear();

            return vcfRow;
        }




    }
}
