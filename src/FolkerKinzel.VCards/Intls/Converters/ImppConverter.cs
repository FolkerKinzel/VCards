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

    internal static Impp? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            TypeValue.PERSONAL => Impp.Personal,
            TypeValue.BUSINESS => Impp.Business,
            TypeValue.MOBILE => Impp.Mobile,
            _ => null
        };
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
