using System.ComponentModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize an <see cref="Models.AddressProperty"></see> instance.
/// </summary>
/// <remarks>
/// <para>
/// An instance of this class can be reused but it's not safe to access its instance methods with
/// multiple threads in parallel.
/// </para>
/// <threadsafety static="true" instance="false"/>
/// </remarks>
public sealed class AddressBuilder
{
    #region Remove with version 8.0.1

    [Obsolete("Use AddExtended instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressBuilder AddExtendedAddress(string? value) => Add(AdrProp.Extended, value);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use AddExtended instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public AddressBuilder AddExtendedAddress(IEnumerable<string?> collection) => AddRange(AdrProp.Extended, collection);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

    private readonly Dictionary<AdrProp, List<string>> _dic = [];

    private AddressBuilder() { }

    /// <summary>
    /// Builds a new <see cref="Address"/> instance with the content of the <see cref="AddressBuilder"/>
    /// and clears all of this content when it returns.
    /// </summary>
    /// <returns>The newly created <see cref="Address"/> instance.</returns>
    public Address Build()
    {
        var name = new Address(this);
        Clear();
        return name;
    }

    /// <summary>
    /// Creates a new <see cref="AddressBuilder"/> instance.
    /// </summary>
    /// <returns>The newly created <see cref="AddressBuilder"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AddressBuilder Create() => new();

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.PostOfficeBox"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.PostOfficeBox"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddPostOfficeBox(string? value) => Add(AdrProp.PostOfficeBox, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.PostOfficeBox"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.PostOfficeBox"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddPostOfficeBox(IEnumerable<string?> collection) => AddRange(AdrProp.PostOfficeBox, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Extended"/> (e.g., apartment or suite number). (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Extended"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddExtended(string? value) => Add(AdrProp.Extended, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Extended"/> (e.g., apartment or suite number). (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Extended"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddExtended(IEnumerable<string?> collection) => AddRange(AdrProp.Extended, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Street"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreet(string? value) => Add(AdrProp.Street, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Street"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreet(IEnumerable<string?> collection) => AddRange(AdrProp.Street, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Locality"/> (e.g., city). (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddLocality(string? value) => Add(AdrProp.Locality, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Locality"/> (e.g., city). (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddLocality(IEnumerable<string?> collection) => AddRange(AdrProp.Locality, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Region"/> (e.g., state or province). (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Region"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddRegion(string? value) => Add(AdrProp.Region, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Region"/> (e.g., state or province). (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Region"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddRegion(IEnumerable<string?> collection) => AddRange(AdrProp.Region, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.PostalCode"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.PostalCode"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddPostalCode(string? value) => Add(AdrProp.PostalCode, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.PostalCode"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.PostalCode"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddPostalCode(IEnumerable<string?> collection) => AddRange(AdrProp.PostalCode, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Country"/>. (The full country name.) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Country"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddCountry(string? value) => Add(AdrProp.Country, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Country"/>. (The full country name.) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Address.Country"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddCountry(IEnumerable<string?> collection) => AddRange(AdrProp.Country, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Room"/>. (The room, suite number, or identifier.) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Extended"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Room"/>
    /// <seealso cref="Address.Extended"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddRoom(string? value) => Add(AdrProp.Room, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Room"/>. (The room, suite number, or identifier.) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Room"/>
    /// <seealso cref="Address.Extended"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddRoom(IEnumerable<string?> collection) => AddRange(AdrProp.Room, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Apartment"/>. (The extension designation 
    /// such as the apartment number, unit, or box number.) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(string?)"/> to make a copy of <paramref name="value"/> in 
    /// <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Apartment"/>
    /// <seealso cref="Address.Extended"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddApartment(string? value) => Add(AdrProp.Apartment, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Apartment"/>. (The extension designation such as the apartment number,
    /// unit, or box number.) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Apartment"/>
    /// <seealso cref="Address.Extended"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddApartment(IEnumerable<string?> collection) 
        => AddRange(AdrProp.Apartment, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Floor"/>. (The floor or level the address 
    /// is located on.) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(string?)"/> to make a copy of <paramref name="value"/> in 
    /// <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Floor"/>
    /// <seealso cref="Address.Extended"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddFloor(string? value) => Add(AdrProp.Floor, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Floor"/>. (The floor or level the address is located on.) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Floor"/>
    /// <seealso cref="Address.Extended"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddFloor(IEnumerable<string?> collection) => AddRange(AdrProp.Floor, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.StreetNumber"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>. <paramref name="value"/> 
    /// is not restricted to numeric values and can include
    /// any value such as number ranges ("112-10"), grid style ("39.2 RD"), alphanumerics ("N6W23001"), or 
    /// fractionals ("123 1/2").</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.StreetNumber"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreetNumber(string? value) => Add(AdrProp.StreetNumber, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.StreetNumber"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add. Values are 
    /// not restricted to numeric values and can include
    /// any value such as number ranges ("112-10"), grid style ("39.2 RD"), alphanumerics ("N6W23001"), or 
    /// fractionals ("123 1/2").</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.StreetNumber"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreetNumber(IEnumerable<string?> collection) => AddRange(AdrProp.StreetNumber, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.StreetName"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.StreetName"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreetName(string? value) => Add(AdrProp.StreetName, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.StreetName"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.StreetName"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddStreetName(IEnumerable<string?> collection) => AddRange(AdrProp.StreetName, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Building"/>. (The building, tower, 
    /// or condominium the address is located in.) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(string?)"/> to make a copy of <paramref name="value"/> in 
    /// <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Building"/>
    /// <seealso cref="Address.Extended"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddBuilding(string? value) => Add(AdrProp.Building, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Building"/>. (The building, tower, or condominium the address is located in.)
    /// (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddExtended(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Extended"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Building"/>
    /// <seealso cref="Address.Extended"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddBuilding(IEnumerable<string?> collection) => AddRange(AdrProp.Building, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Block"/>. (The block name or number.) 
    /// (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Block"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddBlock(string? value) => Add(AdrProp.Block, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Block"/>. (The block name or number.) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Block"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddBlock(IEnumerable<string?> collection) => AddRange(AdrProp.Block, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.SubDistrict"/>. (The subdistrict, 
    /// ward, or other subunit of a district.) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.SubDistrict"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddSubDistrict(string? value) => Add(AdrProp.SubDistrict, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.SubDistrict"/>. (The subdistrict, ward, or other subunit of a district.)
    /// (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.SubDistrict"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddSubDistrict(IEnumerable<string?> collection) => AddRange(AdrProp.SubDistrict, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.District"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.District"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddDistrict(string? value) => Add(AdrProp.District, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.District"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.District"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddDistrict(IEnumerable<string?> collection) => AddRange(AdrProp.District, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Landmark"/>. (The publicly known prominent 
    /// feature that can substitute the street name and number, e.g., "White House" or "Taj Mahal".) 
    /// (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Landmark"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddLandmark(string? value) => Add(AdrProp.Landmark, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Landmark"/>. (The publicly known prominent feature that can substitute the street
    /// name and number, e.g., "White House" or "Taj Mahal".) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Landmark"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddLandmark(IEnumerable<string?> collection) => AddRange(AdrProp.Landmark, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Direction"/>. (The cardinal direction
    /// or quadrant, e.g., "north".) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Direction"/>
    /// <seealso cref="Address.Street"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddDirection(string? value) => Add(AdrProp.Direction, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Direction"/>. (The cardinal direction or quadrant, e.g., "north".) 
    /// (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <seealso cref="Address.Direction"/>
    /// <seealso cref="Address.Street"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddDirection(IEnumerable<string?> collection) => AddRange(AdrProp.Direction, collection);

    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    internal IEnumerable<KeyValuePair<AdrProp, List<string>>> Data => _dic;

    /// <summary>
    /// Clears each content of the <see cref="AddressBuilder"/> instance but keeps the 
    /// allocated memory for reuse.
    /// </summary>
    private void Clear()
    {
        foreach (KeyValuePair<AdrProp, List<string>> pair in _dic)
        {
            pair.Value.Clear();
        }
    }

    private AddressBuilder Add(AdrProp prop, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return this;
        }

        if (_dic.TryGetValue(prop, out List<string>? list))
        {
            list.Add(value);
            return this;
        }

        _dic[prop] = [value];
        return this;
    }

    private AddressBuilder AddRange(AdrProp prop, IEnumerable<string?> collection)
    {
        _ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        IEnumerable<string> valsToAdd = collection.Where(static x => !string.IsNullOrWhiteSpace(x))!;

        if (!valsToAdd.Any())
        {
            return this;
        }

        if (_dic.TryGetValue(prop, out List<string>? list))
        {
            list.AddRange(valsToAdd);
            return this;
        }

        _dic[prop] = new List<string>(valsToAdd);
        return this;
    }

    // Overriding Equals, GetHashCode and ToString to hide these methods in IntelliSense:

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}
