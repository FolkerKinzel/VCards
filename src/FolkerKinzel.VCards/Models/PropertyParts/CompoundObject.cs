using System.Collections.ObjectModel;
using System.ComponentModel;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public abstract class CompoundObject<T> where T : struct, Enum
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected Dictionary<T, ReadOnlyCollection<string>> _dic { get; } = [];

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected ReadOnlyCollection<string> Get(T prop)
        => _dic.TryGetValue(prop, out ReadOnlyCollection<string>? coll)
            ? coll
            : ReadOnlyStringCollection.Empty;

    /// <summary>Returns <c>true</c>, if the <see cref="CompoundObject{T}" /> does not
    /// contain any usable data.</summary>
    public bool IsEmpty => _dic.Count == 0;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => CompoundPropertyConverter.ToString(_dic);
}
