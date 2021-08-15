using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert vCard-Properties, deren Inhalt aus Text besteht.
    /// </summary>
    public class TextProperty : VCardProperty, IEnumerable<TextProperty>
    {
        /// <summary>
        /// Kopierkonstruktor.
        /// </summary>
        /// <param name="prop">Das zu klonende <see cref="TextProperty"/>-Objekt.</param>
        protected TextProperty(TextProperty prop) : base(prop)
            => Value = prop.Value;

        /// <summary>
        /// Initialisiert ein neues <see cref="TextProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/> oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public TextProperty(string? value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup) => Value = string.IsNullOrWhiteSpace(value) ? null : value;


        internal TextProperty(VcfRow vcfRow, VCdVersion version) : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.UnMask(version);
            Value = vcfRow.Value;
        }


        /// <summary>
        /// Die von der <see cref="TextProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new virtual string? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;



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
                _ = this.Parameters.Encoding == VCdEncoding.QuotedPrintable
                    ? builder.Append(QuotedPrintableConverter.Encode(Value, builder.Length))
                    : builder.Append(Value);
            }
            else
            {
                StringBuilder worker = serializer.Worker;

                _ = worker.Clear().Append(Value).Mask(serializer.Version);
                _ = builder.Append(worker);
            }
        }


        IEnumerator<TextProperty> IEnumerable<TextProperty>.GetEnumerator()
        {
            yield return this;
        }


        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TextProperty>)this).GetEnumerator();

        public override object Clone() => new TextProperty(this);
    }
}
