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
    public sealed class TimeStampProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimeStampProperty"/>-Objekt, bei dem der <see cref="ParameterSection.DataType"/>-Parameter
        /// auf <see cref="VCdDataType.Timestamp"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein <see cref="DateTimeOffset"/>-Objekt.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public TimeStampProperty(DateTimeOffset value, string? propertyGroup = null) : base(propertyGroup)
        {
            Value = value;
            Parameters.DataType = VCdDataType.Timestamp;
        }


        internal TimeStampProperty(VcfRow vcfRow, VCardDeserializationInfo info)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            // ein statischer DateAndOrTimeConverter kann nicht benutzt werden, da das die 
            // Threadsafety zerstören würde:
            _ = info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset value);
            Value = value;
            Parameters.DataType = VCdDataType.Timestamp;
        }

        /// <summary>
        /// Die von der <see cref="TimeStampProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new DateTimeOffset Value
        {
            get;
        }

        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        ///// <summary>
        ///// <c>true</c>, wenn das <see cref="TimestampProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value == DateTimeOffset.MinValue;

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder worker = serializer.Worker;
            _ = worker.Clear();
            DateAndOrTimeConverter.AppendTimestampTo(worker, this.Value, serializer.Version);
            _ = worker.Mask(serializer.Version);
            _ = serializer.Builder.Append(worker);
        }

    }
}
