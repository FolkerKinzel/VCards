using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Globalization;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Intls.Converters;

#pragma warning disable CS0618 // Typ oder Element ist veraltet

internal static class AddressToLabelConverter
{
    private const int BUILDER_CAPACITY = 256;
    private const int WORKER_CAPACITY = 64;
    private static readonly AddressOrder _defaultAddressOrder;
    private const int MAX_LINE_LENGTH = 20;

    static AddressToLabelConverter() => _defaultAddressOrder = CultureInfo.CurrentCulture.ToAddressOrder();

    internal static string ConvertToLabel(this Address address)
    {
        StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
        StringBuilder worker = new StringBuilder(WORKER_CAPACITY);
        AppendStreet(address, builder);
        AppendLocality(address, builder, worker);
        return AppendCountry(address, builder, worker);
    }

    private static void AppendStreet(Address address, StringBuilder builder)
    {
        if (address.PostOfficeBox.Count != 0)
        {
            builder.AppendReadableProperty(address.PostOfficeBox);
        }
        else
        {
            builder.AppendReadableProperty(address.Street).AppendReadableProperty(address.ExtendedAddress, MAX_LINE_LENGTH);
        }
    }

    private static void AppendLocality(Address address, StringBuilder builder, StringBuilder worker)
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

        builder.AppendNewLineIfNeeded().Append(worker);
    }

    private static string AppendCountry(Address address, StringBuilder builder, StringBuilder worker)
    {
        worker.Clear().AppendReadableProperty(address.Country).ToUpperInvariant();
        return builder.AppendNewLineIfNeeded().Append(worker).ToString();
    }


    private static StringBuilder AppendNewLineIfNeeded(this StringBuilder builder)
    {
        if(builder.Length != 0) { builder.Append(Environment.NewLine); }    
        return builder;
    }
}
#pragma warning restore CS0618 // Typ oder Element ist veraltet
