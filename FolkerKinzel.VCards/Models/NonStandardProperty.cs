using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using FolkerKinzel.VCards.Models.Interfaces;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert eine vCard-Property, die nicht von den offiziellen Standards definiert ist.
    /// </summary>
    public sealed class NonStandardProperty : VCardProperty, IVCardData, IDataContainer<string?>, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="NonStandardProperty"/>-Objekt, das eine benutzerdefinierte
        /// Erweiterung des vCard-Standards darstellt.
        /// </summary>
        /// <param name="value">Der Wert der Property (Text oder BASE64-codierte Bytes).</param>
        /// <param name="propertyKey">Der Schlüssel (Format X-Name).</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty">VCardProperty</see> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty">VCardProperty</see> keiner Gruppe angehört.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyKey"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyKey"/> ist kein X-Name.</exception>
        public NonStandardProperty(string propertyKey, string? value, string? propertyGroup = null)
        {
            if (propertyKey == null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (propertyKey.Length < 3
                || !propertyKey.StartsWith("X-", StringComparison.OrdinalIgnoreCase)
#if NET40
                || propertyKey.Contains(" ")
#else
                || propertyKey.Contains(' ', StringComparison.Ordinal)
#endif
                )
            {
                throw new ArgumentException(
                    Res.NoXName, nameof(propertyKey));
            }

            PropertyKey = propertyKey;
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

        /// <inheritdoc/>
        public string? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetContainerValue() => Value;




        /////// <summary>
        /////// True, wenn das <see cref="NonStandardProperty"/>-Objekt keine Daten enthält.
        /////// </summary>
        /////  <inheritdoc/>
        //public override bool IsEmpty => false;


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

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

       
    }
}
