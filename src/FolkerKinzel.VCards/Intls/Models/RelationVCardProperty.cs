using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class RelationVCardProperty : RelationProperty
{
    private RelationVCardProperty(RelationVCardProperty prop) : base(prop)
        => Value = (VCard)prop.Value.Clone();

    /// <summary> Initializes a new <see cref="RelationVCardProperty" /> object.
    /// </summary>
    /// <param name="vcard">The <see cref="VCard" /> of a person, with whom there is
    /// a relationship, or <c>null</c>.</param>
    /// <param name="relation">A single <see cref="RelationTypes" /> value or a combination
    /// of several <see cref="RelationTypes" /> values or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="important">
    /// This constructor clones <paramref name="vcard"/> in order to avoid circular references.
    /// Changing the <paramref name="vcard"/> instance AFTER assigning it to this constructor 
    /// leads to unexpected results!
    /// </note>
    /// </remarks>
    internal RelationVCardProperty(
        VCard vcard, RelationTypes? relation = null, string? propertyGroup = null)
        : base(relation, propertyGroup)
    {
        Debug.Assert(vcard != null);

        Value = vcard;
        Parameters.DataType = VCdDataType.VCard;
    }

    public new VCard Value
    {
        get;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone()
        => new RelationVCardProperty(this);


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.DataType = VCdDataType.VCard;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        Debug.Assert(Value != null);
        Debug.Assert(serializer.Version < VCdVersion.V4_0);

        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        string vc = Value.ToVcfString(serializer.Version, 
                                      options: serializer.Options.Unset(VcfOptions.AppendAgentAsSeparateVCard));

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
}
