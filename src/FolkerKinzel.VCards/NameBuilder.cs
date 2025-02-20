using System.ComponentModel;
using System.Data;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize a <see cref="Models.Properties.NameProperty"></see> instance.
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

    private NameBuilder() { }

    /// <summary>
    /// Builds a new <see cref="Name"/> instance with the content of the <see cref="NameBuilder"/>
    /// and clears all of this content when it returns.
    /// </summary>
    /// <returns>The newly created <see cref="Name"/> instance.</returns>
    public Name Build()
    {
        var name = new Name(this);
        Clear();
        return name;
    }

    /// <summary>
    /// Creates a new <see cref="NameBuilder"/> instance.
    /// </summary>
    /// <returns>The newly created <see cref="NameBuilder"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameBuilder Create() => new();

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Generations"/>. (A generation
    /// marker or qualifier, e.g., "Jr." or "III".) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSuffix(string?)"/> to add a copy to 
    /// <see cref="Name.Suffixes"/>.
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
    /// <see cref="Name.Suffixes"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Generations"/>
    /// <seealso cref="Name.Suffixes"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGeneration(IEnumerable<string?> collection) => AddRange(NameProp.Generations, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Given"/>. (A given name, also 
    /// known as "first name".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Given"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGiven(string? value) => Add(NameProp.Given, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Given"/>. (Given names, also known as "first names".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Given"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGiven(IEnumerable<string?> collection)
        => AddRange(NameProp.Given, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Given2"/>. (An additional name,
    /// also known as "middle name".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Given2"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGiven2(string? value) => Add(NameProp.Given2, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Given2"/>. (Additional names, also known as "middle names".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Given2"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddGiven2(IEnumerable<string?> collection) => AddRange(NameProp.Given2, collection);

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

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Surnames"/>. (A family name, 
    /// also known as "surname".) (2,3,4)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// 
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname(string? value) => Add(NameProp.Surnames, value);

    /// <summary>
    /// Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Surnames"/>. (Family names, also known as "surnames".) (2,3,4)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname(IEnumerable<string?> collection)
        => AddRange(NameProp.Surnames, collection);

    /// <summary>Adds a <see cref="string"/> to <see cref="Name.Surnames2"/>. (A secondary surname 
    /// (used in some cultures), also known as "maternal surname".) (4 - RFC 9554)</summary>
    /// <param name="value">The <see cref="string"/> value to add or <c>null</c>.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSurname(string?)"/> to add a copy to 
    /// <see cref="Name.Surnames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames2"/>
    /// <seealso cref="Name.Surnames"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname2(string? value) => Add(NameProp.Surnames2, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of <see cref="string"/>s to 
    /// <see cref="Name.Surnames2"/>. (Secondary surnames (used in some cultures), also known as 
    /// "maternal surnames".) (4 - RFC 9554)</summary>
    /// <param name="collection">The collection containing the <see cref="string"/>s to add.</param>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSurname(IEnumerable{string?})"/> to add a copy to 
    /// <see cref="Name.Surnames"/>.
    /// </note>
    /// </remarks>
    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    /// <seealso cref="Name.Surnames2"/>
    /// <seealso cref="Name.Surnames"/>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameBuilder AddSurname2(IEnumerable<string?> collection)
        => AddRange(NameProp.Surnames2, collection);

    /// <returns>The current <see cref="NameBuilder"/> instance to be able to chain calls.</returns>
    internal IEnumerable<KeyValuePair<NameProp, List<string>>> Data => _dic;

    /// <summary>
    /// Clears each content of the <see cref="NameBuilder"/> instance but keeps the 
    /// allocated memory for reuse.
    /// </summary>
    private void Clear()
    {
        foreach (KeyValuePair<NameProp, List<string>> pair in _dic)
        {
            pair.Value.Clear();
        }
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
