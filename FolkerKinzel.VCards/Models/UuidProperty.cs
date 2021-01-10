using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property <c>UID</c>, die einen eindeutigen Bezeichner für das vCard-Subjekt speichert.
    /// </summary>
    public sealed class UuidProperty : VCardProperty
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
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public UuidProperty(Guid uuid, string? propertyGroup = null) : base(propertyGroup) => Value = uuid;


        internal UuidProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = UuidConverter.ToGuid(vcfRow.Value);


        /// <summary>
        /// Die von der <see cref="UuidProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Guid Value
        {
            get;
        }

        
        /// <inheritdoc/>
        public override bool IsEmpty => Value == Guid.Empty;


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;



        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            _ = serializer.Builder.AppendUuid(this.Value, serializer.Version);
        }

    }
}
