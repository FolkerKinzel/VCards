using System.ComponentModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Enums;

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

    /// <summary>Adds a family name (also known as surname). (2,3,4)</summary>
    /// <returns>The current <see cref="AddressBuilder"/> instance to be able to chain calls.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToExtendedAddress(string? value) => Add(AdrProp.ExtendedAddress, value);

    /// <summary>Adds the content of a <paramref name="collection"/> of family names (also known as surnames). (2,3,4)</summary>
    /// <returns>The current <see cref="AdrBuilder"/> instance to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AddressBuilder AddToExtendedAddress(IEnumerable<string?> collection) => AddRange(AdrProp.ExtendedAddress, collection);

    

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
