using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
    /// for the vCard subject.</summary>
public sealed class UuidProperty : VCardProperty
{
    /// <summary />
    /// <param name="prop" />
    private UuidProperty(UuidProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initialisiert ein neues <see cref="UuidProperty" />-Objekt mit einer
    /// automatisch erzeugten <see cref="Guid" />. </summary>
    public UuidProperty() : this(Guid.NewGuid()) { }


    /// <summary> Initialisiert ein neues <see cref="UuidProperty" />-Objekt. </summary>
    /// <param name="uuid">Ein <see cref="Guid" />-Objekt.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public UuidProperty(Guid uuid, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = uuid;


    internal UuidProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
            => Value = UuidConverter.ToGuid(vcfRow.Value);



    /// <summary> Die von der <see cref="UuidProperty" /> zur Verfügung gestellten Daten.
    /// </summary>
    public new Guid Value
    {
        get;
    }


    /// <inheritdoc />
    public override bool IsEmpty => Value == Guid.Empty;


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.AppendUuid(this.Value, serializer.Version);
    }

    /// <inheritdoc />
    public override object Clone() => new UuidProperty(this);
}
