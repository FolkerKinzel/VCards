using System.Text;
using FolkerKinzel.VCards;
using FolkerKinzel.Strings;

namespace FolkerKinzel.VCards.Utilities
{
    public class AnsiFilter
    {
        private readonly UTF8Encoding _utf8 = new(false, true);
        private Encoding? _latin1;

        public IList<VCard> LoadVcf(string fileName)
        {
            try
            {
                using var reader = new StreamReader(fileName, _utf8);
                reader.ReadToEnd();
            }
            catch (DecoderFallbackException)
            {
                _latin1 ??= TextEncodingConverter.GetEncoding(1252);
                return VCard.LoadVcf(fileName, _latin1);
            }

            return VCard.LoadVcf(fileName);
        }
    }
}
