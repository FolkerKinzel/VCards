using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class DateTimeOffsetProperty : DateAndOrTimeProperty
{
    private DateTimeOffsetProperty(DateTimeOffsetProperty prop)
        : base(prop) => Value = prop.Value;

    internal DateTimeOffsetProperty(DateTimeOffset value,
                                    ParameterSection parameters,
                                    string? propertyGroup)
        : base(parameters, propertyGroup) => Value = value;

    public new DateTimeOffset Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty
        => !DateAndOrTimeConverter.HasDate(Value) && 
           !DateAndOrTimeConverter.HasTime(Value);

    /// <inheritdoc />
    public override object Clone() => new DateTimeOffsetProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);
        Parameters.DataType = VCdDataType.DateAndOrTime;
    }

    internal override void AppendValue(VcfSerializer serializer) 
        => DateAndOrTimeConverter.AppendDateAndOrTimeTo(serializer.Builder,
                                                        Value,
                                                        serializer.Version);
}
