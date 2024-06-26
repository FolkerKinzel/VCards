using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class DateTimeTextProperty : DateAndOrTimeProperty
{
    private readonly TextProperty _textProp;

    internal DateTimeTextProperty(TextProperty textProp) :
        base(textProp.Parameters, textProp.Group) => _textProp = textProp;

    public new string? Value => _textProp.Value;

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => _textProp.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new DateTimeTextProperty((TextProperty)_textProp.Clone());

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(ReferenceEquals(Parameters, _textProp.Parameters));
        _textProp.PrepareForVcfSerialization(serializer);
        Parameters.DataType = Data.Text;
    }

    internal override void AppendValue(VcfSerializer serializer)
        => _textProp.AppendValue(serializer);
}
