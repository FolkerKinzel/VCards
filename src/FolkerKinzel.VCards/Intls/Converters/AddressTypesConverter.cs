using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressTypesConverter
{
    internal static class AdrTypeValue
    {
        internal const string DOM = "DOM";
        internal const string INTL = "INTL";
        internal const string POSTAL = "POSTAL";
        internal const string PARCEL = "PARCEL";
    }

    internal const AddressTypes DEFINED_ADDRESS_TYPES_VALUES = AddressTypes.Dom
                                                             | AddressTypes.Intl
                                                             | AddressTypes.Postal
                                                             | AddressTypes.Parcel;

    internal const int ADDRESS_TYPES_MIN_BIT = 0;
    internal const int ADDRESS_TYPES_MAX_BIT = 3;

    internal static AddressTypes? Parse(string? typeValue)
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

    internal static string ToVcfString(this AddressTypes value)
        => value switch
        {
            AddressTypes.Dom => AdrTypeValue.DOM,
            AddressTypes.Intl => AdrTypeValue.INTL,
            AddressTypes.Postal => AdrTypeValue.POSTAL,
            AddressTypes.Parcel => AdrTypeValue.PARCEL,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
