using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>TZ</c>, which encapsulates the time zone
/// of the vCard.</summary>
/// <seealso cref="VCard.TimeZones"/>
/// <seealso cref="TimeZoneID"/>
public sealed class TimeZoneProperty : VCardProperty, IEnumerable<TimeZoneProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="TimeZoneProperty"/> instance to clone.</param>
    private TimeZoneProperty(TimeZoneProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="TimeZoneProperty" /> object. </summary>
    /// <param name="value">A <see cref="TimeZoneInfo" /> object or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public TimeZoneProperty(TimeZoneID? value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;


    internal TimeZoneProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.UnMask(version);

        if (TimeZoneID.TryParse(vcfRow.Value, out TimeZoneID? tzID))
        {
            Value = tzID;
        }
    }

    /// <summary> The data provided by the <see cref="TimeZoneProperty" />. </summary>
    public new TimeZoneID? Value
    {
        get;
    }

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
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        Value?.AppendTo(serializer.Builder, serializer.Version, serializer.TimeZoneConverter);
    }
}
