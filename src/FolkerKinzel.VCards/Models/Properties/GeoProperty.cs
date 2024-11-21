using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>GEO</c>, which encapsulates a geographic
/// position.</summary>
/// <remarks>See <see cref="VCard.GeoCoordinates"/>.</remarks>
/// <seealso cref="GeoCoordinate"/>
/// <seealso cref="VCard.GeoCoordinates"/>
public sealed class GeoProperty : VCardProperty, IEnumerable<GeoProperty>
{
    #region Remove with 8.0.1

    [Obsolete("Use the ctor that takes a GeoCoordinate.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public GeoProperty(double latitude, double longitude, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        : base(new ParameterSection(), group) => throw new NotImplementedException();

    #endregion

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="GenderProperty"/> instance
    /// to clone.</param>
    private GeoProperty(GeoProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="GeoProperty" /> object. </summary>
    /// <param name="value">A <see cref="GeoCoordinate" /> object.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public GeoProperty(GeoCoordinate value, string? group = null)
        : base(new ParameterSection(), group) 
        => Value = value ?? throw new ArgumentNullException(nameof(value));

    private GeoProperty(GeoCoordinate value, VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
        => Value = value;

    internal static bool TryParse(VcfRow vcfRow, [NotNullWhen(true)] out GeoProperty? geoProp)
    {
        if (GeoCoordinate.TryParse(vcfRow.Value.Span, out GeoCoordinate? geo))
        {
            geoProp = new GeoProperty(geo, vcfRow);
            return true;
        }

        geoProp = null;
        return false;
    }

    /// <summary> The data provided by the <see cref="GeoProperty" />.
    /// </summary>
    public new GeoCoordinate Value  { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

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

        if (!IsEmpty)
        {
            GeoCoordinateSerializer.AppendTo(serializer.Builder, Value, serializer.Version);
        }
    }
}
