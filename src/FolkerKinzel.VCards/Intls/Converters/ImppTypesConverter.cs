using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ImppTypesConverter
{
    internal static class TypeValue
    {
        internal const string PERSONAL = "PERSONAL";
        internal const string BUSINESS = "BUSINESS";
        internal const string MOBILE = "MOBILE";
    }

    internal const ImppTypes DEFINED_IMPP_TYPES_VALUES = ImppTypes.Business | ImppTypes.Mobile | ImppTypes.Personal;

    internal const int IMPP_TYPES_MIN_BIT = 0;
    internal const int IMPP_TYPES_MAX_BIT = 2;


    internal static ImppTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            TypeValue.PERSONAL => ImppTypes.Personal,
            TypeValue.BUSINESS => ImppTypes.Business,
            TypeValue.MOBILE => ImppTypes.Mobile,
            _ => null
        };
    }

    internal static string ToVcfString(this ImppTypes value)
        => value switch
        {
            ImppTypes.Business => TypeValue.BUSINESS,
            ImppTypes.Mobile => TypeValue.MOBILE,
            ImppTypes.Personal => TypeValue.PERSONAL,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
}
