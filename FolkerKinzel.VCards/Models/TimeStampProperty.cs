using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die vCard-Property <c>REV</c>, die einen Zeitstempel der letzten Aktualisierung der <see cref="VCard"/> darstellt.
    /// </summary>
    public sealed class TimestampProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimestampProperty"/>-Objekt, bei dem der <see cref="ParameterSection.DataType"/>-Parameter
        /// auf <see cref="VCdDataType.Timestamp"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein <see cref="DateTimeOffset"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public TimestampProperty(DateTimeOffset value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
            Parameters.DataType = VCdDataType.Timestamp;
        }


        internal TimestampProperty(VcfRow vcfRow, VCardDeserializationInfo info)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            // ein statischer DateAndOrTimeConverter kann nicht benutzt werden, da das die 
            // Threadsafety zerstören würde:
            _ = info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset value);
            Value = value;
            Parameters.DataType = VCdDataType.Timestamp;
        }


        /// <inheritdoc/>
        public new DateTimeOffset Value
        {
            get;
        }

        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;


        ///// <summary>
        ///// True, wenn das <see cref="TimestampProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value == DateTimeOffset.MinValue;

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder worker = serializer.Worker;
            worker.Clear();
            DateAndOrTimeConverter.AppendTimestampTo(worker, this.Value, serializer.Version);
            worker.Mask(serializer.Version);
            serializer.Builder.Append(worker);
        }

    }
}
