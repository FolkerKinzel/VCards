using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class RelationUuidProperty : RelationProperty
{
    private RelationUuidProperty(RelationUuidProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="RelationUuidProperty" /> object.
    /// </summary>
    /// <param name="uuid">UUID of a person to whom a relationship exists. This can, for 
    /// example, be the value of the vCard property <c>UID</c> 
    /// (<see cref="VCard.UniqueIdentifier">VCard.UniqueIdentifier</see>) of this 
    /// person's vCard.</param>
    /// <param name="relation">A single <see cref="RelationTypes" /> value or a combination
    /// of several <see cref="RelationTypes" /> values or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    internal RelationUuidProperty(Guid uuid,
                                  RelationTypes? relation = null,
                                  string? group = null)
        : base(relation, group)
    {
        Parameters.DataType = VCdDataType.Uri;
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

        Parameters.DataType = VCdDataType.Uri;
        Parameters.ContentLocation = ContentLocation.ContentID;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.AppendUuid(Value, serializer.Version);
    }
}
