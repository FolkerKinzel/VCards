using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
/// for the vCard subject.</summary>
/// <seealso cref="VCard.UniqueIdentifier"/>
/// <seealso cref="RelationProperty"/>
public sealed class UuidProperty : VCardProperty, IEquatable<UuidProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="UuidProperty"/> instance
    /// to clone.</param>
    private UuidProperty(UuidProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="UuidProperty" /> object with a
    /// new <see cref="Guid" />. </summary>
    public UuidProperty() : this(Guid.NewGuid()) { }

    /// <summary> Initializes a new <see cref="UuidProperty" /> object with a 
    /// specified <see cref="Guid"/>. </summary>
    /// <param name="uuid">A <see cref="Guid" /> value.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public UuidProperty(Guid uuid, string? propertyGroup = null)
        : base(new ParameterSection(), propertyGroup) => Value = uuid;


    internal UuidProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
            => Value = UuidConverter.ToGuid(vcfRow.Value);

    /// <summary> The <see cref="Guid"/> provided by the <see cref="UuidProperty" />.
    /// </summary>
    public new Guid Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value == Guid.Empty;

    /// <inheritdoc />
    public override object Clone() => new UuidProperty(this);

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is UuidProperty other && Equals(other);

    /// <inheritdoc />
    public bool Equals(UuidProperty? other)
        => other != null && Value.Equals(other.Value);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Value);

    /// <inheritdoc />
    public static bool operator ==(UuidProperty? left, UuidProperty? right)
        => object.ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <inheritdoc />
    public static bool operator !=(UuidProperty? left, UuidProperty? right)
        => !(left == right);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.AppendUuid(this.Value, serializer.Version);
    }
}
