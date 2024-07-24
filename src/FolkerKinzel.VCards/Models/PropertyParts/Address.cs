using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models.PropertyParts;

#pragma warning disable CS0618 // Type or member is obsolete

/// <summary>Encapsulates information about a postal delivery address.</summary>
public sealed class Address
{
    private const int STANDARD_COUNT = (int)AdrProp.Country + 1;
    private const int MAX_COUNT = (int)AdrProp.Direction + 1;
    private readonly Dictionary<AdrProp, ReadOnlyCollection<string>> _dic = [];

    private ReadOnlyCollection<string> Get(AdrProp prop)
        => _dic.TryGetValue(prop, out ReadOnlyCollection<string>? coll)
            ? coll
            : ReadOnlyStringCollection.Empty;


    #region Remove this code with version 8.0.0

    private void Add(AdrProp prop, ReadOnlyCollection<string> coll)
    {
        if (coll.Count != 0)
        {
            _dic[prop] = coll;
        }
    }

    internal Address(ReadOnlyCollection<string> street,
                     ReadOnlyCollection<string> locality,
                     ReadOnlyCollection<string> region,
                     ReadOnlyCollection<string> postalCode,
                     ReadOnlyCollection<string> country,
                     ReadOnlyCollection<string> postOfficeBox,
                     ReadOnlyCollection<string> extendedAddress)
    {
        Add(AdrProp.PostOfficeBox, postOfficeBox);
        Add(AdrProp.ExtendedAddress, extendedAddress);
        Add(AdrProp.Street, street);
        Add(AdrProp.Locality, locality);
        Add(AdrProp.Region, region);
        Add(AdrProp.PostalCode, postalCode);
        Add(AdrProp.Country, country);
    }

    #endregion

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer initializes a new List.")]
    internal Address(AddressBuilder builder)
    {
        foreach (KeyValuePair<AdrProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                _dic[kvp.Key] = new ReadOnlyCollection<string>(kvp.Value.ToArray());
            }
        }
    }

    internal Address() { }
    

    internal Address(in ReadOnlyMemory<char> vCardValue, VCdVersion version)
    {
        int index = -1;

        foreach (ReadOnlyMemory<char> mem in PropertyValueSplitter.SplitIntoMemories(vCardValue, ';'))
        {
            index++;

            if (index >= MAX_COUNT)
            {
                break;
            }

            if (mem.IsEmpty)
            {
                continue;
            }

            ReadOnlySpan<char> span = mem.Span;
            ReadOnlyCollection<string> coll = span.Contains(',')
                ? ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(in mem, version))
                : ReadOnlyCollectionConverter.ToReadOnlyCollection(span.UnMaskValue(version));

            if (coll.Count == 0)
            {
                continue;
            }

            _dic[(AdrProp)index] = coll;
        }//foreach

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static string[] ToArray(in ReadOnlyMemory<char> mem, VCdVersion version)
            => PropertyValueSplitter.Split(mem,
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries,
                                    unMask: true,
                                    version).ToArray();
    }

    /// <summary>The post office box. (Don't use this property!)</summary>
    public ReadOnlyCollection<string> PostOfficeBox => Get(AdrProp.PostOfficeBox);

    /// <summary>The extended address (e.g., apartment or suite number). (Don't use this
    /// property!)</summary>
    public ReadOnlyCollection<string> ExtendedAddress => Get(AdrProp.ExtendedAddress);

    /// <summary>The street address.</summary>
    public ReadOnlyCollection<string> Street => Get(AdrProp.Street);

    /// <summary>The locality (e.g., city).</summary>
    public ReadOnlyCollection<string> Locality => Get(AdrProp.Locality);

    /// <summary>The region (e.g., state or province).</summary>
    public ReadOnlyCollection<string> Region => Get(AdrProp.Region);

    /// <summary>The postal code.</summary>
    public ReadOnlyCollection<string> PostalCode => Get(AdrProp.PostalCode);

    /// <summary>The country name (full name).</summary>
    public ReadOnlyCollection<string> Country => Get(AdrProp.Country);

    /// <summary> The room, suite number, or identifier. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Room => Get(AdrProp.Room);

    /// <summary> The extension designation such as the apartment number, unit, or box number. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Apartment => Get(AdrProp.Apartment);

    /// <summary> The floor or level the address is located on. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Floor => Get(AdrProp.Floor);

    /// <summary> The street number, e.g., "123". This value is not restricted to numeric values and can include
    /// any value such as number ranges ("112-10"), grid style ("39.2 RD"), alphanumerics ("N6W23001"), or 
    /// fractionals ("123 1/2"). (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> StreetNumber => Get(AdrProp.StreetNumber);

    /// <summary> The street name. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> StreetName => Get(AdrProp.StreetName);

    /// <summary> The building, tower, or condominium the address is located in. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Building => Get(AdrProp.Building);

    /// <summary> The block name or number. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Block => Get(AdrProp.Block);

    /// <summary> The subdistrict, ward, or other subunit of a district. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> SubDistrict => Get(AdrProp.SubDistrict);

    /// <summary> The district name. (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> District => Get(AdrProp.District);

    /// <summary> The publicly known prominent feature that can substitute the street name and number,
    /// e.g., "White House" or "Taj Mahal". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> LandMark => Get(AdrProp.LandMark);

    /// <summary> The cardinal direction or quadrant, e.g., "north". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Direction => Get(AdrProp.Direction);


    /// <summary>Returns <c>true</c>, if the <see cref="Address" /> object does not
    /// contain any usable data.</summary>
    public bool IsEmpty => _dic.Count == 0;

    /// <summary>Converts the data encapsulated in the instance into formatted text
    /// for a mailing label.</summary>
    /// <returns>The data encapsulated in the instance, converted to formatted text
    /// for a mailing label.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLabel() => AddressToLabelConverter.ConvertToLabel(this);

    /// <inheritdoc/>
    public override string ToString()
    {
        if (_dic.Count == 0)
        {
            return string.Empty;
        }

        var worker = new StringBuilder();
        var dic = new List<Tuple<string, string>>();

        foreach (KeyValuePair<AdrProp, ReadOnlyCollection<string>> pair in _dic.OrderBy(x => x.Key))
        {
            string s = BuildProperty(pair.Value);
            dic.Add(new Tuple<string, string>(pair.Key.ToString(), s));
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

        string BuildProperty(IList<string> strings)
        {
            _ = worker.Clear();

            Debug.Assert(strings.Count >= 1);

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
        int startIdx = builder.Length;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

        for (int i = 0; i < STANDARD_COUNT; i++)
        {
            AppendProperty(Get((AdrProp)i), joinChar, serializer);
        }

        if (serializer.Version >= VCdVersion.V4_0
            && serializer.Options.HasFlag(Opts.WriteRfc9554Extensions)
            && _dic.Any(x => x.Key >= AdrProp.Room))
        {
            for (int i = STANDARD_COUNT; i < MAX_COUNT; i++)
            {
                AppendProperty(Get((AdrProp)i), joinChar, serializer);
            }
        }

        --builder.Length;

        if (serializer.ParameterSerializer.ParaSection.Encoding == Enc.QuotedPrintable)
        {
            int count = builder.Length - startIdx;
            using ArrayPoolHelper.SharedArray<char> tmp = ArrayPoolHelper.Rent<char>(count);
            builder.CopyTo(startIdx, tmp.Array, 0, count);
            builder.Length = startIdx;
            builder.AppendQuotedPrintable(tmp.Array.AsSpan(0, count), startIdx);
        }

        //////////////////////////////////////////////////////////

        static void AppendProperty(IList<string> strings, char joinChar, VcfSerializer serializer)
        {
            StringBuilder builder = serializer.Builder;

            if (strings.Count == 0)
            {
                builder.Append(';');
                return;
            }

            for (int i = 0; i < strings.Count; i++)
            {
                _ = builder.AppendValueMasked(strings[i], serializer.Version).Append(joinChar);
            }

            --builder.Length;
            builder.Append(';');
        }
    }


    internal bool NeedsToBeQpEncoded()
    {
        foreach (KeyValuePair<AdrProp, ReadOnlyCollection<string>> kvp in _dic)
        {
            if (kvp.Key <= AdrProp.Country
                && kvp.Value.Any(StringExtension.NeedsToBeQpEncoded))
            {
                return true;
            }
        }

        return false;
    }
}

#pragma warning restore CS0618 // Type or member is deprecated

