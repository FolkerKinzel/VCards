using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

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
    /// Creates a new <see cref="RelationProperty"/> instance from a <see cref="Relation"/>.
    /// </summary>
    /// <param name="relation">A <see cref="Relation"/> instance that identifies the person
    /// or organization with whome there is a relationship.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="relation"/> is <c>null</c>.</exception>
    public RelationProperty(Relation relation,
                            string? group = null)
        : base(new ParameterSection(), group)
        => Value = relation ?? throw new ArgumentNullException(nameof(relation));

    /// <summary>
    /// The data provided by the <see cref="RelationProperty"/>.
    /// </summary>
    public new Relation Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

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
            prop = new RelationProperty(Relation.Empty);
            goto done;
        }

        if (UuidConverter.TryAsGuid(vcfRow.Value.Span, out Guid uuid))
        {
            prop = new RelationProperty(Relation.Create(ContactID.Create(uuid)));
            goto done;
        }

        string? val = StringDeserializer.Deserialize(vcfRow, version);

        if (string.IsNullOrWhiteSpace(val))
        {
            prop = new RelationProperty(Relation.Empty);
            goto done;
        }

        if (vcfRow.Parameters.DataType == Data.Text)
        {
            prop = new RelationProperty(Relation.Create(ContactID.Create(val)));
            goto done;
        }

        if (Uri.TryCreate(val, UriKind.Absolute, out Uri? uri))
        {
            prop = new RelationProperty(Relation.Create(ContactID.Create(uri)));
            goto done;
        }
        else
        {
            prop = new RelationProperty(Relation.Create(ContactID.Create(val)));
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

        if (Value.ContactID?.String is not null)
        {
            Parameters.DataType = Data.Text;

            if (serializer.Version == VCdVersion.V2_1 && Value.ContactID.String.NeedsToBeQpEncoded())
            {
                Parameters.Encoding = Enc.QuotedPrintable;
                Parameters.CharSet = VCard.DEFAULT_CHARSET;
            }
        }

        if (Value.VCard is not null) { Parameters.DataType = Data.VCard; }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;

        if (Value.ContactID is not null)
        {
            if (Value.ContactID.Guid.HasValue)
            {
                _ = builder.AppendUuid(Value.ContactID.Guid.Value, serializer.Version);
                return;
            }

            if (Value.ContactID.String is string txt)
            {
                _ = serializer.Version == VCdVersion.V2_1
                    ? Parameters.Encoding == Enc.QuotedPrintable
                        ? builder.AppendQuotedPrintable(txt.AsSpan(), builder.Length)
                        : builder.Append(Value.ContactID.String)
                    : builder.AppendValueMasked(Value.ContactID.String, serializer.Version);
            }

            if (Value.ContactID.Uri is Uri uri)
            {
                _ = builder.Append(uri.AbsoluteUri);
                return;
            }
        }

        if (Value.VCard is VCard vCard)
        {
            Debug.Assert(serializer.Version < VCdVersion.V4_0);
            Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

            string vcf = vCard.ToVcfString(
                serializer.Version,
                options: serializer.Options.Unset(Opts.AppendAgentAsSeparateVCard));

            _ = serializer.Version == VCdVersion.V3_0
                ? builder.AppendValueMasked(vcf, serializer.Version)
                : builder.Append(VCard.NewLine).Append(vcf); // Version 2.1

            return;
        }
    }
}
