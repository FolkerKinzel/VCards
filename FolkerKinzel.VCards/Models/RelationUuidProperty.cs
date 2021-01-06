using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FolkerKinzel.VCards.Models.Interfaces;
using System.ComponentModel;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit der UUID ihrer <see cref="VCard"/>
    /// zu beschreiben.
    /// </summary>
    public sealed class RelationUuidProperty : RelationProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationUuidProperty"/>-Objekt.
        /// </summary>
        /// <param name="uuid">Uuid einer Person, zu der eine Beziehung besteht. Das kann zum Beispiel der Wert der 
        /// vCard-Property "UID" der vCard dieser Person sein.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public RelationUuidProperty(Guid uuid, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Uri;
            this.Value = uuid;
        }


        /// <inheritdoc/>
        public new Guid Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;


        ///// <summary>
        ///// True, wenn das <see cref="RelationUuidProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value == Guid.Empty;


        /// <summary>
        /// Uuid einer Person, zu der eine Beziehung besteht.
        /// </summary>
        [Obsolete("This property is deprecated and will be removed in the release candidate. Use Value instead!")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid Uuid => Value;
        

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            this.Parameters.DataType = VCdDataType.Uri;
            this.Parameters.ContentLocation = VCdContentLocation.ContentID;
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.AppendUuid(this.Value, serializer.Version);
        }

        
    }
}
