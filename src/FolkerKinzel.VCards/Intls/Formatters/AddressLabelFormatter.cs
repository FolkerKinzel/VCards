using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal static class AddressLabelFormatter
{
    private const int BUILDER_CAPACITY = 128;
    private static readonly AddressOrder _defaultAddressOrder;
    private const int MAX_LINE_LENGTH = 30;

    static AddressLabelFormatter() => _defaultAddressOrder = AddressOrderConverter.ParseCultureInfo(CultureInfo.CurrentCulture);

    internal static string ToLabel(AddressProperty prop)
    { 
        AddressOrder addressOrder = AddressOrderConverter.ParseAddressProperty(prop) ?? _defaultAddressOrder;
        Address address = prop.Value;
        Debug.Assert(!address.IsEmpty);

        return new StringBuilder(BUILDER_CAPACITY)
            .AppendStreet(address)
            .AppendLocality(address, addressOrder)
            .AppendCountry(address)
            .TrimEnd()
            .ToString();
    }

    private static StringBuilder AppendStreet(this StringBuilder builder, Address address)
    {
        IReadOnlyList<string> poBox = address.POBox;

        if (poBox.Count != 0)
        {
            return builder.AppendReadableProperty(poBox, MAX_LINE_LENGTH);
        }

        List<string> strings;
        IReadOnlyList<string> street = address.Street;

        if (street.Count != 0)
        {
            strings = new List<string>(street);
        }
        else
        {
            strings = new List<string>(address.StreetName);
            strings.AddRange(address.StreetNumber);
            strings.AddRange(address.Block);
            strings.AddRange(address.Landmark);
            strings.AddRange(address.Direction);
            strings.AddRange(address.SubDistrict);
            strings.AddRange(address.District);
        }

        IReadOnlyList<string> extAddress = address.Extended;

        if (extAddress.Count != 0)
        {
            strings.AddRange(extAddress);
        }
        else
        {
            strings.AddRange(address.Building);
            strings.AddRange(address.Floor);
            strings.AddRange(address.Apartment);
            strings.AddRange(address.Room);
        }

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

    private static StringBuilder AppendReadableProperty(this StringBuilder sb, IReadOnlyList<string> strings, int? maxLen)
    {
        Debug.Assert(sb is not null);
        Debug.Assert(strings is not null);
        Debug.Assert(strings.All(x => !string.IsNullOrEmpty(x)));

        int lineStartIndex = maxLen.HasValue ? sb.LastIndexOf(Environment.NewLine[Environment.NewLine.Length - 1]) + 1 : 0;

        // If strings is empty, the loop is not entered:
        for (int i = 0; i < strings.Count; i++)
        {
            if (maxLen.HasValue)
            {
                AppendEntryWithLength(sb, strings[i], maxLen.Value, ref lineStartIndex);
            }
            else
            {
                AppendEntry(sb, strings[i]);
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