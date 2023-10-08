using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

/// <summary>
/// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit der UUID ihrer <see cref="VCard"/>
/// zu beschreiben.
/// </summary>
internal sealed class RelationUuidProperty : RelationProperty
{
    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private RelationUuidProperty(RelationUuidProperty prop) : base(prop)
        => Value = prop.Value;


    /// <summary>
    /// Initialisiert ein neues <see cref="RelationUuidProperty"/>-Objekt.
    /// </summary>
    /// <param name="uuid">UUID einer Person, zu der eine Beziehung besteht. Das kann zum Beispiel der Wert der 
    /// vCard-Property <c>UID</c> (<see cref="VCard.UniqueIdentifier">VCard.UniqueIdentifier</see>) der vCard dieser Person sein.</param>
    /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    internal RelationUuidProperty(Guid uuid, RelationTypes? relation = null, string? propertyGroup = null)
        : base(relation, propertyGroup)
    {
        Parameters.DataType = VCdDataType.Uri;
        Value = uuid;
    }


    /// <summary>
    /// Die von der <see cref="RelationUuidProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new Guid Value
    {
        get;
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    /// <inheritdoc/>
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

    //IEnumerator<RelationUuidProperty> IEnumerable<RelationUuidProperty>.GetEnumerator()
    //{
    //    yield return this;
    //}

    /// <inheritdoc/>
    public override object Clone() => new RelationUuidProperty(this);

}
