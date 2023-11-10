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
    /// <param name="localID">Local ID of the mapping (value: 1 - 9).</param>
    /// <param name="globalID">A <see cref="Uri" /> that uniquely identifies a 
    /// vCard-property across different versions of the same vCard.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="localID" /> is less
    /// than 1 or greater than 9.</exception>
    /// <exception cref="ArgumentNullException"> <paramref name="globalID" /> is 
    /// <c>null</c>.</exception>
    internal PropertyIDMapping(int localID, Uri globalID)
    {
        localID.ValidateID(nameof(localID));

        LocalID = localID;
        GlobalID = globalID ?? throw new ArgumentNullException(nameof(globalID));
    }

    /// <summary>Gets the Local ID of the mapping.</summary>
    public int LocalID
    {
        get;
    }

    /// <summary>Gets the <see cref="Uri" /> that serves as a cross-platform identifier
    /// for the mapping.</summary>
    public Uri GlobalID
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
    /// <exception cref="ArgumentException">
    /// <paramref name="s" /> is not a <see cref="PropertyIDMapping" />.</exception>
    internal static PropertyIDMapping Parse(string s)
    {
        int? mappingNumber = null;
        int index;
        for (index = 0; index < s.Length; index++)
        {
            char c = s[index];

            if (char.IsWhiteSpace(c))
            {
                continue;
            }

            if (!c.TryParseDecimalDigit(out mappingNumber))
            {
                throw new ArgumentException(Res.MissingMappingID, nameof(s));
            }

            index++;
            break;
        }

        while (index < s.Length)
        {
            char c = s[index++];
            if (c == ';')
            {
                string url = s.Substring(index);

                try
                {
                    return new PropertyIDMapping(mappingNumber!.Value, new Uri(url, UriKind.RelativeOrAbsolute));
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message, nameof(s), e);
                }
            }
            else if (char.IsWhiteSpace(c))
            {
                continue;
            }
            else
            {
                // two-digit MappingNumber
                throw new ArgumentException(Res.IdentifierTooLong, nameof(s));
            }
        }

        // missing URI part:
        throw new ArgumentException(Res.MissingUri, nameof(s));
    }


    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder != null);
        _ = builder.Append(LocalID).Append(';').Append(GlobalID);
    }
}
