using System.Collections;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Encapsulates information that is used to uniquely identify an instance
    /// of a <see cref="VCardProperty" />.</summary>
public sealed class PropertyID : IEquatable<PropertyID>, IEnumerable<PropertyID>
{
    private const int MAPPING_SHIFT = 4;
    private readonly byte _data;

    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local ID
    /// of the vCard property and - optionally - a <see cref="PropertyIDMapping" />
    /// object that allows the identification of the vCard property across different
    /// version states of the same vCard.</summary>
    /// <param name="id">The local ID of the vCard property.</param>
    /// <param name="mapping">A <see cref="PropertyIDMapping" /> object or <c>null</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> is less
    /// than 1 or greater than 9.</exception>
    public PropertyID(int id, PropertyIDMapping? mapping = null)
    {
        id.ValidateID(nameof(id));
        _data = (byte)id;

        if (mapping is not null)
        {
            _data |= (byte)(mapping.ID << MAPPING_SHIFT);
        }
    }

    /// <summary />
    /// <param name="id" />
    /// <param name="mapping"> <see cref="PropertyIDMapping.ID" /> des Mappings der
    /// <see cref="VCardProperty" /> (Wert: zwischen 1 und 9) oder <c>null</c>, um kein
    /// Mapping anzugeben. Das Mapping dient dazu, eine vCard-Property zwischen verschiedenen
    /// Versionszuständen derselben vCard zu identifizieren.</param>
    /// <exception cref="ArgumentOutOfRangeException" />
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

    /// <summary>Gets the local ID of the <see cref="VCardProperty" /> to which the
    /// <see cref="PropertyID" /> object is assigned.</summary>
    /// <remarks> Der lokale Schlüssel zur Identifizierung eines <see cref="VCardProperty"
    /// />-Objekts besteht aus dem Namen der vCard-Property, dem dieses Objekt zugeordnet
    /// ist, und dem Wert der <see cref="PropertyID.ID" /> des <see cref="PropertyID"
    /// />-Objekts, das diesem <see cref="VCardProperty" />-Objekt zugeordnet ist. </remarks>
    public int ID => _data & 0xF;

    /// <summary> Gibt die <see cref="PropertyIDMapping.ID" /> des <see cref="PropertyIDMapping"
    /// />-Objekts zurück, mit dem das <see cref="PropertyID" />-Objekt verbunden ist,
    /// oder <c>null</c>, wenn das <see cref="PropertyID" />-Objekt mit keinem <see
    /// cref="PropertyIDMapping" /> verbunden ist. </summary>
    public int? Mapping
    {
        get
        {
            int value = _data >> MAPPING_SHIFT;
            return value == 0 ? null : value;
        }
    }


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
            else if (c.IsAsciiDigit())
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

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is PropertyID other && Equals(other);

    /// <inheritdoc />
    public bool Equals(PropertyID? other) => other is not null && ID == other.ID && Mapping == other.Mapping;


    /// <inheritdoc />
    public override int GetHashCode()
        => Mapping.HasValue ? HashCode.Combine(ID, Mapping) 
                            : HashCode.Combine(ID);


    /// <summary>Compares two <see cref="PropertyID" /> objects. The result indicates
    /// whether the values of the two <see cref="PropertyID" /> objects are equal.</summary>
    /// <param name="pid1">A <see cref="PropertyID" /> object to compare.</param>
    /// <param name="pid2">A <see cref="PropertyID" /> object to compare.</param>
    /// <returns> <c>true</c> if the values of the two <see cref="PropertyID" /> objects
    /// are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(PropertyID? pid1, PropertyID? pid2)
        => object.ReferenceEquals(pid1, pid2) || (pid1?.Equals(pid2) ?? false);

    /// <summary>Compares two <see cref="PropertyID" /> objects. The result indicates
    /// whether the values of the two <see cref="PropertyID" /> objects are not equal.</summary>
    /// <param name="pid1">A <see cref="PropertyID" /> object to compare.</param>
    /// <param name="pid2">A <see cref="PropertyID" /> object to compare.</param>
    /// <returns> <c>true</c> if the values of the two <see cref="PropertyID" /> objects
    /// are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(PropertyID? pid1, PropertyID? pid2) => !(pid1 == pid2);

    #endregion

    /// <summary>Creates a <see cref="string" /> representation of the <see cref="PropertyID"
    /// /> object. (For debugging only.)</summary>
    /// <returns>A <see cref="string" /> representation of the <see cref="PropertyID"
    /// /> object.</returns>
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
