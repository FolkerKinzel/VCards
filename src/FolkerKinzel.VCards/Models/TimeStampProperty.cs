using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the vCard property <c>REV</c>, which represents 
/// the timestamp of the last update of the <see cref="VCard" />.</summary>
/// <see cref="VCard.TimeStamp"/>
public sealed class TimeStampProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="TimeStampProperty"/> instance to clone.</param>
    private TimeStampProperty(TimeStampProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="TimeStampProperty" /> object that 
    /// encapsulates the time of its constructor call as a UTC time stamp.</summary>
    /// <remarks>The constructor sets the <see cref="ParameterSection.DataType" /> parameter 
    /// to the value <see cref="Data.TimeStamp" />.</remarks>
    public TimeStampProperty()
        : this(DateTimeOffset.UtcNow) { }


    /// <summary>  Initializes a new <see cref="TimeStampProperty" /> object with
    /// the specified time stamp. </summary>
    /// <param name="value">The <see cref="DateTimeOffset" /> value to embed.</param>
    /// <remarks> The constructor sets the <see cref="ParameterSection.DataType" /> parameter 
    /// to the value <see cref="Data.TimeStamp" />. </remarks>
    public TimeStampProperty(DateTimeOffset value)
        : base(new ParameterSection(), null)
    {
        Value = value;
        Parameters.DataType = Data.TimeStamp;
    }

    internal TimeStampProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        // A static DateAndOrTimeConverter can't be used because it would
        // destroy the thread safety:
        if (vcfRow.Info.DateAndOrTimeConverter.TryParse(vcfRow.Value.AsSpan().Trim(), out OneOf<DateOnly, DateTimeOffset> value))
        {
            Value = value.Match<DateTimeOffset>(
                dateOnly => new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local)),
                dateTimeOffset => dateTimeOffset);
            Parameters.DataType = Data.TimeStamp;
        }
    }

    /// <summary> The data provided by the <see cref="TimeStampProperty" />. </summary>
    public new DateTimeOffset Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value < new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <inheritdoc />
    public override object Clone() => new TimeStampProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder worker = serializer.Worker;
        _ = worker.Clear();
        DateTimeConverter.AppendTimeStampTo(worker, this.Value, serializer.Version);
        _ = worker.Mask(serializer.Version);
        _ = serializer.Builder.Append(worker);
    }
}
