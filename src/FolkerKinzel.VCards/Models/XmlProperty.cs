using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the data of the vCard property <c>XML</c>, which enables
/// embedded XML data in VCF files.</summary>
/// <seealso cref="VCard.Xmls"/>
/// <seealso cref="XElement"/>
public sealed class XmlProperty : TextProperty, IEnumerable<XmlProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop"></param>
    private XmlProperty(XmlProperty prop) : base(prop) { }

    /// <summary>Initializes a new <see cref="XmlProperty" /> object. </summary>
    /// <param name="value">A <see cref="XElement" /> or <c>null</c>. The element
    /// must be explicitly assigned to an XML namespace (xmlns attribute). This namespace
    /// must not be the VCARD 4.0 namespace <c>urn:ietf:params:xml:ns:vcard-4.0</c>!</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentException"> <paramref name="value" /> is not assigned
    /// to an XML namespace - or - <paramref name="value" /> is in the reserved
    /// namespace <c>urn:ietf:params:xml:ns:vcard-4.0</c>.</exception>
    public XmlProperty(XElement? value, string? group = null)
        : base(value?.ToString(), group)
    {
        if (value is null)
        {
            return;
        }

        const string XCARD_NAMESPACE = "urn:ietf:params:xml:ns:vcard-4.0";

        if (value.Name.Namespace == XNamespace.None)
        {
            throw new ArgumentException(Res.NoNameSpace, nameof(value));
        }
        else if (value.Name.Namespace == XCARD_NAMESPACE)
        {
            throw new ArgumentException(Res.ReservedNameSpace, nameof(value));
        }
    }

    internal XmlProperty(VcfRow vcfRow) : base(vcfRow, VCdVersion.V4_0) { }

    /// <inheritdoc />
    public override object Clone() => new XmlProperty(this);

    /// <inheritdoc />
    IEnumerator<XmlProperty> IEnumerable<XmlProperty>.GetEnumerator()
    {
        yield return this;
    }
}
