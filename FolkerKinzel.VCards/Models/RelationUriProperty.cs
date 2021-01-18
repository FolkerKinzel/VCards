using FolkerKinzel.VCards.Intls.Attributes;
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
    /// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um eine Person, zu der eine Beziehung besteht, mit einem
    /// <see cref="System.Uri"/> dieser Person zu beschreiben.
    /// </summary>
    public sealed class RelationUriProperty : RelationProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="RelationUriProperty"/>-Objekt.
        /// </summary>
        /// <param name="uri"><see cref="System.Uri"/> einer Person, zu der eine Beziehung besteht oder <c>null</c>.</param>
        /// <param name="relation">Einfacher oder kombinierter Wert der <see cref="RelationTypes"/>-Enum.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public RelationUriProperty(Uri uri, RelationTypes relation, string? propertyGroup = null)
            : base(relation, propertyGroup)
        {
            this.Parameters.DataType = VCdDataType.Uri;
            this.Value = uri;
        }


        /// <summary>
        /// Die von der <see cref="RelationUriProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Uri? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        /// <summary>
        /// <see cref="System.Uri"/> einer Person, zu der eine Beziehung besteht.
        /// </summary>
        [Obsolete(OBSOLETE_MESSAGE, OBSOLETE_AS_ERROR)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Uri? Uri => Value;


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            this.Parameters.DataType = VCdDataType.Uri;

            if (this.Value is null)
            {
                return;
            }

            if (serializer.Version == VCdVersion.V2_1)
            {
                Uri? uri = this.Value;

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

            _ = this.Parameters.Encoding == VCdEncoding.QuotedPrintable
                ? builder.Append(QuotedPrintableConverter.Encode(this.Value?.ToString(), builder.Length))
                : builder.Append(this.Value);
        }

    }
}
