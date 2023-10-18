using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

    /// <summary> Spezialisierung der <see cref="RelationProperty" />-Klasse, um eine
    /// Person, zu der eine Beziehung besteht, mit der UUID ihrer <see cref="VCard"
    /// /> zu beschreiben. </summary>
internal sealed class RelationUuidProperty : RelationProperty
{
    /// <summary />
    /// <param name="prop" />
    private RelationUuidProperty(RelationUuidProperty prop) : base(prop)
        => Value = prop.Value;


    /// <summary> Initialisiert ein neues <see cref="RelationUuidProperty" />-Objekt.
    /// </summary>
    /// <param name="uuid">UUID einer Person, zu der eine Beziehung besteht. Das kann
    /// zum Beispiel der Wert der vCard-Property <c>UID</c> (<see cref="VCard.UniqueIdentifier">VCard.UniqueIdentifier</see>)
    /// der vCard dieser Person sein.</param>
    /// <param name="relation">A single <see cref="RelationTypes" /> value or a combination
    /// of several <see cref="RelationTypes" /> values or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    internal RelationUuidProperty(Guid uuid, RelationTypes? relation = null, string? propertyGroup = null)
        : base(relation, propertyGroup)
    {
        Parameters.DataType = VCdDataType.Uri;
        Value = uuid;
    }


    /// <summary> Die von der <see cref="RelationUuidProperty" /> zur Verfügung gestellten
    /// Daten. </summary>
    public new Guid Value
    {
        get;
    }


    /// <inheritdoc />
    public override bool IsEmpty => Value == Guid.Empty;


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

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new RelationUuidProperty(this);

}
