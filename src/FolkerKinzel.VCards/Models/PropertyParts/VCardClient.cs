using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary> 
/// Identifies a vCard client globally, and locally inside the <see cref="VCard"/>.
/// </summary>
/// <seealso cref="VCardClientProperty"/>
/// <seealso cref="VCard.VCardClients"/>
public sealed class VCardClient
{
    /// <summary>Initializes a new <see cref="VCardClient" /> object.</summary>
    /// <param name="localID">Local ID that identifies the <see cref="VCardClient"/>
    /// in the <see cref="ParameterSection.PropertyIDs"/>. (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="globalID">A URI that identifies the <see cref="VCardClient"/> globally.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="localID" /> is less
    /// than 1.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="globalID" /> is 
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="globalID" /> is 
    /// not a valid URI.</exception>
    internal VCardClient(int localID, string globalID)
    {
        localID.ValidateID(nameof(localID));
        LocalID = localID;

        GlobalID = string.IsNullOrWhiteSpace(globalID)
                            ? globalID is null ? throw new ArgumentNullException(nameof(globalID))
                                               : throw new ArgumentException(Res.NotAUri, nameof(globalID))
                            : globalID;
    }

    /// <summary>Gets the Local ID.</summary>
    public int LocalID
    {
        get;
    }

    /// <summary>Gets the URI that serves as a global identifier.</summary>
    public string GlobalID
    {
        get;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(48);
        AppendTo(sb);
        return sb.ToString();
    }

    /// <summary>Parses a <see cref="string" /> that represents a vCard&#160;4.0 Property-ID Mapping. </summary>
    /// <param name="s">The <see cref="string"/> to parse.</param>
    /// <returns>A <see cref="VCardClient" /> instance.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not a <see cref="VCardClient" />.</exception>
    internal static VCardClient Parse(string s)
    {
        Debug.Assert(s != null);

        var span = s.AsSpan();
        int separatorIdx = span.IndexOf(';');

        if(separatorIdx < 1)
        {
            throw new FormatException();
        }

        int mappingNumber = _Int.Parse(span.Slice(0, separatorIdx));

        try
        {
            return new VCardClient(mappingNumber, span.Slice(separatorIdx + 1).Trim().ToString());
        }
        catch
        {
            throw new FormatException();
        }
    }


    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder != null);
        _ = builder.Append(LocalID).Append(';').Append(GlobalID);
    }
}
