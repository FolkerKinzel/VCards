using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit der UUID ihrer <see cref="VCard"/>
    /// zu beschreiben.
    /// </summary>
    public sealed class RelationUuidProperty : RelationProperty, IEnumerable<RelationUuidProperty>
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationUuidProperty"/>-Objekt.
        /// </summary>
        /// <param name="uuid">UUID einer Person, zu der eine Beziehung besteht. Das kann zum Beispiel der Wert der 
        /// vCard-Property <c>UID</c> (<see cref="VCard.UniqueIdentifier">VCard.UniqueIdentifier</see>) der vCard dieser Person sein.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public RelationUuidProperty(Guid uuid, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Uri;
            this.Value = uuid;
        }


        /// <summary>
        /// Die von der <see cref="RelationUuidProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Guid Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;

        
        /// <inheritdoc/>
        public override bool IsEmpty => Value == Guid.Empty;


        /// <summary>
        /// Uuid einer Person, zu der eine Beziehung besteht.
        /// </summary>
        [Obsolete(OBSOLETE_MESSAGE, OBSOLETE_AS_ERROR)]
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

            _ = serializer.Builder.AppendUuid(this.Value, serializer.Version);
        }

        IEnumerator<RelationUuidProperty> IEnumerable<RelationUuidProperty>.GetEnumerator()
        {
            yield return this;
        }

        //IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<RelationUuidProperty>)this).GetEnumerator();
    }
}
