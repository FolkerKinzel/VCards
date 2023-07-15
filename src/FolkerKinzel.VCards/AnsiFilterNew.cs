using System.Text;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.Strings;

namespace FolkerKinzel.VCards;

/// <summary>
/// Hilfsklasse, die dem korrekten Laden von VCF-Dateien dient, die in einer ANSI-Codepage gespeichert wurden.
/// </summary>
/// <threadsafety static="true" instance="false"/>
/// <remarks>
/// <para>
/// VCF-Dateien des Standards vCard 2.1 wurden von Outlook und Outlook Express früher in der ANSI-Kodierung des jeweiligen Windows-Systems gespeichert, 
/// ohne in jedem Fall die erweiterten Zeichen mit Quoted-Printable-Kodierung zu kodieren. Auch VCF-Dateien des Standards vCard 3.0 können prinzipiell
/// ANSI-kodiert sein, wenn sie z.B. in einer E-Mail übertragen wurden, die in ihrem Content-Header die Kodierung festlegte. Erst seit vCard 4.0 ist die 
/// Kodierung UTF-8 verbindlich festgelegt.
/// </para>
/// <para>
/// Die Klasse ist ein Wrapper um die <see cref="VCard"/>.<see cref="VCard.LoadVcf(string, Encoding?)"/>-Methode, der zunächst prüft, ob die zu
/// ladende Datei korrektes UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut mit einer als Fallback angegebenen Kodierung geladen.
/// </para>
/// <para>
/// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException"/>s 
/// abgefangen und Dateien müssen ggf. zweimal geladen werden.
/// </para>
/// <para>
/// Verwenden Sie die Klasse, wenn Sie mit relativer Sicherheit vorhersagen können, welche ANSI-Codepage ggf. verwendet worden ist oder wenn Sie nicht mit 
/// vCard 2.1 - Dateien arbeiten. Falls Sie es mit
/// vCard 2.1-Dateien zu tun haben, die in verschiedenen ANSI-Codepages gespeichert worden sind, können Sie auf die Klasse <see cref="MultiAnsiFilter"/>
/// zurückgreifen, die zusätzlich in den eingelesenen vCard-Dateien noch nach <c>CHARSET</c>-Parametern fahndet.
/// </para>
/// </remarks>
/// <example>
/// <note type="note">Der leichteren Lesbarkeit wegen, wird in den Beispielen auf Ausnahmebehandlung verzichtet.</note>
/// <code language="cs" source="..\Examples\AnsiFilterExample.cs"/>
/// </example>
public class AnsiFilterNew
{
    private class EncodingCache
    {
        private readonly Dictionary<string, Encoding?> _cache = new(StringComparer.OrdinalIgnoreCase);

        public EncodingCache(Encoding fallbackEncoding, string utf8WebName)
        {
            _cache[fallbackEncoding.WebName] = fallbackEncoding;
            _cache[utf8WebName] = null;
        }

        internal Encoding? GetEncoding(string charSet)
        {
            if (_cache.ContainsKey(charSet))
            {
                return _cache[charSet];
            }

            var enc = TextEncodingConverter.GetEncoding(charSet);

            if (IsUtf8(enc))
            {
                _cache[charSet] = null;
                return null;
            }

            enc = _cache.FirstOrDefault(x => x.Value?.CodePage == enc.CodePage).Value ?? enc;
            
            _cache[charSet] = enc;
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

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="AnsiFilterNew"/>-Klasse mit der Nummer
    /// der Codepage, die für das Lesen von VCF-Dateien verwendet werden soll, die nicht im UTF-8-Format vorliegen.
    /// </summary>
    /// <param name="fallbackCodePage">Die Nummer der Codepage. Default ist
    /// 1252 für windows-1252.</param>
    /// <exception cref="ArgumentException">Die für <paramref name="fallbackCodePage"/> angegebene Nummer
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public AnsiFilterNew(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));

        _utf8 = InitUtf8Encoding();
        _encodingCache = InitEncodingCache();
    }

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="AnsiFilterNew"/>-Klasse mit dem <see cref="Encoding.WebName"/> des 
    /// <see cref="Encoding"/>-Objekts, das zum Lesen von VCF-Dateien verwendet werden soll, die nicht im UTF-8-Format vorliegen.
    /// </summary>
    /// <param name="fallbackEncodingWebName"><see cref="Encoding.WebName"/> des <see cref="Encoding"/>-Objekts.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackEncodingWebName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Der für <paramref name="fallbackEncodingWebName"/> angegebene Bezeichner
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public AnsiFilterNew(string fallbackEncodingWebName)
    {
        _ansi = TextEncodingConverter.GetEncoding(
            fallbackEncodingWebName ?? throw new ArgumentNullException(nameof(fallbackEncodingWebName)));
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));

        _utf8 = InitUtf8Encoding();
        _encodingCache = InitEncodingCache();
    }
    

    /// <summary>
    /// <see cref="Encoding.WebName"/>-Eigenschaft des <see cref="Encoding"/>-Objekts, das zum Laden von VCF-Dateien verwendet wird, 
    /// die nicht im UTF-8-Format vorliegen.
    /// </summary>
    public string FallbackEncodingWebName => _ansi.WebName;


    /// <summary>
    /// Versucht eine VCF-Datei zunächst als UTF-8 zu laden und lädt sie - falls dies fehlschlägt - mit dem durch <see cref="FallbackEncodingWebName"/>
    /// spezifizierten <see cref="Encoding"/>.
    /// </summary>
    /// 
    /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
    ///  
    /// <returns>Eine Sammlung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
    /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
    public virtual IList<VCard> LoadVcf(string fileName) => LoadVcf(fileName, out _);


    /// <summary>
    /// Versucht eine VCF-Datei zunächst als UTF-8 zu laden und lädt sie - falls dies fehlschlägt - mit dem durch <see cref="FallbackEncodingWebName"/>
    /// spezifizierten <see cref="Encoding"/>.
    /// </summary>
    /// 
    /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
    /// <param name="encodingWebName"><see cref="Encoding.WebName"/>-Eigenschaft des <see cref="Encoding"/>-Objekts,
    /// mit dem die VCF-Datei geladen wurde. Der Parameter wird uninitialisiert übergeben.</param>
    ///  
    /// <returns>Eine Sammlung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
    /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
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

        if (charSet is null)
        {
            encodingWebName = FallbackEncodingWebName;
            return VCard.LoadVcf(fileName, _ansi);
        }

        Encoding? enc = _encodingCache.GetEncoding(charSet);

        if (enc is null)
        {
            return vCards;
        }

        encodingWebName = enc.WebName;
        return VCard.LoadVcf(fileName, enc);

        //////////////////////////////////////////////////////////

        static string? GetCharsetFromVCards(IList<VCard> vCards)
        {
            if (vCards.All(x => x.Version != VCdVersion.V2_1))
            {
                return null;
            }
            IEnumerable<IEnumerable<KeyValuePair<VCdProp, object>>> keyValuePairs = vCards;

            return keyValuePairs
                .SelectMany(x => x)
                .Where(x => x.Value is IEnumerable<AddressProperty> or IEnumerable<NameProperty> or IEnumerable<TextProperty>)
                .Select(x => x.Value as IEnumerable<VCardProperty>)
                .SelectMany(x => x!)
                .FirstOrDefault(x => x.Parameters.CharSet != null)?.Parameters.CharSet;
        }
    }

    private static bool IsUtf8(Encoding encoding) => encoding.CodePage == AnsiFilterNew.UTF8_CODEPAGE;

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
