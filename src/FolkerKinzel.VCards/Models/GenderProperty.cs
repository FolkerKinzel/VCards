using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents the vCard property <c>GENDER</c>, introduced in vCard 4.0,
    /// which stores information to specify the components of the sex and gender identity
    /// of the object the vCard represents</summary>
public sealed class GenderProperty : VCardProperty, IEnumerable<GenderProperty>
{
    /// <summary />
    /// <param name="prop" />
    private GenderProperty(GenderProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initialisiert ein neues <see cref="GenderProperty" />-Objekt. </summary>
    /// <param name="sex">Standardized information about the sex of a person.</param>
    /// <param name="genderIdentity">Free text describing the gender identity.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public GenderProperty(Enums.Gender? sex,
                          string? genderIdentity = null,
                          string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = new PropertyParts.GenderInfo(sex, genderIdentity);

    /// <summary> Die von der <see cref="GenderProperty" /> zur Verfügung gestellten
    /// Daten. </summary>
    public new GenderInfo Value
    {
        get;
    }


    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty; // Value ist nie null


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal GenderProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Enums.Gender? sex = null;
        string? genderIdentity = null;
        ValueSplitter semicolonSplitter = vcfRow.Info.SemiColonSplitter;

        bool initGenderIdentity = false;
        semicolonSplitter.ValueString = vcfRow.Value;

        foreach (var s in semicolonSplitter)
        {
            if (initGenderIdentity)
            {
                genderIdentity = s.UnMask(vcfRow.Info.Builder, version);
                break;
            }
            else
            {
                sex = GenderConverter.Parse(s);
                initGenderIdentity = true;
            }
        }

        Value = new PropertyParts.GenderInfo(sex, genderIdentity);
    }


    internal override void AppendValue(VcfSerializer serializer) => Value.AppendVCardStringTo(serializer);


    IEnumerator<GenderProperty> IEnumerable<GenderProperty>.GetEnumerator()
    {
        yield return this;
    }


    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GenderProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new GenderProperty(this);
}
