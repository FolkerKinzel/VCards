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


    protected DateAndOrTimeProperty(VCardProperty prop) : base(prop) { }
   

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

    public static DateAndOrTimeProperty Create(DateTimeOffset value,
                                               string? propertyGroup = null) => 
        new DateTimeOffsetProperty(value,
                                   new ParameterSection() { DataType = VCdDataType.DateAndOrTime },
                                   propertyGroup);

    public static DateAndOrTimeProperty Create(DateOnly value,
                                               string? propertyGroup = null) =>
        new DateOnlyProperty(value,
                             new ParameterSection() { DataType = VCdDataType.Date },
                             propertyGroup);

    public static DateAndOrTimeProperty Create(TimeOnly value,
                                              string? propertyGroup = null) =>
       new TimeOnlyProperty(value,
                            new ParameterSection() { DataType = VCdDataType.Time },
                            propertyGroup);

    public static DateAndOrTimeProperty Create(string? value,
                                               string? propertyGroup = null)
    {
        var prop = new DateTimeTextProperty(new TextProperty(value, propertyGroup));
        prop.Parameters.DataType = VCdDataType.Text;
        return prop;
    }

    IEnumerator<DateAndOrTimeProperty> IEnumerable<DateAndOrTimeProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateAndOrTimeProperty>)this).GetEnumerator();


    private void InitializeValue()
    {
        if(_isValueInitialized) 
        {
            return;
        }

        _isValueInitialized = true;

        _value = GetVCardPropertyValue() switch
        {
            DateOnly dateOnly => new DateAndOrTime(dateOnly),
            TimeOnly timeOnly => new DateAndOrTime(timeOnly),
            DateTimeOffset dateTimeOffset => new DateAndOrTime(dateTimeOffset),
            string s => new DateAndOrTime(s),
            _ => null
        };
    }
}
