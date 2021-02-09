using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property <c>TZ</c>, die die Zeitzone der vCard speichert.
    /// </summary>
    public sealed class TimeZoneProperty : VCardProperty, IEnumerable<TimeZoneProperty>
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimeZoneProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="TimeZoneInfo"/>-Objekt oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public TimeZoneProperty(TimeZoneInfo? value, string? propertyGroup = null) : base(propertyGroup) => Value = value;


        internal TimeZoneProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = TimeZoneInfoConverter.Parse(vcfRow.Value);


        /// <summary>
        /// Die von der <see cref="TimeZoneProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new TimeZoneInfo? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;



        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            if (Value != null)
            {
                TimeZoneInfoConverter.AppendTo(serializer.Builder, Value, serializer.Version);
            }
        }

        IEnumerator<TimeZoneProperty> IEnumerable<TimeZoneProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TimeZoneProperty>)this).GetEnumerator();
    }
}
