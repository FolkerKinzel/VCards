using System.Text;
using FolkerKinzel.VCards.Resources;

#if NET40
using FolkerKinzel.VCards.Intls.Converters;
#else
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards;

public class AnsiFilter
{
    private readonly UTF8Encoding _utf8 = new(false, true);
    private readonly Encoding _ansi;

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
