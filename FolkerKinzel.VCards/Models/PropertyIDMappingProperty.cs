using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using System;
using System.Globalization;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Informationen einer CLIENTPIDMAP-Property.
    /// </summary>
    /// <remarks>
    /// Der Standard erlaubt, dass das Mapping mit einer beliebigen URI signiert wird. Unterstützt werden hier
    /// aber nur UUIDs.
    /// </remarks>
    public sealed class PropertyIDMappingProperty : VCardProperty<PropertyIDMapping>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="PropertyIDMappingProperty"/>-Objekt.
        /// </summary>
        /// <param name="mappingNumber">Nummer des Mappings.</param>
        /// <param name="uuid">Identifier des Mappings.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mappingNumber"/> muss größer als 0 sein.</exception>
        public PropertyIDMappingProperty(int mappingNumber, Guid uuid)
        {
            Value = new PropertyIDMapping(mappingNumber, uuid);
        }


        internal PropertyIDMappingProperty(VcfRow vcfRow, VCardDeserializationInfo info)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value is null)
            {
                Value = new PropertyIDMapping();
                return;
            }

            var arr = vcfRow.Value.Split(info.Semicolon, 2, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length < 2 ||
                !int.TryParse(arr[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int mappingNumber))
            {
                Value = new PropertyIDMapping();
                return;
            }

            try
            {
                Value = new PropertyIDMapping(mappingNumber, UuidConverter.ToGuid(arr[1]));
            }
            catch(ArgumentOutOfRangeException) // mappingNumber ist 0 oder negativ
            {
                Value = new PropertyIDMapping();
            }

        }


        /// <summary>
        /// True, wenn das <see cref="PropertyIDMappingProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => Value.IsEmpty;


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Value.AppendTo(serializer.Builder);
        }

    }
}
