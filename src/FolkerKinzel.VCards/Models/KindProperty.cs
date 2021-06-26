using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die in vCard 4.0 eingeführte Property <c>KIND</c>, die die Art des Objekts beschreibt, das durch die vCard repräsentiert wird.
    /// </summary>
    public sealed class KindProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="KindProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein Member der <see cref="VCdKind"/>-Enumeration.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public KindProperty(VCdKind value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = value;


        internal KindProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = VCdKindConverter.Parse(vcfRow.Value);


        /// <summary>
        /// Die von der <see cref="KindProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new VCdKind Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        
        /// <inheritdoc/>
        public override bool IsEmpty => false;


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            _ = serializer.Builder.Append(Value.ToVCardString());
        }

    }
}
