using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

#pragma warning disable CS0618 // Type or member is obsolete

internal static class AddressToLabelConverter
{
    private const int BUILDER_CAPACITY = 256;
    private const int WORKER_CAPACITY = 64;
    private static readonly AddressOrder _defaultAddressOrder;
    private const int MAX_LINE_LENGTH = 30;

    static AddressToLabelConverter() => _defaultAddressOrder = CultureInfo.CurrentCulture.ToAddressOrder();

    internal static string ConvertToLabel(Address address)
    {
        StringBuilder worker = new StringBuilder(WORKER_CAPACITY);

        return new StringBuilder(BUILDER_CAPACITY)
            .AppendStreet(address)
            .AppendExtendedAddress(address)
            .AppendLocality(address, worker)
            .AppendCountry(address, worker);
    }

    private static StringBuilder AppendStreet(this StringBuilder builder, Address address)
    {
        return address.PostOfficeBox.Count != 0
            ? builder.AppendReadableProperty(address.PostOfficeBox)
            : builder.AppendReadableProperty(address.Street, MAX_LINE_LENGTH);
    }


    private static StringBuilder AppendExtendedAddress(this StringBuilder builder, Address address)
    {
        return address.ExtendedAddress.Count != 0
            ? builder.AppendNewLineIfNeeded().AppendReadableProperty(address.ExtendedAddress, MAX_LINE_LENGTH)
            : builder;
    }

    private static StringBuilder AppendLocality(this StringBuilder builder, Address address, StringBuilder worker)
    {
        AddressOrder addressOrder = address.GetAddressOrder() ?? _defaultAddressOrder;

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
            default:
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
#pragma warning restore CS0618 // Type or member is obsolete