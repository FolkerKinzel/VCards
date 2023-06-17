using System.Text;
using FolkerKinzel.VCards.Resources;

#if NET40
using FolkerKinzel.VCards.Intls.Converters;
#else
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards;

/// <summary>
/// Hilfsklasse, die dem Ausfiltern und korrekten Einlesen vCard-Dateien dient, die in einer ANSI-Codepage gespeichert wurden.
/// </summary>
/// <remarks>
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
/// Verwenden Sie die Klasse direkt, wenn Sie mit relativer Sicherheit vorhersagen können, welche ANSI-Codepage ggf. verwendet worden ist. Falls Sie es mit
/// vCard 2.1-Dateien zu tun haben, die in verschiedenen ANSI-Codepages gespeichert worden sind, können Sie stattdessen auf die Klasse <see cref="MultiAnsiFilter"/>
/// zurückgreifen, die zusätzlich in den eingelesenen vCard-Dateien noch nach <c>CHARSET</c>-Parametern fahndet.
/// </para>
/// </remarks>
public sealed class AnsiFilter
{
    private readonly UTF8Encoding _utf8 = new(false, true);
    private readonly Encoding _ansi;

    /// <summary>
    /// Initialisiert eine Instanz der <see cref="AnsiFilter"/>-Klasse mit der Nummer
    /// der für das Lesen von ANSI-VCF-Dateien zu verwendenden Codepage.
    /// </summary>
    /// <param name="fallbackCodePage">Die Codepagenummer
    /// der für das Lesen von ANSI-VCF-Dateien zu verwendenden Codepage.</param>
    /// <exception cref="ArgumentException">Die für <paramref name="fallbackCodePage"/> angegebene Nummer
    /// war 65001 (utf-8) oder die Nummer konnte keiner ANSI-Codepage zugeordnet werden.</exception>
    public AnsiFilter(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));
    }

    public AnsiFilter(string fallbackEncodingWebName)
    {
        if (fallbackEncodingWebName is null)
        {
            throw new ArgumentNullException(nameof(fallbackEncodingWebName));
        }
        _ansi = TextEncodingConverter.GetEncoding(fallbackEncodingWebName);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));
    }

    private void ThrowArgumentExceptionIfUtf8(string parameterName)
    {
        if(_ansi.CodePage == 65001)
        {
            throw new ArgumentException(Res.NoAnsiEncoding, parameterName);
        }
    }

    public Encoding FallbackEncoding => _ansi;

    public bool LoadVcf(string fileName, out IList<VCard> vCards)
    {
        try
        {
            vCards = VCard.LoadVcf(fileName, _utf8);
            return false;
        }
        catch (DecoderFallbackException)
        {
            vCards = VCard.LoadVcf(fileName, _ansi);
            return true;
        }
    }
}
