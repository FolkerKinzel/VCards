using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

public abstract class DateAndOrTimeProperty
    : VCardProperty, IEnumerable<DateAndOrTimeProperty>
{
    private DateAndOrTime? _value;
    private bool _isValueInitialized;


    protected DateAndOrTimeProperty(DateAndOrTimeProperty prop) : base(prop) { }
   

    protected DateAndOrTimeProperty(ParameterSection parameters,
                                    string? propertyGroup) 
        : base(parameters, propertyGroup) { }


    public new DateAndOrTime? Value
    {
        get
        {
            InitializeValue();
            return _value;
        }
    }

    /// <inheritdoc/>
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;


    /// <inheritdoc/>
    protected override object? GetVCardPropertyValue() => Value;


    internal static DateAndOrTimeProperty Create(VcfRow vcfRow, VCdVersion version)
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

    public static DateAndOrTimeProperty FromDateTime(DateTimeOffset dateTime,
                                                     string? propertyGroup = null) => 
        new DateTimeOffsetProperty(dateTime,
                                   new ParameterSection() { DataType = VCdDataType.DateAndOrTime },
                                   propertyGroup);

    
    public static DateAndOrTimeProperty FromDate(int year, int month, int day, string? propertyGroup = null)
        => FromDate(new DateOnly(year, month, day), propertyGroup);

    public static DateAndOrTimeProperty FromDate(DateOnly date,
                                                 string? propertyGroup = null) =>
        new DateOnlyProperty(date,
                             new ParameterSection() { DataType = VCdDataType.Date },
                             propertyGroup);

    public static DateAndOrTimeProperty FromTime(TimeOnly time,
                                                 string? propertyGroup = null) =>
       new TimeOnlyProperty(time,
                            new ParameterSection() { DataType = VCdDataType.Time },
                            propertyGroup);

    public static DateAndOrTimeProperty FromText(string? text,
                                                 string? propertyGroup = null)
    {
        var prop = new DateTimeTextProperty(new TextProperty(text, propertyGroup));
        prop.Parameters.DataType = VCdDataType.Text;
        return prop;
    }

    IEnumerator<DateAndOrTimeProperty> IEnumerable<DateAndOrTimeProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateAndOrTimeProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();


    private void InitializeValue()
    {
        if(_isValueInitialized) 
        {
            return;
        }

        _isValueInitialized = true;

        _value = this switch
        {
            DateOnlyProperty dateOnlyProperty => new DateAndOrTime(dateOnlyProperty.Value),
            TimeOnlyProperty timeOnlyProperty => new DateAndOrTime(timeOnlyProperty.Value),
            DateTimeOffsetProperty dateTimeOffsetProperty => dateTimeOffsetProperty.IsEmpty ? null : new DateAndOrTime(dateTimeOffsetProperty.Value),
            DateTimeTextProperty dateTimeTextProperty => dateTimeTextProperty.IsEmpty ? null : new DateAndOrTime(dateTimeTextProperty.Value),
            _ => null
        };
    }
}
