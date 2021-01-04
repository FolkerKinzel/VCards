using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Verbindet die <see cref="PropertyID.MappingNumber"/> einer vCard-Property mit einer
    /// <see cref="Guid"/>, die diese vCard-Property über mehrere vCards hinweg eindeutig identifiziert.
    /// </summary>
    public readonly struct PropertyIDMapping : IEquatable<PropertyIDMapping>, IDataContainer
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="PropertyIDMapping"/>-Objekt.
        /// </summary>
        /// <param name="mappingNumber">Nummer des Mappings.</param>
        /// <param name="uuid">Identifier des Mappings.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mappingNumber"/> muss größer als 0 sein.</exception>
        internal PropertyIDMapping(int mappingNumber, Guid uuid)
        {
            if(mappingNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(mappingNumber), Res.ValueMustBeGreaterThanZero);
            }
            MappingNumber = mappingNumber;
            Uuid = uuid;
        }


        /// <summary>
        /// Nummer des Mappings. (Entspricht <see cref="PropertyID.MappingNumber">PropertyID.MappingNumber</see>).
        /// </summary>
        public int MappingNumber { get; }


        /// <summary>
        /// Identifier des Mappings.
        /// </summary>
        public Guid Uuid { get; }


        ///// <summary>
        ///// True, wenn die <see cref="PropertyIDMapping"/>-Struct keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public bool IsEmpty => Uuid == Guid.Empty;

        object IDataContainer.Value => this;



        #region IEquatable

        ///// <summary>
        ///// Gibt einen Wert zurück, der angibt, ob diese Instanz gleich einem angegebenen <see cref="object"/> ist.
        ///// </summary>
        ///// <param name="obj">Ein mit dieser Instanz zu vergleichendes <see cref="object"/>.</param>
        ///// <returns><c>true</c>, wenn <paramref name="obj"/> eine <see cref="PropertyIDMapping"/>-Struktur ist, die 
        ///// über dieselben Werte wie diese Instanz verfügt, andernfalls <c>false</c>.</returns>
        //// override object.Equals
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj is PropertyIDMapping other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }


        ///// <summary>
        ///// Gibt einen Wert zurück, der angibt, ob diese Instanz gleich einer angegebenen <see cref="PropertyIDMapping"/>-Struktur ist.
        ///// </summary>
        ///// <param name="other">Eine mit dieser Instanz zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        ///// <returns><c>true</c>, wenn <paramref name="other"/> über dieselben Werte wie diese Instanz verfügt, 
        ///// andernfalls <c>false</c>.</returns>
        /// <inheritdoc/>
        public bool Equals(PropertyIDMapping other)
        {
            return this.MappingNumber == other.MappingNumber && this.Uuid == other.Uuid;
        }


        ///// <summary>
        ///// Gibt den Hashcode für diese Instanz zurück.
        ///// </summary>
        ///// <returns>Ein 32-Bit-Hashcode als ganze Zahl mit Vorzeichen.</returns>
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1;
            return (hashCode ^ MappingNumber.GetHashCode()) ^ (hashCode ^ Uuid.GetHashCode());
        }

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyID"/>-Strukturen gleich sind.
        /// </summary>
        /// <param name="pidMap1">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <param name="pidMap2">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Strukturen gleich sind, andernfalls <c>false</c>.</returns>
        public static bool operator ==(PropertyIDMapping pidMap1, PropertyIDMapping pidMap2)
        {
            return pidMap1.Equals(pidMap2);
        }

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyID"/>-Strukturen ungleich sind.
        /// </summary>
        /// <param name="pidMap1">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <param name="pidMap2">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Strukturen ungleich sind, 
        /// andernfalls <c>false</c>.</returns>
        public static bool operator !=(PropertyIDMapping pidMap1, PropertyIDMapping pidMap2)
        {
            return !pidMap1.Equals(pidMap2);
        }

        #endregion


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Struct. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Struct.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(48);
            AppendTo(sb);
            return sb.ToString();
        }

        internal void AppendTo(StringBuilder builder)
        {
            Debug.Assert(builder != null);

            builder.Append(MappingNumber.ToString(CultureInfo.InvariantCulture));
            builder.Append(';');
            builder.AppendUuid(Uuid);
        }
    }

}
