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


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Daten einer vCard-Property, die Informationen über die Postanschrift enthält.
    /// </summary>
    public sealed class AddressProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
        /// </summary>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="region">Bundesland</param>
        /// <param name="country">Land (Staat)</param>
        /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <remarks>
        /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
        /// eine formatierte Darstellung der Adresse zuzuweisen.
        /// </remarks>
        public AddressProperty(IEnumerable<string?>? street = null,
                               IEnumerable<string?>? locality = null,
                               IEnumerable<string?>? postalCode = null,
                               IEnumerable<string?>? region = null,
                               IEnumerable<string?>? country = null,
                               IEnumerable<string?>? postOfficeBox = null,
                               IEnumerable<string?>? extendedAddress = null,
                               string? propertyGroup = null) : base(propertyGroup)
        {
            Value = new Address(street: street, locality: locality, postalCode: postalCode, region: region,
                                     country: country, postOfficeBox: postOfficeBox, extendedAddress: extendedAddress);
        }


        /// <summary>
        /// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
        /// </summary>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="region">Bundesland</param>
        /// <param name="country">Land (Staat)</param>
        /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <remarks>
        /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
        /// eine formatierte Darstellung der Adresse zuzuweisen.
        /// </remarks>
        public AddressProperty(
            string? street,
            string? locality,
            string? postalCode,
            string? region = null,
            string? country = null,
            string? postOfficeBox = null,
            string? extendedAddress = null,
            string? propertyGroup = null)
            : this(street: string.IsNullOrWhiteSpace(street) ? null : new string[] { street },
                   locality: string.IsNullOrWhiteSpace(locality) ? null : new string[] { locality },
                   postalCode: string.IsNullOrWhiteSpace(postalCode) ? null : new string[] { postalCode },
                   region: string.IsNullOrWhiteSpace(region) ? null : new string[] { region },
                   country: string.IsNullOrWhiteSpace(country) ? null : new string[] { country },
                   postOfficeBox: string.IsNullOrWhiteSpace(postOfficeBox) ? null : new string[] { postOfficeBox },
                   extendedAddress: string.IsNullOrWhiteSpace(extendedAddress) ? null : new string[] { extendedAddress },
                   propertyGroup: propertyGroup)
        {

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


        /// <summary>
        /// Die von der <see cref="AddressProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Address Value
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty;

        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;

    }
}
