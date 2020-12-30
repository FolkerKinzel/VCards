using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property GEO, die eine geographische Position speichert.
    /// </summary>
    public sealed class GeoProperty : VCardProperty<GeoCoordinate?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="GeoProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="GeoCoordinate"/>-Objekt.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public GeoProperty(GeoCoordinate? value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
        }

        internal GeoProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = GeoCoordinateConverter.Parse(vcfRow.Value);
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            if (Value != null)
            {
                GeoCoordinateConverter.AppendTo(serializer.Builder, Value, serializer.Version);
            }
        }


        ///// <summary>
        ///// True, wenn das <see cref="GeoProperty"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public override bool IsEmpty => Value is null || Value.IsUnknown;
    }
}
