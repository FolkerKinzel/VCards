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
using FolkerKinzel.VCards.Intls.Converters;
using System.Collections;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Daten einer vCard-Property, die Informationen über die Postanschrift enthält.
    /// </summary>
    public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>
    {
        /// <summary>
        /// Copy ctor
        /// </summary>
        /// <param name="prop">The <see cref="AddressProperty"/> object to clone.</param>
        private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

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
                               string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
        {
            Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                                locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                                postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                                region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                                country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                                postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                                extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));
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
            : base(new ParameterSection(), propertyGroup)
        {
            Value = new Address(street: ReadOnlyCollectionConverter.ToReadOnlyCollection(street),
                                locality: ReadOnlyCollectionConverter.ToReadOnlyCollection(locality),
                                postalCode: ReadOnlyCollectionConverter.ToReadOnlyCollection(postalCode),
                                region: ReadOnlyCollectionConverter.ToReadOnlyCollection(region),
                                country: ReadOnlyCollectionConverter.ToReadOnlyCollection(country),
                                postOfficeBox: ReadOnlyCollectionConverter.ToReadOnlyCollection(postOfficeBox),
                                extendedAddress: ReadOnlyCollectionConverter.ToReadOnlyCollection(extendedAddress));
        }



        internal AddressProperty(VcfRow vcfRow, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.DecodeQuotedPrintable();

            if (vcfRow.Value is null)
            {
                Value = new Address();
            }
            else
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));
                Value = new Address(vcfRow.Value, vcfRow.Info, version);
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

                _ = builder.Append(QuotedPrintableConverter.Encode(toEncode, startIndex));
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


        IEnumerator<AddressProperty> IEnumerable<AddressProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<AddressProperty>)this).GetEnumerator();


        public override object Clone() => new AddressProperty(this);
    }
}
