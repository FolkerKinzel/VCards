using System.Collections;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Kapselt Informationen, die dazu dienen, vCard-Properties über verschiedene Bearbeitungsstände derselben vCard hinweg
/// eindeutig zu identifizieren.
/// </summary>
public sealed class PropertyIDMappingProperty : VCardProperty, IEnumerable<PropertyIDMappingProperty>
{
    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private PropertyIDMappingProperty(PropertyIDMappingProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="PropertyIDMappingProperty"/>-Objekt.
    /// </summary>
    /// <param name="value">Ein <see cref="PropertyIDMapping"/>-Objekt oder <c>null</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> ist kleiner als 1 oder größer als 9.</exception>
    public PropertyIDMappingProperty(PropertyIDMapping? value) : base(new ParameterSection(), null)
        => Value = value;


    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="vcfRow"><see cref="VcfRow"/></param>
    /// <exception cref="ArgumentException">Aus <paramref name="vcfRow"/> kann kein <see cref="PropertyIDMapping"/>
    /// geparst werden.</exception>
    internal PropertyIDMappingProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (vcfRow.Value is null)
        {
            return;
        }

        Value = PropertyIDMapping.Parse(vcfRow.Value);
    }


    /// <summary>
    /// Die von der <see cref="PropertyIDMappingProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new PropertyIDMapping? Value
    {
        get;
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();
        Value?.AppendTo(serializer.Builder);
    }

    IEnumerator<PropertyIDMappingProperty> IEnumerable<PropertyIDMappingProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PropertyIDMappingProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    public override object Clone() => new PropertyIDMappingProperty(this);
}
