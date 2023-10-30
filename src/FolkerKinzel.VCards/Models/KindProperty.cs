using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the <c>KIND</c> property, introduced in vCard&#160;4.0, which
/// describes the type of object represented by the vCard.</summary>
/// <seealso cref="VCard.Kind"/>
/// <seealso cref="VCdKind"/>
public sealed class KindProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="KindProperty"/> instance to clone.</param>
    private KindProperty(KindProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="KindProperty" /> object. </summary>
    /// <param name="value">A member of the <see cref="VCdKind" /> enum.</param>
    public KindProperty(VCdKind value)
        : base(new ParameterSection(), null) => Value = value;

    internal KindProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group) => Value = VCdKindConverter.Parse(vcfRow.Value);

    /// <summary> The data provided by the <see cref="KindProperty" />.
    /// </summary>
    public new VCdKind Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => false;

    /// <inheritdoc />
    public override object Clone() => new KindProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(Value.ToVcfString());
    }
}
