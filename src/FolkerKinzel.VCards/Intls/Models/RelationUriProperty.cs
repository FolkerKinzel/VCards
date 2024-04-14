using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class RelationUriProperty : RelationProperty
{
    private readonly UriProperty _uriProp;

    internal RelationUriProperty(UriProperty prop)
        : base(prop.Parameters, prop.Group)
        => _uriProp = prop;

    public new Uri Value => _uriProp.Value;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone()
        => new RelationUriProperty((UriProperty)_uriProp.Clone());

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        => _uriProp.PrepareForVcfSerialization(serializer);

    internal override void AppendValue(VcfSerializer serializer)
        => _uriProp.AppendValue(serializer);
}
