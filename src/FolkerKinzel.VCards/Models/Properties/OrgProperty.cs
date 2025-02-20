using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>ORG</c>, which stores information
/// about the organization to which the <see cref="VCard"/> object is associated.</summary>
/// <seealso cref="VCard.Organizations"/>
/// <seealso cref="Organization"/>
public sealed class OrgProperty : VCardProperty, IEnumerable<OrgProperty>
{
    /// <summary>
    /// <summary>Copy ctor.</summary>
    /// </summary>
    /// <param name="prop">The <see cref="OrgProperty"/> instance to clone.</param>
    private OrgProperty(OrgProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initializes a new <see cref="OrgProperty" /> instance with an existing
    /// <see cref="Organization"/> object.
    /// </summary>
    /// <param name="value">The <see cref="Organization"/> instance to use as the new 
    /// <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty" /> objects,
    /// which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public OrgProperty(Organization value, string? group = null)
        : base(new ParameterSection(), group) => Value = value ?? throw new ArgumentNullException(nameof(value));

    internal OrgProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        ReadOnlyMemory<char> val = vcfRow.Value;

        if (Parameters.Encoding == Enc.QuotedPrintable)
        {
            val = QuotedPrintable.Decode(
                    val.Span,
                    TextEncodingConverter.GetEncoding(Parameters.CharSet)).AsMemory(); // null-check not needed
        }

        var orgList = new List<string>(
            PropertyValueSplitter.Split(val, ';', version: version));

        Debug.Assert(orgList.Count > 0);

        string orgName = orgList[0];
        orgList.RemoveAt(0);
        Value = new Organization(orgName, orgList);
    }

    /// <summary> The data provided by the  <see cref="OrgProperty" />. </summary>
    public new Organization Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new OrgProperty(this);

    /// <inheritdoc />
    IEnumerator<OrgProperty> IEnumerable<OrgProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<OrgProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer is not null);

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            Parameters.Encoding = Enc.QuotedPrintable;
            Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendValue(VcfSerializer serializer)
        => Value.AppendVCardStringTo(serializer);
}
