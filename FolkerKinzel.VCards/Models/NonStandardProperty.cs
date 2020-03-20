using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert eine vCard-Property, die nicht von den offiziellen Standards definiert ist.
    /// </summary>
    public sealed class NonStandardProperty : VCardProperty<string?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="NonStandardProperty"/>-Objekt, das eine benutzerdefinierte
        /// Erweiterung des vCard-Standards darstellt.
        /// </summary>
        /// <param name="value">Der Wert der Property (Text oder BASE64-codierte Bytes).</param>
        /// <param name="propertyKey">Der Schlüssel (Format X-Name).</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyKey"/> ist null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyKey"/> ist kein X-Name.</exception>
        public NonStandardProperty(string propertyKey, string? value, string? propertyGroup = null)
        {
            if (propertyKey == null) throw new ArgumentNullException(nameof(propertyKey));

            PropertyKey = propertyKey.Trim().ToUpperInvariant();

            if (PropertyKey.Length < 3 || !PropertyKey.StartsWith("X-", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    Res.NoXName, nameof(propertyKey));
            }

            Value = value;
            Group = propertyGroup;
        }

        internal NonStandardProperty(VcfRow vcfRow)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            PropertyKey = vcfRow.Key;
            Value = vcfRow.Value;
        }


        /// <summary>
        /// Bezeichner der vCard-Property.
        /// </summary>
        public string PropertyKey { get; }


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Key:   ").Append(PropertyKey).Append(Environment.NewLine);
            sb.Append("Value: ").Append(Value);

            return sb.ToString();
        }

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            // darf die Basisklassen-Implementation nicht aufrufen!
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.Append(Value);
        }

        /// <summary>
        /// True, wenn das <see cref="NonStandardProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => false;
    }
}
