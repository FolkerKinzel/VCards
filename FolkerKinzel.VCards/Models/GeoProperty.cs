using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Runtime.CompilerServices;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property <c>GEO</c>, die eine geographische Position speichert.
    /// </summary>
    public sealed class GeoProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="GeoProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="GeoCoordinate"/>-Objekt oder <c>null</c>.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public GeoProperty(GeoCoordinate? value, string? propertyGroup = null) : base(propertyGroup) => this.Value = (value is null) || value.IsUnknown ? null : value;

        internal GeoProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            GeoCoordinate? value = GeoCoordinateConverter.Parse(vcfRow.Value);
            this.Value = (value is null) || value.IsUnknown ? null : value;
        }

        /// <summary>
        /// Die von der <see cref="GeoProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new GeoCoordinate? Value
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
            
            GeoCoordinateConverter.AppendTo(serializer.Builder, Value, serializer.Version);
        }

    }
}
