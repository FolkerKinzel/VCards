using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

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

    internal static AddressTypes? Parse(string typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            AdrTypeValue.DOM => AddressTypes.Dom,

            AdrTypeValue.INTL => AddressTypes.Intl,

            AdrTypeValue.POSTAL => AddressTypes.Postal,

            AdrTypeValue.PARCEL => AddressTypes.Parcel,

            _ => null
        };
    }

}
