using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GramConverter
{
    internal static class TypeValue
    {
        internal const string ANIMATE = "animate";
        internal const string COMMON = "common";
        internal const string FEMININE = "feminine";
        internal const string INANIMATE = "inanimate";
        internal const string MASCULINE = "masculine";
        internal const string NEUTER = "neuter";
    }

    internal static Gram? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(TypeValue.FEMININE, comp) ? Gram.Feminine
             : typeValue.Equals(TypeValue.MASCULINE, comp) ? Gram.Masculine
             : typeValue.Equals(TypeValue.NEUTER, comp) ? Gram.Neuter
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
