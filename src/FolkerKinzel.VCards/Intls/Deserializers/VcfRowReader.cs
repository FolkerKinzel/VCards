using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal class VcfRowReader : IEnumerable<VcfRow>
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
    private readonly List<string> _list = [];


    internal VcfRowReader(TextReader reader, VcfDeserializationInfo info, VCdVersion versionHint = VCdVersion.V2_1)
    {
        this._reader = reader;
        this._info = info;
        this.EOF = false;
        this._versionHint = versionHint;
    }

    public bool EOF { get; private set; }

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
        // The TextReader.ReadLine() method normalizes the line breaks.
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

        do //finds the begin of the vCard
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
        Debug.WriteLine("");

        if (EOF)
        {
            yield break;
        }

        if (!FindBeginVcard())
        {
            yield break;
        }

        // needed if the VCF file contains more than one vCard
        _list.Clear();

        bool isFirstLine = true;
        bool isVcard_2_1 = _versionHint == VCdVersion.V2_1;

        string? s;
        do
        {
            if (!ReadNextLine(out s))
            {
                yield break;
            }

            Debug.WriteLine(s);

            if (isFirstLine && isVcard_2_1 && s.Length != 0)
            {
                isFirstLine = false;
                isVcard_2_1 = GetIsVcard_2_1(s);
            }

            if (s.Length != 0 && char.IsWhiteSpace(s[0]))
            {
                //vCard-Wrapping (This can't be "BEGIN:VCARD" or "END:VCARD".)
                Debug.WriteLine("  == vCard Line-Wrapping detected ==");

                if(isVcard_2_1)
                {
                    _list.Add(s);
                }
                else
                {
                    // remove inserted SPACE character
                    _list.Add(s.Substring(1));
                }

                continue;
            }
            else if (isVcard_2_1 && _list.Count != 0)
            {
                VcfRow? tmpRow = CreateVcfRow(out string tmp);

                if (tmpRow is null)
                {
                    _list.Add(s);
                    continue;
                }

                if (tmpRow.Parameters.Encoding == Enc.QuotedPrintable && tmp[tmp.Length - 1] == '=')
                {
                    // QuotedPrintable soft-linebreak (This can't be "BEGIN:VCARD" or "END:VCARD".)
                    Debug.WriteLine("  == QuotedPrintable soft-linebreak detected ==");

                    _list.Add(tmp);

                    if (ConcatQuotedPrintableSoftLineBreak(s))
                    {
                        VcfRow? vcfRow = CreateVcfRow(out _);

                        if (vcfRow is not null)
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
                else if (tmpRow.Parameters.Encoding == Enc.Base64)
                {
                    Debug.WriteLine("  == vCard 2.1 Base64 detected ==");

                    if (s.Length == 0) // one-line Base64
                    {
                        yield return tmpRow;
                        continue;
                    }
                    else
                    {
                        _list.Add(tmp);
                    
                        if (ConcatVcard2_1Base64(s))
                        {
                            VcfRow? vcfRow = CreateVcfRow(out _);

                            if (vcfRow is not null)
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
                else if (s.StartsWith(BEGIN_VCARD, StringComparison.OrdinalIgnoreCase)) // embedded VCard 2.1. AGENT-vCard:
                {
                    Debug.WriteLine("  == Embedded VCARD 2.1 vCard detected ==");

                    // Caution: Version 2.1. supports such embedded vCards in the AGENT property:
                    //
                    // AGENT:
                    // BEGIN:VCARD
                    // VERSION:2.1
                    // N:Friday;Fred
                    // TEL; WORK;VOICE:+1-213-555-1234
                    // TEL;WORK;FAX:+1-213-555-5678
                    // END:VCARD

                    _list.Add(tmp);

                    if (ConcatNested_2_1Vcard(s))
                    {
                        VcfRow? vcfRow = CreateVcfRow(out _);

                        if (vcfRow is not null)
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

                _list.Add(s);
            }
            else
            {
                if (_list.Count != 0)
                {
                    // s represents the start of a new VcfRow. Therefore _list already contains
                    // a complete VcfRow-String. This must be parsed before s can be added to _list.
                    VcfRow? vcfRow = CreateVcfRow(out _);

                    if (vcfRow is not null)
                    {
                        yield return vcfRow;
                    }

                } //if

                _list.Add(s);

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
        
        _list.Add(QuotedPrintable.NEW_LINE);
        _list.Add(s);

        while (s.Length == 0 || s[s.Length - 1] == '=')
        {
            if (!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

            if (s.Length == 0)
            {
                continue;
            }

            _list.Add(QuotedPrintable.NEW_LINE);
            _list.Add(s);
        }

        return true;
    }

    private bool ConcatVcard2_1Base64(string? s)
    {
        Debug.Assert(s is not null);

        while (s.Length != 0) // an empty line closes Base64
        {
            _list.Add(s);

            if (!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);
        }

        return true;
    }

    private bool ConcatNested_2_1Vcard(string? s)
    {
        Debug.Assert(s is not null);

        _list.Add(s);

        do
        {
            if (!ReadNextLine(out s))
            {
                return false;
            }

            Debug.WriteLine(s);

            if (s.Length == 0)
            {
                continue;
            }

            _list.Add(Environment.NewLine);
            _list.Add(s);
        }
        while (!s.StartsWith(END_VCARD, StringComparison.OrdinalIgnoreCase));

        // if the embedded vCard embeds another vCard parsing fails
        return true;
    }

    private VcfRow? CreateVcfRow(out string toParse)
    {
        toParse = _list.Count == 1 ? _list[0] :  string.Concat(_list);
        var vcfRow = VcfRow.Parse(toParse, _info);
        _list.Clear();

        return vcfRow;
    }
}
