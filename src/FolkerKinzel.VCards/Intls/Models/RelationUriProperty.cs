using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

    /// <summary> Spezialisierung der <see cref="RelationProperty" />-Klasse, um eine
    /// Person, zu der eine Beziehung besteht, mit einem <see cref="Uri" /> dieser Person
    /// zu beschreiben. </summary>
internal sealed class RelationUriProperty : RelationProperty
{
    private readonly UriProperty _uriProp;

    /// <summary />
    /// <param name="prop" />
    internal RelationUriProperty(UriProperty prop) : base(prop.Parameters, prop.Group)
        => _uriProp = prop;


    /// <summary> Die von der <see cref="RelationUriProperty" /> zur Verfügung gestellten
    /// Daten. </summary>
    public new Uri Value => _uriProp.Value;


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        => _uriProp.PrepareForVcfSerialization(serializer);


    internal override void AppendValue(VcfSerializer serializer)
        => _uriProp.AppendValue(serializer);


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new RelationUriProperty((UriProperty)_uriProp.Clone());

}
