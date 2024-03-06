using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
/// for the vCard subject.</summary>
/// <seealso cref="VCard.ID"/>
/// <seealso cref="RelationProperty"/>
public sealed class IDProperty : VCardProperty, IEquatable<IDProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="IDProperty"/> instance
    /// to clone.</param>
    private IDProperty(IDProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="IDProperty" /> object with a
    /// new <see cref="Guid" />. </summary>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public IDProperty(string? group = null) : this(Guid.NewGuid(), group) { }

    /// <summary> Initializes a new <see cref="IDProperty" /> object with a 
    /// specified <see cref="Guid"/>. </summary>
    /// <param name="value">A <see cref="Guid" /> value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public IDProperty(Guid value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;

    internal IDProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
            => Value = UuidConverter.ToGuid(vcfRow.Value);

    /// <summary> The <see cref="Guid"/> provided by the <see cref="IDProperty" />.
    /// </summary>
    public new Guid Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value == Guid.Empty;

    /// <inheritdoc />
    public override object Clone() => new IDProperty(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as IDProperty);

    /// <inheritdoc />
    public bool Equals(IDProperty? other)
        => (other is not null) && Value.Equals(other.Value);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Value);

    /// <summary>
    /// Overloads the equality operator for <see cref="IDProperty"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(IDProperty? left, IDProperty? right)
        => object.ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="IDProperty"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="IDProperty"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(IDProperty? left, IDProperty? right)
        => !(left == right);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        _ = serializer.Builder.AppendUuid(this.Value, serializer.Version);
    }
}
