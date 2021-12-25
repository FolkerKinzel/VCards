using System.Collections;
using System.Text;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Repräsentiert die vCard-Property <c>ORG</c>, die Informationen über die Organisation speichert, der das vCard-Objekt zugeordnet ist.
/// </summary>
public sealed class OrganizationProperty : VCardProperty, IEnumerable<OrganizationProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private OrganizationProperty(OrganizationProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="OrganizationProperty"/>-Objekt.
    /// </summary>
    /// <param name="organizationName">Name der Organisation oder <c>null</c>.</param>
    /// <param name="organizationalUnits">Nam(en) der Unterorganisation(en) oder <c>null</c>.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public OrganizationProperty(string? organizationName,
                                IEnumerable<string?>? organizationalUnits = null,
                                string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = new Organization(organizationName, organizationalUnits);


    internal OrganizationProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Debug.Assert(vcfRow != null);

        vcfRow.DecodeQuotedPrintable();

        ValueSplitter semicolonSplitter = vcfRow.Info.SemiColonSplitter;
        StringBuilder? builder = vcfRow.Info.Builder;

        semicolonSplitter.ValueString = vcfRow.Value;
        var list = semicolonSplitter.Select(x => x.UnMask(builder, version)).ToList();

        if (list.Count != 0)
        {
            string organizationName = list[0];
            list.RemoveAt(0);

            Value = new Organization(organizationName, list);
        }
        else
        {
            Value = new Organization();
        }
    }

    /// <summary>
    /// Die von der <see cref="OrganizationProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new Organization Value
    {
        get;
    }


    /// <inheritdoc/>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    protected override object? GetVCardPropertyValue() => Value;


    /// <inheritdoc/>
    public override bool IsEmpty => Value.IsEmpty; // Value ist nie null


    [InternalProtected]
    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();

        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded)
        {
            this.Parameters.Encoding = VCdEncoding.QuotedPrintable;
            this.Parameters.Charset = VCard.DEFAULT_CHARSET;
        }
    }


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();

        Debug.Assert(serializer != null);
        Debug.Assert(Value != null); // value ist nie null

        StringBuilder builder = serializer.Builder;
        int valueStartIndex = builder.Length;


        Value.AppendVCardString(serializer);

        if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
        {
            string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
            builder.Length = valueStartIndex;

            _ = builder.Append(QuotedPrintableConverter.Encode(toEncode, valueStartIndex));
        }
    }

    IEnumerator<OrganizationProperty> IEnumerable<OrganizationProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<OrganizationProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    public override object Clone() => new OrganizationProperty(this);
}
