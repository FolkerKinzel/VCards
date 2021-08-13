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
        /// Copy ctor.
        /// </summary>
        /// <param name="prop"></param>
        private TimeStampProperty(TimeStampProperty prop) : base(prop)
            => Value = prop.Value;

        /// <summary>
        /// Initialisiert ein neues <see cref="TimeStampProperty"/>-Objekt, das den Zeitpunkt des 
        /// Konstruktoraufrufs als Zeitstempel kapselt.
        /// </summary>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <remarks>
        /// Der Konstruktor setzt den <see cref="ParameterSection.DataType"/>-Parameter automatisch
        /// auf den Wert <see cref="VCdDataType.TimeStamp"/>.
        /// </remarks>
        public TimeStampProperty(string? propertyGroup = null) : this(DateTimeOffset.UtcNow, propertyGroup) { }


        /// <summary>
        /// Initialisiert ein neues <see cref="TimeStampProperty"/>-Objekt mit dem angegebenen Zeitstempel.
        /// </summary>
        /// <param name="value">Ein <see cref="DateTimeOffset"/>-Objekt.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <remarks>
        /// Der Konstruktor setzt den <see cref="ParameterSection.DataType"/>-Parameter automatisch
        /// auf den Wert <see cref="VCdDataType.TimeStamp"/>.
        /// </remarks>
        public TimeStampProperty(DateTimeOffset value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
        {
            Value = value;
            Parameters.DataType = VCdDataType.TimeStamp;
        }


        internal TimeStampProperty(VcfRow vcfRow)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            // ein statischer DateAndOrTimeConverter kann nicht benutzt werden, da das die 
            // Threadsafety zerstören würde:
            _ = vcfRow.Info.DateAndOrTimeConverter.TryParse(vcfRow.Value, out DateTimeOffset value);
            Value = value;
            Parameters.DataType = VCdDataType.TimeStamp;
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


        /// <inheritdoc/>
        public override bool IsEmpty => Value < new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);

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

        public override object Clone() => new TimeStampProperty(this);
    }
}
