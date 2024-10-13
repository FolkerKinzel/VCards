using System.ComponentModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize a <see cref="Models.NameProperty"></see> instance.
/// </summary>
/// <remarks>
/// <para>
/// An instance of this class can be reused but it's not safe to access its instance methods with
/// multiple threads in parallel.
/// </para>
/// <threadsafety static="true" instance="false"/>
/// </remarks>
public sealed class NameBuilder
{
    private readonly Dictionary<NameProp, List<string>> _dic = [];
    private List<string>? _worker;

    internal List<string> Worker
    {
        get
        {
            _worker ??= [];
            return _worker;
        }
    }

    /// <summary>
    /// Creates a new <see cref="NameBuilder"/> instance.
    /// </summary>
    /// <returns>The newly created <see cref="NameBuilder"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameBuilder Create() => new();

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.FamilyNames"/>. (A family name, 
    /// also known as "surname".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// 
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.FamilyNames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddFamilyName(string? value) => Add(NameProp.FamilyNames, value);

    /// <summary>
    /// Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.FamilyNames"/>. (Family names, also known as "surnames".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.FamilyNames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddFamilyName(IEnumerable<string?> collection) 
        => AddRange(NameProp.FamilyNames, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.GivenNames"/>. (A given name, also 
    /// known as "first name".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.GivenNames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGivenName(string? value) => Add(NameProp.GivenNames, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.GivenNames"/>. (Given names, also known as "first names".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.GivenNames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGivenName(IEnumerable<string?> collection)
        => AddRange(NameProp.GivenNames, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.AdditionalNames"/>. (An additional name,
    /// also known as "middle name".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.AdditionalNames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddAdditionalName(string? value) => Add(NameProp.AdditionalNames, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.AdditionalNames"/>. (Additional names, also known as "middle names".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.AdditionalNames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddAdditionalName(IEnumerable<string?> collection) => AddRange(NameProp.AdditionalNames, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Prefixes"/>. (A honorific prefix.) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Prefixes"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddPrefix(string? value) => Add(NameProp.Prefixes, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Prefixes"/>. (Honorific prefixes.) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Prefixes"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddPrefix(IEnumerable<string?> collection) => AddRange(NameProp.Prefixes, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Suffixes"/>. (A honorific suffix.)
    /// (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Suffixes"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSuffix(string? value) => Add(NameProp.Suffixes, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Suffixes"/>. (Honorific suffixes.) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Suffixes"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSuffix(IEnumerable<string?> collection) 
        => AddRange(NameProp.Suffixes, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Surnames2"/>. (A secondary surname 
    /// (used in some cultures), also known as "maternal surname".) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddFamilyName(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.FamilyNames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames2"/>
    /// <seealso cref="Name.FamilyNames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname2(string? value) => Add(NameProp.Surnames2, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Surnames2"/>. (Secondary surnames (used in some cultures), also known as 
    /// "maternal surnames".) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddFamilyName(IEnumerable{string?})"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.FamilyNames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames2"/>
    /// <seealso cref="Name.FamilyNames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname2(IEnumerable<string?> collection) 
        => AddRange(NameProp.Surnames2, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Generations"/>. (A generation
    /// marker or qualifier, e.g., "Jr." or "III".) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSuffix(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.Suffixes"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Generations"/>
    /// <seealso cref="Name.Suffixes"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGeneration(string? value) => Add(NameProp.Generations, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Surnames2"/>. (Generation markers or qualifiers, e.g., "Jr." or "III".) 
    /// (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSuffix(IEnumerable{string?})"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.Suffixes"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Generations"/>
    /// <seealso cref="Name.Suffixes"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGeneration(IEnumerable<string?> collection) => AddRange(NameProp.Generations, collection);

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
