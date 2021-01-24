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


        internal static ImppTypes? Parse(string? typeValue, ImppTypes? imppType)
        {
            Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

            return typeValue switch
            {
                TypeValue.Personal => imppType.Set(ImppTypes.Personal),
                TypeValue.Business => imppType.Set(ImppTypes.Business),
                TypeValue.Mobile => imppType.Set(ImppTypes.Mobile),
                _ => imppType
            };
        }
    }
}
