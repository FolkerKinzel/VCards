using System.Text;

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

    public AnsiFilter(int fallbackCodePage = 1252) => _ansi = TextEncodingConverter.GetEncoding(fallbackCodePage);

    public AnsiFilter(string fallbackEncodingName) => _ansi = TextEncodingConverter.GetEncoding(fallbackEncodingName);

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
