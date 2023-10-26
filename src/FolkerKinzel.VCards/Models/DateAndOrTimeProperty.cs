using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the information of vCard properties that provides
/// information about a date and/or a time.</summary>
/// <seealso cref="VCard.BirthDayViews"/>
/// <seealso cref="VCard.AnniversaryViews"/>
/// <seealso cref="VCard.DeathDateViews"/>
public abstract class DateAndOrTimeProperty
    : VCardProperty, IEnumerable<DateAndOrTimeProperty>
{
    private DateAndOrTime? _value;
    private bool _isValueInitialized;

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="DateAndOrTimeProperty"/> instance 
    /// to clone.</param>
    protected DateAndOrTimeProperty(DateAndOrTimeProperty prop) : base(prop) { }

    /// <summary>
    /// Constructor used by derived classes.
    /// </summary>
    /// <param name="parameters">The <see cref="ParameterSection"/>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    protected DateAndOrTimeProperty(ParameterSection parameters,
                                    string? propertyGroup)
        : base(parameters, propertyGroup) { }

    /// <summary>
    /// The data provided by the <see cref="DateAndOrTimeProperty"/>.
    /// </summary>
    public new DateAndOrTime? Value
    {
        get
        {
            InitializeValue();
            return _value;
        }
    }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a recurring date in the
    /// Gregorian calendar.
    /// </summary>
    /// <param name="month">The month (1 bis 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month"/>).
    /// (A leap year may be assumed.)</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    /// <remarks>
    /// This overload is intended to be used for recurring dates, like, e.g., birthdays, or 
    /// if the year is unknown.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="month"/> is less than 1 or greater than 12.</para>
    /// <para>-or-</para>
    /// <para><paramref name="day"/> is less than 1 or greater than the number of days 
    /// that <paramref name="month"/> has in a leap year.</para>
    /// </exception>
    public static DateAndOrTimeProperty FromDate(int month, int day, string? propertyGroup = null)
        => FromDate(new DateOnly(DateAndOrTimeConverter.FIRST_LEAP_YEAR, month, day), propertyGroup);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a date in the Gregorian
    /// calendar.
    /// </summary>
    /// <param name="year">The year (1 bis 9999).</param>
    /// <param name="month">The month (1 bis 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month"/>).</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="year"/> is less than 1 or greater than 9999.</para>
    /// <para>-or-</para>
    /// <para><paramref name="month"/> is less than 1 or greater than 12.</para>
    /// <para>-or-</para>
    /// <para><paramref name="day"/> is less than 1 or greater than the number of days in 
    /// <paramref name="month"/>.</para>
    /// </exception>
    public static DateAndOrTimeProperty FromDate(int year, int month, int day, string? propertyGroup = null)
        => FromDate(new DateOnly(year, month, day), propertyGroup);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a <see cref="DateOnly"/> 
    /// value.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly"/> value.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    public static DateAndOrTimeProperty FromDate(DateOnly date,
                                                 string? propertyGroup = null)
        => new DateOnlyProperty(date,
                                new ParameterSection() { DataType = VCdDataType.Date },
                                propertyGroup);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a <see cref="System.DateTime"/> or
    /// <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="dateTime">A <see cref="System.DateTime"/> or <see cref="DateTimeOffset"/> value.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    /// <remarks><see cref="System.DateTime"/> has an implicit conversion to <see cref="DateTimeOffset"/>.
    /// </remarks>
    public static DateAndOrTimeProperty FromDateTime(DateTimeOffset dateTime,
                                                     string? propertyGroup = null)
        => new DateTimeOffsetProperty(dateTime,
                                      new ParameterSection() { DataType = VCdDataType.DateAndOrTime },
                                      propertyGroup);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a time.
    /// </summary>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="hour"/> is less than 0 or greater than 23.</para>
    /// <para>-or-</para>
    /// <para><paramref name="minute"/> is less than 0 or greater than 59.</para>
    /// </exception>
    public static DateAndOrTimeProperty FromTime(int hour, int minute,
                                                 string? propertyGroup = null)
        => FromTime(new TimeOnly(hour, minute), propertyGroup);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTimeProperty"/> instance from a 
    /// <see cref="TimeOnly"/> value.
    /// </summary>
    /// <param name="time">A <see cref="TimeOnly"/> value.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    public static DateAndOrTimeProperty FromTime(TimeOnly time,
                                                 string? propertyGroup = null)
        => new TimeOnlyProperty(time,
                                new ParameterSection() { DataType = VCdDataType.Time },
                                propertyGroup);

    /// <summary> Creates a new <see cref="DateAndOrTimeProperty"/> instance from text. </summary>
    /// <param name="text">Any text or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DateAndOrTimeProperty"/> instance.</returns>
    /// <example>
    /// <code language="none">
    /// After midnight.
    /// </code>
    /// </example>
    public static DateAndOrTimeProperty FromText(string? text,
                                                 string? propertyGroup = null)
    {
        var prop = new DateTimeTextProperty(new TextProperty(text, propertyGroup));
        prop.Parameters.DataType = VCdDataType.Text;
        return prop;
    }

    /// <inheritdoc />
    IEnumerator<DateAndOrTimeProperty> IEnumerable<DateAndOrTimeProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateAndOrTimeProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();

    /// <inheritdoc />
    protected override object? GetVCardPropertyValue() => Value;


    internal static DateAndOrTimeProperty Parse(VcfRow vcfRow, VCdVersion version)
    {
        Debug.Assert(vcfRow != null);

        var dataType = vcfRow.Parameters.DataType;

        if (dataType == VCdDataType.Text)
        {
            return new DateTimeTextProperty(vcfRow, version);
        }

        ReadOnlySpan<char> valueSpan = vcfRow.Value.AsSpan().Trim();

        return dataType == VCdDataType.Time || valueSpan.StartsWith('T')
            ? vcfRow.Info.TimeConverter.TryParse(valueSpan, out OneOf<TimeOnly, DateTimeOffset> oneOf1)
                ? oneOf1.Match<DateAndOrTimeProperty>
                    (
                    timeOnly => new TimeOnlyProperty(timeOnly, vcfRow.Parameters, vcfRow.Group),
                    dateTimeOffset => new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Parameters, vcfRow.Group)
                    )
                : new DateTimeTextProperty(new TextProperty(vcfRow, version))
            : vcfRow.Info.DateAndOrTimeConverter.TryParse(valueSpan, out OneOf<DateOnly, DateTimeOffset> oneOf2)
                ? oneOf2.Match<DateAndOrTimeProperty>
                   (
                    dateOnly => new DateOnlyProperty(dateOnly, vcfRow.Parameters, vcfRow.Group),
                    dateTimeOffset => new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Parameters, vcfRow.Group)
                   )
                : new DateTimeTextProperty(new TextProperty(vcfRow, version));
    }


    private void InitializeValue()
    {
        if (_isValueInitialized)
        {
            return;
        }

        _isValueInitialized = true;

        _value = this switch
        {
            DateOnlyProperty dateOnlyProperty => new DateAndOrTime(dateOnlyProperty.Value),
            TimeOnlyProperty timeOnlyProperty => new DateAndOrTime(timeOnlyProperty.Value),
            DateTimeOffsetProperty dateTimeOffsetProperty
                                        => dateTimeOffsetProperty.IsEmpty ? null 
                                                                          : new DateAndOrTime(dateTimeOffsetProperty.Value),
            DateTimeTextProperty dateTimeTextProperty 
                                        => dateTimeTextProperty.IsEmpty ? null 
                                                                        : new DateAndOrTime(dateTimeTextProperty.Value),
            _ => null
        };
    }
}
