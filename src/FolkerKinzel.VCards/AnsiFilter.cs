using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

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
/// This class is a wrapper around the <see cref="Vcf.Load(string, Encoding?)" /> 
/// method that initially loads the file as UTF-8. If a decoding error occurs, the method 
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
public sealed class AnsiFilter
{
    private readonly DecoderValidationFallback _decoderFallback = new();
    private readonly Encoding _utf8;

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
        FallbackEncoding = TextEncodingConverter.GetEncoding(fallbackCodePage);
        ThrowArgumentExceptionIfUtf8(nameof(fallbackCodePage));

        _utf8 = InitUtf8Encoding();
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
        FallbackEncoding = TextEncodingConverter.GetEncoding(
            fallbackEncodingWebName ?? throw new ArgumentNullException(nameof(fallbackEncodingWebName)));
        ThrowArgumentExceptionIfUtf8(nameof(fallbackEncodingWebName));

        _utf8 = InitUtf8Encoding();
    }

    /// <summary>
    /// The <see cref="Encoding" /> object to use as fallback. </summary>
    public Encoding FallbackEncoding { get; }

    /// <summary>
    /// The <see cref="Encoding"/> used to read the VCF file.
    /// </summary>
    public Encoding UsedEncoding { get; private set; } = Encoding.UTF8;

    /// <summary>  Loads a VCF file and automatically selects the appropriate 
    /// <see cref="Encoding" />. </summary>
    /// <param name="filePath">Absolute or relative path to a VCF file.</param>
    /// 
    /// <returns>A collection of parsed <see cref="VCard" /> objects.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="filePath" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="filePath" /> is not a valid
    /// file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    internal IReadOnlyList<VCard> Load(string filePath)
    {
        Reset();

        IReadOnlyList<VCard> vCards = Vcf.Load(filePath, _utf8);

        if (!HasError)
        {
            return vCards;
        }

        string? charSet = GetCharsetFromVCards(vCards);

        Encoding? enc = charSet is null ? FallbackEncoding : TextEncodingConverter.GetEncoding(charSet);
        enc = IsUtf8(enc) ? FallbackEncoding : enc;
        UsedEncoding = enc;

        return Vcf.Load(filePath, enc);
    }

    [SuppressMessage("Style", "IDE0301:Simplify collection initialization",
        Justification = "Performance: The collection expression creates a new List<VCard> instead of Array.Empty<VCard>().")]
    internal IReadOnlyList<VCard> Deserialize(Func<Stream?> factory)
    {
        _ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        Reset();

        using Stream? stream = factory();

        if (stream is null)
        {
            return Array.Empty<VCard>();
        }

        long initialPosition = stream.CanSeek ? stream.Position : 0;

        IReadOnlyList<VCard> vCards = Vcf.Deserialize(stream, _utf8, leaveStreamOpen: true);

        if (!HasError)
        {
            return vCards;
        }

        string? charSet = GetCharsetFromVCards(vCards);

        Encoding? enc = charSet is null ? FallbackEncoding
                                        : TextEncodingConverter.GetEncoding(charSet);
        enc = IsUtf8(enc) ? FallbackEncoding : enc;
        UsedEncoding = enc;

        if (stream.CanSeek)
        {
            stream.Position = initialPosition;
            return Vcf.Deserialize(stream, enc, leaveStreamOpen: false);
        }
        else
        {
            using Stream? stream2 = factory();

            return stream2 is null ? vCards
                                   : Vcf.Deserialize(stream2, enc, leaveStreamOpen: false);
        }
    }

    [SuppressMessage("Style", "IDE0301:Simplify collection initialization",
        Justification = "Performance: The collection expression creates a new List<VCard> instead of Array.Empty<VCard>().")]
    internal async Task<IReadOnlyList<VCard>> DeserializeAsync(Func<CancellationToken, Task<Stream>> factory,
                                                               CancellationToken token)
    {
        _ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        Reset();

        using Stream? stream = await factory(token).ConfigureAwait(false);

        if (stream is null)
        {
            return Array.Empty<VCard>();
        }

        long initialPosition = stream.CanSeek ? stream.Position : 0;

        IReadOnlyList<VCard> vCards = Vcf.Deserialize(stream, _utf8, leaveStreamOpen: true);

        if (!HasError)
        {
            return vCards;
        }

        string? charSet = GetCharsetFromVCards(vCards);

        Encoding? enc = charSet is null ? FallbackEncoding
                                        : TextEncodingConverter.GetEncoding(charSet);
        enc = IsUtf8(enc) ? FallbackEncoding : enc;
        UsedEncoding = enc;

        if (stream.CanSeek)
        {
            stream.Position = initialPosition;
            return Vcf.Deserialize(stream, enc, leaveStreamOpen: false);
        }
        else
        {
            using Stream? stream2 = await factory(token).ConfigureAwait(false);

            return stream2 is null ? vCards
                                   : Vcf.Deserialize(stream2, enc, leaveStreamOpen: false);
        }
    }

    private static string? GetCharsetFromVCards(IReadOnlyList<VCard> vCards)
    {
        foreach (VCard vCard in vCards.Where(x => x.Version == VCdVersion.V2_1))
        {
            string? charSet = vCard
                .Entities
                .Where(x => x.Value is AddressProperty or NameProperty or TextProperty)
                .Select(x => x.Value.Parameters.CharSet)
                .FirstOrDefault(x => x is not null);

            if (charSet is not null) { return charSet; }
        }
        return null;
    }

    private static bool IsUtf8(Encoding encoding) => encoding.CodePage == AnsiFilter.UTF8_CODEPAGE;

    private void ThrowArgumentExceptionIfUtf8(string parameterName)
    {
        if (IsUtf8(FallbackEncoding))
        {
            throw new ArgumentException(Res.NoAnsiEncoding, parameterName);
        }
    }

    private bool HasError => _decoderFallback.HasError;

    private void Reset()
    {
        // Don't provide _utf.8  because _decoderFallback MUST remain
        // unaccessible from outside
        UsedEncoding = Encoding.UTF8;

        _decoderFallback.Reset();
    }

    private Encoding InitUtf8Encoding() =>
        Encoding.GetEncoding(UTF8_CODEPAGE, EncoderFallback.ReplacementFallback, _decoderFallback);
}
