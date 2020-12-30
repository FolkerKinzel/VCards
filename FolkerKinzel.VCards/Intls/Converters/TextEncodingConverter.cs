using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class TextEncodingConverter
    {
        internal static Encoding GetEncoding(string? s)
        {
            if (s is null) return Encoding.UTF8;
            try
            {
                return Encoding.GetEncoding(s);
            }
            catch
            {
                return Encoding.UTF8;
            }


        }
    }
}
