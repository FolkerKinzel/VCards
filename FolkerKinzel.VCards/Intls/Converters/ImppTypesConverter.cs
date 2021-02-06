using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class ImppTypesConverter
    {
        internal static class TypeValue
        {
            internal const string Personal = "PERSONAL";
            internal const string Business = "BUSINESS";
            internal const string Mobile = "MOBILE";
        }


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
    }
}
