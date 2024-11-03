using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal static class AddressLabelFormatter
{
    private const int BUILDER_CAPACITY = 128;
    private static readonly AddressOrder _defaultAddressOrder;
    private const int MAX_LINE_LENGTH = 30;

    static AddressLabelFormatter() => _defaultAddressOrder = AddressOrderConverter.ParseCultureInfo(CultureInfo.CurrentCulture);

    [Obsolete()]
    internal static string? ToLabel(Address address)
    {
        return address.IsEmpty
            ? null
            : DoConvertToLabel(address, AddressOrderConverter.ParseAddress(address) ?? _defaultAddressOrder);
    }

    internal static string ToLabel(AddressProperty prop)
    {
        Debug.Assert(!prop.Value.IsEmpty);
        return DoConvertToLabel(prop.Value, AddressOrderConverter.ParseAddressProperty(prop) ?? _defaultAddressOrder);
    }

    private static string DoConvertToLabel(Address address, AddressOrder addressOrder)
    {
        return new StringBuilder(BUILDER_CAPACITY)
            .AppendStreet(address)
            .AppendLocality(address, addressOrder)
            .AppendCountry(address)
            .TrimEnd()
            .ToString();
    }

    private static StringBuilder AppendStreet(this StringBuilder builder, Address address)
    {
        IReadOnlyList<string> poBox = address.PostOfficeBox;

        if (poBox.Count != 0)
        {
            return builder.AppendReadableProperty(poBox, MAX_LINE_LENGTH);
        }

        IReadOnlyList<string> street = address.Street;

        IEnumerable<string> strings =
            street.Count != 0 ? street
                              : address.StreetName
                                .Concat(address.StreetNumber)
                                .Concat(address.Block)
                                .Concat(address.Landmark)
                                .Concat(address.Direction)
                                .Concat(address.SubDistrict)
                                .Concat(address.District);

        IReadOnlyList<string> extAddress = address.ExtendedAddress;

        strings = extAddress.Count != 0 ? strings.Concat(extAddress)
                                        : strings.Concat(address.Building)
                                                 .Concat(address.Floor)
                                                 .Concat(address.Apartment)
                                                 .Concat(address.Room);

        return builder.AppendReadableProperty(strings, MAX_LINE_LENGTH);
    }

    private static StringBuilder AppendLocality(this StringBuilder builder,
                                                Address address,
                                                AddressOrder addressOrder)
    {
        builder.AppendNewLineIfNeeded();

        return addressOrder switch
        {
            AddressOrder.Usa =>
                builder.AppendReadableProperty(address.Locality, null)
                       .AppendReadableProperty(address.Region, MAX_LINE_LENGTH)
                       .AppendReadableProperty(address.PostalCode, MAX_LINE_LENGTH),

            AddressOrder.Venezuela =>
                builder.AppendReadableProperty(address.Locality, null)
                       .AppendReadableProperty(address.PostalCode, MAX_LINE_LENGTH)
                       .AppendReadableProperty(address.Region, MAX_LINE_LENGTH),
            _ => // AddressOrder.Din
                builder.AppendReadableProperty(address.PostalCode, null)
                       .AppendReadableProperty(address.Locality, null)
                       .AppendReadableProperty(address.Region, MAX_LINE_LENGTH)
        };
    }

    private static StringBuilder AppendCountry(this StringBuilder builder, Address address)
        => builder.AppendNewLineIfNeeded().AppendReadableProperty(address.Country, null);

    private static StringBuilder AppendNewLineIfNeeded(this StringBuilder builder)
    {
        if (builder.Length != 0) { builder.Append(Environment.NewLine); }
        return builder;
    }

    private static StringBuilder AppendReadableProperty(this StringBuilder sb, IEnumerable<string> strings, int? maxLen)
    {
        Debug.Assert(sb is not null);
        Debug.Assert(strings is not null);
        Debug.Assert(strings.All(x => !string.IsNullOrEmpty(x)));

        int lineStartIndex = maxLen.HasValue ? sb.LastIndexOf(Environment.NewLine[Environment.NewLine.Length - 1]) + 1 : 0;

        // If strings is empty, the loop is not entered:
        foreach (string s in strings)
        {
            if (maxLen.HasValue)
            {
                AppendEntryWithLength(sb, s, maxLen.Value, ref lineStartIndex);
            }
            else
            {
                AppendEntry(sb, s);
            }
        }

        return sb;

        static void AppendEntry(StringBuilder sb, string entry)
        {
            if (sb.Length != 0 && !sb[sb.Length - 1].IsNewLine())
            {
                _ = sb.Append(' ');
            }

            _ = sb.Append(entry);
        }

        static void AppendEntryWithLength(StringBuilder sb, string entry, int maxLen, ref int lineStartIndex)
        {
            if (sb.Length != 0 && lineStartIndex != sb.Length)
            {
                if (sb.Length - lineStartIndex + entry.Length >= maxLen)
                {
                    _ = sb.AppendLine();
                    lineStartIndex = sb.Length;
                }
                else
                {
                    _ = sb.Append(' ');
                }
            }

            _ = sb.Append(entry);
        }
    }
}