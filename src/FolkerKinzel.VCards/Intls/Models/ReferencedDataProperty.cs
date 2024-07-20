using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ReferencedDataProperty : DataProperty
{
    private readonly UriProperty _uriProp;

    internal ReferencedDataProperty(UriProperty prop)
        : base(prop.Parameters, prop.Group) => _uriProp = prop;

    public new Uri Value => _uriProp.Value;

    public override string GetFileTypeExtension()
    {
        string? mime = Parameters.MediaType;

        return mime is not null ? MimeString.ToFileTypeExtension(mime)
                                : UriConverter.GetFileTypeExtensionFromUri(Value);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new ReferencedDataProperty((UriProperty)_uriProp.Clone());

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        => _uriProp.PrepareForVcfSerialization(serializer);

    internal override void AppendValue(VcfSerializer serializer)
        => _uriProp.AppendValue(serializer);
}
