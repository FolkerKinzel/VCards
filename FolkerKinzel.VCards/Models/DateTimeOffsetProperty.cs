using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Eine von <see cref="DateTimeProperty"/> abgeleitete Klasse, die darauf spezialisiert ist <see cref="DateTimeOffset"/>-Werte zu speichern.
    /// </summary>
    public class DateTimeOffsetProperty : DateTimeProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="DateTimeOffsetProperty"/>-Objekt, bei dem der PropertyValue-Parameter
        /// auf <see cref="VCdDataType.DateAndOrTime"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein <see cref="DateTimeOffset"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public DateTimeOffsetProperty(DateTimeOffset value, string? propertyGroup = null) : base(propertyGroup)
        {
            this.DateTimeOffset = value;
            Parameters.DataType = VCdDataType.DateAndOrTime;
        }

        /// <summary>
        /// Überschreibt <see cref="VCardProperty{T}.Value"/>. Gibt den Inhalt von <see cref="DateTimeOffset"/> zurück.
        /// </summary>
        public override object? Value => this.DateTimeOffset == DateTimeOffset.MinValue ? null : (object)this.DateTimeOffset;

        /// <summary>
        /// Der gespeicherte <see cref="DateTimeOffset"/>-Wert.
        /// </summary>
        public DateTimeOffset DateTimeOffset { get; }


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            base.PrepareForVcfSerialization(serializer);

            if (serializer.Version < VCdVersion.V4_0)
            {
                this.Parameters.DataType =
                    DateAndOrTimeConverter.HasTimeComponent(this.DateTimeOffset)
                    ? VCdDataType.DateTime : VCdDataType.Date;
            }
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder worker = serializer.Worker;

            worker.Clear();

            DateAndOrTimeConverter.AppendDateTimeStringTo(worker, this.DateTimeOffset, serializer.Version);

            //serializer.Worker.Mask(serializer.Version);
            serializer.Builder.Append(worker);
        }
    }


}
