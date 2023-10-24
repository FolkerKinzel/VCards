using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class EmbeddedTextProperty : DataProperty
{
    private readonly TextProperty _textProp;

    internal EmbeddedTextProperty(TextProperty textProp) 
       : base(textProp.Parameters,
              textProp.Group) => _textProp = textProp;

    public new string? Value => _textProp.Value;

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => _textProp.IsEmpty;

    public override string GetFileTypeExtension()
    {
        string? mime = Parameters.MediaType;
        return mime is null ? ".txt" : MimeString.ToFileTypeExtension(mime);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new EmbeddedTextProperty((TextProperty)_textProp.Clone());

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(ReferenceEquals(Parameters, _textProp.Parameters));
        _textProp.PrepareForVcfSerialization(serializer);
        Parameters.DataType = VCdDataType.Text;
    }

    internal override void AppendValue(VcfSerializer serializer)
        => _textProp.AppendValue(serializer);
}
