using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die vCard-Property REV, die einen Zeitstempel der letzten Aktualisierung der <see cref="VCard"/> darstellt.
    /// </summary>
    public sealed class TimestampProperty : VCardProperty<DateTimeOffset>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimestampProperty"/>-Objekt, bei dem der PropertyValue-Parameter
        /// auf <see cref="VCdDataType.Timestamp"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein <see cref="DateTimeOffset"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public TimestampProperty(DateTimeOffset value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
            Parameters.DataType = VCdDataType.Timestamp;
        }


        internal TimestampProperty(VcfRow vcfRow, VCardDeserializationInfo info)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            // ein statischer DateAndOrTimeConverter kann nicht benutzt werden, da die die 
            // Threadsafety zerstören würde:
            _ = info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset value);
            Value = value;
            Parameters.DataType = VCdDataType.Timestamp;
        }



        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            var worker = serializer.Worker;
            worker.Clear();
            DateAndOrTimeConverter.AppendTimestampTo(worker, this.Value, serializer.Version);
            worker.Mask(serializer.Version);
            serializer.Builder.Append(worker);
        }

        ///// <summary>
        ///// True, wenn das <see cref="TimestampProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value == DateTimeOffset.MinValue;
    }
}
