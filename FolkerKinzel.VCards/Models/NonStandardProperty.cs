using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert eine vCard-Property, die nicht von den offiziellen Standards definiert ist.
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// <para>Um Objekte der Klasse <see cref="NonStandardProperty"/> in eine vCard zu schreiben, muss beim Schreibvorgang das Flag
    /// <see cref="VcfOptions.WriteNonStandardProperties">VcfOptions.WriteNonStandardProperties</see> explizit gesetzt werden.</para>
    /// <para>Bedenken Sie, dass Sie sich bei Verwendung der Klasse um die standardgerechte Maskierung, Demaskierung, Enkodierung und Dekodierung der 
    /// Daten selbst kümmern müssen.</para>
    /// </note>
    /// </remarks>
    public sealed class NonStandardProperty : VCardProperty
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="NonStandardProperty"/>-Objekt, das eine benutzerdefinierte
        /// Erweiterung des vCard-Standards darstellt.
        /// </summary>
        /// <param name="propertyKey">Der Schlüssel (Format: <c>X-Name</c>).</param>
        /// <param name="value">Der Wert der Property (beliebige als <see cref="string"/> kodierte Daten oder <c>null</c>).</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyKey"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyKey"/> ist kein X-Name.</exception>
        public NonStandardProperty(string propertyKey, string? value, string? propertyGroup = null) : base(propertyGroup)
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
        /// Die von der <see cref="NonStandardProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new string? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="NonStandardProperty"/>-Objekts.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            _ = sb.Append("Key:   ").AppendLine(PropertyKey);
            _ = sb.Append("Value: ").Append(Value);

            return sb.ToString();
        }

        [InternalProtected]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Ausdruckskörper für Methoden verwenden", Justification = "<Ausstehend>")]
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

            _ = serializer.Builder.Append(Value);
        }

    }
}
