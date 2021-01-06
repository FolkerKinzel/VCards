using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Text;
using System.Runtime.CompilerServices;
using FolkerKinzel.VCards.Models.Interfaces;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Daten einer vCard-Property, die Informationen über die Postanschrift enthält.
    /// </summary>
    public sealed class AddressProperty : VCardProperty, IDataContainer<Address>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
        /// </summary>
        /// <param name="postOfficeBox">Postfach</param>
        /// <param name="extendedAddress">Adresszusatz</param>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="region">Bundesland</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="country">Land (Staat)</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public AddressProperty(
            IEnumerable<string?>? postOfficeBox = null,
            IEnumerable<string?>? extendedAddress = null,
            IEnumerable<string?>? street = null,
            IEnumerable<string?>? locality = null,
            IEnumerable<string?>? region = null,
            IEnumerable<string?>? postalCode = null,
            IEnumerable<string?>? country = null,
            string? propertyGroup = null)
        {
            Value = new Address(postOfficeBox, extendedAddress, street, locality, region, postalCode, country);
            Group = propertyGroup;
        }

        /// <summary>
        /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
        /// </summary>
        /// <param name="postOfficeBox">Postfach</param>
        /// <param name="extendedAddress">Adresszusatz</param>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="region">Bundesland</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="country">Land (Staat)</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe, der die Property zugehören soll.</param>
        /// <remarks>
        /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
        /// eine formatierte Darstellung der Adresse zuzuweisen.
        /// </remarks>
        public AddressProperty(
            string? postOfficeBox,
            string? extendedAddress,
            string? street,
            string? locality,
            string? region,
            string? postalCode,
            string? country,
            string? propertyGroup = null)
        {
            Value = new Address(
                new string?[] { postOfficeBox }, new string?[] { extendedAddress }, new string?[] { street },
                new string?[] { locality }, new string?[] { region }, new string?[] { postalCode }, new string?[] { country });
            Group = propertyGroup;
        }

        internal AddressProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value is null)
            {
                Value = new Address();
            }
            else
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));

                vcfRow.DecodeQuotedPrintable();
                Value = new Address(vcfRow.Value, info.Builder, version);
            }
        }

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            base.PrepareForVcfSerialization(serializer);

            Debug.Assert(serializer != null);
            Debug.Assert(Value != null); // value ist nie null

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
            Debug.Assert(Value != null); // value ist nie null


            StringBuilder builder = serializer.Builder;
            int startIndex = builder.Length;

            Value.AppendVCardString(serializer);

            if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                string toEncode = builder.ToString(startIndex, builder.Length - startIndex);
                builder.Length = startIndex;

                builder.Append(QuotedPrintableConverter.Encode(toEncode, startIndex));
            }
        }

        ///// <summary>
        ///// True, wenn das <see cref="AddressProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty; // Value ist nie null


        /// <inheritdoc/>
        public new Address Value
        {
            get;
        }

        
        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object GetContainerValue() => Value;

    }
}
