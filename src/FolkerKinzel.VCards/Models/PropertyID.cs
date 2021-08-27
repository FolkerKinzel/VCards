using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using FolkerKinzel.VCards.Intls.Extensions;
using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt Informationen, die dazu dienen, eine Instanz einer <see cref="VCardProperty"/> eindeutig
    /// zu identifizieren.
    /// </summary>
    public sealed class PropertyID : IEquatable<PropertyID>, IEnumerable<PropertyID>
    {
        private const int MAPPING_SHIFT = 4;
        private readonly byte _data;

        /// <summary>
        /// Initialisiert eine neues <see cref="PropertyID"/>-Objekt
        /// mit der lokalen ID der vCard-Property und - optional - 
        /// einem <see cref="PropertyIDMapping"/>-Objekt, das die Identifizierung der vCard-Property über
        /// verschiedene Versionszustände derselben vCard hinweg erlaubt.
        /// </summary>
        /// <param name="id">Die lokale ID der vCard-Property.</param>
        /// <param name="mapping">Ein <see cref="PropertyIDMapping"/>-Objekt oder <c>null</c>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> ist kleiner als 1 oder größer als 9.</exception>
        public PropertyID(int id, PropertyIDMapping? mapping = null)
        {
            id.ValidateID(nameof(id));
            _data = (byte)id;

            if (mapping is not null)
            {
                _data |= (byte)(mapping.ID << MAPPING_SHIFT);
            }
        }

        /// <summary>
        /// Initialisiert eine neues <see cref="PropertyID"/>-Objekt
        /// mit der lokalen Nummer der <see cref="VCardProperty"/> und der Nummer des Mappings dieser 
        /// <see cref="VCardProperty"/>.
        /// </summary>
        /// <param name="id">Lokaler Identifier der <see cref="VCardProperty"/> (Wert: zwischen 1 und 9). Der Name der
        /// vCard-Property und diese Nummer identifizieren eine Property lokal eindeutig.</param>
        /// <param name="mapping"><see cref="PropertyIDMapping.ID"/> des Mappings der 
        /// <see cref="VCardProperty"/> (Wert: zwischen 1 und 9) oder <c>null</c>, um kein Mapping anzugeben. Das
        /// Mapping dient dazu, eine vCard-Property zwischen verschiedenen Versionszuständen derselben vCard
        /// zu identifizieren.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> und/oder <paramref name="mapping"/>
        /// sind kleiner als 1 oder größer als 9.</exception>
        private PropertyID(int id, int? mapping = null)
        {
            id.ValidateID(nameof(id));
            _data = (byte)id;

            if (mapping.HasValue)
            {
                int mappingValue = mapping.Value;
                mappingValue.ValidateID(nameof(mapping));

                _data |= (byte)(mapping.Value << MAPPING_SHIFT);
            }
        }

        /// <summary>
        /// Gibt die lokale ID der <see cref="VCardProperty"/> zurück, der das <see cref="PropertyID"/>-Objekt
        /// zugeordnet ist.
        /// </summary>
        /// <remarks>
        /// Der lokale Schlüssel zur Identifizierung eines <see cref="VCardProperty"/>-Objekts besteht aus dem 
        /// Namen der vCard-Property, dem dieses Objekt zugeordnet ist, und dem Wert der <see cref="PropertyID.ID"/>
        /// des <see cref="PropertyID"/>-Objekts, das diesem <see cref="VCardProperty"/>-Objekt zugeordnet ist.
        /// </remarks>
        public int ID => _data & 0xF;

        /// <summary>
        /// Gibt die <see cref="PropertyIDMapping.ID"/> des <see cref="PropertyIDMapping"/>-Objekts
        /// zurück, mit dem das <see cref="PropertyID"/>-Objekt verbunden ist, oder <c>null</c>, wenn das 
        /// <see cref="PropertyID"/>-Objekt mit keinem <see cref="PropertyIDMapping"/> verbunden ist.
        /// </summary>
        public int? Mapping
        {
            get
            {
                int value = _data >> MAPPING_SHIFT;
                return value == 0 ? null : value;
            }
        }

        ///// <summary>
        ///// <c>true</c>, wenn die Instanz der <see cref="PropertyID"/>-Struktur keine verwertbaren Daten enthält.
        ///// </summary>
        //public bool IsEmpty => PropertyNumber < 1;


        internal static void ParseInto(List<PropertyID> list, string pids)
        {
            if (pids.Length == 0)
            {
                return;
            }

            int index = 0;

            int id = 0;
            int? mapping = null;
            bool parseMapping = false;


            while (index < pids.Length)
            {
                char c = pids[index++];

                if (c == ',')
                {
                    try
                    {
                        list.Add(new PropertyID(id, mapping));
                    }
                    catch (ArgumentOutOfRangeException) { }

                    id = 0;
                    mapping = null;
                    parseMapping = false;
                }
                else if (c == '.')
                {
                    parseMapping = true;
                }
                else if (c.IsDecimalDigit())
                {
                    if (parseMapping)
                    {
                        // Exception bei mehrstelligen Nummern:
                        mapping = mapping.HasValue ? 0 : c.ParseDecimalDigit();
                    }
                    else
                    {
                        // Exception bei mehrstelligen Nummern:
                        id = id == 0 ? c.ParseDecimalDigit() : 0;
                    }
                }//else
            }//while

            // if vermeidet unnötige Exception, falls der letzte Wert (standardungerecht)
            // mit einem Komma endet
            if (id != 0)
            {
                try
                {
                    list.Add(new PropertyID(id, mapping));
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }



        #region IEquatable


        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is PropertyID other && Equals(other);


        /// <inheritdoc/>
        public bool Equals(PropertyID? other) => other is not null && ID == other.ID && Mapping == other.Mapping;



        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1;
            return Mapping.HasValue ? (hashCode ^ ID.GetHashCode()) ^ (hashCode ^ Mapping.GetHashCode()) : hashCode ^ ID.GetHashCode();
        }


        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Objekte. Das Ergebnis gibt an, ob die Werte 
        /// der beiden <see cref="PropertyID"/>-Objekte gleich sind.
        /// </summary>
        /// <param name="pid1">Ein zu vergleichendes <see cref="PropertyID"/>-Objekt.</param>
        /// <param name="pid2">Ein zu vergleichendes <see cref="PropertyID"/>-Objekt.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Objekte gleich sind,
        /// andernfalls <c>false</c>.</returns>
        public static bool operator ==(PropertyID? pid1, PropertyID? pid2)
            => object.ReferenceEquals(pid1, pid2) || (pid1?.Equals(pid2) ?? false);

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyID"/>-Objekte. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyID"/>-Objekte ungleich sind.
        /// </summary>
        /// <param name="pid1">Ein zu vergleichendes <see cref="PropertyID"/>-Objekt.</param>
        /// <param name="pid2">Ein zu vergleichendes <see cref="PropertyID"/>-Objekt.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyID"/>-Objekte ungleich sind, 
        /// andernfalls <c>false</c>.</returns>
        public static bool operator !=(PropertyID? pid1, PropertyID? pid2) => !(pid1 == pid2);

        #endregion

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Instanz. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyID"/>-Instanz.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(3);
            AppendTo(sb);
            return sb.ToString();
        }

        internal void AppendTo(StringBuilder builder)
        {
            Debug.Assert(builder != null);

            _ = builder.Append(ID);

            if (Mapping.HasValue)
            {
                _ = builder.Append('.');
                _ = builder.Append(Mapping.Value);
            }
        }

        IEnumerator<PropertyID> IEnumerable<PropertyID>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PropertyID>)this).GetEnumerator();
    }
}
