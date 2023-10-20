using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class StringExtension
{
    // The regex isn't perfect but finds most IETF language tags:
    private const string IETF_LANGUAGE_TAG_PATTERN = @"^[a-z]{2,3}-[A-Z]{2,3}$";

    [return: NotNullIfNotNull(nameof(value))]
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


    /// <summary> Gibt <c>true</c> zurück, wenn <paramref name="s" /> NON-ASCII-Zeichen
    /// oder Zeilenwechsel enthält. Needs no <c>null</c> check! </summary>
    /// <param name="s">Ein <see cref="string" />.</param>
    /// <returns />
    public static bool NeedsToBeQpEncoded(this string? s)
    {
        if (s is null)
        {
            return false;
        }

        if (s.ContainsNewLine())
        {
            return true;
        }

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] > 126)
            {
                return true;
            }
        }

        return false;
    }



    internal static bool IsIetfLanguageTag(this string value)
#if NET461 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0 || NET6_0
    {
        try
        {
            return Regex.IsMatch(value, IETF_LANGUAGE_TAG_PATTERN, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(50));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
#else
    {
        try
        {
            return IetfLanguageTagRegex().IsMatch(value);
        }
        catch(RegexMatchTimeoutException)
        {
            return false;
        }
    }

    [GeneratedRegex(IETF_LANGUAGE_TAG_PATTERN, RegexOptions.CultureInvariant, 50)]
    private static partial Regex IetfLanguageTagRegex();
#endif

}
