using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.ComponentModel;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, 
    /// mit ihrer <see cref="VCards.VCard"/> zu beschreiben.
    /// </summary>
    public sealed class RelationVCardProperty : RelationProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationVCardProperty"/>-Objekt.
        /// </summary>
        /// <param name="vcard"><see cref="VCards.VCard"/> einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public RelationVCardProperty(VCard? vcard, RelationTypes relation, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Value = vcard;
            this.Parameters.DataType = VCdDataType.VCard;
        }


        /// <summary>
        /// Die von der <see cref="RelationVCardProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new VCard? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        /// <summary>
        /// <see cref="VCards.VCard"/> einer Person, zu der eine Beziehung besteht.
        /// </summary>
        [Obsolete(OBSOLETE_MESSAGE, OBSOLETE_AS_ERROR)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VCard? VCard => Value;


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            this.Parameters.DataType = VCdDataType.VCard;
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            if (this.Value is null)
            {
                return;
            }

            Debug.Assert(serializer != null);

            StringBuilder builder = serializer.Builder;
            StringBuilder worker = serializer.Worker;

            if (serializer.Version >= VCdVersion.V4_0)
            {
                Debug.Assert(this.Value.UniqueIdentifier != null);
                Debug.Assert(!this.Value.UniqueIdentifier.IsEmpty);

                _ = builder.AppendUuid(this.Value.UniqueIdentifier.Value);
                return;
            }


            string vc = this.Value.ToVcfString(serializer.Version, serializer.Options.Unset(VcfOptions.IncludeAgentAsSeparateVCard));

            if (serializer.Version == VCdVersion.V3_0)
            {
                Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

                _ = worker.Clear().Append(vc).Mask(serializer.Version);

                _ = builder.Append(worker);
            }
            else //vCard 2.1
            {
                Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

                _ = builder.Append(VCard.NewLine).Append(vc);
            }
        }

    }
}
