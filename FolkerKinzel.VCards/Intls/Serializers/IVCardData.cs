using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Interfaces;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers
{
    /// <summary>
    /// Interface, das von <see cref="VCardProperty"/>-Objekten implementiert wird, um den Zugriff auf die in Ihnen gespeicherten
    /// Daten zu erleichtern.
    /// </summary>
    internal interface IVCardData : IDataContainer
    {
        /// <summary>
        /// Gruppenbezeichner einer vCard-Property oder <c>null</c>, wenn die vCard-Property keinen Gruppenbezeichner hat.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Entspricht dem Parameter-Teil einer vCard-Property (nie <c>null</c>).
        /// </summary>
        public ParameterSection Parameters { get; }

        
    }
}
