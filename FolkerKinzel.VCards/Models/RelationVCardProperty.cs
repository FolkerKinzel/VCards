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
using FolkerKinzel.VCards.Models.Interfaces;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit ihrer <see cref="VCard"/>
    /// zu beschreiben.
    /// </summary>
    public sealed class RelationVCardProperty : RelationProperty, IDataContainer<VCard?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationVCardProperty"/>-Objekt.
        /// </summary>
        /// <param name="vcard"><see cref="VCard"/> einer Person, zu der eine Beziehung besteht.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public RelationVCardProperty(VCard? vcard, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Value = vcard;
            this.Parameters.DataType = VCdDataType.VCard;
        }


        /// <inheritdoc/>
        public VCard? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;


        /// <summary>
        /// <see cref="VCard"/> einer Person, zu der eine Beziehung besteht.
        /// </summary>
        [Obsolete("This property is deprecated and will be removed in the release candidate. Use Value instead!")]
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

                builder.AppendUuid(this.Value.UniqueIdentifier.Value);
                return;
            }


            string vc = this.Value.ToVcfString(serializer.Version, serializer.Options.Unset(VcfOptions.IncludeAgentAsSeparateVCard));

            if (serializer.Version == VCdVersion.V3_0)
            {
                Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

                worker.Clear().Append(vc).Mask(serializer.Version);

                builder.Append(worker);
            }
            else //vCard 2.1
            {
                Debug.Assert(serializer.PropertyKey == VCard.PropKeys.AGENT);

                builder.Append(VCard.NewLine).Append(vc);
            }
        }

    }
}
