using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;

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
    private readonly List<ReadOnlyMemory<char>> _list = [];

    internal bool EOF { get; private set; }

    internal VcfRowReader(TextReader reader, VcfDeserializationInfo info, VCdVersion versionHint = VCdVersion.V2_1)
    {
        this._reader = reader;
        this._info = info;
        this._versionHint = versionHint;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

            s = null;
        }

        if (s == null)
        {
            this.EOF = true;
            return false;
        }

        return true;

        //////////////////////////////////////////////////

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool HandleException(Exception e)
            => e is ArgumentOutOfRangeException or OutOfMemoryException;
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
                    _list.Add(s.AsMemory());
                }
                else
                {
                    // remove inserted SPACE character
                    _list.Add(s.AsMemory(1));
                }

                continue;
            }
            else if (isVcard_2_1 && _list.Count != 0)
            {
                VcfRow? tmpRow = CreateVcfRowAndClearList(out ReadOnlyMemory<char> tmp);

                if (tmpRow is null)
                {
                    _list.Add(s.AsMemory());
                    continue;
                }

                if (tmpRow.Parameters.Encoding == Enc.QuotedPrintable && HasQuotedPrintableSoftLineBreak(tmp.Span)) 
                {
                    // QuotedPrintable soft-linebreak (This can't be "BEGIN:VCARD" or "END:VCARD".)
                    Debug.WriteLine("  == QuotedPrintable soft-linebreak detected ==");

                    _list.Add(tmp);

                    if (ConcatQuotedPrintableSoftLineBreak(s))
                    {
                        VcfRow? vcfRow = CreateVcfRowAndClearList(out _);

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
                            VcfRow? vcfRow = CreateVcfRowAndClearList(out _);

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
                        VcfRow? vcfRow = CreateVcfRowAndClearList(out _);

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

                _list.Add(s.AsMemory());
            }
            else
            {
                if (_list.Count != 0)
                {
                    // s represents the start of a new VcfRow. Therefore _list already contains
                    // a complete VcfRow-String. This must be parsed before s can be added to _list.
                    VcfRow? vcfRow = CreateVcfRowAndClearList(out _);

                    if (vcfRow is not null)
                    {
                        yield return vcfRow;
                    }

                } //if

                _list.Add(s.AsMemory());

            }//else

        } while (!s.StartsWith(END_VCARD, StringComparison.OrdinalIgnoreCase));

        yield break;
    }

    private static bool HasQuotedPrintableSoftLineBreak(ReadOnlySpan<char> span) 
        => span.TrimEnd().EndsWith('='); // QP soft line breaks may be padded with white space

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

        ReadOnlyMemory<char> newLine = QuotedPrintable.NEW_LINE.AsMemory();

        _list.Add(newLine);
        _list.Add(s.AsMemory());

        while (s.Length == 0 || HasQuotedPrintableSoftLineBreak(s.AsSpan()))
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

            _list.Add(newLine);
            _list.Add(s.AsMemory());
        }

        return true;
    }

    private bool ConcatVcard2_1Base64(string? s)
    {
        Debug.Assert(s is not null);

        while (s.Length != 0) // an empty line closes Base64
        {
            _list.Add(s.AsMemory());

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

        ReadOnlyMemory<char> newLine = VCard.NewLine.AsMemory();

        _list.Add(s.AsMemory());

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

            _list.Add(newLine);
            _list.Add(s.AsMemory());
        }
        while (!s.StartsWith(END_VCARD, StringComparison.OrdinalIgnoreCase));

        // if the embedded vCard embeds another vCard parsing fails
        return true;
    }

    /// <summary>
    /// Creates a new VCF row and clears List <see cref="_list"/>.
    /// </summary>
    /// <param name="parsed">Contains the parsed memory after the method returns.</param>
    /// <returns>The parsed <see cref="VcfRow"/>, or <c>null</c> if the content of
    /// <see cref="_list"/> could not be parsed.</returns>
    private VcfRow? CreateVcfRowAndClearList(out ReadOnlyMemory<char> parsed)
    {
        parsed = _list.Concat();
        var vcfRow = VcfRow.Parse(in parsed, _info);
        _list.Clear();

        return vcfRow;
    }
}
