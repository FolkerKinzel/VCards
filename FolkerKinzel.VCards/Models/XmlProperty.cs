using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Resources;
using System;
using System.Xml.Linq;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Die Klasse kapselt die Daten der vCard-Property "XML", die eingebettete XML-Daten ermöglicht.
    /// </summary>
    public sealed class XmlProperty : TextProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="XmlProperty"/>-Objekt.
        /// </summary>
        /// <param name="xmlContent">Ein <see cref="XElement"/> oder null. Das Element muss einem XML-Namespace
        /// explizit zugeordnet sein (xmlns-Attribut). Dieser Namespace darf nicht der VCARD 4.0-Namespace 
        /// "urn:ietf:params:xml:ns:vcard-4.0" sein.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property
        /// zugehören soll.</param>
        /// <exception cref="ArgumentException"><paramref name="xmlContent"/> ist keinem XML-Namespace zugeordnet - oder -
        /// <paramref name="xmlContent"/> ist dem reservierten Namespace "urn:ietf:params:xml:ns:vcard-4.0"
        /// zugeordnet.</exception>
        public XmlProperty(XElement? xmlContent, string? propertyGroup = null)
            : base(xmlContent?.ToString(), propertyGroup)
        {
            if (xmlContent is null) return;

            const string XCARD_NAMESPACE = "urn:ietf:params:xml:ns:vcard-4.0";

            if (xmlContent.Name.Namespace == XNamespace.None)
            {
                throw new ArgumentException(Res.NoNameSpace, nameof(xmlContent));
            }
            else if (xmlContent.Name.Namespace == XCARD_NAMESPACE)
            {
                throw new ArgumentException(Res.ReservedNameSpace, nameof(xmlContent));
            }
        }


        internal XmlProperty(VcfRow vcfRow, VCardDeserializationInfo info) : base(vcfRow, info, VCdVersion.V4_0) { }
    }
}
