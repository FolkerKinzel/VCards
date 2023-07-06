using System.Text;
using FolkerKinzel.VCards.Models.Enums;

#if !NET40
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class StringExtension
{
    [return: NotNullIfNotNull("value")]
    internal static string? UnMask(this string? value, StringBuilder sb, VCdVersion version)
    {
        Debug.Assert(sb != null);

        if (value is null || value.Length == 0)
        {
            return value;
        }

        _ = sb.Clear().Append(value).UnMask(version);

        if (sb.Length != value.Length)
        {
            return sb.ToString();
        }

        if (version == VCdVersion.V2_1)
        {
            return value;
        }

        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] != value[i])
            {
                return sb.ToString();
            }
        }

        return value;
    }


    /// <summary>
    /// Gibt <c>true</c> zurück, wenn <paramref name="s"/> NON-ASCII-Zeichen oder Zeilenwechsel enthält.
    /// Benötigt keine NULL-Prüfung.
    /// </summary>
    /// <param name="s">Ein <see cref="string"/> oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn <paramref name="s"/> enkodiert werden muss.</returns>
    public static bool NeedsToBeQpEncoded(this string? s)
    {
        if (s is null)
        {
            return false;
        }

        if (s.Contains(Environment.NewLine, StringComparison.Ordinal))
        {
            return true;
        }

        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] > 126)
            {
                return true;
            }
        }

        return false;
    }

}
