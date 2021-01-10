using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Resources;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Verbindet die <see cref="PropertyID.MappingNumber"/> einer <see cref="VCardProperty"/> mit einer
    /// <see cref="Guid"/>, die diese <see cref="VCardProperty"/> über mehrere vCards hinweg eindeutig identifiziert.
    /// </summary>
    /// <remarks>
    /// Der Standard erlaubt zwar, dass das Mapping mit einer beliebigen <see cref="Uri"/> signiert wird, unterstützt
    /// werden von dieser Bibliothek aber nur <see cref="Guid"/>-Instanzen.
    /// </remarks>
    public readonly struct PropertyIDMapping : IEquatable<PropertyIDMapping>
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="PropertyIDMapping"/>-Struktur.
        /// </summary>
        /// <param name="mappingNumber">Nummer des Mappings (Wert: zwischen 1 und 9).</param>
        /// <param name="uuid">Eine Instanz der <see cref="Guid"/>-Struktur, die als plattformübergreifender Bezeichner für eine 
        /// <see cref="VCardProperty"/> dient.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mappingNumber"/> ist kleiner als 1 oder größer als 9.</exception>
        internal PropertyIDMapping(int mappingNumber, Guid uuid)
        {
            if (mappingNumber < 1 || mappingNumber > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(mappingNumber), Res.PidValue);
            }
            MappingNumber = mappingNumber;
            Uuid = uuid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="s"/> ist kein <see cref="PropertyIDMapping"/>.</exception>
        internal static PropertyIDMapping Parse(string s)
        {
            int mappingNumber = 0;
            int index;
            for (index = 0; index < s.Length; index++)
            {
                char c = s[index];

                if (char.IsDigit(c))
                {
                    mappingNumber = DigitParser.Parse(c);
                    index++;
                    break;
                }
            }

            if (mappingNumber == 0)
            {
                // keine MappingNumber
                throw new ArgumentOutOfRangeException(nameof(s));
            }

            while (index < s.Length)
            {
                char c = s[index++];
                if (c == ';')
                {
#if NET40
                    string guid = s.Substring(index);
#else
                    ReadOnlySpan<char> guid = s.AsSpan(index);
#endif
                    if (guid.IsUuidUri())
                    {
                        return new PropertyIDMapping(mappingNumber, UuidConverter.ToGuid(guid));
                    }
                    else
                    {
                        // Obwohl der Standard beliebige URIs erlaubt, werden
                        // hier nur UUIDs unterstützt
                        throw new ArgumentOutOfRangeException(nameof(s));
                    }
                }
                else if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                {
                    // 2stellige MappingNumber
                    throw new ArgumentOutOfRangeException(nameof(s));
                }
            }

            // fehlender URI-Teil:
            throw new ArgumentOutOfRangeException(nameof(s));
        }


        /// <summary>
        /// Nummer des Mappings. (Entspricht <see cref="PropertyID.MappingNumber">PropertyID.MappingNumber</see>).
        /// </summary>
        public int MappingNumber
        {
            get;
        }
        


        /// <summary>
        /// Plattformübergreifender Bezeichner des Mappings.
        /// </summary>
        public Guid Uuid
        {
            get;
        }


        /// <summary>
        /// <c>true</c>, wenn die Instanz der <see cref="PropertyIDMapping"/>-Struktur keine verwertbaren Daten enthält.
        /// </summary>
        public bool IsEmpty => Uuid == Guid.Empty;


        #region IEquatable

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PropertyIDMapping other && Equals(other);

        
        /// <inheritdoc/>
        public bool Equals(PropertyIDMapping other) => this.MappingNumber == other.MappingNumber && this.Uuid == other.Uuid;

        
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1;
            return (hashCode ^ MappingNumber.GetHashCode()) ^ (hashCode ^ Uuid.GetHashCode());
        }

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyIDMapping"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyIDMapping"/>-Strukturen gleich sind.
        /// </summary>
        /// <param name="pidMap1">Eine zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        /// <param name="pidMap2">Eine zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyIDMapping"/>-Strukturen gleich sind, andernfalls <c>false</c>.</returns>
        public static bool operator ==(PropertyIDMapping pidMap1, PropertyIDMapping pidMap2) => pidMap1.Equals(pidMap2);

        /// <summary>
        /// Vergleicht zwei <see cref="PropertyIDMapping"/>-Strukturen. Das Ergebnis gibt an, ob die Werte der beiden 
        /// <see cref="PropertyIDMapping"/>-Strukturen ungleich sind.
        /// </summary>
        /// <param name="pidMap1">Eine zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        /// <param name="pidMap2">Eine zu vergleichende <see cref="PropertyIDMapping"/>-Struktur.</param>
        /// <returns><c>true</c>, wenn die Werte der beiden <see cref="PropertyIDMapping"/>-Strukturen ungleich sind, 
        /// andernfalls <c>false</c>.</returns>
        public static bool operator !=(PropertyIDMapping pidMap1, PropertyIDMapping pidMap2) => !pidMap1.Equals(pidMap2);

        #endregion


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Struktur. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Struktur.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(48);
            AppendTo(sb);
            return sb.ToString();
        }

        internal void AppendTo(StringBuilder builder)
        {
            Debug.Assert(builder != null);

            if (MappingNumber == 0)
            {
                return;
            }

            _ = builder.Append(MappingNumber.ToString(CultureInfo.InvariantCulture));
            _ = builder.Append(';');
            _ = builder.AppendUuid(Uuid);
        }
    }

}
