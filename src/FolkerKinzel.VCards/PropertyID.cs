using System.Collections;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates information that is used to uniquely identify an instance
/// of a <see cref="VCardProperty" />.</summary>
/// <seealso cref="Models.PropertyParts.ParameterSection.PropertyIDs"/>
/// <seealso cref="Models.PropertyParts.PropertyIDMapping"/>
/// <seealso cref="Models.PropertyIDMappingProperty"/>
/// <seealso cref="VCard.PropertyIDMappings"/>
public sealed class PropertyID : IEquatable<PropertyID>, IEnumerable<PropertyID>
{
    private const int MAPPING_SHIFT = 4;
    private readonly byte _data;

    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local ID
    /// of the vCard property and - optionally - a <see cref="PropertyIDMappingProperty" />
    /// object that allows the identification of the vCard property across different
    /// version states of the same vCard.</summary>
    /// <param name="id">The local ID of the vCard property (value: 1 - 9). The name 
    /// of the vCard property and this number uniquely identify a property locally.</param>
    /// <param name="mapping">A <see cref="PropertyIDMappingProperty" /> or <c>null</c>.
    /// The mapping is used to identify a vCard-property platform-independent between 
    /// different version states of the same vCard.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> is less
    /// than 1 or greater than 9.</exception>
    public PropertyID(int id, PropertyIDMappingProperty? mapping = null)
    {
        id.ValidateID(nameof(id));
        _data = (byte)id;

        if (mapping?.Value != null)
        {
            _data |= (byte)(mapping.Value.LocalID << MAPPING_SHIFT);
        }
    }

    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local number of the 
    /// <see cref="VCardProperty" /> and the number of the mapping of this 
    /// <see cref="VCardProperty" />.</summary>
    /// <param name="id">The local ID of the vCard property (Value: 1 - 9). The name of the vCard 
    /// property and this number uniquely identify a property locally.</param>
    /// <param name="mapping"> <see cref="PropertyIDMapping.LocalID" /> of the mapping of the 
    /// <see cref="VCardProperty" /> (value: 1 - 9) or <c>null</c> to not specify any mapping. 
    /// The mapping is used to identify a vCard property between different version states of the 
    /// same vCard.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> or <paramref name="mapping"/>
    /// is less than 1 or greater than 9.</exception>
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
    /// <remarks>
    /// The local key for identifying a <see cref="VCardProperty" /> consists of the name 
    /// of the vCard property to which this object is assigned and the value of the 
    /// <see cref="ID" /> of the <see cref="PropertyID" /> object, which is 
    /// assigned to this <see cref="VCardProperty" />.</remarks>
    public int ID => _data & 0xF;

    /// <summary>
    /// Gets the <see cref="PropertyIDMapping.LocalID" /> of the <see cref="PropertyIDMapping"
    /// /> object with which the <see cref="PropertyID" /> object is connected, or <c>null</c>
    /// if the <see cref="PropertyID" /> object is not connected with any <see
    /// cref="PropertyIDMapping" />.
    /// </summary>
    public int? Mapping
    {
        get
        {
            int value = _data >> MAPPING_SHIFT;
            return value == 0 ? null : value;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(3);
        AppendTo(sb);
        return sb.ToString();
    }

    /// <inheritdoc/>
    IEnumerator<PropertyID> IEnumerable<PropertyID>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PropertyID>)this).GetEnumerator();

    #region IEquatable

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is PropertyID other && Equals(other);

    /// <inheritdoc />
    public bool Equals(PropertyID? other) => other is not null && ID == other.ID && Mapping == other.Mapping;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ID, Mapping);

    /// <summary>Compares two <see cref="PropertyID" /> objects. The result indicates
    /// whether the values of the two <see cref="PropertyID" /> objects are equal.</summary>
    /// <param name="pid1">A <see cref="PropertyID" /> object to compare.</param>
    /// <param name="pid2">A <see cref="PropertyID" /> object to compare.</param>
    /// <returns> <c>true</c> if the values of the two <see cref="PropertyID" /> objects
    /// are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(PropertyID? pid1, PropertyID? pid2)
        => ReferenceEquals(pid1, pid2) || (pid1?.Equals(pid2) ?? false);

    /// <summary>Compares two <see cref="PropertyID" /> objects. The result indicates
    /// whether the values of the two <see cref="PropertyID" /> objects are not equal.</summary>
    /// <param name="pid1">A <see cref="PropertyID" /> object to compare.</param>
    /// <param name="pid2">A <see cref="PropertyID" /> object to compare.</param>
    /// <returns> <c>true</c> if the values of the two <see cref="PropertyID" /> objects
    /// are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(PropertyID? pid1, PropertyID? pid2) => !(pid1 == pid2);

    #endregion

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
}
