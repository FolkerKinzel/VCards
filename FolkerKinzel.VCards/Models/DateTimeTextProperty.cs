using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Spezialisierung der <see cref="DateTimeProperty"/>-Klasse, die es erlaubt eine Zeit- und/oder Datumsangabe
    /// als freien Text zu speichern.
    /// </summary>
    public class DateTimeTextProperty : DateTimeProperty, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="DateTimeTextProperty"/>-Objekt, bei dem der 
        /// <see cref="FolkerKinzel.VCards.Models.PropertyParts.ParameterSection.DataType"/>-Parameter
        /// auf <see cref="VCdDataType.Text"/> gesetzt ist.
        /// </summary>
        /// <param name="value">Ein beliebiger <see cref="string"/>.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty{T}">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty{T}">VCardProperty</see> keiner Gruppe angehört.</param>
        public DateTimeTextProperty(string? value, string? propertyGroup = null) : base(propertyGroup)
        {
            this.Text = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            Parameters.DataType = VCdDataType.Text;
        }

        /// <summary>
        /// Überschreibt <see cref="VCardProperty{T}.Value"/>. Gibt den Inhalt von <see cref="Text"/> zurück.
        /// </summary>
        public override object? Value
        {
            get => this.Text;
            //protected set => base.Value = value;
        }

        /// <summary>
        /// Die als freier Text gespeicherte Zeit- und/oder Datumsangabe.
        /// </summary>
        public string? Text { get; }


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            this.Parameters.DataType = VCdDataType.Text;
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            var builder = serializer.Builder;
            var worker = serializer.Worker;

            worker.Clear().Append(this.Text).Mask(serializer.Version);
            builder.Append(worker);
        }
    }

}
