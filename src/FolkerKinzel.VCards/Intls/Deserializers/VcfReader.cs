using System.Collections;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

#if !NET40
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal class VcfReader : IEnumerable<VcfRow>
{
    private const string BEGIN_VCARD = "BEGIN:VCARD";
    private const string END_VCARD = "END:VCARD";

    //private static readonly Regex _vCardBegin =
    //    new Regex(@"\ABEGIN[ \t]*:[ \t]*VCARD[ \t]*\z",
    //        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    //private static readonly Regex _vCardEnd =
    //    new Regex(@"\AEND[ \t]*:[ \t]*VCARD[ \t]*\z",
    //        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private readonly TextReader _reader;
    private readonly VcfDeserializationInfo _info;
    private readonly VCdVersion _versionHint;


    internal VcfReader(TextReader reader, VcfDeserializationInfo info, VCdVersion versionHint = VCdVersion.V2_1)
    {
        this._reader = reader;
        this._info = info;
        this.EOF = false;
        this._versionHint = versionHint;
    }

    public bool EOF { get; private set; }


    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private bool HandleException(Exception e)
    {
        if (e is ArgumentOutOfRangeException or OutOfMemoryException)
        {
            this.EOF = true;
            return true;
        }
        return false;
    }

    private bool ReadNextLine([NotNullWhen(true)] out string? s)
    {
        try
        {
            s = _reader.ReadLine();
        }
        catch (Exception e)
        {
            if (!HandleException(e))
            {
                throw;
            }
            EOF = true;
            s = null;
            return false;
        }

        if (s is null)
        {
            EOF = true;
            return false;
        }
        return true;
    }

    private bool FindBeginVcard()
    {
        string? s;

        do //findet den Anfang der vCard
        {
            if (!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

        } while (!s.StartsWith(BEGIN_VCARD, StringComparison.OrdinalIgnoreCase));

        return true;
    }

    public IEnumerator<VcfRow> GetEnumerator()
    {
        // Die TextReader.ReadLine()-Methode normalisiert die Zeilenwechselzeichen!

        Debug.WriteLine("");


        if (EOF)
        {
            yield break;
        }

        if (!FindBeginVcard())
        {
            yield break;
        }

        _ = _info.Builder.Clear(); // nötig, wenn die vcf-Datei mehrere vCards enthält

        bool isFirstLine = true;
        bool isVcard_2_1 = _versionHint == VCdVersion.V2_1;

        string? s;
        do
        {
            if(!ReadNextLine(out s))
            {
                // Dateiende: Sollte END:VCARD fehlen, wird die vCard nicht gelesen.
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
                else if (s.StartsWith(BEGIN_VCARD, StringComparison.OrdinalIgnoreCase)) //eingebettete VCard 2.1. AGENT-vCard:
                {
                    Debug.WriteLine("  == Embedded VCARD 2.1 vCard detected ==");

                    // Achtung: Version 2.1. unterstützt solche eingebetteten VCards in der AGENT-Property:
                    //
                    // AGENT:
                    // BEGIN:VCARD
                    // VERSION:2.1
                    // N:Friday;Fred
                    // TEL; WORK;VOICE:+1-213-555-1234
                    // TEL;WORK;FAX:+1-213-555-5678
                    // END:VCARD

                    _ = _info.Builder.Append(tmp);

                    if (ConcatNested_2_1Vcard(s))
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
                    // s stellt den Beginn einer neuen VcfRow dar. Deshalb enthält
                    // builder bereits eine vollständige VcfRow, die erzeugt werden muss,
                    // bevor s in builder geladen werden kann:

                    VcfRow? vcfRow = CreateVcfRow(out _);

                    if (vcfRow != null)
                    {
                        yield return vcfRow;
                    }
                } //if
                _ = _info.Builder.Append(s);
            }//else

        } while (!s.StartsWith(END_VCARD, StringComparison.OrdinalIgnoreCase));

        yield break;
    }



    private static bool GetIsVcard_2_1(string s)
    {
        if (s.StartsWith("VERSION", StringComparison.OrdinalIgnoreCase) && !s.Contains("2.1", StringComparison.Ordinal))
        {
            Debug.WriteLine("  == No vCard 2.1 detected ==");
            return false;
        }

        return true;
    }


    private bool ConcatQuotedPrintableSoftLineBreak(string? s)
    {
        Debug.Assert(s is not null);

        // QuotedPrintableConverter arbeitet plattformunabhängig mit 
        // Environment.NewLine

        _ = _info.Builder.AppendLine().Append(s);

        while (s.Length == 0 || s[s.Length - 1] == '=')
        {
            if(!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

            if (s.Length == 0)
            {
                continue;
            }

            //if (_vCardEnd.IsMatch(s))
            //{
            //    return false;
            //}

            _ = _info.Builder.AppendLine().Append(s);
        }

        return true;
    }


    private bool ConcatVcard2_1Base64(string? s)
    {
        Debug.Assert(s is not null);

        while (s.Length != 0) // Leerzeile beendet Base64
        {
            _ = _info.Builder.Append(s);

            if(!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

            //if (_vCardEnd.IsMatch(s))
            //{
            //    return false;
            //}
        }

        return true;
    }

 

    private bool ConcatNested_2_1Vcard(string? s)
    {
        Debug.Assert(s is not null);

        _ = _info.Builder.Append(s);

        do
        {
            if(!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

            if (s.Length == 0)
            {
                continue;
            }

            _ = _info.Builder.Append(VCard.NewLine).Append(s);
        }
        while (!s.StartsWith(END_VCARD, StringComparison.OrdinalIgnoreCase));
        // wenn die eingebettete vCard eine weitere eingebettete vCard enthält,
        // scheitert das Parsen

        return true;
    }


    private VcfRow? CreateVcfRow(out string toParse)
    {
        toParse = _info.Builder.ToString();
        var vcfRow = VcfRow.Parse(toParse, _info);
        _ = _info.Builder.Clear();

        return vcfRow;
    }

}
