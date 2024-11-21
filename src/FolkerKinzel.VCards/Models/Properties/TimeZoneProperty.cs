using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>TZ</c>, which encapsulates the time zone
/// of the vCard.</summary>
/// <seealso cref="VCard.TimeZones"/>
/// <seealso cref="TimeZoneID"/>
public sealed class TimeZoneProperty : VCardProperty, IEnumerable<TimeZoneProperty>
{
    #region Remove with 8.0.1

    [Obsolete("Use TimeZoneIDProperty(TimeZoneID, string?) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public TimeZoneProperty(string value, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        : base(new ParameterSection(), group) => throw new NotImplementedException();

    #endregion

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="TimeZoneProperty"/> instance to clone.</param>
    private TimeZoneProperty(TimeZoneProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="TimeZoneProperty" /> object from
    /// a specified <see cref="TimeZoneID"/>. </summary>
    /// <param name="value">A <see cref="TimeZoneID" /> object or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="TimeZoneID"/>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public TimeZoneProperty(TimeZoneID value, string? group = null)
        : base(new ParameterSection(), group)
        => Value = value ?? throw new ArgumentNullException(nameof(value));

    private TimeZoneProperty(TimeZoneID value, VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
        => Value = value;

    internal static bool TryParse(VcfRow vcfRow,
                                  VCdVersion version,
                                  [NotNullWhen(true)] out TimeZoneProperty? prop)
    {
        string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMaskValue(version);

        if (TimeZoneID.TryParse(val, out TimeZoneID? tzID))
        {
            prop = new TimeZoneProperty(tzID, vcfRow);
            return true;
        }

        prop = null;
        return false;
    }

    /// <summary> The data provided by the <see cref="TimeZoneProperty" />. </summary>
    public new TimeZoneID Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new TimeZoneProperty(this);

    /// <inheritdoc />
    IEnumerator<TimeZoneProperty> IEnumerable<TimeZoneProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TimeZoneProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
        => TimeZoneIDSerializer.AppendTo(serializer.Builder,
                                         Value,
                                         serializer.Version,
                                         serializer.TimeZoneConverter,
                                         asParameter: false);
}
