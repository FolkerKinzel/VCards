using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

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
    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);
        Parameters.DataType = VCdDataType.Date;
    }

    internal override void AppendValue(VcfSerializer serializer) =>
        DateAndOrTimeConverter.AppendDateTo(serializer.Builder, Value, serializer.Version);
}
