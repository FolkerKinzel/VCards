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
/// <threadsafety static="true" instance="false"/>
/// <remarks>
/// <para>
/// VCF-Dateien des Standards vCard 2.1 wurden von Outlook und Outlook Express früher in der ANSI-Kodierung des jeweiligen Windows-Systems gespeichert, 
/// ohne dass in jedem Fall die erweiterten Zeichen mit Quoted-Printable-Kodierung kodiert worden wären. Solche Dateien enthalten aber mitunter an irgendeiner Stelle
/// einen <c>CHARSET</c>-Parameter, der Rückschlüsse auf die verwendete Enkodierung zulässt. Die Wirkungsweise der Klasse beruht auf der Annahme, dass innerhalb einer 
/// VCF-Datei jeweils nur eine ANSI-Kodierung Verwendung fand.
/// </para>
/// 
/// <para>
/// Die Klasse ist ein Wrapper um die <see cref="VCard"/>.<see cref="VCard.LoadVcf(string, Encoding?)"/>-Methode, der zunächst
/// prüft, ob die zu
/// ladende Datei korrektes UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut mit <see cref="FallbackEncoding"/> geladen. Anschließend 
/// wird der Inhalt der so geladenen VCF-Datei
/// nach <c>CHARSET</c>-Parametern durchsucht, die einen Anhaltspunkt über die verwendete Kodierung geben. Erweist sich bei dieser Untersuchung, dass diese
/// Kodierung von der Kodierung abweicht, mit der die VCF-Datei geladen wurde, wird sie mit der ermittelten Kodierung erneut geladen.
/// </para>
/// 
/// <para>
/// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException"/>s 
/// abgefangen und Dateien müssen ggf. dreimal geladen werden.
/// </para>
/// <para>
/// Verwenden Sie die Klasse nur dann, wenn Sie es auch mit vCard 2.1 - Dateien zu tun haben, denn nur in diesem Standard existieren 
/// <c>CHARSET</c>-Parameter. Zwar können auch VCF-Dateien des Standards vCard 3.0
/// ANSI-kodiert sein, aber diese enthalten eben keinen 
/// <c>CHARSET</c>-Parameter. Deshalb ist zum Erkennen ANSI-kodierter vCard 3.0 - Dateien die Klasse <see cref="AnsiFilter"/> effizienter. 
/// </para>
/// </remarks>
/// <example>
/// <note type="note">Der leichteren Lesbarkeit wegen, wird in den Beispielen auf Ausnahmebehandlung verzichtet.</note>
/// <code language="cs" source="..\Examples\MultiAnsiFilterExample.cs"/>
/// </example>
public sealed class MultiAnsiFilter : AnsiFilter
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

    private readonly EncodingCache _encodingCache;

    ///// <summary>
    ///// Initialisiert ein ein <see cref="MultiAnsiFilter"/>-Objekt mit einem <see cref="AnsiFilter"/>-Objekt.
    ///// </summary>
    ///// <param name="filter">Ein <see cref="AnsiFilter"/>-Objekt,
    ///// das ein <see cref="FallbackEncoding"/> zur Verfügung stellt, das zum Laden einer VCF-Datei verwendet wird,
    ///// wenn diese nicht als UTF-8
    ///// geladen werden kann und sich in der Datei keine Hinweise zur Verwendung eines anderen <see cref="Encoding"/>-Objekts
    ///// finden.</param>
    ///// <exception cref="ArgumentNullException"><paramref name="filter"/> ist <c>null</c>.</exception>
    //public MultiAnsiFilter(AnsiFilter filter)
    //{
    //    if (filter is null)
    //    {
    //        throw new ArgumentNullException(nameof(filter));
    //    }

    //    _filter = filter;
    //    _encodingCache = new EncodingCache(FallbackEncodingWebName);
    //}

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="MultiAnsiFilter"/>-Klasse mit der Nummer
    /// der Codepage des <see cref="Encoding"/>-Objekts, das zum Lesen von VCF-Dateien verwendet werden soll, 
    /// die kein gültiges UTF-8 darstellen und die in ihren <c>CHARSET</c>-Parametern keinen Hinweis enthalten, 
    /// der die Verwendung eines anderen <see cref="Encoding"/>s nahelegt.
    /// </summary>
    /// <param name="fallbackCodePage">Die Codepagenummer
    /// der für das Lesen von ANSI-VCF-Dateien zu verwendenden Codepage.</param>
    /// <exception cref="ArgumentException">Die für <paramref name="fallbackCodePage"/> angegebene Nummer
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public MultiAnsiFilter(int fallbackCodePage = 1252) : base(fallbackCodePage) 
        => _encodingCache = new EncodingCache(FallbackEncodingWebName);


    /// <summary>
    /// Initialisiert eine Instanz der <see cref="MultiAnsiFilter"/>-Klasse mit dem <see cref="Encoding.WebName"/> des 
    /// <see cref="Encoding"/>-Objekts, das zum Lesen von VCF-Dateien verwendet werden soll, die kein gültiges UTF-8
    /// darstellen und die in ihren <c>CHARSET</c>-Parametern keinen Hinweis enthalten, der die Verwendung eines anderen
    /// <see cref="Encoding"/>s nahelegt.
    /// </summary>
    /// <param name="fallbackEncodingWebName"><see cref="Encoding.WebName"/> des <see cref="Encoding"/>-Objekts.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackEncodingWebName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Der für <paramref name="fallbackEncodingWebName"/> angegebene Bezeichner
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public MultiAnsiFilter(string fallbackEncodingWebName) : base(fallbackEncodingWebName) 
        => _encodingCache = new EncodingCache(FallbackEncodingWebName);




    ///// <summary>
    ///// Das <see cref="Encoding"/>-Objekt, das zum Laden von VCF-Dateien verwendet wird, die nicht
    ///// als UTF-8 gespeichert sind, wenn in der VCF-Datei kein <c>CHARSET</c>-Parameter gefunden wurde, der einen anderen
    ///// Zeichensatz verlangt.
    ///// </summary>
    //public override Encoding FallbackEncoding => base.FallbackEncoding;

    /// <summary>
    /// Versucht eine VCF-Datei zunächst als UTF-8 zu laden und lädt sie - falls dies fehlschlägt - mit <see cref="FallbackEncoding"/>
    /// oder - falls die VCF-Datei einen <c>CHARSET</c>-Parameter enthält - mit einer entsprechenden Kodierung.
    /// </summary>
    /// 
    /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
    /// <param name="encodingWebName"><see cref="Encoding.WebName"/>-Eigenschaft des <see cref="Encoding"/>-Objekts,
    /// mit dem die VCF-Datei geladen wurde. Der Parameter wird uninitialisiert übergeben.</param>
    ///  
    /// <returns>Eine Sammlung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
    /// 
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
    /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
    public override IList<VCard> LoadVcf(string fileName, out string encodingWebName)
    {
        var vCards = base.LoadVcf(fileName, out encodingWebName);

        if (StringComparer.Ordinal.Equals(encodingWebName, FallbackEncodingWebName))
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
                encodingWebName = FallbackEncodingWebName;
                return vCards;
            }

            var newEncoding = _encodingCache.GetEncoding(vCardProperty.Parameters.CharSet!);

            if (newEncoding is null)
            {
                encodingWebName = FallbackEncodingWebName;
                return vCards;
            }

            encodingWebName = newEncoding.WebName;
            return VCard.LoadVcf(fileName, newEncoding);
        }

        encodingWebName = Encoding.UTF8.WebName;
        return vCards;
    }

}

