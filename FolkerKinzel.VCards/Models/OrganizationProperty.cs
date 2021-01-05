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
    /// Repräsentiert die vCard-Property <c>ORG</c>, die Informationen über die Organisation speichert, der das vCard-Objekt zugeordnet ist.
    /// </summary>
    public sealed class OrganizationProperty : VCardProperty, IDataContainer<Organization>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="OrganizationProperty"/>-Objekt.
        /// </summary>
        /// <param name="organizationName">Name der Organisation.</param>
        /// <param name="organizationalUnits">Namen der Unterorganisationen.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public OrganizationProperty(string? organizationName, IEnumerable<string?>? organizationalUnits = null, string? propertyGroup = null)
        {
            Value = new Organization(organizationName, organizationalUnits);
            Group = propertyGroup;
        }

        internal OrganizationProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            Debug.Assert(vcfRow != null);

            vcfRow.DecodeQuotedPrintable();

            Value = new Organization(vcfRow.Value, info.Builder, version);
        }

        /// <inheritdoc/>
        public Organization Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;



        ///// <summary>
        ///// True, wenn das <see cref="OrganizationProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value.IsEmpty; // Value ist nie null



        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            base.PrepareForVcfSerialization(serializer);

            Debug.Assert(serializer != null);
            Debug.Assert(Value != null); // value ist nie null

            if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded)
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

                builder.Append(QuotedPrintableConverter.Encode(toEncode, valueStartIndex));
            }
        }

    }
}
