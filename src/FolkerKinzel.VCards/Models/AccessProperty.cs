using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the data of the vCard property <c>CLASS</c>, which defines
/// the level of confidentiality of the vCard in vCard&#160;3.0.</summary>
/// <remarks>See <see cref="VCard.Access"/>.</remarks>
/// <seealso cref="VCard.Access"/>
public sealed class AccessProperty : VCardProperty
{
    /// <summary>Copy ctor</summary>
    /// <param name="prop">The <see cref="AccessProperty" /> object to clone.</param>
    private AccessProperty(AccessProperty prop) : base(prop) => Value = prop.Value;

    /// <summary> Initializes a new <see cref="AccessProperty" /> object. </summary>
    /// <param name="value">A member of the <see cref="Acs" /> enum.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public AccessProperty(Acs value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;


    internal AccessProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        => Value = AccessConverter.Parse(vcfRow.Value);

    /// <inheritdoc />
    public override bool IsEmpty => false;

    /// <summary>The data provided by the <see cref="AccessProperty" />.</summary>
    public new Acs Value
    {
        get;
    }

    /// <inheritdoc />
    public override object Clone() => new AccessProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        _ = serializer.Builder.Append(Value.ToVCardString());
    }
}
