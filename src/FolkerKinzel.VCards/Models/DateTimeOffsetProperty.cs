using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Eine von <see cref="DateTimeProperty"/> abgeleitete Klasse, die darauf spezialisiert ist <see cref="System.DateTimeOffset"/>-Werte zu speichern.
    /// </summary>
    public sealed class DateTimeOffsetProperty : DateTimeProperty, IEnumerable<DateTimeOffsetProperty>
    {
        /// <summary>
        /// Copy ctor.
        /// </summary>
        /// <param name="prop"></param>
        private DateTimeOffsetProperty(DateTimeOffsetProperty prop) : base(prop) => Value = prop.Value;

        /// <summary>
        /// Initialisiert ein neues <see cref="DateTimeOffsetProperty"/>-Objekt, bei dem der <see cref="ParameterSection.DataType"/>-Parameter
        /// auf <see cref="VCdDataType.DateAndOrTime"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein <see cref="System.DateTimeOffset"/>-Objekt oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public DateTimeOffsetProperty(DateTimeOffset? value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
        {
            this.Value = (value == System.DateTimeOffset.MinValue) ? null : value;
            Parameters.DataType = VCdDataType.DateAndOrTime;
        }


        internal DateTimeOffsetProperty(DateTimeOffset value, VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = value;
            Parameters.DataType ??= VCdDataType.DateAndOrTime;
        }

        /// <summary>
        /// Die von der <see cref="DateTimeOffsetProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new DateTimeOffset? Value
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
            base.PrepareForVcfSerialization(serializer);

            if (serializer.Version < VCdVersion.V4_0)
            {
                this.Parameters.DataType =
                    DateAndOrTimeConverter.HasTimeComponent(this.Value)
                    ? VCdDataType.DateTime : VCdDataType.Date;
            }
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            StringBuilder worker = serializer.Worker;

            _ = worker.Clear();

            DateAndOrTimeConverter.AppendDateTimeStringTo(worker, this.Value, serializer.Version);

            //serializer.Worker.Mask(serializer.Version);
            _ = serializer.Builder.Append(worker);
        }

        IEnumerator<DateTimeOffsetProperty> IEnumerable<DateTimeOffsetProperty>.GetEnumerator()
        {
            yield return this;
        }

        /// <inheritdoc/>
        public override object Clone() => new DateTimeOffsetProperty(this);

        //IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DateTimeOffsetProperty>)this).GetEnumerator();
    }
}
