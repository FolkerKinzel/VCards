using System.Text;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.Strings;

namespace FolkerKinzel.VCards;

    /// <summary>Helper class used for correct loading of VCF files saved in an ANSI
    /// code page.</summary>
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
    /// Encoding?)" />-Methode, der zunächst prüft, ob die zu ladende Datei korrektes
    /// UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut mit einer
    /// als Fallback angegebenen Kodierung geladen.
    /// </para>
    /// <para>
    /// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance
    /// ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException" />s
    /// abgefangen und Dateien müssen ggf. zweimal geladen werden.
    /// </para>
    /// <para>
    /// Verwenden Sie die Klasse, wenn Sie mit relativer Sicherheit vorhersagen können,
    /// welche ANSI-Codepage ggf. verwendet worden ist oder wenn Sie nicht mit vCard
    /// 2.1 - Dateien arbeiten. Falls Sie es mit vCard 2.1-Dateien zu tun haben, die
    /// in verschiedenen ANSI-Codepages gespeichert worden sind, können Sie auf die
    /// Klasse <see cref="MultiAnsiFilter" /> zurückgreifen, die zusätzlich in den eingelesenen
    /// vCard-Dateien noch nach <c>CHARSET</c>-Parametern fahndet.
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
    private readonly UTF8Encoding _utf8 = new(false, true);
    private readonly Encoding _ansi;

    internal const int UTF8_CODEPAGE = 65001;


    /// <summary>Initializes an instance of the <see cref="AnsiFilter" /> class with
    /// the number of the code page to use for the reading of VCF files that are not
    /// saved as UTF-8.</summary>
    /// <param name="fallbackCodePage">The number of the code page. The default value
    /// is 1252 (for windows-1252).</param>
    /// <exception cref="ArgumentException">The number specified for <paramref name="fallbackCodePage"
    /// /> could not be mapped to an ANSI code page.</exception>
    public AnsiFilter(int fallbackCodePage = 1252)
    {
        _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));
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
        if (fallbackEncodingWebName is null)
        {
            throw new ArgumentNullException(nameof(fallbackEncodingWebName));
        }
        _ansi = TextEncodingConverter.GetEncoding(fallbackEncodingWebName);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));
    }


    /// <summary> <see cref="Encoding.WebName" /> property of the <see cref="Encoding"
    /// /> object, which is used to load VCF files that are not UTF-8.</summary>
    public string FallbackEncodingWebName => _ansi.WebName;


    /// <summary>Tries to load a VCF file as UTF-8 first and - if that fails - loads
    /// it with the <see cref="Encoding" /> specified with <see cref="FallbackEncodingWebName"
    /// />.</summary>
    /// <param name="fileName">Absolute or relative path to a VCF file.</param>
    /// <returns>A collection of parsed <see cref="VCard" /> objects, which represents
    /// the content of the VCF file.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="fileName" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public virtual IList<VCard> LoadVcf(string fileName) => LoadVcf(fileName, out _);


    /// <summary>Tries to load a VCF file as UTF-8 first and - if that fails - loads
    /// it with the <see cref="Encoding" /> specified with <see cref="FallbackEncodingWebName"
    /// />.</summary>
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
