using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressToLabelConverter
{
    private const int BUILDER_CAPACITY = 256;
    private const int WORKER_CAPACITY = 64;
    private static readonly AddressOrder _defaultAddressOrder;
    private const int MAX_LINE_LENGTH = 30;

    static AddressToLabelConverter() => _defaultAddressOrder = AddressOrderConverter.ParseCultureInfo(CultureInfo.CurrentCulture);

    internal static string ConvertToLabel(Address address)
        => DoConvertToLabel(address, AddressOrderConverter.ParseAddress(address) ?? _defaultAddressOrder);

    internal static string ConvertToLabel(AddressProperty prop)
        => DoConvertToLabel(prop.Value, AddressOrderConverter.ParseAddressProperty(prop) ?? _defaultAddressOrder);

    private static string DoConvertToLabel(Address address, AddressOrder addressOrder)
    {
        var worker = new StringBuilder(WORKER_CAPACITY);

        return new StringBuilder(BUILDER_CAPACITY)
            .AppendStreet(address)
            .AppendExtendedAddress(address)
            .AppendLocality(address, worker, addressOrder)
            .AppendCountry(address, worker);
    }

    private static StringBuilder AppendStreet(this StringBuilder builder, Address address)
    {
        return address.PostOfficeBox.Count != 0 ? builder.AppendReadableProperty(address.PostOfficeBox, MAX_LINE_LENGTH)
            : address.Street.Count != 0 ? builder.AppendReadableProperty(address.Street, MAX_LINE_LENGTH)
            : builder.AppendReadableProperty(
                address.StreetName
                .Concat(address.StreetNumber)
                .Concat(address.Block)
                .Concat(address.Building)
                .Concat(address.Floor)
                .Concat(address.Apartment)
                .Concat(address.Room)
                .Concat(address.District)
                .Concat(address.SubDistrict)
                .Concat(address.Landmark)
                .Concat(address.Direction), MAX_LINE_LENGTH);
    }

    private static StringBuilder AppendExtendedAddress(this StringBuilder builder, Address address)
    {
        return address.ExtendedAddress.Count != 0
            ? builder.AppendNewLineIfNeeded().AppendReadableProperty(address.ExtendedAddress, MAX_LINE_LENGTH)
            : builder;
    }

    private static StringBuilder AppendLocality(this StringBuilder builder,
                                                Address address,
                                                StringBuilder worker,
                                                AddressOrder addressOrder)
    {
        switch (addressOrder)
        {
            case AddressOrder.Usa:
                worker.AppendReadableProperty(address.Locality)
                      .AppendReadableProperty(address.Region, MAX_LINE_LENGTH)
                      .AppendReadableProperty(address.PostalCode, MAX_LINE_LENGTH);
                break;
            case AddressOrder.Venezuela:
                worker.AppendReadableProperty(address.Locality)
                      .AppendReadableProperty(address.PostalCode, MAX_LINE_LENGTH)
                      .AppendReadableProperty(address.Region, MAX_LINE_LENGTH);
                break;
            default: // AddressOrder.Din
                worker.AppendReadableProperty(address.PostalCode)
                      .AppendReadableProperty(address.Locality)
                      .AppendReadableProperty(address.Region, MAX_LINE_LENGTH);
                break;
        }

        return builder.AppendNewLineIfNeeded().Append(worker);
    }

    private static string AppendCountry(this StringBuilder builder, Address address, StringBuilder worker)
    {
        worker.Clear().AppendReadableProperty(address.Country).ToUpperInvariant();
        return builder.AppendNewLineIfNeeded().Append(worker).ToString();
    }


    private static StringBuilder AppendNewLineIfNeeded(this StringBuilder builder)
    {
        if (builder.Length != 0) { builder.Append(Environment.NewLine); }
        return builder;
    }
}