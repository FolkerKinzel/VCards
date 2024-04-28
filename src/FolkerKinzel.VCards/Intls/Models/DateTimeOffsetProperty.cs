using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class DateTimeOffsetProperty : DateAndOrTimeProperty
{
    private DateTimeOffsetProperty(DateTimeOffsetProperty prop)
        : base(prop) => Value = prop.Value;

    internal DateTimeOffsetProperty(DateTimeOffset value,
                                    ParameterSection parameters,
                                    string? group)
        : base(parameters, group) => Value = value;

    public new DateTimeOffset Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty
        => !DateTimeConverter.HasDate(Value) &&
           !DateTimeConverter.HasTime(Value);

    /// <inheritdoc />
    public override object Clone() => new DateTimeOffsetProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);
        Parameters.DataType = Data.DateAndOrTime;
    }

    internal override void AppendValue(VcfSerializer serializer)
        => DateTimeConverter.AppendDateAndOrTimeTo(serializer.Builder,
                                                        Value,
                                                        serializer.Version);
}
