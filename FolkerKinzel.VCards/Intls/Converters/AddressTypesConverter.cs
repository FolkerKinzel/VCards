using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class AddressTypesConverter
    {

        //Post
        internal static class AdrTypeValue
        {
            internal const string DOM = "DOM";
            internal const string INTL = "INTL";
            internal const string POSTAL = "POSTAL";
            internal const string PARCEL = "PARCEL";
        }

        internal static AddressTypes? Parse(string typeValue, AddressTypes? addressType)
        {
            Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

            return typeValue switch
            {
                AdrTypeValue.DOM => addressType.Set(AddressTypes.Dom),

                AdrTypeValue.INTL => addressType.Set(AddressTypes.Intl),

                AdrTypeValue.POSTAL => addressType.Set(AddressTypes.Postal),

                AdrTypeValue.PARCEL => addressType.Set(AddressTypes.Parcel),

                _ => addressType,
            };
        }

    }
}
