using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um den Namen einer Person, zu der eine Beziehung besteht, anzugeben.
    /// </summary>
    public sealed class RelationTextProperty : RelationProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationTextProperty"/>-Objekt.
        /// </summary>
        /// <param name="text">Name einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public RelationTextProperty(string? text, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                this.Value = text;
            }
        }

        internal RelationTextProperty(VcfRow row, VCardDeserializationInfo info, VCdVersion version) : base(row.Parameters, row.Group)
        {
            row.DecodeQuotedPrintable();

            if (version != VCdVersion.V2_1)
            {
                row.UnMask(info, version);
            }

            this.Value = row.Value;
        }

        /// <summary>
        /// Die von der <see cref="RelationTextProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new string? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        /// <summary>
        /// Text zur Beschreibung einer Beziehung, z.B. Name der Person, zu der die Beziehung besteht.
        /// </summary>
        [Obsolete("This property is deprecated and will be removed in the release candidate. Use Value instead!")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? Text => Value;
        

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
            {
                this.Parameters.Encoding = VCdEncoding.QuotedPrintable;
                this.Parameters.Charset = VCard.DEFAULT_CHARSET;
            }

            this.Parameters.DataType = VCdDataType.Text;
        }

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder builder = serializer.Builder;
            StringBuilder worker = serializer.Worker;

            if (serializer.Version == VCdVersion.V2_1)
            {
                if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
                {
                    builder.Append(QuotedPrintableConverter.Encode(Value, builder.Length));
                }
                else
                {
                    builder.Append(Value);
                }
            }
            else
            {
                worker.Clear().Append(Value).Mask(serializer.Version);
                builder.Append(worker);
            }
        }

    }
}
