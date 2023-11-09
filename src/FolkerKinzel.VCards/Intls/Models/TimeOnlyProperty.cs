using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class TimeOnlyProperty : DateAndOrTimeProperty
{
    private TimeOnlyProperty(TimeOnlyProperty prop) : base(prop) => Value = prop.Value;

    internal TimeOnlyProperty(TimeOnly value,
                            ParameterSection parameters,
                            string? group)
        : base(parameters, group) => Value = value;

    public new TimeOnly Value { get; }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new TimeOnlyProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);
        Parameters.DataType = Data.Time;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendValue(VcfSerializer serializer) =>
        TimeConverter.AppendTimeTo(serializer.Builder, Value, serializer.Version);
}
