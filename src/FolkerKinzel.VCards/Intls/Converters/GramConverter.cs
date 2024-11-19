using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GramConverter
{
    private static class TypeValue
    {
        internal const string ANIMATE = "animate";
        internal const string COMMON = "common";
        internal const string FEMININE = "feminine";
        internal const string INANIMATE = "inanimate";
        internal const string MASCULINE = "masculine";
        internal const string NEUTER = "neuter";
    }

    internal static bool TryParse(ReadOnlySpan<char> typeValue, out Gram gram)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        Gram? result = typeValue.Equals(TypeValue.FEMININE, comp) ? Gram.Feminine
             : typeValue.Equals(TypeValue.MASCULINE, comp) ? Gram.Masculine
             : typeValue.Equals(TypeValue.NEUTER, comp) ? Gram.Neuter
             : typeValue.Equals(TypeValue.ANIMATE, comp) ? Gram.Animate
             : typeValue.Equals(TypeValue.INANIMATE, comp) ? Gram.Inanimate
             : typeValue.Equals(TypeValue.COMMON, comp) ? Gram.Common
             : null;

        if (result.HasValue)
        {
            gram = result.Value;
            return true;
        }

        gram = default;
        return false;
    }

    internal static string? ToVcfString(this Gram value)
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
