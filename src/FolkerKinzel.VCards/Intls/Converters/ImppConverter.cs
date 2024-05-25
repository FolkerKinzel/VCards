using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ImppConverter
{
    internal static class TypeValue
    {
        internal const string PERSONAL = "PERSONAL";
        internal const string BUSINESS = "BUSINESS";
        internal const string MOBILE = "MOBILE";
    }

    internal const Impp DEFINED_IMPP_TYPES_VALUES =
        Impp.Business | Impp.Mobile | Impp.Personal;

    internal const int IMPP_TYPES_MIN_BIT = 0;
    internal const int IMPP_TYPES_MAX_BIT = 2;

    internal static Impp? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(TypeValue.PERSONAL, comp) ? Impp.Personal
             : typeValue.Equals(TypeValue.BUSINESS, comp) ? Impp.Business
             : typeValue.Equals(TypeValue.MOBILE, comp) ? Impp.Mobile
             : null;
    }

    internal static string ToVcfString(this Impp value)
        => value switch
        {
            Impp.Business => TypeValue.BUSINESS,
            Impp.Mobile => TypeValue.MOBILE,
            Impp.Personal => TypeValue.PERSONAL,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
}
