using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard property <c>ORG</c>, which stores information
/// about the organization to which the <see cref="VCard"/> object is associated.</summary>
/// <seealso cref="VCard.Organizations"/>
/// <seealso cref="Organization"/>
public sealed class OrganizationProperty : VCardProperty, IEnumerable<OrganizationProperty>
{
    /// <summary>
    /// <summary>Copy ctor.</summary>
    /// </summary>
    /// <param name="prop">The <see cref="OrganizationProperty"/> instance to clone.</param>
    private OrganizationProperty(OrganizationProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="OrganizationProperty" /> object.
    /// </summary>
    /// <param name="organizationName">Organization name or <c>null</c>.</param>
    /// <param name="organizationalUnits">Organization unit(s) or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public OrganizationProperty(string? organizationName,
                                IEnumerable<string?>? organizationalUnits = null,
                                string? group = null) : base(new ParameterSection(), group)
    {
        var list = new List<string>() { organizationName ?? "" };
        list.AddRange(organizationalUnits?.WhereNotNull() ?? Array.Empty<string>());
        Value = new Organization(list);
    }

    internal OrganizationProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Debug.Assert(vcfRow != null);

        vcfRow.DecodeQuotedPrintable();

        ValueSplitter semicolonSplitter = vcfRow.Info.SemiColonSplitter;
        StringBuilder? builder = vcfRow.Info.Builder;

        semicolonSplitter.ValueString = vcfRow.Value;
        var list = semicolonSplitter.Select(x => x.UnMask(builder, version)).ToList();
        Value = new Organization(list);
    }

    /// <summary> The data provided by the  <see cref="OrganizationProperty" />. </summary>
    public new Organization Value
    {
        get;
    }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new OrganizationProperty(this);

    /// <inheritdoc />
    IEnumerator<OrganizationProperty> IEnumerable<OrganizationProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<OrganizationProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer != null);

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = ValueEncoding.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder builder = serializer.Builder;
        int valueStartIndex = builder.Length;

        Value.AppendVCardStringTo(serializer);

        if (Parameters.Encoding == ValueEncoding.QuotedPrintable)
        {
            string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
            builder.Length = valueStartIndex;

            _ = builder.Append(QuotedPrintable.Encode(toEncode, valueStartIndex));
        }
    }
}
