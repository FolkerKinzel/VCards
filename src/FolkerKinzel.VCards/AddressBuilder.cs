using System.ComponentModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize a <see cref="Models.AddressProperty"></see> instance.
/// </summary>
/// <remarks>
/// <threadsafety static="true" instance="false"/>
/// </remarks>
public class AddressBuilder
{
    private readonly Dictionary<AdrProp, List<string>> _dic = [];

    /// <summary>
    /// Creates a new <see cref="AddressBuilder"/> instance.
    /// </summary>
    /// <returns>The newly created <see cref="AddressBuilder"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AddressBuilder Create() => new();

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.PostOfficeBox"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToPostOfficeBox(string? value) => Add(AdrProp.PostOfficeBox, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="strings"/> to <see cref="Address.PostOfficeBox"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToPostOfficeBox(IEnumerable<string?> collection) => AddRange(AdrProp.PostOfficeBox, collection);

    /// <summary>Adds a family name (also known as surname). (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToExtendedAddress(string? value) => Add(AdrProp.ExtendedAddress, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of family names (also known as surnames). (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToExtendedAddress(IEnumerable<string?> collection) => AddRange(AdrProp.ExtendedAddress, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Street"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreet(string? value) => Add(AdrProp.Street, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Street"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreet(IEnumerable<string?> collection) => AddRange(AdrProp.Street, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Locality"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToLocality(string? value) => Add(AdrProp.Locality, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Locality"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToLocality(IEnumerable<string?> collection) => AddRange(AdrProp.Locality, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Region"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToRegion(string? value) => Add(AdrProp.Region, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Region"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToRegion(IEnumerable<string?> collection) => AddRange(AdrProp.Region, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.PostalCode"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToPostalCode(string? value) => Add(AdrProp.PostalCode, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.PostalCode"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToPostalCode(IEnumerable<string?> collection) => AddRange(AdrProp.PostalCode, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Country"/>. (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToCountry(string? value) => Add(AdrProp.Country, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Country"/>. (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToCountry(IEnumerable<string?> collection) => AddRange(AdrProp.Country, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Room"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToRoom(string? value) => Add(AdrProp.Room, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Room"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToRoom(IEnumerable<string?> collection) => AddRange(AdrProp.Room, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Apartment"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToApartment(string? value) => Add(AdrProp.Apartment, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Apartment"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToApartment(IEnumerable<string?> collection) => AddRange(AdrProp.Apartment, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Floor"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToFloor(string? value) => Add(AdrProp.Floor, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Floor"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToFloor(IEnumerable<string?> collection) => AddRange(AdrProp.Floor, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.StreetNumber"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreetNumber(string? value) => Add(AdrProp.StreetNumber, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.StreetNumber"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreetNumber(IEnumerable<string?> collection) => AddRange(AdrProp.StreetNumber, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.StreetName"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreetName(string? value) => Add(AdrProp.StreetName, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.StreetName"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToStreetName(IEnumerable<string?> collection) => AddRange(AdrProp.StreetName, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Building"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToBuilding(string? value) => Add(AdrProp.Building, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Building"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToBuilding(IEnumerable<string?> collection) => AddRange(AdrProp.Building, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Block"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToBlock(string? value) => Add(AdrProp.Block, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Block"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToBlock(IEnumerable<string?> collection) => AddRange(AdrProp.Block, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.SubDistrict"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToSubDistrict(string? value) => Add(AdrProp.SubDistrict, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.SubDistrict"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToSubDistrict(IEnumerable<string?> collection) => AddRange(AdrProp.SubDistrict, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.District"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToDistrict(string? value) => Add(AdrProp.District, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.District"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToDistrict(IEnumerable<string?> collection) => AddRange(AdrProp.District, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Landmark"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToLandmark(string? value) => Add(AdrProp.Landmark, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Landmark"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToLandmark(IEnumerable<string?> collection) => AddRange(AdrProp.Landmark, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Address.Direction"/>. (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(string?)"/> to make a copy of <paramref name="value"/> in <see cref="Address.Street"/>
    /// for backwards compatibility.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToDirection(string? value) => Add(AdrProp.Direction, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Address.Direction"/>. (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Use <see cref="AddToStreet(IEnumerable{string?})"/> to make a copy of <paramref name="collection"/> 
    /// in <see cref="Address.Street"/> for backwards compatibility.
    /// </note>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToDirection(IEnumerable<string?> collection) => AddRange(AdrProp.Direction, collection);



    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    internal IEnumerable<KeyValuePair<AdrProp, List<string>>> Data => _dic;

    /// <summary>
    /// Clears every content of the <see cref="AddressBuilder"/> instance but keeps the 
    /// allocated memory for reuse.
    /// </summary>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    public AddressBuilder Clear()
    {
        foreach (KeyValuePair<AdrProp, List<string>> pair in _dic)
        {
            pair.Value.Clear();
        }

        return this;
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
