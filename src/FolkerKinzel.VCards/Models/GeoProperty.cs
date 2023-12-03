using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>GEO</c>, which encapsulates a geographic
/// position.</summary>
/// <remarks>See <see cref="VCard.GeoCoordinates"/>.</remarks>
/// <seealso cref="GeoCoordinate"/>
/// <seealso cref="VCard.GeoCoordinates"/>
public sealed class GeoProperty : VCardProperty, IEnumerable<GeoProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="GenderProperty"/> instance
    /// to clone.</param>
    private GeoProperty(GeoProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="GeoProperty" /> object. </summary>
    /// <param name="value">A <see cref="GeoCoordinate" /> object or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public GeoProperty(GeoCoordinate? value, string? group = null)
        : base(new ParameterSection(), group) => this.Value = value;

    /// <summary>
    /// Initializes a new <see cref="GeoProperty" /> object.
    /// </summary>
    /// <param name="latitude">Latitude (value between -90 and 90).</param>
    /// <param name="longitude">Longitude (value between -180 and 180).</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    ///  <exception cref="ArgumentOutOfRangeException"> <paramref name="latitude" />
    /// or <paramref name="longitude" /> does not have a valid value.</exception>
    public GeoProperty(double latitude, double longitude, string? group = null)
        : this(new GeoCoordinate(latitude, longitude), group) { }


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
    public override object Clone() => new GeoProperty(this);

    /// <inheritdoc />
    IEnumerator<GeoProperty> IEnumerable<GeoProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GeoProperty>)this).GetEnumerator();

    /// <inheritdoc />
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        GeoCoordinateConverter.AppendTo(serializer.Builder, Value, serializer.Version);
    }
}
