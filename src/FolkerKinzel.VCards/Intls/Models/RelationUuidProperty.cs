using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class RelationUuidProperty : RelationProperty
{
    private RelationUuidProperty(RelationUuidProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="RelationUuidProperty" /> object.
    /// </summary>
    /// <param name="uuid">UUID of a person to whom a relationship exists. This can, for 
    /// example, be the value of the vCard property <c>UID</c> 
    /// (<see cref="VCard.ID">VCard.UniqueIdentifier</see>) of this 
    /// person's vCard.</param>
    /// <param name="relation">A single <see cref="Rel" /> value or a combination
    /// of several <see cref="Rel" /> values or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    internal RelationUuidProperty(Guid uuid,
                                  Rel? relation = null,
                                  string? group = null)
        : base(relation, group)
    {
        Parameters.DataType = Data.Uri;
        Value = uuid;
    }
    
    public new Guid Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value == Guid.Empty;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new RelationUuidProperty(this);


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.DataType = Data.Uri;
        Parameters.ContentLocation = Loc.Cid;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.AppendUuid(Value, serializer.Version);
    }
}
