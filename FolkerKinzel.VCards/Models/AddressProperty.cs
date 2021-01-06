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
        /// <param name="address">Ein <see cref="VCardAddress"/>-Objekt oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <remarks>
        /// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
        /// eine formatierte Darstellung der Adresse zuzuweisen.
        /// </remarks>
        public AddressProperty(VCardAddress? address, string? propertyGroup = null)
        {
            Value = (address is null || address.IsEmpty) ? null : address;
            Group = propertyGroup;
        }

        ///// <summary>
        ///// Initialisiert ein neues <see cref="AddressProperty"/>-Objekt.
        ///// </summary>
        ///// <param name="street">Straße</param>
        ///// <param name="locality">Ort</param>
        ///// <param name="postalCode">Postleitzahl</param>
        ///// <param name="region">Bundesland</param>
        ///// <param name="country">Land (Staat)</param>
        ///// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        ///// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        ///// <param name="propertyGroup">Bezeichner der Gruppe,
        ///// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        ///// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        ///// <remarks>
        ///// Es ist empfehlenswert, dem Parameter <see cref="ParameterSection.Label"/> des <see cref="AddressProperty"/>-Objekts
        ///// eine formatierte Darstellung der Adresse zuzuweisen.
        ///// </remarks>
        //public AddressProperty(
        //    string? street,
        //    string? locality,
        //    string? postalCode,
        //    string? region,
        //    string? country,
        //    string? postOfficeBox,
        //    string? extendedAddress,
        //    string? propertyGroup = null)
        //{
        //    Value = new VCardAddress(
        //        new string?[] { postOfficeBox }, new string?[] { extendedAddress }, new string?[] { street },
        //        new string?[] { locality }, new string?[] { region }, new string?[] { postalCode }, new string?[] { country });
        //    Group = propertyGroup;
        //}

        internal AddressProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value is null)
            {
                Value = new VCardAddress();
            }
            else
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));

                vcfRow.DecodeQuotedPrintable();
                Value = new VCardAddress(vcfRow.Value, info.Builder, version);
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

        /////// <summary>
        /////// True, wenn das <see cref="AddressProperty"/>-Objekt keine Daten enthält.
        /////// </summary>
        ///// <inheritdoc/>
        //public override bool IsEmpty => Value.IsEmpty; // Value ist nie null

        /// <summary>
        /// Die von der <see cref="AddressProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new VCardAddress? Value
        {
            get;
        }

        
        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;

    }
}
