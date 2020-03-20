using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit der Uuid ihrer <see cref="VCard"/>
    /// zu beschreiben.
    /// </summary>
    public sealed class RelationUuidProperty : RelationProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein <see cref="RelationUuidProperty"/>-Objekt.
        /// </summary>
        /// <param name="uuid">Uuid einer Person, zu der eine Beziehung besteht. Das kann zum Beispiel der Wert der 
        /// vCard-Property "UID" der vCard dieser Person sein.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public RelationUuidProperty(Guid uuid, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Uri;
            this.Uuid = uuid;
        }

       

        /// <summary>
        /// Überschreibt <see cref="VCardProperty{T}.Value"/>. Gibt den Inhalt von <see cref="Uuid"/> zurück.
        /// </summary>
        public override object Value
        {
            get => this.Uuid;
            //protected set => base.Value = value; 
        }


        /// <summary>
        /// Uuid einer Person, zu der eine Beziehung besteht.
        /// </summary>
        public Guid Uuid { get; }

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

            serializer.Builder.AppendUuid(this.Uuid, serializer.Version);
        }


        /// <summary>
        /// True, wenn das <see cref="RelationUuidProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => Uuid == Guid.Empty;
    }

}
