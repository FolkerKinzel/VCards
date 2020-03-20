using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property TZ, die die Zeitzone der vCard speichert.
    /// </summary>
    public class TimeZoneProperty : VCardProperty<TimeZoneInfo?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TimeZoneProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="TimeZoneInfo"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public TimeZoneProperty(TimeZoneInfo? value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
        }

        internal TimeZoneProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = TimeZoneInfoConverter.Parse(vcfRow.Value);
        }


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
