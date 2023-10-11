using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

/// <summary>
/// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit einem
/// <see cref="Uri"/> dieser Person zu beschreiben.
/// </summary>
internal sealed class RelationUriProperty : RelationProperty
{
    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private RelationUriProperty(RelationUriProperty prop) : base(prop)
        => Value = prop.Value;


    /// <summary>
    /// Initialisiert ein neues <see cref="RelationUriProperty"/>-Objekt.
    /// </summary>
    /// <param name="uri"><see cref="Uri"/> einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
    /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    internal RelationUriProperty(Uri uri, RelationTypes? relation, string? propertyGroup)
        : base(relation, propertyGroup)
    {
        Debug.Assert(uri != null);
        Debug.Assert(uri.IsAbsoluteUri);

        Parameters.DataType = VCdDataType.Uri;
        Value = uri;
    }


    /// <summary>
    /// Die von der <see cref="RelationUriProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new Uri Value
    {
        get;
    }


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.DataType = VCdDataType.Uri;

        if (serializer.Version == VCdVersion.V2_1)
        {
            Uri uri = Value;

            if (uri.IsContentId())
            {
                Parameters.ContentLocation = ContentLocation.ContentID;
            }
            else if (Parameters.ContentLocation != ContentLocation.ContentID)
            {
                Parameters.ContentLocation = ContentLocation.Url;
            }
        }
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        _ = serializer.Builder.Append(Value.AbsoluteUri);
    }
    

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new RelationUriProperty(this);


}
