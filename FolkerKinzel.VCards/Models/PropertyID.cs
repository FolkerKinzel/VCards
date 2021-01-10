using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Intls.Serializers;
using System.Collections.Generic;
using FolkerKinzel.VCards.Intls.Deserializers;

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


        internal static void ParseInto(List<PropertyID> list, string pids)
        {
            if (pids.Length == 0)
            {
                return;
            }
            
            int index = 0;

            int propertyNumber = 0;
            int? mappingNumber = null;
            bool parseMapping = false;

            
            while (index < pids.Length)
            {
                char c = pids[index++];

                if (c == ',')
                {
                    try
                    {
                        list.Add(new PropertyID(propertyNumber, mappingNumber));
                    }
                    catch (ArgumentOutOfRangeException) { }

                    propertyNumber = 0;
                    mappingNumber = null;
                    parseMapping = false;
                }
                else if (c == '.')
                {
                    parseMapping = true;
                }
                else if (char.IsDigit(c))
                {
                    if (parseMapping)
                    {
                        // Exception bei mehrstelligen Nummern:
                        mappingNumber = mappingNumber.HasValue ? 0 : DigitParser.Parse(c);
                    }
                    else
                    {
                        // Exception bei mehrstelligen Nummern:
                        propertyNumber = propertyNumber == 0 ? DigitParser.Parse(c) : 0;
                    }
                }//else
            }//while

            // if vermeidet unnötige Exception, falls der letzte Wert (standardungerecht)
            // mit einem Komma endet
            if (propertyNumber != 0)
            {
                try
                {
                    list.Add(new PropertyID(propertyNumber, mappingNumber));
                }
                catch (ArgumentOutOfRangeException) { }
            }
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

        
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is PropertyID other && Equals(other);

        
        /// <inheritdoc/>
        public bool Equals(PropertyID other) => PropertyNumber == other.PropertyNumber && MappingNumber == other.MappingNumber;


        
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
        public static bool operator ==(PropertyID pid1, PropertyID pid2) => pid1.Equals(pid2);

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyID"/>-Strukturen ungleich sind.
        /// </summary>
        /// <param name="pid1">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <param name="pid2">Eine zu vergleichende <see cref="PropertyID"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Strukturen ungleich sind, 
        /// andernfalls <c>false</c>.</returns>
        public static bool operator !=(PropertyID pid1, PropertyID pid2) => !pid1.Equals(pid2);

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

            if(PropertyNumber == 0) 
            { 
                return; 
            }

            _ = builder.Append(PropertyNumber.ToString(CultureInfo.InvariantCulture));

            if (MappingNumber.HasValue)
            {
                _ = builder.Append('.');
                _ = builder.Append(MappingNumber.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

    }
}
