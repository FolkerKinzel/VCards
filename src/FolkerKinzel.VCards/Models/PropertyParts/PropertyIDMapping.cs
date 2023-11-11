using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary> 
/// Connects the local <see cref="PropertyID.Mapping" /> of a <see cref="VCardProperty" 
/// /> with a <see cref="Uri" />, which uniquely identifies a vCard-property across 
/// different versions of the same vCard.
/// </summary>
/// <seealso cref="PropertyIDMappingProperty"/>
/// <seealso cref="VCard.PropertyIDMappings"/>
public sealed class PropertyIDMapping
{
    /// <summary>Initializes a new <see cref="PropertyIDMapping" /> object.</summary>
    /// <param name="localID">Local ID of the mapping. (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="globalID">A URI that uniquely identifies a 
    /// vCard-property platform-independent across different versions of the same vCard.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="localID" /> is less
    /// than 1.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="globalID" /> is 
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="globalID" /> is 
    /// not a valid URI.</exception>
    internal PropertyIDMapping(int localID, string globalID)
    {
        localID.ValidateID(nameof(localID));
        LocalID = localID;

        GlobalID = string.IsNullOrWhiteSpace(globalID)
                            ? globalID is null ? throw new ArgumentNullException(nameof(globalID))
                                               : throw new ArgumentException(Res.NotAUri, nameof(globalID))
                            : globalID;
    }

    /// <summary>Gets the Local ID of the mapping.</summary>
    public int LocalID
    {
        get;
    }

    /// <summary>Gets the URI that serves as a cross-platform identifier
    /// for the mapping.</summary>
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
    /// <returns>A <see cref="PropertyIDMapping" /> instance.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not a <see cref="PropertyIDMapping" />.</exception>
    internal static PropertyIDMapping Parse(string s)
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
            return new PropertyIDMapping(mappingNumber, span.Slice(separatorIdx + 1).ToString());
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
