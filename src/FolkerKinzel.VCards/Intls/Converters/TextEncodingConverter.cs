#if NET40
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class TextEncodingConverter
{
    private const int UTF_8 = 65001;
    private const int CODEPAGE_MAX = 65535;

    internal static Encoding GetEncoding(string? s)
    {
        if (s is null)
        {
            return Encoding.UTF8;
        }

        try
        {
            return Encoding.GetEncoding(s);
        }
        catch
        {
            return Encoding.UTF8;
        }
    }

    /// <summary>
    /// Gibt für die Nummer einer Codepage ein entsprechendes <see cref="Encoding"/>-Objekt
    /// zurück, bei dem <see cref="Encoding.EncoderFallback"/> und <see cref="Encoding.DecoderFallback"/>
    /// auf ReplacementFallback eingestellt sind. <see cref="Encoding.UTF8"/> ist der Fallback-Wert.
    /// </summary>
    /// <param name="codepage">Die Nummer der Codepage.</param>
    /// <returns>Ein <see cref="Encoding"/>-Objekt, das der angegebenen Nummer der Codepage
    /// entspricht oder <see cref="Encoding.UTF8"/>, falls keine Entsprechung gefunden wurde.</returns>
    /// <remarks>
    /// .NET Standard und .NET 5.0 erkennen in der Standardeinstellung nur eine geringe Anzahl von Zeichensätzen.
    /// Die Methode überschreibt diese Standardeinstellung.
    /// </remarks>
    internal static Encoding GetEncoding(int codepage)
    {
        if (codepage is < 1 or > CODEPAGE_MAX)
        {
            return Encoding.UTF8;
        }

        try
        {
            return Encoding.GetEncoding(codepage);
        }
        catch
        {
            return Encoding.UTF8;
        }
    }


    /// <summary>
    /// Untersucht eine schreibgeschützte <see cref="byte"/>-Spanne daraufhin, ob
    /// sie mit einem Byte Order Mark (BOM) beginnt und gibt eine geeignete Codepage
    /// zurück. (Das Fallback-Value ist 65001 für UTF-8.)
    /// </summary>
    /// <param name="data">Die zu untersuchende Spanne.</param>
    /// <param name="bomLength">Die Länge des gefundenen BOM oder 0, wenn kein BOM
    /// gefunden wurde.</param>
    /// <returns>Eine geeignete Codepage für <paramref name="data"/> oder die Codepage
    /// für UTF-8 (65001), falls die Codepage nicht aus <paramref name="data"/> ermittelt
    /// werden konnte.</returns>
    /// <remarks>
    /// Die Methode erkennt die Byte Order Marks für die folgenden Zeichensätze:
    /// <list type="bullet">
    /// <item>UTF-8</item>
    /// <item>UTF-16LE</item>
    /// <item>UTF-16BE</item>
    /// <item>UTF-32LE</item>
    /// <item>UTF-32BE</item>
    /// <item>UTF-7</item>
    /// <item>GB18030</item>
    /// </list>
    /// <para>
    /// UTF-16LE, UTF-16BE, UTF-32LE und UTF-32BE können von der Methode u.U. auch dann aus den
    /// Daten erkannt werden, wenn kein Byte Order Mark vorliegt.
    /// </para>
    /// </remarks>
    internal static int GetCodePage(byte[] data, out int bomLength)
    {
        Debug.Assert(data != null);

        const int UTF16LE = 1200;
        const int UTF16BE = 1201;
        const int UTF32LE = 12000;
        const int UTF32BE = 12001;
        const int GB18030 = 54936;
        const int UTF7 = 65000;

        if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
        {
            bomLength = 3;
            return UTF_8;
        }

        if (data.Length >= 2)
        {
            if (data[0] == 0xFF && data[1] == 0xFE)
            {
                bomLength = 2;
                return data.Length >= 4 && data[2] == 0x00 && data[3] == 0x00 ? UTF32LE : UTF16LE;
            }

            if (data[0] == 0xFE && data[1] == 0xFF)
            {
                bomLength = 2;
                return UTF16BE;
            }

            if (data[0] != 0x00 && data[1] == 0x00)
            {
                bomLength = 0;
                return UTF16LE;
            }

            if (data[0] == 0x00 && data[1] != 0x00)
            {
                bomLength = 0;
                return UTF16BE;
            }
        }

        if (data.Length >= 4)
        {
            if (data[0] == 0x00 && data[1] == 0x00 && data[2] == 0xFE && data[3] == 0xFF)
            {
                bomLength = 4;
                return UTF32BE;
            }

            if (data[0] == 0x84 && data[1] == 0x31 && data[2] == 0x95 && data[3] == 0x33)
            {
                bomLength = 4;
                return GB18030;
            }

            if (data[0] == 0x2B && data[1] == 0x2F && data[2] == 0x76 && (data[3] == 0x38 || data[3] == 0x39 || data[3] == 0x2B || data[3] == 0x2F))
            {
                bomLength = 4;
                return UTF7;
            }

            if ((data[0] != 0x00 || data[1] != 0x00) && data[2] == 0x00 && data[3] == 0x00)
            {
                bomLength = 0;
                return UTF32LE;
            }

            if (data[0] == 0x00 && data[1] == 0x00 && (data[2] != 0x00 || data[3] != 0x00))
            {
                bomLength = 0;
                return UTF32BE;
            }
        }

        bomLength = 0;
        return UTF_8;
    }


}
#endif
