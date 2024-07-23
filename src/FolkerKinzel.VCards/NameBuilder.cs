using FolkerKinzel.VCards.Intls.Enums;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize a <see cref="Models.NameProperty"></see> instance.
/// </summary>
public class NameBuilder
{
    private readonly Dictionary<NameProp, List<string>> _dic = [];

    /// <summary>Adds a family name (also known as surname). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToFamilyNames(string? familyName) => Add(NameProp.FamilyNames, familyName);

    /// <summary>Adds the content of a <paramref name="collection"/> of family names (also known as surnames). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToFamilyNames(IEnumerable<string?> collection) => AddRange(NameProp.FamilyNames, collection);

    /// <summary>Adds a given name (first name). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToGivenNames(string? givenName) => Add(NameProp.GivenNames, givenName);

    /// <summary>Adds the content of a <paramref name="collection"/> of given names (first names). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToGivenNames(IEnumerable<string?> collection) => AddRange(NameProp.GivenNames, collection);

    /// <summary>Adds an additional name (middle name). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToAdditionalNames(string? givenName2) => Add(NameProp.AdditionalNames, givenName2);

    /// <summary>Adds the content of a <paramref name="collection"/> of additional names (middle names). (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToAdditionalNames(IEnumerable<string?> collection) => AddRange(NameProp.AdditionalNames, collection);

    /// <summary>Adds a honorific prefix. (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToPrefixes(string? prefix) => Add(NameProp.Prefixes, prefix);

    /// <summary>Adds the content of a <paramref name="collection"/> of honorific prefixes. (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToPrefixes(IEnumerable<string?> collection) => AddRange(NameProp.Prefixes, collection);

    /// <summary>Adds a honorific suffix. (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToSuffixes(string? suffix) => Add(NameProp.Suffixes, suffix);

    /// <summary>Adds the content of a <paramref name="collection"/> of honorific suffixes. (2,3,4)</summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToSuffixes(IEnumerable<string?> collection) => AddRange(NameProp.Suffixes, collection);

    /// <summary>Adds a secondary surname (used in some cultures), also known as "maternal surname". (4 - RFC 9554)</summary>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddToFamilyNames(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.FamilyNames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToSurname2(string? surname2) => Add(NameProp.Surname2, surname2);

    /// <summary>Adds the content of a <paramref name="collection"/> of secondary surnames (used in some cultures), 
    /// also known as "maternal surnames". (4 - RFC 9554)</summary>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddToFamilyNames(IEnumerable{string?})"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.FamilyNames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToSurname2(IEnumerable<string?> collection) => AddRange(NameProp.Surname2, collection);

    /// <summary>Adds a generation marker or qualifier, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddToSuffixes(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.Suffixes"/>.
    /// </note>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToGeneration(string? generation) => Add(NameProp.Generation, generation);

    /// <summary>Adds the content of a <paramref name="collection"/> of generation markers or qualifiers,
    /// e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddToSuffixes(IEnumerable{string?})"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.Suffixes"/>.
    /// </note>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddToGeneration(IEnumerable<string?> collection) => AddRange(NameProp.Generation, collection);

    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    internal IEnumerable<KeyValuePair<NameProp, List<string>>> Data => _dic;

    /// <summary>
    /// Clears every content of the <see cref="NameBuilder"/> instance but keeps the 
    /// allocated memory for reuse.
    /// </summary>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    public NameBuilder Clear()
    {
        foreach (KeyValuePair<NameProp, List<string>> pair in _dic)
        {
            pair.Value.Clear();
        }

        return this;
    }

    private NameBuilder Add(NameProp prop, string? value)
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

    private NameBuilder AddRange(NameProp prop, IEnumerable<string?> collection)
    {
        IEnumerable<string> valsToAdd = (collection?.Where(static x => !string.IsNullOrWhiteSpace(x))
            ?? throw new ArgumentOutOfRangeException(nameof(collection)))!;

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

}
