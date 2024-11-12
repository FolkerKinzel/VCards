using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the <c>KIND</c> property, introduced in vCard&#160;4.0, which
/// describes the type of object represented by the vCard.</summary>
/// <seealso cref="VCard.Kind"/>
/// <seealso cref="Kind"/>
public sealed class KindProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="KindProperty"/> instance to clone.</param>
    private KindProperty(KindProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="KindProperty" /> object. </summary>
    /// <param name="value">A member of the <see cref="Kind" /> enum.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public KindProperty(Kind value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;

    internal KindProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group) => Value = KindConverter.Parse(vcfRow.Value.Span);

    /// <summary> The data provided by the <see cref="KindProperty" />.
    /// </summary>
    public new Kind Value
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
        Debug.Assert(serializer is not null);

        _ = serializer.Builder.Append(Value.ToVcfString());
    }
}
