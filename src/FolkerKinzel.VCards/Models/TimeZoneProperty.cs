using System.Collections;
using FolkerKinzel.VCards.Enums;
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

    /// <summary>  Initializes a new <see cref="TimeZoneProperty" /> object from
    /// a specified <see cref="TimeZoneID"/>. </summary>
    /// <param name="value">A <see cref="TimeZoneID" /> object or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <seealso cref="TimeZoneID"/>
    public TimeZoneProperty(TimeZoneID? value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;

    /// <summary>  Initializes a new <see cref="TimeZoneProperty" /> object from
    /// a specified <see cref="string"/>, which represents an identifier
    /// from the "IANA Time Zone Database".
    /// (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .) </summary>
    /// <param name="value">A <see cref="string"/> that represents an identifier
    /// from the "IANA Time Zone Database".</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// 
    /// <remarks>
    /// This constructor initializes a new <see cref="TimeZoneID"/> instance. Use the overload
    /// <see cref="TimeZoneProperty(TimeZoneID?, string?)"/> to reuse an existing one.
    /// </remarks>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="value" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"> <paramref name="value" /> is an empty
    /// <see cref="string" /> or consists only of white space characters.</exception>
    public TimeZoneProperty(string value, string? group = null)
        : base(new ParameterSection(), group) => Value = TimeZoneID.Parse(value);


    internal TimeZoneProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecode(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMask(version);

        if (TimeZoneID.TryParse(val, out TimeZoneID? tzID))
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
        Debug.Assert(serializer is not null);

        Value?.AppendTo(serializer.Builder, serializer.Version, serializer.TimeZoneConverter);
    }
}
