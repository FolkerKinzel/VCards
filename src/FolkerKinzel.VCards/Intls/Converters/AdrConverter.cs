using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AdrConverter
{
    internal static class AddressTypesValue
    {
        internal const string DOM = "DOM";
        internal const string INTL = "INTL";
        internal const string POSTAL = "POSTAL";
        internal const string PARCEL = "PARCEL";
    }

    internal const Addr DEFINED_ADDRESS_TYPES_VALUES = Addr.Dom
                                                             | Addr.Intl
                                                             | Addr.Postal
                                                             | Addr.Parcel;

    internal const int ADDRESS_TYPES_MIN_BIT = 0;
    internal const int ADDRESS_TYPES_MAX_BIT = 3;

    internal static Addr? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            AddressTypesValue.DOM => Addr.Dom,
            AddressTypesValue.INTL => Addr.Intl,
            AddressTypesValue.POSTAL => Addr.Postal,
            AddressTypesValue.PARCEL => Addr.Parcel,
            _ => null
        };
    }

    internal static string ToVcfString(this Addr value)
        => value switch
        {
            Addr.Dom => AddressTypesValue.DOM,
            Addr.Intl => AddressTypesValue.INTL,
            Addr.Postal => AddressTypesValue.POSTAL,
            Addr.Parcel => AddressTypesValue.PARCEL,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
