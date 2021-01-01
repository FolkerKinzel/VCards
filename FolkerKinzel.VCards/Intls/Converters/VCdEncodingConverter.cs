using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class VCdEncodingConverter
    {
        internal static VCdEncoding? Parse(string? val)
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
                    return (char.ToUpperInvariant(c)) switch
                    {
                        'B' => VCdEncoding.Base64,
                        'Q' => VCdEncoding.QuotedPrintable,
                        '8' => VCdEncoding.Ansi,
                        '7' => VCdEncoding.Ascii,
                        _ => (VCdEncoding?)null,
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
}
