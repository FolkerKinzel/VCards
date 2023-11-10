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

    internal const Adr DEFINED_ADDRESS_TYPES_VALUES = Adr.Dom
                                                             | Adr.Intl
                                                             | Adr.Postal
                                                             | Adr.Parcel;

    internal const int ADDRESS_TYPES_MIN_BIT = 0;
    internal const int ADDRESS_TYPES_MAX_BIT = 3;

    internal static Adr? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            AddressTypesValue.DOM => Adr.Dom,
            AddressTypesValue.INTL => Adr.Intl,
            AddressTypesValue.POSTAL => Adr.Postal,
            AddressTypesValue.PARCEL => Adr.Parcel,
            _ => null
        };
    }

    internal static string ToVcfString(this Adr value)
        => value switch
        {
            Adr.Dom => AddressTypesValue.DOM,
            Adr.Intl => AddressTypesValue.INTL,
            Adr.Postal => AddressTypesValue.POSTAL,
            Adr.Parcel => AddressTypesValue.PARCEL,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
