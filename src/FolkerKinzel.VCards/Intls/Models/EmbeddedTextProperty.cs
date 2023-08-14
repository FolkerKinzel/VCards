using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class EmbeddedTextProperty : DataProperty
{
    private readonly TextProperty _textProp;

    internal EmbeddedTextProperty(TextProperty textProp) :
        base(textProp.Parameters.MediaType, textProp.Parameters, textProp.Group)
    {
        _textProp = textProp;
    }

    internal EmbeddedTextProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters.MediaType,
               vcfRow.Parameters,
               vcfRow.Group) => _textProp = new TextProperty(vcfRow, version);

    public new string? Value => _textProp.Value;

    public override string GetFileTypeExtension()
    {
        string? mime = Parameters.MediaType;
        return mime is null ? ".txt" : MimeString.ToFileTypeExtension(mime);
    }

    public override object Clone() => new EmbeddedTextProperty((TextProperty)_textProp.Clone());


    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(ReferenceEquals(Parameters, _textProp.Parameters));
        _textProp.PrepareForVcfSerialization(serializer);
    }

    internal override void AppendValue(VcfSerializer serializer) => _textProp.AppendValue(serializer);

}
