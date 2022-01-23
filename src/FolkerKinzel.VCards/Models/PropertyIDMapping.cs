using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Verbindet das lokale <see cref="PropertyID.Mapping"/> einer <see cref="VCardProperty"/> mit einem
/// <see cref="Uri"/>, der die vCard-Property über verschiedene Versionszustände derselben vCard hinweg
/// eindeutig identifiziert.
/// </summary>
public sealed class PropertyIDMapping
{
    /// <summary>
    /// Initialisiert eine neues <see cref="PropertyIDMapping"/>-Objekt.
    /// </summary>
    /// <param name="id">Lokale ID des Mappings (Wert: zwischen 1 und 9).</param>
    /// <param name="mapping">Ein <see cref="Uri"/>, der die vCard-Property über 
    /// verschiedene Versionszustände derselben vCard hinweg
    /// eindeutig identifiziert.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> ist kleiner als 1 oder größer als 9.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="mapping"/> ist <c>null</c>.</exception>
    public PropertyIDMapping(int id, Uri mapping)
    {
        id.ValidateID(nameof(id));

        ID = id;
        Mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
    }

    /// <summary>
    /// Lokale ID des Mappings.
    /// </summary>
    public int ID
    {
        get;
    }

    /// <summary>
    /// Ein <see cref="Uri"/>, der als plattformübergreifender Bezeichner des Mappings dient.
    /// </summary>
    public Uri Mapping
    {
        get;
    }

    /// <summary>
    /// Parses a <see cref="string"/> that represents a vCard 4.0 Property ID Mapping.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>A <see cref="PropertyIDMapping"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> ist kein <see cref="PropertyIDMapping"/>.</exception>
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





    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Instanz. (Nur zum 
    /// Debuggen.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation der <see cref="PropertyIDMapping"/>-Instanz.</returns>
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
