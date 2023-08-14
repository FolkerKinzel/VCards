using System.Collections;
using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using OneOf;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

[GenerateOneOf]
public sealed partial class DateAndOrTime : OneOfBase<DateOnly, DateTime, DateTimeOffset, TimeOnly, string> { }


internal sealed class DateOnlyProperty : DateAndOrTimeProperty
{
    public DateOnlyProperty(VCardProperty prop) : base(prop)
    {
    }

    public DateOnlyProperty(DateOnly value, ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup)
    {
    }

    public override object Clone() => throw new NotImplementedException();
    protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

internal sealed class TimeOnlyProperty : DateAndOrTimeProperty
{
    public TimeOnlyProperty(VCardProperty prop) : base(prop)
    {
    }

    public TimeOnlyProperty(TimeOnly value,
                            ParameterSection parameters,
                            string? propertyGroup)
        : base(parameters, propertyGroup) => Value = value;


    public new TimeOnly Value { get; }

    public override object Clone() => throw new NotImplementedException();
    protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

internal sealed class DateTimeProperty : DateAndOrTimeProperty
{
    public DateTimeProperty(VCardProperty prop) : base(prop)
    {
    }

    public DateTimeProperty(DateTimeOffset value, ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup)
    {
    }

    public override object Clone() => throw new NotImplementedException();
    protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}


internal class DateTimeOffsetProperty : DateAndOrTimeProperty
{
    internal DateTimeOffsetProperty(VCardProperty prop) : base(prop)
    {
    }

    public DateTimeOffsetProperty(DateTimeOffset value, string? group = null) : base(new ParameterSection(), group) => Value = value;

    internal DateTimeOffsetProperty(DateTimeOffset value, ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup)
    {
    }

    public new DateTimeOffset Value { get; }

    public override object Clone() => throw new NotImplementedException();
    protected override object? GetVCardPropertyValue() => throw new NotImplementedException();
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
}

public abstract class DateAndOrTimeProperty
    : VCardProperty, IEnumerable<DateAndOrTimeProperty>
{
    private DateAndOrTime? _value;
    private bool _isValueInitialized;

    protected DateAndOrTimeProperty(VCardProperty prop) : base(prop)
    {
    }

    protected DateAndOrTimeProperty(ParameterSection parameters, string? propertyGroup) : base(parameters, propertyGroup)
    {
    }


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

        // Time
        if (dataType == VCdDataType.Time || valueSpan.StartsWith('T'))
        {
            if(vcfRow.Info.TimeConverter.TryParse(valueSpan, out OneOf<TimeOnly, DateTimeOffset> oneOf1))
            {
                return oneOf1.Match<DateAndOrTimeProperty>(
                    timeOnly => new TimeOnlyProperty(timeOnly, vcfRow.Parameters, vcfRow.Group),
                    dateTimeOffset => new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Parameters, vcfRow.Group));
            }

            return new DateTimeTextProperty(vcfRow, version);
        }

        if(vcfRow.Info.DateAndOrTimeConverter.TryParse(valueSpan, out OneOf<DateOnly, DateTime, DateTimeOffset> oneOf2))
        {
            return oneOf2.Match<DateAndOrTimeProperty>(
                dateOnly => new DateOnlyProperty(dateOnly, vcfRow.Parameters, vcfRow.Group),
                dateTime => new DateTimeProperty(dateTime, vcfRow.Parameters, vcfRow.Group),
                dateTimeOffset => new DateTimeOffsetProperty(dateTimeOffset, vcfRow.Parameters, vcfRow.Group));
        }

        return new DateTimeTextProperty(vcfRow, version);
    }

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

            _ => throw new NotImplementedException()
        };
    }
}
