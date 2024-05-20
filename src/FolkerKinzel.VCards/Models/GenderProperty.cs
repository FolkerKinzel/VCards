using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>GENDER</c>, introduced in vCard&#160;4.0,
/// which stores information to specify the components of gender and gender identity
/// of the object the <see cref="VCard"/> represents.
/// </summary>
/// <seealso cref="GenderInfo"/>
/// <seealso cref="VCard.GenderViews"/>
public sealed class GenderProperty : VCardProperty, IEnumerable<GenderProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="GenderProperty"/> instance
    /// to clone.</param>
    private GenderProperty(GenderProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="GenderProperty" /> object. </summary>
    /// <param name="sex">Standardized information about the sex of the object the 
    /// <see cref="VCard"/> represents.</param>
    /// <param name="identity">Free text describing the gender identity.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public GenderProperty(Sex? sex,
                          string? identity = null,
                          string? group = null) : base(new ParameterSection(), group)
        => Value = new GenderInfo(sex, identity);


    internal GenderProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Sex? sex = null;
        string? genderIdentity = null;
        ValueSplitter semicolonSplitter = vcfRow.Info.SemiColonSplitter;

        bool initGenderIdentity = false;
        semicolonSplitter.ValueString = vcfRow.Value;

        foreach (var s in semicolonSplitter)
        {
            if (initGenderIdentity)
            {
                genderIdentity = s.UnMask(version);
                break;
            }
            else
            {
                sex = SexConverter.Parse(s);
                initGenderIdentity = true;
            }
        }

        Value = new GenderInfo(sex, genderIdentity);
    }

    /// <summary>The data provided by the <see cref="GenderProperty" />. </summary>
    public new GenderInfo Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty; // Value ist nie null

    /// <inheritdoc />
    public override object Clone() => new GenderProperty(this);

    /// <inheritdoc />
    IEnumerator<GenderProperty> IEnumerable<GenderProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GenderProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer) => Value.AppendVCardStringTo(serializer);
}
