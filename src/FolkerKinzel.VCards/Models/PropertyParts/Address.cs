using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models.PropertyParts;

#pragma warning disable CS0618 // Type or member is obsolete

/// <summary>Encapsulates information about a postal delivery address.</summary>
public sealed class Address
{
    private const int MAX_COUNT = 7;

    private const int POST_OFFICE_BOX = 0;
    private const int EXTENDED_ADDRESS = 1;
    private const int STREET = 2;
    private const int LOCALITY = 3;
    private const int REGION = 4;
    private const int POSTAL_CODE = 5;
    private const int COUNTRY = 6;

    /// <summary />
    /// <param name="street">The street address.</param>
    /// <param name="locality">The locality (e.g. city).</param>
    /// <param name="region">The region (e.g. state or province).</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country name (full name).</param>
    /// <param name="postOfficeBox">The post office box. (Don't use this parameter!)</param>
    /// <param name="extendedAddress">The extended address (e.g. apartment or suite
    /// number). (Don't use this parameter!)</param>
    internal Address(ReadOnlyCollection<string> street,
                     ReadOnlyCollection<string> locality,
                     ReadOnlyCollection<string> region,
                     ReadOnlyCollection<string> postalCode,
                     ReadOnlyCollection<string> country,
                     ReadOnlyCollection<string> postOfficeBox,
                     ReadOnlyCollection<string> extendedAddress)
    {
        PostOfficeBox = postOfficeBox;
        ExtendedAddress = extendedAddress;
        Street = street;
        Locality = locality;
        Region = region;
        PostalCode = postalCode;
        Country = country;
    }


    internal Address()
    {
        PostOfficeBox =
        ExtendedAddress =
        Street =
        Locality =
        Region =
        PostalCode =
        Country = ReadOnlyCollectionString.Empty;
    }


    internal Address(string vCardValue, VcfDeserializationInfo info, VCdVersion version)
    {
        Debug.Assert(vCardValue != null);

        StringBuilder builder = info.Builder;
        ValueSplitter semicolonSplitter = info.SemiColonSplitter;
        ValueSplitter commaSplitter = info.CommaSplitter;

        int index = 0;
        semicolonSplitter.ValueString = vCardValue;

        foreach (var s in semicolonSplitter)
        {
            switch (index++)
            {
                case POST_OFFICE_BOX:
                    {
                        if (s.Length == 0)
                        {
                            PostOfficeBox = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            PostOfficeBox = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case EXTENDED_ADDRESS:
                    {
                        if (s.Length == 0)
                        {
                            ExtendedAddress = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            ExtendedAddress = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case STREET:
                    {
                        if (s.Length == 0)
                        {
                            Street = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Street = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case LOCALITY:
                    {
                        if (s.Length == 0)
                        {
                            Locality = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Locality = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case REGION:
                    {
                        if (s.Length == 0)
                        {
                            Region = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Region = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case POSTAL_CODE:
                    {
                        if (s.Length == 0)
                        {
                            PostalCode = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            PostalCode = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case COUNTRY:
                    {
                        if (s.Length == 0)
                        {
                            Country = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Country = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                default:
                    break;
            }//switch
        }//foreach

        // If the VCF file is invalid, properties could be null:
        // (PostOfficeBox can never be null)
        Debug.Assert(PostOfficeBox != null);
        //PostOfficeBox ??= ReadOnlyCollectionString.Empty;
        ExtendedAddress ??= ReadOnlyCollectionString.Empty;
        Street ??= ReadOnlyCollectionString.Empty;
        Locality ??= ReadOnlyCollectionString.Empty;
        Region ??= ReadOnlyCollectionString.Empty;
        PostalCode ??= ReadOnlyCollectionString.Empty;
        Country ??= ReadOnlyCollectionString.Empty;
    }

    /// <summary>The post office box. (Don't use this property!)</summary>
    [Obsolete("Don't use this property.", false)]
    public ReadOnlyCollection<string> PostOfficeBox { get; }

    /// <summary>The extended address (e.g. apartment or suite number). (Don't use this
    /// property!)</summary>
    [Obsolete("Don't use this property.", false)]
    public ReadOnlyCollection<string> ExtendedAddress { get; }

    /// <summary>The street address.</summary>
    public ReadOnlyCollection<string> Street { get; }

    /// <summary>The locality (e.g. city).</summary>
    public ReadOnlyCollection<string> Locality { get; }

    /// <summary>The region (e.g. state or province).</summary>
    public ReadOnlyCollection<string> Region { get; }

    /// <summary>The postal code.</summary>
    public ReadOnlyCollection<string> PostalCode { get; }

    /// <summary>The country name (full name).</summary>
    public ReadOnlyCollection<string> Country { get; }


    /// <summary>Returns <c>true</c>, if the <see cref="Address" /> object does not
    /// contain any usable data.</summary>
    public bool IsEmpty => Locality.Count == 0 &&
                           Street.Count == 0 &&
                           Country.Count == 0 &&
                           Region.Count == 0 &&
                           PostalCode.Count == 0 &&
                           PostOfficeBox.Count == 0 &&
                           ExtendedAddress.Count == 0;

    /// <summary>Converts the data encapsulated in the instance into formatted text
    /// for a mailing label.</summary>
    /// <returns>The data encapsulated in the instance, converted to formatted text
    /// for a mailing label.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLabel() => this.ConvertToLabel();

    /// <inheritdoc/>
    public override string ToString()
    {
        var worker = new StringBuilder();
        var dic = new List<Tuple<string, string>>();

        for (int i = 0; i < MAX_COUNT; i++)
        {
            switch (i)
            {
                case POST_OFFICE_BOX:
                    {
                        string? s = BuildProperty(PostOfficeBox);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(PostOfficeBox), s));
                        break;
                    }
                case EXTENDED_ADDRESS:
                    {
                        string? s = BuildProperty(ExtendedAddress);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(ExtendedAddress), s));
                        break;
                    }
                case STREET:
                    {
                        string? s = BuildProperty(Street);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Street), s));
                        break;
                    }
                case LOCALITY:
                    {
                        string? s = BuildProperty(Locality);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Locality), s));
                        break;
                    }
                case REGION:
                    {
                        string? s = BuildProperty(Region);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Region), s));
                        break;
                    }
                case POSTAL_CODE:
                    {
                        string? s = BuildProperty(PostalCode);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(PostalCode), s));
                        break;
                    }
                case COUNTRY:
                    {
                        string? s = BuildProperty(Country);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Country), s));
                        break;
                    }
            }
        }

        if (dic.Count == 0)
        {
            return string.Empty;
        }

        int maxLength = dic.Select(x => x.Item1.Length).Max();
        maxLength += 2;

        _ = worker.Clear();

        for (int i = 0; i < dic.Count; i++)
        {
            Tuple<string, string>? tpl = dic[i];
            string s = tpl.Item1 + ": ";
            _ = worker.Append(s.PadRight(maxLength)).Append(tpl.Item2).Append(Environment.NewLine);
        }

        worker.Length -= Environment.NewLine.Length;
        return worker.ToString();

        ////////////////////////////////////////////

        string? BuildProperty(IList<string> strings)
        {
            _ = worker.Clear();

            if (strings.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < strings.Count - 1; i++)
            {
                _ = worker.Append(strings[i]).Append(", ");
            }

            _ = worker.Append(strings[strings.Count - 1]);

            return worker.ToString();
        }
    }


    internal void AppendVCardString(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

        AppendProperty(PostOfficeBox);
        _ = builder.Append(';');

        AppendProperty(ExtendedAddress);
        _ = builder.Append(';');

        AppendProperty(Street);
        _ = builder.Append(';');

        AppendProperty(Locality);
        _ = builder.Append(';');

        AppendProperty(Region);
        _ = builder.Append(';');

        AppendProperty(PostalCode);
        _ = builder.Append(';');

        AppendProperty(Country);


        void AppendProperty(IList<string> strings)
        {
            if (strings.Count == 0)
            {
                return;
            }

            for (int i = 0; i < strings.Count - 1; i++)
            {
                _ = worker.Clear().Append(strings[i]).Mask(serializer.Version);
                _ = builder.Append(worker).Append(joinChar);
            }

            _ = worker.Clear().Append(strings[strings.Count - 1]).Mask(serializer.Version);
            _ = builder.Append(worker);
        }
    }


    internal bool NeedsToBeQpEncoded()
        => Locality.Any(StringExtension.NeedsToBeQpEncoded) ||
           Street.Any(StringExtension.NeedsToBeQpEncoded) ||
           Country.Any(StringExtension.NeedsToBeQpEncoded) ||
           Region.Any(StringExtension.NeedsToBeQpEncoded) ||
           PostalCode.Any(StringExtension.NeedsToBeQpEncoded) ||
           PostOfficeBox.Any(StringExtension.NeedsToBeQpEncoded) ||
           ExtendedAddress.Any(StringExtension.NeedsToBeQpEncoded);
}

#pragma warning restore CS0618 // Type or member is deprecated

