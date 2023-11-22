using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Enums;
using System.Text;

namespace FolkerKinzel.VCards;

/// <summary> Helper class used to automatically detect and correctly load VCF files 
/// stored in a code page other than UTF-8.</summary>
/// <threadsafety static="true" instance="false" />
/// <remarks>
/// <para>
/// VCF files of the vCard&#160;2.1 standard were previously saved by Outlook and Outlook 
/// Express in the ANSI encoding of the current Windows system, without always encoding
/// the extended characters with quoted printable encoding. In principle, VCF files of 
/// the vCard&#160;3.0 standard can also be ANSI-encoded if, for example, they were transmitted
/// in an e-mail that specified the encoding in its content header. UTF-8 encoding has only
/// been mandatory since vCard&#160;4.0. 
/// </para>
/// <para>
/// This class is a wrapper around the <see cref="VCard" />.<see cref="VCard.LoadVcf(string, Encoding?)" /> 
/// method, which initially loads the file as UTF-8. If a decoding error occurs, the method 
/// examines the <see cref="VCard"/> objects for any <see cref="ParameterSection.CharSet"/> 
/// (<c>CHARSET</c>) parameters, which provide an indication of the <see cref="Encoding"/> 
/// to be used. If a hint is found, the file is reloaded with the determined <see cref="Encoding"/>,
/// otherwise with the <see cref="Encoding"/> specified in the 
/// <see cref="AnsiFilter.AnsiFilter(string)">constructor</see> as a fallback.
/// </para>
/// <para>
/// (The <c>CHARSET</c> parameter exists only in the vCard&#160;2.1 standard.)
/// </para>
/// </remarks>
/// <example>
/// <note type="note">
/// To make the code more readable, exception handling has been omitted from the
/// examples.
/// </note>
/// <code language="cs" source="..\Examples\AnsiFilterExample.cs" />
/// </example>
public class AnsiFilter
{
    private class EncodingCache
    {
        private readonly Dictionary<string, Encoding?> _cache = new(StringComparer.OrdinalIgnoreCase);

        public EncodingCache(Encoding fallbackEncoding, string utf8WebName)
        {
            _cache[fallbackEncoding.WebName] = fallbackEncoding;
            _cache[utf8WebName] = null;
        }

        internal Encoding? GetEncoding(string charSetName)
        {
            if (_cache.TryGetValue(charSetName, out Encoding? enc))
            {
                return enc;
            }

            enc = TextEncodingConverter.GetEncoding(charSetName);

            if (IsUtf8(enc))
            {
                _cache[charSetName] = null;
                return null;
            }

            enc = _cache.FirstOrDefault(x => x.Value?.CodePage == enc.CodePage).Value ?? enc;
            
            _cache[charSetName] = enc;
            _cache[enc.WebName] = enc;
            return enc;
        }
    }

    /// ///////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly EncodingCache _encodingCache;

    private readonly DecoderValidationFallback _decoderFallback = new DecoderValidationFallback();
    private readonly Encoding _utf8;
    private readonly Encoding _ansi;

    private const int UTF8_CODEPAGE = 65001;

    /// <summary>Initializes an instance of the <see cref="AnsiFilter" /> class with
    /// the number of the code page to use for the reading of VCF files that are not
    /// UTF-8 and that do not contain a hint in their CHARSET parameters which suggests 
    /// the use of a different <see cref="Encoding" />.</summary>
    /// <param name="fallbackCodePage">The number of the code page to use as a fallback. 
    /// The default value is 1252 (for <c>windows-1252</c>).</param>
    /// <exception cref="ArgumentException">The number specified for 
    /// <paramref name="fallbackCodePage" /> could not be mapped to an ANSI code page.</exception>
    public AnsiFilter(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));

        _utf8 = InitUtf8Encoding();
        _encodingCache = InitEncodingCache();
    }

    /// <summary>
    /// Initializes an instance of the <see cref="AnsiFilter" /> class with
    /// the <see cref="Encoding.WebName" /> property of the <see cref="Encoding" />
    /// object to use for reading VCF files that are not
    /// UTF-8 and that do not contain a hint in their CHARSET parameters which suggests 
    /// the use of a different <see cref="Encoding" />.
    /// </summary>
    /// <param name="fallbackEncodingWebName"> <see cref="Encoding.WebName" /> property
    /// of the <see cref="Encoding" /> object to use as a fallback.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="fallbackEncodingWebName"
    /// /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The identifier specified for <paramref name="fallbackEncodingWebName"
    /// /> could not be mapped to an ANSI code page.</exception>
    public AnsiFilter(string fallbackEncodingWebName)
    {
        _ansi = TextEncodingConverter.GetEncoding(
            fallbackEncodingWebName ?? throw new ArgumentNullException(nameof(fallbackEncodingWebName)));
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));

        _utf8 = InitUtf8Encoding();
        _encodingCache = InitEncodingCache();
    }

    /// <summary> <see cref="Encoding.WebName" /> property
    /// of the <see cref="Encoding" /> object to use as a fallback. </summary>
    public string FallbackEncodingWebName => _ansi.WebName;

    /// <summary> Loads a VCF file and automatically selects the appropriate 
    /// <see cref="Encoding" />.</summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public virtual IList<VCard> LoadVcf(string fileName) => LoadVcf(fileName, out _);

    /// <summary>  Loads a VCF file and automatically selects the appropriate 
    /// <see cref="Encoding" />. </summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <param name="encodingWebName"> After the method has finished the parameter
    /// contains the <see cref="Encoding.WebName" /> of the
    /// <see cref="Encoding" /> object, which had been used to load the VCF file. The
    /// parameter is passed uninitialized.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public virtual IList<VCard> LoadVcf(string fileName, out string encodingWebName)
    {
        Reset();
        encodingWebName = _utf8.WebName;

        IList<VCard> vCards = VCard.LoadVcf(fileName, _utf8);

        if(!HasError)
        {
            return vCards; 
        }

        string? charSet = GetCharsetFromVCards(vCards);

        if (charSet is null) // No CHARSET parameter
        {
            return ReadWithFallbackEncoding(fileName, out encodingWebName);
        }

        Encoding? enc = _encodingCache.GetEncoding(charSet);

        if (enc is null)
        {
            // It's not valid UTF-8, because the first reading produced an error. It
            // has a CHARSET parameter, but it's not readable. We decide to read the
            // file with the FallbackEncoding because ANSI decoded text can be
            // converted to another ANSI encoding after reading too.
            return ReadWithFallbackEncoding(fileName, out encodingWebName);
        }

        encodingWebName = enc.WebName;
        return VCard.LoadVcf(fileName, enc);

        //////////////////////////////////////////////////////////

        static string? GetCharsetFromVCards(IList<VCard> vCards)
        {
            foreach (var vCard in vCards.Where(x => x.Version == VCdVersion.V2_1))
            {
                IEnumerable<KeyValuePair<Prop, object>> keyValuePairs = vCard;

                string? charSet = keyValuePairs
                    .Where(x => x.Value is IEnumerable<AddressProperty> or IEnumerable<NameProperty> or IEnumerable<TextProperty>)
                    .Select(x => x.Value as IEnumerable<VCardProperty>)
                    .SelectMany(x => x!)
                    .FirstOrDefault(x => x.Parameters.CharSet != null)?.Parameters.CharSet;

                if (charSet != null) { return charSet; }
            }
            return null;
        }
    }

    private IList<VCard> ReadWithFallbackEncoding(string fileName, out string encodingWebName) 
    {
        encodingWebName = FallbackEncodingWebName;
        return VCard.LoadVcf(fileName, _ansi);
    }

    private static bool IsUtf8(Encoding encoding) => encoding.CodePage == AnsiFilter.UTF8_CODEPAGE;

    private void ThrowArgumentExceptionIfUtf8(string parameterName)
    {
        if (IsUtf8(_ansi))
        {
            throw new ArgumentException(Res.NoAnsiEncoding, parameterName);
        }
    }

    private bool HasError => _decoderFallback.HasError;

    private void Reset() => _decoderFallback.Reset();

    private Encoding InitUtf8Encoding() =>
        Encoding.GetEncoding(UTF8_CODEPAGE, EncoderFallback.ReplacementFallback, _decoderFallback);

    private EncodingCache InitEncodingCache()
    {
        Debug.Assert(_ansi != null);
        Debug.Assert(_utf8 != null);
        return new EncodingCache(_ansi, _utf8.WebName);
    }
}
