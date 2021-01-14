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
    /// Repräsentiert die vCard-Property <c>N</c>, die den Namen des vCard-Subjekts speichert.
    /// </summary>
    public sealed class NameProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="NameProperty"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public NameProperty(
            IEnumerable<string?>? lastName = null,
            IEnumerable<string?>? firstName = null,
            IEnumerable<string?>? middleName = null,
            IEnumerable<string?>? prefix = null,
            IEnumerable<string?>? suffix = null,
            string? propertyGroup = null) : base(propertyGroup) => Value = new Name(lastName, firstName, middleName, prefix, suffix);


        /// <summary>
        /// Initialisiert ein neues <see cref="NameProperty"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public NameProperty(
            string? lastName,
            string? firstName = null,
            string? middleName = null,
            string? prefix = null,
            string? suffix = null,
            string? propertyGroup = null) : base(propertyGroup)
        {
            Value = new Name(
                lastName is null ? null : new string?[] { lastName },
                firstName is null ? null : new string?[] { firstName },
                middleName is null ? null : new string?[] { middleName },
                prefix is null ? null : new string?[] { prefix },
                suffix is null ? null : new string?[] { suffix });
        }


        internal NameProperty(VcfRow vcfRow, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value == null)
            {
                Value = new Name();
            }
            else
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));

                vcfRow.DecodeQuotedPrintable();

                Value = new Name(vcfRow.Value, vcfRow.Info.Builder, version);
            }
        }

        /// <summary>
        /// Die von der <see cref="NameProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Name Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        
        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty;


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
            int valueStartIndex = builder.Length;


            Value.AppendVCardString(serializer);

            if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
                builder.Length = valueStartIndex;

                _ = builder.Append(QuotedPrintableConverter.Encode(toEncode, valueStartIndex));
            }
        }

    }
}
