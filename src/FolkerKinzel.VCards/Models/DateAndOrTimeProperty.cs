using System.Collections;
using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;


internal sealed class DateOnlyProperty : DateAndOrTimeProperty
{
    private DateOnlyProperty(DateOnlyProperty prop) : base(prop) => Value = prop.Value;

    internal DateOnlyProperty(DateOnly value,
                              ParameterSection parameters,
                              string? propertyGroup)
        : base(parameters, propertyGroup) => Value = value;


    public new DateOnly Value { get; }

    public override bool IsEmpty => false;


    public override object Clone() => new DateOnlyProperty(this);
    protected override object? GetVCardPropertyValue() => Value;
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

internal sealed class TimeOnlyProperty : DateAndOrTimeProperty
{
    public TimeOnlyProperty(TimeOnlyProperty prop) : base(prop) => Value = prop.Value;

    public TimeOnlyProperty(TimeOnly value,
                            ParameterSection parameters,
                            string? propertyGroup)
        : base(parameters, propertyGroup) => Value = value;


    public new TimeOnly Value { get; }

    public override bool IsEmpty => false;


    public override object Clone() => new TimeOnlyProperty(this);
    protected override object? GetVCardPropertyValue() => Value;
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();

}



internal class DateTimeOffsetProperty : DateAndOrTimeProperty
{
    internal DateTimeOffsetProperty(DateTimeOffsetProperty prop)
        : base(prop) => Value = prop.Value;


    internal DateTimeOffsetProperty(DateTimeOffset value,
                                    ParameterSection parameters,
                                    string? propertyGroup)
        : base(parameters, propertyGroup) => Value = value;


    public new DateTimeOffset Value { get; }

    public override bool IsEmpty => false;


    public override object Clone() => new DateTimeOffsetProperty(this);
    protected override object? GetVCardPropertyValue() => Value;
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}


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
            if (!_isValueInitialized)
            {
                InitializeValue();
            }

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
                : new DateTimeTextProperty(new TextProperty(null, vcfRow.Group))
            : vcfRow.Info.DateAndOrTimeConverter.TryParse(valueSpan, out OneOf<DateOnly, DateTimeOffset> oneOf2)
                ? oneOf2.Match<DateAndOrTimeProperty>
                   (
                    dateOnly => new DateOnlyProperty(dateOnly, vcfRow.Parameters, vcfRow.Group),
                    dateTimeOffset => new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Parameters, vcfRow.Group)
                   )
                : new DateTimeTextProperty(new TextProperty(null, vcfRow.Group));
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
                                               string? propertyGroup = null) =>
       new DateTimeTextProperty(new TextProperty(value, propertyGroup));


    IEnumerator<DateAndOrTimeProperty> IEnumerable<DateAndOrTimeProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateAndOrTimeProperty>)this).GetEnumerator();


    private void InitializeValue()
    {
        _isValueInitialized = true;
        _value = GetVCardPropertyValue() switch
        {
            DateOnly dateOnly => dateOnly,
            TimeOnly timeOnly => timeOnly,
            DateTimeOffset dateTimeOffset => dateTimeOffset,
            string s => s,
            _ => null
        };
    }
}
