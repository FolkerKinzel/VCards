using System.Text;
using FolkerKinzel.VCards.Resources;

#if NET40
using FolkerKinzel.VCards.Intls.Converters;
#else
using FolkerKinzel.Strings;
#endif

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
/// ladende Datei korrektes UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut in einer als <see cref="AnsiFilter.FallbackEncoding"/>
/// angegebenen Kodierung geladen.
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
/// <code language="cs" source="..\Examples\MultiAnsiFilterExample.cs"/>
/// </example>
public class AnsiFilter
{
    private readonly UTF8Encoding _utf8 = new(false, true);
    private readonly Encoding _ansi;

    internal const int UTF8_CODEPAGE = 65001;

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="AnsiFilter"/>-Klasse mit der Nummer
    /// der für das Lesen von ANSI-VCF-Dateien zu verwendenden Codepage.
    /// </summary>
    /// <param name="fallbackCodePage">Die Codepagenummer
    /// der für das Lesen von ANSI-VCF-Dateien zu verwendenden Codepage.</param>
    /// <exception cref="ArgumentException">Die für <paramref name="fallbackCodePage"/> angegebene Nummer
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public AnsiFilter(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));
    }

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="AnsiFilter"/>-Klasse mit dem <see cref="Encoding.WebName"/> des 
    /// <see cref="Encoding"/>-Objekts, das zum Lesen von
    /// ANSI-VCF-Dateien verwendet werden soll.
    /// </summary>
    /// <param name="fallbackEncodingWebName"><see cref="Encoding.WebName"/> des 
    /// <see cref="Encoding"/>-Objekts, das zum Lesen von
    /// ANSI-VCF-Dateien verwendet werden soll.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackEncodingWebName"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Der für <paramref name="fallbackEncodingWebName"/> angegebene Bezeichner
    /// konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public AnsiFilter(string fallbackEncodingWebName)
    {
        if (fallbackEncodingWebName is null)
        {
            throw new ArgumentNullException(nameof(fallbackEncodingWebName));
        }
        _ansi = TextEncodingConverter.GetEncoding(fallbackEncodingWebName);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));
    }



    /// <summary>
    /// <see cref="Encoding.WebName"/>-Eigenschaft des <see cref="Encoding"/>-Objekts, das zum Laden von VCF-Dateien verwendet wird, 
    /// die nicht als UTF-8 gespeichert sind.
    /// </summary>
    public string FallbackEncodingWebName => _ansi.WebName;


    /// <summary>
    /// Versucht eine VCF-Datei zunächst als UTF-8 zu laden und lädt sie - falls dies fehlschlägt - mit <see cref="FallbackEncoding"/>.
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
        encodingWebName = _utf8.WebName;
        try
        {
            return VCard.LoadVcf(fileName, _utf8);
        }
        catch (DecoderFallbackException)
        {
            encodingWebName = _ansi.WebName;
            return VCard.LoadVcf(fileName, _ansi);
        }
    }

    private void ThrowArgumentExceptionIfUtf8(string parameterName)
    {
        if (_ansi.CodePage == UTF8_CODEPAGE)
        {
            throw new ArgumentException(Res.NoAnsiEncoding, parameterName);
        }
    }
}
