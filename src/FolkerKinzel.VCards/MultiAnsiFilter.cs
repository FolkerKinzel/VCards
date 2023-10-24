using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.Strings;

namespace FolkerKinzel.VCards;

    /// <summary>Helper class used for the correct loading of vCard 2.1 VCF files that
    /// might have been saved in different code pages.</summary>
    /// <threadsafety static="true" instance="false" />
    /// <remarks>
    /// <para>
    /// VCF-Dateien des Standards vCard 2.1 wurden von Outlook und Outlook Express früher
    /// in der ANSI-Kodierung des jeweiligen Windows-Systems gespeichert, ohne dass
    /// in jedem Fall die erweiterten Zeichen mit Quoted-Printable-Kodierung kodiert
    /// worden wären. Solche Dateien enthalten aber mitunter an irgendeiner Stelle einen
    /// <c>CHARSET</c>-Parameter, der Rückschlüsse auf die verwendete Enkodierung zulässt.
    /// Die Wirkungsweise der Klasse beruht auf der Annahme, dass innerhalb einer VCF-Datei
    /// jeweils nur eine ANSI-Kodierung Verwendung fand.
    /// </para>
    /// <para>
    /// Die Klasse ist ein Wrapper um die <see cref="VCard" />.<see cref="VCard.LoadVcf(string,
    /// Encoding?)" />-Methode, der zunächst prüft, ob die zu ladende Datei korrektes
    /// UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut mit dem
    /// durch <see cref="AnsiFilter.FallbackEncodingWebName" /> spezifizierten <see
    /// cref="Encoding" /> geladen. Anschließend wird der Inhalt der so geladenen VCF-Datei
    /// nach <c>CHARSET</c>-Parametern durchsucht, die einen Hinweis auf die tatsächlich
    /// verwendete Kodierung geben. Erweist sich bei dieser Untersuchung, dass diese
    /// Kodierung von der Kodierung abweicht, mit der die VCF-Datei geladen wurde, wird
    /// die Datei mit der ermittelten Kodierung erneut geladen.
    /// </para>
    /// <para>
    /// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance
    /// ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException" />s
    /// abgefangen und Dateien müssen ggf. dreimal geladen werden.
    /// </para>
    /// <para>
    /// Verwenden Sie die Klasse nur dann, wenn Sie es auch mit vCard 2.1 - Dateien
    /// zu tun haben, denn nur in diesem Standard existieren <c>CHARSET</c>-Parameter.
    /// Zwar können auch VCF-Dateien des Standards vCard 3.0 ANSI-kodiert sein, aber
    /// diese enthalten eben keinen <c>CHARSET</c>-Parameter. Deshalb ist zum Erkennen
    /// ANSI-kodierter vCard 3.0 - Dateien die Verwendung der Klasse <see cref="AnsiFilter"
    /// /> effizienter.
    /// </para>
    /// </remarks>
    /// <example>
    /// <note type="note">
    /// To make the code more readable, exception handling has been omitted from the
    /// examples.
    /// </note>
    /// <code language="cs" source="..\Examples\MultiAnsiFilterExample.cs" />
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

    /// <summary> Initialisiert eine Instanz der <see cref="MultiAnsiFilter" />-Klasse
    /// mit der Nummer der Codepage des <see cref="Encoding" />-Objekts, das zum Lesen
    /// von VCF-Dateien verwendet werden soll, die nicht im UTF-8-Format vorliegen und
    /// die in ihren <c>CHARSET</c>-Parametern keinen Hinweis enthalten, der die Verwendung
    /// eines anderen <see cref="Encoding" />s nahelegt. </summary>
    /// <param name="fallbackCodePage">The number of the code page. The default value
    /// is 1252 (for windows-1252).</param>
    /// <exception cref="ArgumentException">The number specified for <paramref name="fallbackCodePage"
    /// /> could not be mapped to an ANSI code page.</exception>
    public MultiAnsiFilter(int fallbackCodePage = 1252) : base(fallbackCodePage) 
        => _encodingCache = new EncodingCache(FallbackEncodingWebName);


    /// <summary> Initialisiert eine Instanz der <see cref="MultiAnsiFilter" />-Klasse
    /// mit dem <see cref="Encoding.WebName" /> des <see cref="Encoding" />-Objekts,
    /// das zum Lesen von VCF-Dateien verwendet werden soll, die nicht im UTF-8-Format
    /// vorliegen und die in ihren <c>CHARSET</c>-Parametern keinen Hinweis enthalten,
    /// der die Verwendung eines anderen <see cref="Encoding" />s nahelegt. </summary>
    /// <param name="fallbackEncodingWebName"> <see cref="Encoding.WebName" /> property
    /// of the <see cref="Encoding" /> object.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="fallbackEncodingWebName"
    /// /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The identifier specified for <paramref name="fallbackEncodingWebName"
    /// /> could not be mapped to an ANSI code page.</exception>
    public MultiAnsiFilter(string fallbackEncodingWebName) : base(fallbackEncodingWebName) 
        => _encodingCache = new EncodingCache(FallbackEncodingWebName);

    /// <summary>Tries to load a VCF file as UTF-8 first and - if that fails - loads
    /// it with the <see cref="Encoding" /> specified with <see cref="FallbackEncodingWebName"
    /// /> or - if the VCF file contains a <c>CHARSET</c> parameter - with a corresponding
    /// <see cref="Encoding" />.</summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public override IList<VCard> LoadVcf(string fileName) => LoadVcf(fileName, out _);


    /// <summary>Tries to load a VCF file as UTF-8 first and - if that fails - loads
    /// it with the <see cref="Encoding" /> specified with <see cref="FallbackEncodingWebName"
    /// /> or - if the VCF file contains a <c>CHARSET</c> parameter - with a corresponding
    /// <see cref="Encoding" />.</summary>
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

