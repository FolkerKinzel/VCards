using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Daten der vCard-Property <c>CLASS</c>, die in vCard 3.0 die Geheimhaltungsstufe der 
    /// vCard definiert.
    /// </summary>
    public sealed class AccessProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="AccessProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein Member der <see cref="VCdAccess"/>-Enumeration.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public AccessProperty(VCdAccess value, string? propertyGroup = null) : base(propertyGroup) => Value = value;

        internal AccessProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = VCdAccessConverter.Parse(vcfRow.Value);


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            _ = serializer.Builder.Append(Value.ToVCardString());
        }

        
        /// <inheritdoc/>
        public override bool IsEmpty => false;

        /// <summary>
        /// Die von der <see cref="AccessProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new VCdAccess Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object GetVCardPropertyValue() => Value;


    }
}
