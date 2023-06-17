using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

#if NET40
using FolkerKinzel.VCards.Intls.Converters;
#else
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards;

/// <summary>
/// Hilfsklasse, die dem korrekten Laden von VCF-Dateien dient, die in ANSI-Codepages gespeichert wurden.
/// </summary>
/// <remarks>
/// <para>
/// Die Klasse ist ein Wrapper um die <see cref="VCard"/>.<see cref="VCard.LoadVcf(string, Encoding?)"/>-Methode, der zunächst prüft, ob die zu
/// ladende Datei korrektes UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut mit <see cref="FallbackEncoding"/> geladen. Anschließend 
/// wird der Inhalt der so geladenen VCF-Datei
/// nach <c>CHARSET</c>-Parametern durchsucht, die einen Anhaltspunkt über die verwendete Kodierung geben. Erweist sich bei dieser Untersuchung, dass diese
/// Kodierung von der Kodierung abweicht, mit der die VCF-Datei geladen wurde, wird sie mit der ermittelten Kodierung erneut geladen.
/// </para>
/// <para>
/// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException"/>s 
/// abgefangen und Dateien müssen ggf. dreimal geladen werden.
/// </para>
/// <para>
/// Die Verwendung der Klasse ist nur dann sinnvoll, wenn Sie es mit vCard 2.1 - Dateien zu tun haben, denn nur in diesem Standard existieren 
/// <c>CHARSET</c>-Parameter. Die Wirkungsweise der Klasse beruht auf der Annahme, dass innerhalb einer VCF-Datei jeweils nur eine ANSI-Kodierung Verwendung fand.
/// </para>
/// </remarks>
public sealed class MultiAnsiFilter
{
    private class EncodingCache
    {
        private readonly Dictionary<string, Encoding?> _cache = new(StringComparer.OrdinalIgnoreCase);

        public EncodingCache(string fallbackEncodingWebName)
        {
            _cache[fallbackEncodingWebName] = null;
            _cache["utf-8"] = null;
        }
        internal Encoding? GetEncoding(string charSet)
        {
            if(_cache.ContainsKey(charSet))
            {
                return _cache[charSet];
            }

            var enc = TextEncodingConverter.GetEncoding(charSet);

            if(EncodingCache.IsUtf8(enc))
            {
                _cache[charSet] = null;
                return null;
            }

            _cache[charSet] = enc;
            _cache[enc.WebName] = enc;
            return enc;
        }

        private static bool IsUtf8(Encoding encoding) => encoding.CodePage == AnsiFilter.UTF8_CODEPAGE;
    }
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly AnsiFilter _filter;
    private readonly EncodingCache _encodingCache;

    /// <summary>
    /// Initialisiert ein ein <see cref="MultiAnsiFilter"/>-Objekt mit einem <see cref="AnsiFilter"/>-Objekt.
    /// </summary>
    /// <param name="filter">Ein <see cref="AnsiFilter"/>-Objekt,
    /// das ein <see cref="FallbackEncoding"/> zur Verfügung stellt, das zum Laden einer VCF-Datei verwendet wird,
    /// wenn diese nicht als UTF-8
    /// geladen werden kann und sich in der Datei keine Hinweise zur Verwendung eines anderen <see cref="Encoding"/>-Objekts
    /// finden.</param>
    /// <exception cref="ArgumentNullException"><paramref name="filter"/> ist <c>null</c>.</exception>
    public MultiAnsiFilter(AnsiFilter filter)
    {
        if (filter is null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        _filter = filter;
        _encodingCache = new EncodingCache(FallbackEncoding.WebName);
    }

    /// <summary>
    /// Das <see cref="Encoding"/>-Objekt, das zum Laden von VCF-Dateien verwendet wird, die nicht
    /// als UTF-8 gespeichert sind, wenn in der VCF-Datei kein Hinweis gefunden wird, ein anderes
    /// zu verwenden.
    /// </summary>
    public Encoding FallbackEncoding => _filter.FallbackEncoding;

    /// <summary>
    /// Versucht eine VCF-Datei zunächst als UTF-8 zu laden und lädt sie - falls dies fehlschlägt - mit <see cref="FallbackEncoding"/>
    /// oder - falls die VCF-Datei entsprechende Hinweise enthält - mit einer entsprechenden Kodierung.
    /// </summary>
    /// 
    /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
    /// <param name="enc">Das <see cref="Encoding"/>-Objekt, das zum Laden der VCF-Datei verwendet wurde. Der Parameter 
    /// wird uninitialisiert übergeben.</param>
    ///  
    /// <returns>Eine Sammlung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
    /// 
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
    /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
    public IList<VCard> LoadVcf(string fileName, out Encoding enc)
    {
        var vCards = _filter.LoadVcf(fileName, out bool isAnsi);

        if (isAnsi)
        {
            IEnumerable<IEnumerable<KeyValuePair<VCdProp, object>>> keyValuePairs = vCards;

            var vCardProperty = keyValuePairs
                .SelectMany(x => x)
                .Where(x => x.Value is IEnumerable<AddressProperty> or IEnumerable<NameProperty> or IEnumerable<TextProperty>)
                .Select(x => x.Value as IEnumerable<VCardProperty>)
                .SelectMany(x => x!)
                .FirstOrDefault(x => x.Parameters.CharSet != null);

            if (vCardProperty is null)
            {
                enc = FallbackEncoding;
                return vCards;
            }

            var newEncoding = _encodingCache.GetEncoding(vCardProperty.Parameters.CharSet!);

            if (newEncoding is null)
            {
                enc = FallbackEncoding;
                return vCards;
            }

            enc = newEncoding;
            return VCard.LoadVcf(fileName, newEncoding);
        }

        enc = Encoding.UTF8;
        return vCards;
    }

}

