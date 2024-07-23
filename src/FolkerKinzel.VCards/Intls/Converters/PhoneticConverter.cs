using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class PhoneticConverter
{
    internal static class TypeValue
    {
        internal const string ANIMATE = "ipa";
        internal const string COMMON = "jyut";
        internal const string FEMININE = "piny";
        internal const string INANIMATE = "script";
    }

    internal static Gram? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(TypeValue.FEMININE, comp) ? Gram.Feminine
             : typeValue.Equals(TypeValue.ANIMATE, comp) ? Gram.Animate
             : typeValue.Equals(TypeValue.INANIMATE, comp) ? Gram.Inanimate
             : typeValue.Equals(TypeValue.COMMON, comp) ? Gram.Common
             : null;
    }

    internal static string? ToVcfString(this Gram? value)
        => value switch
        {
            Gram.Feminine => TypeValue.FEMININE,
            Gram.Masculine => TypeValue.MASCULINE,
            Gram.Neuter => TypeValue.NEUTER,
            Gram.Animate => TypeValue.ANIMATE,
            Gram.Inanimate => TypeValue.INANIMATE,
            Gram.Common => TypeValue.COMMON,
            _ => null,
        };
}
