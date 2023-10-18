using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards;

    /// <summary> Hilfsklasse, die dem automatischen Erkennen und korrekten Laden von
    /// VCF-Dateien dient, die in einer anderen Codepage als UTF-8 gespeichert sind.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    /// <remarks>
    /// <para>
    /// VCF-Dateien des Standards vCard 2.1 wurden von Outlook und Outlook Express früher
    /// in der ANSI-Kodierung des jeweiligen Windows-Systems gespeichert, ohne in jedem
    /// Fall die erweiterten Zeichen mit Quoted-Printable-Kodierung zu kodieren. Auch
    /// VCF-Dateien des Standards vCard 3.0 können prinzipiell ANSI-kodiert sein, wenn
    /// sie z.B. in einer E-Mail übertragen wurden, die in ihrem Content-Header die
    /// Kodierung festlegte. Erst seit vCard 4.0 ist die Kodierung UTF-8 verbindlich
    /// festgelegt.
    /// </para>
    /// <para>
    /// Die Klasse ist ein Wrapper um die <see cref="VCard" />.<see cref="VCard.LoadVcf(string,
    /// Encoding?)" />-Methode, der die Datei zunächst als UTF-8 lädt. Wenn es dabei
    /// zu Dekodierungsfehlern kommt, untersucht die Methode die VCard-Objekte auf evtl.
    /// vorhandene <c>CHARSET</c>-Parameter, die einen Anhaltspunkt auf die zu verwendende
    /// Textenkodierung geben. Wird ein Hinweis gefunden, wird die Datei mit der ermittelten
    /// Textenkodierung erneut geladen, andernfalls mit der im Konstruktor als Fallback
    /// angegebenen Kodierung.
    /// </para>
    /// <para>
    /// <c>CHARSET</c>-Parameter gibt es nur im vCard-Standard 2.1.
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
    /// saved as UTF-8.</summary>
    /// <param name="fallbackCodePage">Die Nummer der Codepage. Default ist 1252 für
    /// <c>windows-1252</c>.</param>
    /// <exception cref="ArgumentException">The number specified for <paramref name="fallbackCodePage"
    /// /> could not be mapped to an ANSI code page.</exception>
    public AnsiFilter(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));

        _utf8 = InitUtf8Encoding();
        _encodingCache = InitEncodingCache();
    }

    /// <summary>Initializes an instance of the <see cref="AnsiFilter" /> class with
    /// the <see cref="Encoding.WebName" /> property of the <see cref="Encoding" />
    /// object that shall be used for the reading of VCF files that are not UTF-8.</summary>
    /// <param name="fallbackEncodingWebName"> <see cref="Encoding.WebName" /> property
    /// of the <see cref="Encoding" /> object.</param>
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
    

    /// <summary> <see cref="Encoding.WebName" />-Eigenschaft des <see cref="Encoding"
    /// />-Objekts, das zum Laden von VCF-Dateien verwendet wird, die nicht im UTF-8-Format
    /// vorliegen, wenn kein Hinweis zur Verwendung eines anderen <see cref="Encoding"
    /// />s gefunden wird. </summary>
    public string FallbackEncodingWebName => _ansi.WebName;


    /// <summary> Lädt eine VCF-Datei und wählt dabei automatisch das geeignete <see
    /// cref="Encoding" /> aus. </summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public virtual IList<VCard> LoadVcf(string fileName) => LoadVcf(fileName, out _);


    /// <summary> Lädt eine VCF-Datei und wählt dabei automatisch das geeignete <see
    /// cref="Encoding" /> aus. </summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <param name="encodingWebName"> <see cref="Encoding.WebName" /> property of the
    /// <see cref="Encoding" /> object, which had been used to load the VCF file. The
    /// parameter is passed uninitialized.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
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
                IEnumerable<KeyValuePair<VCdProp, object>> keyValuePairs = vCard;

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
