using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit ihrer <see cref="VCard"/>
    /// zu beschreiben.
    /// </summary>
    public sealed class RelationVCardProperty : RelationProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein <see cref="RelationVCardProperty"/>-Objekt.
        /// </summary>
        /// <param name="vcard"><see cref="VCard"/> einer Person, zu der eine Beziehung besteht.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public RelationVCardProperty(VCard? vcard, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.VCard = vcard;
            this.Parameters.DataType = VCdDataType.VCard;
        }

        /// <summary>
        /// Überschreibt <see cref="VCardProperty{T}.Value"/>. Gibt den Inhalt von <see cref="VCard"/> zurück.
        /// </summary>
        public override object? Value
        {
            get => this.VCard;
            //protected set => base.Value = value; 
        }

        /// <summary>
        /// <see cref="VCard"/> einer Person, zu der eine Beziehung besteht.
        /// </summary>
        public VCard? VCard { get; }

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

            if (this.VCard is null) return;

            Debug.Assert(serializer != null);

            var builder = serializer.Builder;
            var worker = serializer.Worker;

            if (serializer.Version >= VCdVersion.V4_0)
            {
                Debug.Assert(this.VCard.UniqueIdentifier != null);
                Debug.Assert(!this.VCard.UniqueIdentifier.IsEmpty);

                builder.AppendUuid(this.VCard.UniqueIdentifier.Value);
                return;
            }


            string vc = this.VCard.ToVcfString(serializer.Version, serializer.Options.Unset(VcfOptions.IncludeAgentAsSeparateVCard));

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
