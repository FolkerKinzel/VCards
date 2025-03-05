using System.Collections;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information that is used to identify an instance
/// of a <see cref="VCardProperty" />.</summary>
/// <seealso cref="ParameterSection.PropertyIDs"/>
/// <seealso cref="AppID"/>
/// <seealso cref="AppIDProperty"/>
/// <seealso cref="VCard.AppIDs"/>
public sealed class PropertyID : IEquatable<PropertyID>, IEnumerable<PropertyID>
{
    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local ID
    /// of the <see cref="VCardProperty"/> and - optionally - a <see cref="AppID" />
    /// object that allows global identification of the vCard property.</summary>
    /// <param name="id">The local ID of the <see cref="VCardProperty"/>. The name 
    /// of the vCard property and this number uniquely identify a <see cref="VCardProperty"/> 
    /// in the <see cref="VCard"/> instance. 
    /// (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="client">A <see cref="AppID" /> object to enable global identification
    /// of the vCard property, or <c>null</c> to have only local identification.
    /// (The value normally is <see cref="SyncOperation.CurrentAppID"/>.)
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> is less
    /// than 1.</exception>
    internal PropertyID(int id, AppID? client)
    {
        Debug.Assert(id.ValidateID());
        ID = id;
        App = client?.LocalID;
    }

    /// <summary>Initializes a new <see cref="PropertyID" /> object with the local number of the 
    /// <see cref="VCardProperty" /> and the <see cref="AppID.LocalID"/> of the 
    /// <see cref="AppID"/>.</summary>
    /// <param name="id">The local ID of the vCard property. The name of the vCard 
    /// property and this number uniquely identify a property locally. (The value is a positive 
    /// <see cref="int"/>, not zero.)</param>
    /// <param name="client"> <see cref="AppID.LocalID" /> of the 
    /// <see cref="AppID"/>, or <c>null</c> to not specify any <see cref="AppID"/>. 
    /// (The value is a positive <see cref="int"/>, not zero.)</param>
    private PropertyID(int id, int? client)
    {
        Debug.Assert(id.ValidateID());
        Debug.Assert(client.ValidateID());

        ID = id;
        App = client;
    }

    /// <summary>Gets the local ID of the <see cref="VCardProperty" />.</summary>
    /// <remarks>
    /// The local key for identifying a <see cref="VCardProperty" /> consists of the name 
    /// of the vCard property to which this instance is assigned and the value of this
    /// property.</remarks>
    public int ID { get; }

    /// <summary>
    /// Gets the <see cref="AppID.LocalID" /> of the <see cref="AppID"
    /// /> object with which the <see cref="PropertyID" /> object is connected, or <c>null</c>
    /// if the <see cref="PropertyID" /> object is not connected with any <see
    /// cref="AppID" />.
    /// </summary>
    /// <seealso cref="AppID"/>
    /// <seealso cref="VCard.RegisterApp(Uri?)"/>
    /// <seealso cref="VCard.Sync"/>
    /// <seealso cref="VCard.AppIDs"/>
    public int? App { get; }

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
    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as PropertyID);

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] PropertyID? other)
        => other is not null && ID == other.ID && App == other.App;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ID, App);

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

    internal static IEnumerable<PropertyID> Parse(ReadOnlySpan<char> span)
    {
        int sepIdx;
        PropertyID? propID;

        IEnumerable<PropertyID> coll = [];

        while ((sepIdx = span.IndexOf(',')) != -1)
        {
            if (TryParsePropertyID(span.Slice(0, sepIdx), out propID))
            {
                coll = coll.Concat(propID);
            }

            span = span.Slice(sepIdx + 1);
        }

        if (TryParsePropertyID(span, out propID))
        {
            coll = coll.Concat(propID);
        }

        return coll;

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
                if (id.ValidateID() && client.ValidateID())
                {
                    propID = new PropertyID(id, client);
                    return true;
                }

                propID = null;
                return false;
            }
        }
    }

    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder is not null);

        _ = builder.Append(ID);

        if (App.HasValue)
        {
            _ = builder.Append('.');
            _ = builder.Append(App.Value);
        }
    }
}
