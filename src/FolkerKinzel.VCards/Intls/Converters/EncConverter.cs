using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class EncConverter
{
    internal static Enc? Parse(string? val)
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
                    'B' => Enc.Base64,
                    'Q' => Enc.QuotedPrintable,
                    '8' => Enc.Ansi,

                    // Ascii is the standard in vCard 2.1 und has no symbol
                    // '7' => VCdEncoding.Ascii,
                    _ => (Enc?)null,
                };
            }
        }

        return null;
    }
}
