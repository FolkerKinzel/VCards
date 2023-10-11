using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

/// <summary>
/// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, 
/// mit ihrer <see cref="VCard"/> zu beschreiben.
/// </summary>
internal sealed class RelationVCardProperty : RelationProperty
{
    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private RelationVCardProperty(RelationVCardProperty prop) : base(prop)
        => Value = (VCard)prop.Value.Clone();

    /// <summary>
    /// Initialisiert ein neues <see cref="RelationVCardProperty"/>-Objekt.
    /// </summary>
    /// <param name="vcard"><see cref="VCard"/> einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
    /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    internal RelationVCardProperty(VCard vcard, RelationTypes? relation = null, string? propertyGroup = null)
        : base(relation, propertyGroup)
    {
        Debug.Assert(vcard != null);

        Value = vcard;
        Parameters.DataType = VCdDataType.VCard;
    }


    /// <summary>
    /// Die von der <see cref="RelationVCardProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new VCard Value
    {
        get;
    }


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.DataType = VCdDataType.VCard;
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        if (Value is null)
        {
            return;
        }

        Debug.Assert(serializer != null);

        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        Debug.Assert(serializer.Version < VCdVersion.V4_0);

        string vc = Value.ToVcfString(serializer.Version, options: serializer.Options.Unset(VcfOptions.IncludeAgentAsSeparateVCard));

        if (serializer.Version == VCdVersion.V3_0)
        {
            Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

            _ = worker.Clear().Append(vc).Mask(serializer.Version);

            _ = builder.Append(worker);
        }
        else //vCard 2.1
        {
            Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

            _ = builder.Append(VCard.NewLine).Append(vc);
        }
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone()
        => new RelationVCardProperty(this);


}
