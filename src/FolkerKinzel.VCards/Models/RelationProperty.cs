using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates data from vCard properties that describe relationships 
/// with other people. </summary>
/// <remarks>
/// vCard properties whose data is encapsulated by <see cref="RelationProperty"/> 
/// objects are in particular the vCard&#160;4.0 properties <c>RELATED</c> and <c>MEMBER</c>,
/// the vCard&#160;2.1 and vCard&#160;3.0 property <c>AGENT</c>, as well as 
/// non-standard properties for specifying the name of the spouse (such as <c>X-SPOUSE</c>).
/// </remarks>
/// <seealso cref="VCard.Relations"/>
/// <seealso cref="VCard.Members"/>
/// <seealso cref="Relation"/>
public sealed class RelationProperty : VCardProperty, IEnumerable<RelationProperty>
{
    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The <see cref="RelationProperty" /> instance to clone.</param>
    private RelationProperty(RelationProperty prop) : base(prop) => Value = prop.Value;

    /// <summary>
    /// Creates a new <see cref="RelationProperty"/> instance from a <see cref="ContactID"/>.
    /// </summary>
    /// <param name="id">A <see cref="ContactID"/> that refers to the vCard of the person
    /// or organization via its <see cref="VCard.ID"/> property (<c>UID</c>).</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization to whose vCard the <paramref name="id"/> refers. 
    /// <see cref="ParameterSection.RelationType"/> of the returned instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    public RelationProperty(ContactID id,
                            Rel? relationType = null,
                            string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = Relation.Create(id);
        Parameters.RelationType = relationType;
    }

    /// <summary>
    /// Initializes a new <see cref="RelationProperty"/> object from a specified <see cref="VCard"/> 
    /// instance.
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/>-object that represents a person or 
    /// organization to whom there is a relationship.</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization that the <paramref name="vCard"/> represents.
    /// <see cref="ParameterSection.RelationType"/> of the new instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the returned <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the returned <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <remarks>
    /// <note type="important">
    /// This constructor clones <paramref name="vCard"/> in order to avoid circular references.
    /// Changing the <paramref name="vCard"/> instance AFTER assigning it to this constructor 
    /// leads to unexpected results!
    /// </note>
    /// <para>
    /// vCard&#160;2.1 and vCard&#160;3.0 can embed nested vCards if the flag <see cref="Rel.Agent"/> is 
    /// set in their <see cref="ParameterSection.RelationType"/> property . When serializing a vCard&#160;4.0, 
    /// embedded <see cref="VCard"/>s will be automatically replaced by <see cref="Guid"/> references and
    /// appended as separate vCards to the VCF file.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\EmbeddedVCardExample.cs" />
    /// </example>
    /// <exception cref="ArgumentNullException"><paramref name="vCard"/> is <c>null</c>.</exception>
    public RelationProperty(VCard vCard, Rel? relationType = null, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = Relation.Create(vCard);
        Parameters.RelationType = relationType;
        Parameters.DataType = Data.VCard;
    }

    /// <summary>
    /// The data provided by the <see cref="RelationProperty"/>.
    /// </summary>
    public new Relation Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => object.ReferenceEquals(Value, Relation.Empty);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();

    /// <inheritdoc />
    IEnumerator<RelationProperty> IEnumerable<RelationProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<RelationProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal static RelationProperty Parse(VcfRow vcfRow, VCdVersion version)
    {
        RelationProperty prop;
        ReadOnlySpan<char> valSpan = vcfRow.Value.Span.Trim();

        if (valSpan.IsEmpty)
        {
            prop = new RelationProperty(ContactID.Empty);
            goto done;
        }

        if (UuidConverter.TryAsGuid(vcfRow.Value.Span, out Guid uuid))
        {
            prop = new RelationProperty(ContactID.Create(uuid));
            goto done;
        }

        string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? valSpan.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : valSpan.UnMaskValue(version);

        if (string.IsNullOrWhiteSpace(val))
        {
            prop = new RelationProperty(ContactID.Empty);
            goto done;
        }

        if (vcfRow.Parameters.DataType == Data.Text)
        {
            prop = new RelationProperty(ContactID.Create(val));
            goto done;
        }

        if (Uri.TryCreate(val, UriKind.Absolute, out Uri? uri))
        {
            prop = new RelationProperty(ContactID.Create(uri));
            goto done;
        }
        else
        {
            prop = new RelationProperty(ContactID.Create(val));
            goto done;
        }

done:
        prop.Parameters.Assign(vcfRow.Parameters);
        prop.Group = vcfRow.Group;
        return prop;
    }

    /// <inheritdoc/>
    public override object Clone() => new RelationProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        if (Value.IsContactID && Value.ContactID.IsString)
        {
            Parameters.DataType = Data.Text;

            if (serializer.Version == VCdVersion.V2_1 && Value.ContactID.String.NeedsToBeQpEncoded())
            {
                this.Parameters.Encoding = Enc.QuotedPrintable;
                this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
            }
        }

        if (Value.IsVCard) { Parameters.DataType = Data.VCard; }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;

        if (Value.IsContactID)
        {
            if (Value.ContactID.IsGuid)
            {
                _ = builder.AppendUuid(this.Value.ContactID.Guid.Value, serializer.Version);
                return;
            }

            if (Value.ContactID.IsString)
            {
                _ = serializer.Version == VCdVersion.V2_1
                    ? this.Parameters.Encoding == Enc.QuotedPrintable
                        ? builder.AppendQuotedPrintable(Value.ContactID.String.AsSpan(), builder.Length)
                        : builder.Append(Value.ContactID.String)
                    : builder.AppendValueMasked(Value.ContactID.String, serializer.Version);
            }

            if (Value.ContactID.IsUri)
            {
                _ = builder.Append(Value.ContactID.Uri.AbsoluteUri);
                return;
            }
        }

        if (Value.IsVCard)
        {
            Debug.Assert(serializer.Version < VCdVersion.V4_0);
            Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

            string vc = Value.VCard.ToVcfString(
                serializer.Version,
                options: serializer.Options.Unset(Opts.AppendAgentAsSeparateVCard));

            _ = serializer.Version == VCdVersion.V3_0
                ? builder.AppendValueMasked(vc, serializer.Version)
                : builder.Append(VCard.NewLine).Append(vc); // Version 2.1

            return;
        }
    }
}
