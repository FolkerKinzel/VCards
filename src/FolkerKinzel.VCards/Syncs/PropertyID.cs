using System;
using System.Collections;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Syncs;

/// <summary>Encapsulates information that is used to identify an instance
/// of a <see cref="VCardProperty" />.</summary>
/// <seealso cref="ParameterSection.PropertyIDs"/>
/// <seealso cref="VCardClient"/>
/// <seealso cref="VCardClientProperty"/>
/// <seealso cref="VCard.VCardClients"/>
public sealed class PropertyID : IEquatable<PropertyID>, IEnumerable<PropertyID>
{
    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local ID
    /// of the <see cref="VCardProperty"/> and - optionally - a <see cref="VCardClient" />
    /// object that allows global identification of the vCard property.</summary>
    /// <param name="id">The local ID of the <see cref="VCardProperty"/>. The name 
    /// of the vCard property and this number uniquely identify a <see cref="VCardProperty"/> 
    /// in the <see cref="VCard"/> instance. 
    /// (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="client">A <see cref="VCardClient" /> object to enable global identification
    /// of the vCard property, or <c>null</c> to have only local identification.
    /// (The value normally is <see cref="VCard.ExecutingApp"/>.)
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> is less
    /// than 1.</exception>
    /// <remarks>
    /// <note type="caution">
    /// Using this constructor in own code endangers the referential integrity. Prefer using
    /// <see cref="VCard.SetPropertyIDs"/> or
    /// <see cref="ParameterSection.SetPropertyID(IEnumerable{VCardProperty?}, VCard)"/> instead.
    /// </note>
    /// </remarks>
    public PropertyID(int id, VCardClient? client = null)
    {
        id.ValidateID(nameof(id));
        ID = id;
        Client = client?.LocalID;
    }

    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local number of the 
    /// <see cref="VCardProperty" /> and the <see cref="VCardClient.LocalID"/> of the 
    /// <see cref="VCardClient"/>.</summary>
    /// <param name="id">The local ID of the vCard property. The name of the vCard 
    /// property and this number uniquely identify a property locally. (The value is a positive 
    /// <see cref="int"/>, not zero.)</param>
    /// <param name="client"> <see cref="VCardClient.LocalID" /> of the 
    /// <see cref="VCardClient"/>, or <c>null</c> to not specify any <see cref="VCardClient"/>. 
    /// (The value is a positive <see cref="int"/>, not zero.)</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> or <paramref name="client"/>
    /// is less than 1.</exception>
    private PropertyID(int id, int? client)
    {
        id.ValidateID(nameof(id));
        ID = id;

        if (client.HasValue)
        {
            int clientValue = client.Value;
            clientValue.ValidateID(nameof(client));
            Client = clientValue;
        }
    }

    /// <summary>Gets the local ID of the <see cref="VCardProperty" />.</summary>
    /// <remarks>
    /// The local key for identifying a <see cref="VCardProperty" /> consists of the name 
    /// of the vCard property to which this instance is assigned and the value of this
    /// property.</remarks>
    public int ID { get; }

    /// <summary>
    /// Gets the <see cref="VCardClient.LocalID" /> of the <see cref="VCardClient"
    /// /> object with which the <see cref="PropertyID" /> object is connected, or <c>null</c>
    /// if the <see cref="PropertyID" /> object is not connected with any <see
    /// cref="VCardClient" />.
    /// </summary>
    /// <seealso cref="VCard.ExecutingApp"/>
    /// <seealso cref="VCard.RegisterApp(Uri)"/>
    /// <seealso cref="VCard.VCardClients"/>
    public int? Client { get; }

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
    public bool Equals(PropertyID? other) => other is not null && ID == other.ID && Client == other.Client;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ID, Client);

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
        var span = pids.AsSpan();

        int sepIdx;
        PropertyID? propID;

        while ((sepIdx = span.IndexOf(',')) != -1)
        {
            if (TryParsePropertyID(span.Slice(0, sepIdx), out propID))
            {
                list.Add(propID);
            }

            span = span.Slice(sepIdx + 1);
        }

        if (TryParsePropertyID(span, out propID))
        {
            list.Add(propID);
        }

        static bool TryParsePropertyID(ReadOnlySpan<char> value, [NotNullWhen(true)] out PropertyID? propID)
        {
            propID = null;

            int sepIdx = value.IndexOf('.');

            return sepIdx == -1
                ? _Int.TryParse(value, out int id) && TryCreatePropertyID(id, null, out propID)
                : _Int.TryParse(value.Slice(0, sepIdx), out id)
                    && _Int.TryParse(value.Slice(sepIdx + 1), out int client)
                    && TryCreatePropertyID(id, client, out propID);

            static bool TryCreatePropertyID(int id, int? client, [NotNullWhen(true)] out PropertyID? propID)
            {
                try
                {
                    propID = new PropertyID(id, client);
                    return true;
                }
                catch
                {
                    propID = null;
                    return false;
                }
            }
        }
    }

    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder != null);

        _ = builder.Append(ID);

        if (Client.HasValue)
        {
            _ = builder.Append('.');
            _ = builder.Append(Client.Value);
        }
    }
}
