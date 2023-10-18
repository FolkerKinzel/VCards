using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

    /// <summary> Verbindet das lokale <see cref="PropertyID.Mapping" /> einer <see
    /// cref="VCardProperty" /> mit einem <see cref="Uri" />, der die vCard-Property
    /// über verschiedene Versionszustände derselben vCard hinweg eindeutig identifiziert.
    /// </summary>
public sealed class PropertyIDMapping
{
    /// <summary>Initializes a new <see cref="PropertyIDMapping" /> object.</summary>
    /// <param name="id">Local ID of the mapping (value: between 1 and 9).</param>
    /// <param name="mapping">A <see cref="Uri" /> that uniquely identifies the vCard
    /// property across different versions of the same vCard.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="id" /> is less
    /// than 1 or greater than 9.</exception>
    /// <exception cref="ArgumentNullException"> <paramref name="mapping" /> is <c>null</c>.</exception>
    public PropertyIDMapping(int id, Uri mapping)
    {
        id.ValidateID(nameof(id));

        ID = id;
        Mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
    }

    /// <summary>Gets the Local ID of the mapping.</summary>
    public int ID
    {
        get;
    }

    /// <summary>Gets the <see cref="Uri" /> that serves as a cross-platform identifier
    /// for the mapping.</summary>
    public Uri Mapping
    {
        get;
    }

    /// <summary />
    /// <param name="s" />
    /// <returns />
    /// <exception cref="ArgumentException" />
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
                // 2stellige MappingNumber
                throw new ArgumentException(Res.IdentifierTooLong, nameof(s));
            }
        }

        // fehlender URI-Teil:
        throw new ArgumentException(Res.MissingUri, nameof(s));
    }





    /// <summary>Creates a <see cref="string" /> representation of the <see cref="PropertyIDMapping"
    /// /> object. (For debugging only.)</summary>
    /// <returns>A <see cref="string" /> representation of the <see cref="PropertyIDMapping"
    /// /> object.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(48);
        AppendTo(sb);
        return sb.ToString();
    }

    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder != null);
        _ = builder.Append(ID).Append(';').Append(Mapping);
    }
}
