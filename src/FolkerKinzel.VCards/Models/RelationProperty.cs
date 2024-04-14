using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates data from vCard properties which describe relationships 
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
public abstract class RelationProperty : VCardProperty, IEnumerable<RelationProperty>
{
    private bool _isValueInitialized;
    private Relation? _value;

    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The <see cref="RelationProperty" /> instance to clone.</param>
    protected RelationProperty(RelationProperty prop) : base(prop) { }

    /// <summary> Constructor used by derived classes when parsing VCF. </summary>
    /// <param name="parameters">The <see cref="ParameterSection" /> of the 
    /// newly initialized <see cref="RelationProperty" /> object.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    protected RelationProperty(ParameterSection parameters, string? group)
        : base(parameters, group) { }

    /// <summary>Constructor used by derived classes.</summary>
    /// <param name="relationType">A single <see cref="Rel" /> value, a combination
    /// of several <see cref="Rel" /> values, or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    protected RelationProperty(Rel? relationType, string? group)
        : base(new ParameterSection(), group)
        => this.Parameters.RelationType = relationType;

    /// <summary>
    /// The data provided by the <see cref="RelationProperty"/>.
    /// </summary>
    public new Relation? Value
    {
        get
        {
            if (!_isValueInitialized)
            {
                InitializeValue();
            }

            return _value;
        }
    }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;

    /// <summary>
    /// Creates a new <see cref="RelationProperty"/> instance from an absolute 
    /// <see cref="Uri"/>.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri"/> or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization that <paramref name="uri"/> represents. 
    /// <see cref="ParameterSection.RelationType"/> of the returned instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="RelationProperty"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is neither <c>null</c> nor
    /// an absolute <see cref="Uri"/>.</exception>
    public static RelationProperty FromUri(Uri? uri,
                                           Rel? relationType = null,
                                           string? group = null)
        => uri is null
            ? FromText(null, relationType, group)
            : !uri.IsAbsoluteUri
                ? throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)), nameof(uri))
                : UuidConverter.IsUuidUri(uri.OriginalString)
                        ? new RelationUuidProperty(UuidConverter.ToGuid(uri.OriginalString))
                        : new RelationUriProperty
                          (
                            new UriProperty(uri,
                                            new ParameterSection() { RelationType = relationType },
                                            group)
                          );

    /// <summary>
    /// Creates a new <see cref="RelationProperty"/> instance from text.
    /// </summary>
    /// <param name="text">Text that represents a person or organization, e.g., the name
    /// of the person or organization, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization that the <paramref name="text"/> represents.
    /// <see cref="ParameterSection.RelationType"/> of the returned instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="RelationProperty"/> instance.</returns>
    public static RelationProperty FromText(string? text,
                                            Rel? relationType = null,
                                            string? group = null)
    {
        var prop = new TextProperty(text, group);
        prop.Parameters.RelationType = relationType;
        prop.Parameters.DataType = Data.Text;

        return new RelationTextProperty(prop);
    }

    /// <summary>
    /// Creates a new <see cref="RelationProperty"/> instance from a <c>Guid</c>.
    /// </summary>
    /// <param name="uuid">A <see cref="Guid"/> that refers to the vCard of the person
    /// or organization via its <see cref="VCard.ID"/> property (<c>UID</c>).</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization to whose vCard the <paramref name="uuid"/> refers. 
    /// <see cref="ParameterSection.RelationType"/> of the returned instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="RelationProperty"/> instance.</returns>
    /// <remarks>
    /// Normally this method will not be used in own code. Use 
    /// <see cref="FromVCard(VCard?, Rel?, string?)"/> instead if you own the 
    /// <see cref="VCard"/>. When serializing VCF the <see cref="VCard"/>s will be automatically
    /// replaced by their <see cref="Guid"/>s and when parsing VCF the <see cref="Guid"/>s will
    /// be automatically dereferenced to their corresponding <see cref="VCard"/>s if these
    /// <see cref="VCard"/>s are available.
    /// </remarks>
    public static RelationProperty FromGuid(Guid uuid,
                                            Rel? relationType = null,
                                            string? group = null)
        => new RelationUuidProperty(uuid, relationType, group);

    /// <summary>
    /// Creates a new <see cref="RelationProperty"/> object from a <see cref="VCard"/>.
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/>-object that represents a person or 
    /// organization to whom there is a relationship, or <c>null</c>.</param>
    /// <param name="relationType">Standardized description of the relationship with the
    /// person or organization that the <paramref name="vCard"/> represents.
    /// <see cref="ParameterSection.RelationType"/> of the returned instance will be
    /// set to this value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the returned <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the returned <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>A <see cref="RelationProperty"/> object that provides a copy of <paramref name="vCard"/>.
    /// </returns>
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
    public static RelationProperty FromVCard(VCard? vCard,
                                             Rel? relationType = null,
                                             string? group = null)
        => vCard is null
            ? FromText(null, relationType, group)
            // Clone vCard in order to avoid circular references:
            : new RelationVCardProperty((VCard)vCard.Clone(), relationType, group);

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
        if (string.IsNullOrWhiteSpace(vcfRow.Value) || vcfRow.Parameters.DataType == Data.Text)
        {
            return new RelationTextProperty(vcfRow, version);
        }

        vcfRow.UnMask(version);

        if (vcfRow.Value.IsUuidUri())
        {
            var relation = new RelationUuidProperty(UuidConverter.ToGuid(vcfRow.Value),
                                                    vcfRow.Parameters.RelationType,
                                                    group: vcfRow.Group);

            relation.Parameters.Assign(vcfRow.Parameters);

            return relation;
        }
        else if (Uri.TryCreate(vcfRow.Value.Trim(), UriKind.Absolute, out Uri? uri))
        {
            var relation = new RelationUriProperty
                (
                new UriProperty(uri, vcfRow.Parameters, group: vcfRow.Group)
                );

            return relation;
        }
        else
        {
            return new RelationTextProperty(vcfRow, version);
        }
    }
    

    private void InitializeValue()
    {
        _isValueInitialized = true;

        _value = this switch
        {
            RelationTextProperty tProp => tProp.IsEmpty ? null : new Relation(tProp.Value),

            // The vCard could be empty or not when initializing, but users get
            // always a reference to it to change it afterwards:
            RelationVCardProperty vcProp => new Relation(vcProp.Value),

            RelationUuidProperty guidProp => guidProp.IsEmpty ? null : new Relation(guidProp.Value),
            RelationUriProperty uriProp => new Relation(uriProp.Value),
            _ => null
        };
    }
}
