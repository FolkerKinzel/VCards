using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class TextEncodingConverter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Keine allgemeinen Ausnahmetypen abfangen", Justification = "<Ausstehend>")]
        internal static Encoding GetEncoding(string? s)
        {
            if (s is null) return Encoding.UTF8;
            try
            {
                var enc = Encoding.GetEncoding(s);
                return enc;
            }
            catch
            {
                return Encoding.UTF8;
            }


        }
    }
}
