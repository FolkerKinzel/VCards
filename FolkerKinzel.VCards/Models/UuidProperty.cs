using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property UID, die einen eindeutigen Bezeichner für das vCard-Subjekt speichert.
    /// </summary>
    public sealed class UuidProperty : VCardProperty<Guid>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="UuidProperty"/>-Objekt mit einer automatisch erzeugten
        /// <see cref="Guid"/>.
        /// </summary>
        public UuidProperty() : this(Guid.NewGuid()) { }

        /// <summary>
        /// Initialisiert ein neues <see cref="UuidProperty"/>-Objekt.
        /// </summary>
        /// <param name="uuid">Ein <see cref="Guid"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public UuidProperty(Guid uuid, string? propertyGroup = null)
        {
            Value = uuid;
            Group = propertyGroup;
        }

        internal UuidProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = UuidConverter.ToGuid(vcfRow.Value);
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.AppendUuid(this.Value, serializer.Version);
        }


        /// <summary>
        /// True, wenn das <see cref="UuidProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => Value == Guid.Empty;

    }
}
