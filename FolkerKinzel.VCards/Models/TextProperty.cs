using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Models.Interfaces;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert vCard-Properties, deren Inhalt aus Text besteht.
    /// </summary>
    public class TextProperty : VCardProperty, IDataContainer<string?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TextProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/> oder <c>null</c>.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public TextProperty(string? value, string? propertyGroup = null)
        {
            Value = string.IsNullOrWhiteSpace(value) ? null : value;
            Group = propertyGroup;
        }

        internal TextProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version) : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.DecodeQuotedPrintable();

            if (version != VCdVersion.V2_1)
            {
                vcfRow.UnMask(info, version);
            }

            Value = vcfRow.Value;
        }


        /// <inheritdoc/>
        public virtual string? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;



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
        }

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder builder = serializer.Builder;


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
                StringBuilder worker = serializer.Worker;

                worker.Clear().Append(Value).Mask(serializer.Version);
                builder.Append(worker);
            }



        }
    }
}
