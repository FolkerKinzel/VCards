using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates the information of vCard properties that provides
/// information about a date and/or a time.</summary>
/// <seealso cref="VCard.BirthDayViews"/>
/// <seealso cref="VCard.AnniversaryViews"/>
/// <seealso cref="VCard.DeathDateViews"/>
public sealed class DateAndOrTimeProperty
    : VCardProperty, IEnumerable<DateAndOrTimeProperty>
{
    #region Remove with 8.0.1

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromDate(DateOnly date,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                                 string? group = null)
        => throw new NotImplementedException();

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromDateTime(DateTimeOffset dateTime,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                                     string? group = null)
        => throw new NotImplementedException();

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromTime(TimeOnly time,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                                 string? group = null)
        => throw new NotImplementedException();

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromText(string? text,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                                 string? group = null)
        => throw new NotImplementedException();

    [Obsolete("Use the ctor and DateAndOrTime.Create(int, int) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromDate(int month, int day, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
       => throw new NotImplementedException();

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromDate(int year, int month, int day, string? group = null)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
       => throw new NotImplementedException();

    [Obsolete("Use the ctor instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTimeProperty FromTime(int hour, int minute,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                                 string? group = null)
        => throw new NotImplementedException();

    #endregion

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="DateAndOrTimeProperty"/> instance 
    /// to clone.</param>
    private DateAndOrTimeProperty(DateAndOrTimeProperty prop)
        : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initializes a new <see cref="DateAndOrTimeProperty"/> instance with
    /// a specified <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The <see cref="DateAndOrTime"/> instance to use as 
    /// <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// 
    /// <remarks>
    /// <para><see cref="DateAndOrTime"/> supports implicit conversion.</para>
    /// <para>The following data types can be passed directly:</para>
    /// <list type="bullet">
    /// <item><see cref="DateOnly"/></item>
    /// <item><see cref="System.DateTime"/></item>
    /// <item><see cref="DateTimeOffset"/></item>
    /// <item><see cref="TimeOnly"/></item>
    /// <item><see cref="string"/></item>
    /// </list>
    /// </remarks>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public DateAndOrTimeProperty(DateAndOrTime value, string? group = null)
        : base(new ParameterSection(), group)
        => Value = value ?? throw new ArgumentNullException(nameof(value));

    internal DateAndOrTimeProperty(VcfRow vcfRow, VCdVersion version, VcfDeserializationInfo info)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Debug.Assert(vcfRow is not null);

        Data? dataType = Parameters.DataType;

        if (dataType == Data.Text)
        {
            Value = StringDeserializer.Deserialize(vcfRow, version);
            return;
        }

        ReadOnlySpan<char> valueSpan = vcfRow.Value.Span.Trim();

        Value = dataType == Data.Time || valueSpan.StartsWith('T')
            ? info.TimeConverter.TryParse(valueSpan, out DateAndOrTime? dateAndOrTime)
                ? dateAndOrTime
                : StringDeserializer.Deserialize(vcfRow, version)
            : info.DateAndOrTimeConverter.TryParse(valueSpan, out dateAndOrTime)
                ? dateAndOrTime
                : StringDeserializer.Deserialize(vcfRow, version);
    }

    /// <summary>
    /// The data provided by the <see cref="DateAndOrTimeProperty"/>.
    /// </summary>
    public new DateAndOrTime Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    IEnumerator<DateAndOrTimeProperty> IEnumerable<DateAndOrTimeProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateAndOrTimeProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new DateAndOrTimeProperty(this);

    /// <inheritdoc />
    protected override object GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        if (IsEmpty)
        {
            Parameters.DataType = null;
            return;
        }

        if (Value.DateOnly.HasValue)
        {
            Parameters.DataType = Data.Date;
        }
        else if (Value.DateTimeOffset.HasValue)
        {
            Parameters.DataType = Data.DateAndOrTime;
        }
        else if (Value.TimeOnly.HasValue)
        {
            Parameters.DataType = Data.Time;
        }
        else // Value is string
        {
            StringSerializer.Prepare(Value.String, this, serializer.Version);
            Parameters.DataType = Data.Text;
        }
    }

    /// <inheritdoc/>
    internal override void AppendValue(VcfSerializer serializer)
    {
        if (IsEmpty)
        {
            return;
        }

        if (Value.DateOnly.HasValue)
        {
            DateAndOrTimeConverter.AppendDateTo(serializer.Builder,
                                           Value.DateOnly.Value,
                                           serializer.Version);
        }
        else if (Value.DateTimeOffset.HasValue)
        {
            DateAndOrTimeConverter.AppendDateTimeOffsetTo(serializer.Builder,
                                                     Value.DateTimeOffset.Value,
                                                     serializer.Version);
        }
        else if (Value.TimeOnly.HasValue)
        {
            TimeConverter.AppendTimeTo(serializer.Builder, Value.TimeOnly.Value, serializer.Version);
        }
        else // Value is string
        {
            StringSerializer.AppendVcf(serializer.Builder, Value.String, Parameters, serializer.Version);
        }
    }
}
