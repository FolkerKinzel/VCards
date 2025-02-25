using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates the vCard property <c>REV</c>, which represents 
/// the timestamp of the last update of the <see cref="VCard" />.</summary>
/// <see cref="VCard.Updated"/>
public sealed class TimeStampProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="TimeStampProperty"/> instance to clone.</param>
    private TimeStampProperty(TimeStampProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="TimeStampProperty" /> object that 
    /// encapsulates the time of its constructor call as a UTC time stamp.</summary>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>The constructor sets the <see cref="ParameterSection.DataType" /> parameter 
    /// to the value <see cref="Data.TimeStamp" />.</remarks>
    internal TimeStampProperty(string? group = null)
        : this(DateTimeOffset.UtcNow, group) { }

    /// <summary>  Initializes a new <see cref="TimeStampProperty" /> object with
    /// the specified time stamp. </summary>
    /// <param name="value">The <see cref="DateTimeOffset" /> value to embed.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks> The constructor sets the <see cref="ParameterSection.DataType" /> parameter 
    /// to the value <see cref="Data.TimeStamp" />. </remarks>
    public TimeStampProperty(DateTimeOffset value, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = value;
        Parameters.DataType = Data.TimeStamp;
    }

    internal TimeStampProperty(VcfRow vcfRow, VcfDeserializationInfo info)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (info.TimeStampConverter.TryParse(vcfRow.Value.Span.Trim(), out DateTimeOffset value))
        {
            Value = value;
            Parameters.DataType = Data.TimeStamp;
        }
    }

    /// <summary> The data provided by the <see cref="TimeStampProperty" />. </summary>
    public new DateTimeOffset Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value < new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <inheritdoc />
    public override string ToString() => IsEmpty ? base.ToString() : Value.ToString("u");

    /// <inheritdoc />
    public override object Clone() => new TimeStampProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        TimeStampConverter.AppendTo(serializer.Builder, Value, serializer.Version);
    }
}
