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

                    // Ascii is the standard in vCard 2.1 und has no symbol
                    // '7' => VCdEncoding.Ascii,
                    _ => (ValueEncoding?)null,
                };
            }
        }

        return null;
    }
}
