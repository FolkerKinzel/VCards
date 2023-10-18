using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ValueEncodingConverter
{
    internal static ValueEncoding? Parse(string? val)
    {
        if (val is null)
        {
            return null;
        }

        for (int i = 0; i < val.Length; i++) // ersetzt Trim
        {
            char c = val[i];

            if (char.IsLetterOrDigit(c))
            {
                return char.ToUpperInvariant(c) switch
                {
                    'B' => ValueEncoding.Base64,
                    'Q' => ValueEncoding.QuotedPrintable,
                    '8' => ValueEncoding.Ansi,

                    // Ascii ist der Standard in vCard 2.1 und hat kein Symbol
                    // '7' => VCdEncoding.Ascii,
                    _ => (ValueEncoding?)null,
                };
            }
        }

        return null;
    }

    //internal static string ToPropertyParameterString(this VCdEncoding? vCardEncoding, VCdVersion version)
    //{
    //    if (!vCardEncoding.HasValue || version >= VCdVersion.V4_0) return string.Empty;

    //    if(vCardEncoding == VCdEncoding.Base64)
    //    {
    //        return (version == VCdVersion.V3_0) ? "b" : "BASE64";
    //    }

    //    return (vCardEncoding == VCdEncoding.QuotedPrintable && version == VCdVersion.V2_1)
    //        ? "QUOTED-PRINTABLE" : string.Empty;
    //}
}
