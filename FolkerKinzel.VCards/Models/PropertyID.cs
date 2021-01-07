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
    /// Kapselt Informationen, die dazu dienen, eine vCard-Property eindeutig
    /// zu identifizieren.
    /// </summary>
    public readonly struct PropertyID : IEquatable<PropertyID>
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="PropertyID"/>-<c>struct</c>
        /// mit der Nummer der vCard-Property und der Nummer des Mappings dieser 
        /// vCard-Property.
        /// </summary>
        /// <param name="propertyNumber">Nummer der vCard-Property</param>
        /// <param name="mappingNumber">Nummer des Mappings der 
        /// vCard-Property</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="propertyNumber"/> oder <paramref name="mappingNumber"/>
        /// haben den Wert 0 oder sind negativ.</exception>
        public PropertyID(int propertyNumber, int? mappingNumber = null)
        {
            if (propertyNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyNumber), Res.ValueMustBeGreaterThanZero);
            }

            if(mappingNumber.HasValue && mappingNumber.Value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(mappingNumber), Res.ValueMustBeGreaterThanZero);
            }

            PropertyNumber = propertyNumber;
            MappingNumber = mappingNumber;
        }


        internal PropertyID(string[] arr)
        {
            Debug.Assert(arr != null);
            Debug.Assert(arr.Length > 0);

            PropertyNumber = 0;
            MappingNumber = null;


            if (int.TryParse(arr[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int intVal))
            {
                PropertyNumber = intVal;
            }


            if (arr.Length == 2)
            {
                if (int.TryParse(arr[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out intVal))
                {
                    if (intVal > 0)
                    {
                        MappingNumber = intVal;
                    }
                }
            }
        }

        /// <summary>
        /// Nummer der vCard-Property
        /// </summary>
        public int PropertyNumber { get; }

        /// <summary>
        /// Nummer des Mappings der 
        /// vCard-Property oder <c>null</c>, wenn diese nicht angegeben ist.
        /// </summary>
        public int? MappingNumber { get; }

        /// <summary>
        /// <c>true</c>, wenn die <see cref="PropertyID"/>-Struct keine verwertbaren Daten enthält.
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
        /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Struct. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Struct.</returns>
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
