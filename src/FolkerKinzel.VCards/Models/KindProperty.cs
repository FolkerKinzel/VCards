using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents the <c>KIND</c> property, introduced in vCard 4.0, which
    /// describes the type of object represented by the vCard.</summary>
public sealed class KindProperty : VCardProperty
{
    /// <summary />
    /// <param name="prop" />
    private KindProperty(KindProperty prop) : base(prop)
        => Value = prop.Value;


    /// <summary> Initialisiert ein neues <see cref="KindProperty" />-Objekt. </summary>
    /// <param name="value">Ein Member der <see cref="VCdKind" />-Enumeration.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public KindProperty(VCdKind value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = value;


    internal KindProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = VCdKindConverter.Parse(vcfRow.Value);


    /// <summary> Die von der <see cref="KindProperty" /> zur Verfügung gestellten Daten.
    /// </summary>
    public new VCdKind Value
    {
        get;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    /// <inheritdoc />
    public override bool IsEmpty => false;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(Value.ToVcfString());
    }

    /// <inheritdoc />
    public override object Clone() => new KindProperty(this);
}
