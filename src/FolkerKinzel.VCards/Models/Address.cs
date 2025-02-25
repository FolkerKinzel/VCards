using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information about a postal delivery address.</summary>
/// <remarks>
/// <note type="tip">Use <see cref="AddressBuilder"/> to create an instance.</note>
/// </remarks>
/// <seealso cref="AddressBuilder"/>
/// <seealso cref="AddressProperty"/>
/// <seealso cref="VCard.Addresses"/>
public sealed class Address : IReadOnlyList<IReadOnlyList<string>>
{
    private const int STANDARD_COUNT = (int)AdrProp.Country + 1;
    private const int MAX_COUNT = (int)AdrProp.Direction + 1;
    private readonly Dictionary<AdrProp, string[]> _dic = [];

    private string[] Get(AdrProp prop)
        => _dic.TryGetValue(prop, out string[]? coll) ? coll : [];

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer initializes a new List.")]
    internal Address(AddressBuilder builder)
    {
        Dictionary<AdrProp, string[]>? newVals = null;
        bool streetHasData = false;
        bool extendedHasData = false;
        bool newStreetHasData = false;
        bool newExtendedHasData = false;

        foreach (KeyValuePair<AdrProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                // Copy kvp.Value because it comes from AddressBuilder and can be reused!
                string[] val = kvp.Value.ToArray();
                _dic[kvp.Key] = val;

                switch (kvp.Key)
                {
                    case AdrProp.Street:
                        streetHasData = true;
                        break;
                    case AdrProp.Extended:
                        extendedHasData = true;
                        break;
                    case AdrProp.Room:
                    case AdrProp.Apartment:
                    case AdrProp.Floor:
                    case AdrProp.Building:
                        newVals ??= [];
                        newVals[kvp.Key] = val;
                        newExtendedHasData = true;
                        break;
                    case AdrProp.Block:
                    case AdrProp.StreetNumber:
                    case AdrProp.StreetName:
                    case AdrProp.SubDistrict:
                    case AdrProp.District:
                    case AdrProp.Landmark:
                    case AdrProp.Direction:
                        newVals ??= [];
                        newVals[kvp.Key] = val;
                        newStreetHasData = true;
                        break;
                    default:
                        break;
                }
            }
        }

        if (!streetHasData && newStreetHasData)
        {
            Debug.Assert(newVals is not null);

            ReadOnlySpan<AdrProp> keys = [AdrProp.StreetName,
                                          AdrProp.StreetNumber,
                                          AdrProp.Block,
                                          AdrProp.Landmark,
                                          AdrProp.Direction,
                                          AdrProp.SubDistrict,
                                          AdrProp.District];

            AddForCompatibility(keys, newVals, AdrProp.Street);
        }

        if (!extendedHasData && newExtendedHasData)
        {
            Debug.Assert(newVals is not null);
            ReadOnlySpan<AdrProp> keys = [AdrProp.Building,
                                          AdrProp.Floor,
                                          AdrProp.Apartment,
                                          AdrProp.Room];

            AddForCompatibility(keys, newVals, AdrProp.Extended);
        }

        Street = newStreetHasData ? [] : Get(AdrProp.Street);
        Extended = newExtendedHasData ? [] : Get(AdrProp.Extended);

        IsEmpty = _dic.Count == 0;
    }

    private Address()
    {
        Street = Extended = [];
        IsEmpty = true;
    }

    internal Address(in ReadOnlyMemory<char> vCardValue, VCdVersion version)
    {
        IsEmpty = true;
        int index = -1;
        bool newStreetHasData = false;
        bool newExtendedHasData = false;

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
            string[] coll = span.Contains(',')
                ? Splitted(in mem, version).ToArray()
                : StringArrayConverter.ToStringArray(span.UnMaskValue(version));

            if (!coll.ContainsData())
            {
                continue;
            }

            IsEmpty = false;

            var key = (AdrProp)index;
            _dic[key] = coll;

            switch (key)
            {
                case AdrProp.Room:
                case AdrProp.Apartment:
                case AdrProp.Floor:
                case AdrProp.Building:
                    newExtendedHasData = true;
                    break;
                case AdrProp.Block:
                case AdrProp.StreetNumber:
                case AdrProp.StreetName:
                case AdrProp.SubDistrict:
                case AdrProp.District:
                case AdrProp.Landmark:
                case AdrProp.Direction:
                    newStreetHasData = true;
                    break;
                default:
                    break;
            }

        }//foreach

        Street = newStreetHasData ? [] : Get(AdrProp.Street);
        Extended = newExtendedHasData ? [] : Get(AdrProp.Extended);

        ////////////////////////////////////////////////

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IEnumerable<string> Splitted(in ReadOnlyMemory<char> mem, VCdVersion version)
        => PropertyValueSplitter.Split(mem,
                                ',',
                                version: version);
    }

    /// <summary>
    /// Collects the new RFC 9554 properties and adds them to the corresponding vCard 4.0 property for
    /// compatibility.
    /// </summary>
    /// <param name="newKeys">Keys of the corresponding RFC 9554 properties.</param>
    /// <param name="newVals">Dictionary containing all RFC 9554 values.</param>
    /// <param name="oldProp">Key of the corresponding vCard 4.0 property.</param>
    private void AddForCompatibility(ReadOnlySpan<AdrProp> newKeys, Dictionary<AdrProp, string[]> newVals, AdrProp oldProp)
    {
        List<string> list = [];

        for (int i = 0; i < newKeys.Length; i++)
        {
            if (newVals.TryGetValue(newKeys[i], out string[]? strings))
            {
                Debug.Assert(strings.Length != 0);
                list.AddRange(strings);
            }
        }

        Debug.Assert(list.Count != 0);

        _dic[oldProp] = [.. list];
    }

    /// <summary>The post office box.</summary>
    public IReadOnlyList<string> POBox => Get(AdrProp.POBox);

    /// <summary>The extended address (e.g., apartment or suite number).</summary>
    /// <remarks>
    /// <para>
    /// This property returns an empty collection if any of these properties
    /// contains data:
    /// </para>
    /// <list type="bullet">
    /// <item><see cref="Building"/></item>
    /// <item><see cref="Floor"/></item>
    /// <item><see cref="Apartment"/></item>
    /// <item><see cref="Room"/></item>
    /// </list>
    /// </remarks>
    public IReadOnlyList<string> Extended { get; }

    /// <summary>The street address.</summary>
    /// <remarks>
    /// <para>
    /// This property returns an empty collection if any of these properties
    /// contains data:
    /// </para>
    /// <list type="bullet">
    /// <item><see cref="StreetName"/></item>
    /// <item><see cref="StreetNumber"/></item>
    /// <item><see cref="Block"/></item>
    /// <item><see cref="Landmark"/></item>
    /// <item><see cref="Direction"/></item>
    /// <item><see cref="SubDistrict"/></item>
    /// <item><see cref="District"/></item>
    /// </list>
    /// </remarks>
    public IReadOnlyList<string> Street { get; }

    /// <summary>The locality (e.g., city).</summary>
    public IReadOnlyList<string> Locality => Get(AdrProp.Locality);

    /// <summary>The region (e.g., state or province).</summary>
    public IReadOnlyList<string> Region => Get(AdrProp.Region);

    /// <summary>The postal code.</summary>
    public IReadOnlyList<string> PostalCode => Get(AdrProp.PostalCode);

    /// <summary>The country name (full name).</summary>
    public IReadOnlyList<string> Country => Get(AdrProp.Country);

    /// <summary> The room, suite number, or identifier. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Room => Get(AdrProp.Room);

    /// <summary> The extension designation such as the apartment number, unit,
    /// or box number. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Apartment => Get(AdrProp.Apartment);

    /// <summary> The floor or level the address is located on. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Floor => Get(AdrProp.Floor);

    /// <summary> The street number, e.g., "123". (4 - RFC 9554)</summary>
    /// <value>This value is not restricted to numeric values and can include
    /// any value such as number ranges ("112-10"), grid style ("39.2 RD"), 
    /// alphanumerics ("N6W23001"), or fractionals ("123 1/2").</value>
    public IReadOnlyList<string> StreetNumber => Get(AdrProp.StreetNumber);

    /// <summary> The street name. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> StreetName => Get(AdrProp.StreetName);

    /// <summary> The building, tower, or condominium the address is located in.
    /// (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Building => Get(AdrProp.Building);

    /// <summary> The block name or number. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Block => Get(AdrProp.Block);

    /// <summary> The subdistrict, ward, or other subunit of a district.
    /// (4 - RFC 9554)</summary>
    public IReadOnlyList<string> SubDistrict => Get(AdrProp.SubDistrict);

    /// <summary> The district name. (4 - RFC 9554)</summary>
    public IReadOnlyList<string> District => Get(AdrProp.District);

    /// <summary> The publicly known prominent feature that can substitute
    /// the street name and number, e.g., "White House" or "Taj Mahal". 
    /// (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Landmark => Get(AdrProp.Landmark);

    /// <summary> The cardinal direction or quadrant, e.g., "north". 
    /// (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Direction => Get(AdrProp.Direction);

    /// <summary>Returns <c>true</c>, if the <see cref="Address" /> object does not
    /// contain any usable data.</summary>
    public bool IsEmpty { get; }

    internal static Address Empty => new(); // Not a singleton

    /// <inheritdoc/>
    int IReadOnlyCollection<IReadOnlyList<string>>.Count => MAX_COUNT;

    /// <inheritdoc/>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is 
    /// less than zero, or equal or greater than
    /// <see cref="IReadOnlyCollection{T}.Count"/>.</exception>
    IReadOnlyList<string> IReadOnlyList<IReadOnlyList<string>>.this[int index]
    {
        get
        {
            if ((uint)index >= MAX_COUNT)
            {
                throw new IndexOutOfRangeException();
            }

            var adr = (AdrProp)index;

            return adr switch
            {
                AdrProp.Street => Street,
                AdrProp.Extended => Extended,
                _ => Get(adr)
            };
        }
    }

    /// <inheritdoc/>
    IEnumerator<IReadOnlyList<string>> IEnumerable<IReadOnlyList<string>>.GetEnumerator()
    {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            yield return ((IReadOnlyList<IReadOnlyList<string>>)this)[i];
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyList<IReadOnlyList<string>>)this).GetEnumerator();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => CompoundObjectConverter.ToString(_dic);

    internal void AppendVcfString(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

        for (int i = 0; i < STANDARD_COUNT; i++)
        {
            CompoundObjectConverter.SerializeProperty(Get((AdrProp)i), joinChar, serializer);
        }

        if (serializer.Version >= VCdVersion.V4_0
            && serializer.Options.HasFlag(VcfOpts.WriteRfc9554Extensions)
            && _dic.Any(x => x.Key >= AdrProp.Room))
        {
            for (int i = STANDARD_COUNT; i < MAX_COUNT; i++)
            {
                CompoundObjectConverter.SerializeProperty(Get((AdrProp)i), joinChar, serializer);
            }
        }

        --builder.Length;

        if (serializer.ParameterSerializer.ParaSection.Encoding == Enc.QuotedPrintable)
        {
            CompoundObjectConverter.EncodeQuotedPrintable(builder, startIdx);
        }
    }

    internal bool NeedsToBeQpEncoded()
    {
        foreach (KeyValuePair<AdrProp, string[]> kvp in _dic)
        {
            if (kvp.Key <= AdrProp.Country
                && kvp.Value.ContainsAnyThatNeedsQpEncoding())
            {
                return true;
            }
        }

        return false;
    }


}
