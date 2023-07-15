using System.Collections;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Repräsentiert die in vCard 4.0 eingeführte vCard-Property <c>GENDER</c>, die Informationen über das Geschlecht
/// und die Geschlechtsidentität speichert.
/// </summary>
public sealed class GenderProperty : VCardProperty, IEnumerable<GenderProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private GenderProperty(GenderProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="GenderProperty"/>-Objekt.
    /// </summary>
    /// <param name="sex">Standardisierte Geschlechtsangabe.</param>
    /// <param name="genderIdentity">Freie Beschreibung des sexuellen Identität.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public GenderProperty(Enums.Gender? sex,
                          string? genderIdentity = null,
                          string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = new PropertyParts.GenderInfo(sex, genderIdentity);

    /// <summary>
    /// Die von der <see cref="GenderProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new PropertyParts.GenderInfo Value
    {
        get;
    }


    /// <inheritdoc/>
    public override bool IsEmpty => Value.IsEmpty; // Value ist nie null


    /// <inheritdoc/>
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


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();

        Value.AppendVCardStringTo(serializer);
    }


    IEnumerator<GenderProperty> IEnumerable<GenderProperty>.GetEnumerator()
    {
        yield return this;
    }


    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GenderProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    public override object Clone() => new GenderProperty(this);
}
