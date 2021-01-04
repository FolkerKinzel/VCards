using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using FolkerKinzel.VCards.Models.Interfaces;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property <c>TZ</c>, die die Zeitzone der vCard speichert.
    /// </summary>
    public class TimeZoneProperty : VCardProperty, IDataContainer<TimeZoneInfo?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimeZoneProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="TimeZoneInfo"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public TimeZoneProperty(TimeZoneInfo? value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
        }

        internal TimeZoneProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = TimeZoneInfoConverter.Parse(vcfRow.Value);
        }


        /// <inheritdoc/>
        public TimeZoneInfo? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;



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

    }
}
