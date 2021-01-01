using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit einer
    /// <see cref="Uri"/> dieser Person zu beschreiben.
    /// </summary>
    public sealed class RelationUriProperty : RelationProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationUriProperty"/>-Objekt.
        /// </summary>
        /// <param name="uri">Uri einer Person, zu der eine Beziehung besteht.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum, der die 
        /// Beziehung beschreibt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public RelationUriProperty(Uri uri, RelationTypes? relation = null, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Uri;
            this.Uri = uri;
        }


        /// <summary>
        /// Überschreibt <see cref="VCardProperty{T}.Value"/>. Gibt den Inhalt von <see cref="Uri"/> zurück.
        /// </summary>
        public override object? Value => this.Uri;

        /// <summary>
        /// <see cref="Uri"/> einer Person, zu der eine Beziehung besteht.
        /// </summary>
        public Uri? Uri { get; }

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            this.Parameters.DataType = VCdDataType.Uri;

            if (this.Uri is null)
            {
                return;
            }

            if (serializer.Version == VCdVersion.V2_1)
            {
                Uri? uri = this.Uri;

                if (uri.IsAbsoluteUri && (uri.Scheme?.StartsWith("cid", StringComparison.Ordinal) ?? false))
                {
                    Parameters.ContentLocation = VCdContentLocation.ContentID;
                }
                else if (Parameters.ContentLocation != VCdContentLocation.ContentID)
                {
                    Parameters.ContentLocation = VCdContentLocation.Url;
                }

                if (uri.ToString().NeedsToBeQpEncoded())
                {
                    Parameters.Encoding = VCdEncoding.QuotedPrintable;
                    Parameters.Charset = VCard.DEFAULT_CHARSET;
                }
            }
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder builder = serializer.Builder;

            if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                builder.Append(QuotedPrintableConverter.Encode(this.Uri?.ToString(), builder.Length));
            }
            else
            {
                builder.Append(this.Uri);
            }
        }


    }

}
