using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class DateTimeTextProperty : DateAndOrTimeProperty
{
    private readonly TextProperty _textProp;

    internal DateTimeTextProperty(TextProperty textProp) :
        base(textProp.Parameters, textProp.Group) => _textProp = textProp;

    internal DateTimeTextProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters,
               vcfRow.Group) => _textProp = new TextProperty(vcfRow, version);

    public new string? Value => _textProp.Value;

    

    public override object Clone() => new DateTimeTextProperty((TextProperty)_textProp.Clone());


    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(ReferenceEquals(Parameters, _textProp.Parameters));
        _textProp.PrepareForVcfSerialization(serializer);
    }

    internal override void AppendValue(VcfSerializer serializer) => _textProp.AppendValue(serializer);

    public override string ToString() => _textProp.ToString();

}
