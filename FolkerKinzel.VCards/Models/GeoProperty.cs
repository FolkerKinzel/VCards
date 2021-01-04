using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Runtime.CompilerServices;
using FolkerKinzel.VCards.Models.Interfaces;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property <c>GEO</c>, die eine geographische Position speichert.
    /// </summary>
    public sealed class GeoProperty : VCardProperty, IDataContainer<GeoCoordinate?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="GeoProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="GeoCoordinate"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        public GeoProperty(GeoCoordinate? value, string? propertyGroup = null)
        {
            this.Value = (value is null) || value.IsUnknown ? null : value;
            Group = propertyGroup;
        }

        internal GeoProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            GeoCoordinate? value = GeoCoordinateConverter.Parse(vcfRow.Value);
            this.Value = (value is null) || value.IsUnknown ? null : value;
        }


        /// <inheritdoc/>
        public GeoCoordinate? Value
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
            
            GeoCoordinateConverter.AppendTo(serializer.Builder, Value, serializer.Version);
        }

    }
}
