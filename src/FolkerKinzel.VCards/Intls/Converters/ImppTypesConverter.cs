using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ImppTypesConverter
{
    internal static class TypeValue
    {
        internal const string Personal = "PERSONAL";
        internal const string Business = "BUSINESS";
        internal const string Mobile = "MOBILE";
    }

    internal const ImppTypes DEFINED_IMPP_TYPES_VALUES = ImppTypes.Business | ImppTypes.Mobile | ImppTypes.Personal;

    internal const int ImppTypesMinBit = 0;
    internal const int ImppTypesMaxBit = 2;


    internal static ImppTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            TypeValue.Personal => ImppTypes.Personal,
            TypeValue.Business => ImppTypes.Business,
            TypeValue.Mobile => ImppTypes.Mobile,
            _ => null
        };
    }

    internal static string ToVcfString(this ImppTypes value)
        => value switch
        {
            ImppTypes.Business => TypeValue.Business,
            ImppTypes.Mobile => TypeValue.Mobile,
            ImppTypes.Personal => TypeValue.Personal,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
}
