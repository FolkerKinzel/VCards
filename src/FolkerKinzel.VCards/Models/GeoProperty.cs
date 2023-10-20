using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents the vCard property <c>GEO</c>, which encapsulates a geographic
    /// position.</summary>
public sealed class GeoProperty : VCardProperty, IEnumerable<GeoProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop" />
    private GeoProperty(GeoProperty prop) : base(prop)
        => Value = prop.Value;


    /// <summary>  Initializes a new <see cref="GeoProperty" /> object. </summary>
    /// <param name="value">Ein <see cref="GeoCoordinate" />-Objekt oder <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public GeoProperty(GeoCoordinate? value, string? propertyGroup = null) 
        : base(new ParameterSection(), propertyGroup) => this.Value = value;

    internal GeoProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (GeoCoordinate.TryParse(vcfRow.Value.AsSpan(), out GeoCoordinate? geo))
        {
            this.Value = geo;
        }
    }


    /// <summary> The data provided by the <see cref="GeoProperty" />.
    /// </summary>
    public new GeoCoordinate? Value
    {
        get;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        GeoCoordinateConverter.AppendTo(serializer.Builder, Value, serializer.Version);
    }

    IEnumerator<GeoProperty> IEnumerable<GeoProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GeoProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new GeoProperty(this);
}
