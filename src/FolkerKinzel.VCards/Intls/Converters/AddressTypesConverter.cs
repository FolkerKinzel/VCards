using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressTypesConverter
{
    internal static class AddressTypesValue
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
            AddressTypesValue.DOM => AddressTypes.Dom,
            AddressTypesValue.INTL => AddressTypes.Intl,
            AddressTypesValue.POSTAL => AddressTypes.Postal,
            AddressTypesValue.PARCEL => AddressTypes.Parcel,
            _ => null
        };
    }

    internal static string ToVcfString(this AddressTypes value)
        => value switch
        {
            AddressTypes.Dom => AddressTypesValue.DOM,
            AddressTypes.Intl => AddressTypesValue.INTL,
            AddressTypes.Postal => AddressTypesValue.POSTAL,
            AddressTypes.Parcel => AddressTypesValue.PARCEL,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
