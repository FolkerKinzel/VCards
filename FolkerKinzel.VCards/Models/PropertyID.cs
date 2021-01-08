using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt Informationen, die dazu dienen, eine Instanz einer <see cref="VCardProperty"/> eindeutig
    /// zu identifizieren.
    /// </summary>
    public readonly struct PropertyID : IEquatable<PropertyID>
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="PropertyID"/>-Struktur
        /// mit der Nummer der <see cref="VCardProperty"/> und der Nummer des Mappings dieser 
        /// <see cref="VCardProperty"/>.
        /// </summary>
        /// <param name="propertyNumber">Nummer der <see cref="VCardProperty"/> (Wert: zwischen 1 und 9).</param>
        /// <param name="mappingNumber">Nummer des Mappings der 
        /// <see cref="VCardProperty"/> (Wert: zwischen 1 und 9) oder <c>null</c>, um kein Mapping anzugeben.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="propertyNumber"/> und/oder <paramref name="mappingNumber"/>
        /// sind kleiner als 1 oder größer als 9.</exception>
        public PropertyID(int propertyNumber, int? mappingNumber = null)
        {
            if (propertyNumber < 1 || propertyNumber > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyNumber), Res.PidValue);
            }

            if(mappingNumber.HasValue && (mappingNumber.Value < 1 || mappingNumber.Value > 9))
            {
                throw new ArgumentOutOfRangeException(nameof(mappingNumber), Res.PidValue);
            }

            PropertyNumber = propertyNumber;
            MappingNumber = mappingNumber;
        }


        internal static PropertyID Create(string[] arr)
        {
            Debug.Assert(arr != null);
            Debug.Assert(arr.Length > 0);

            return new PropertyID(
                int.Parse(arr[0], NumberStyles.Integer, CultureInfo.InvariantCulture),
                (arr.Length == 2) ? int.Parse(arr[1], NumberStyles.Integer, CultureInfo.InvariantCulture) : (int?)null);
        }


        /// <summary>
        /// Gibt die Nummer der <see cref="VCardProperty"/> zurück.
        /// </summary>
        public int PropertyNumber { get; }

        /// <summary>
        /// Gibt die Nummer des Mappings der 
        /// <see cref="VCardProperty"/> zurück oder <c>null</c>, wenn kein Mapping angegeben ist.
        /// </summary>
        public int? MappingNumber { get; }

        /// <summary>
        /// <c>true</c>, wenn die Instanz der <see cref="PropertyID"/>-Struktur keine verwertbaren Daten enthält.
        /// </summary>
        public bool IsEmpty => PropertyNumber < 1;



        #region IEquatable

        ///// <summary>
        ///// Gibt einen Wert zurück, der angibt, ob diese Instanz gleich einem angegebenen <see cref="object"/> ist.
        ///// </summary>
        ///// <param name="obj">Ein mit dieser Instanz zu vergleichendes <see cref="object"/>.</param>
        ///// <returns><c>true</c>, wenn <paramref name="obj"/> eine <see cref="PropertyID"/>-Struktur ist, die 
        ///// über dieselben Werte wie diese Instanz verfügt, andernfalls <c>false</c>.</returns>
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if ((obj is PropertyID other))
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        ///// <summary>
        ///// Gibt einen Wert zurück, der angibt, ob diese Instanz gleich einer angegebenen <see cref="PropertyID"/>-Struktur ist.
        ///// </summary>
        ///// <param name="other">Eine mit dieser Instanz zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        ///// <returns><c>true</c>, wenn <paramref name="other"/> über dieselben Werte wie diese Instanz verfügt, andernfalls <c>false</c>.</returns>
        /// <inheritdoc/>
        public bool Equals(PropertyID other) => PropertyNumber == other.PropertyNumber && MappingNumber == other.MappingNumber;


        ///// <summary>
        ///// Gibt den Hashcode für diese Instanz zurück.
        ///// </summary>
        ///// <returns>Ein 32-Bit-Hashcode als ganze Zahl mit Vorzeichen.</returns>
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1;
            return MappingNumber.HasValue ? (hashCode ^ PropertyNumber.GetHashCode()) ^ (hashCode ^ MappingNumber.GetHashCode()) : hashCode ^ PropertyNumber.GetHashCode();
        }


        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden <see cref="PropertyID"/>-Strukturen gleich sind.
        /// </summary>
        /// <param name="pid1">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <param name="pid2">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Strukturen gleich sind, andernfalls <c>false</c>.</returns>
        public static bool operator ==(PropertyID pid1, PropertyID pid2)
        {
            return pid1.Equals(pid2);
        }

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyID"/>-Strukturen ungleich sind.
        /// </summary>
        /// <param name="pid1">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <param name="pid2">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Strukturen ungleich sind, 
        /// andernfalls <c>false</c>.</returns>
        public static bool operator !=(PropertyID pid1, PropertyID pid2)
        {
            return !pid1.Equals(pid2);
        }

        #endregion

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Struktur. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Struktur.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(5);
            AppendTo(sb);
            return sb.ToString();
        }

        internal void AppendTo(StringBuilder builder)
        {
            Debug.Assert(builder != null);

            builder.Append(PropertyNumber.ToString(CultureInfo.InvariantCulture));

            if (MappingNumber.HasValue)
            {
                builder.Append('.');
                builder.Append(MappingNumber.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

    }
}
