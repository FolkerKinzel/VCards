using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class StringExtension
{
    // The regex isn't perfect but finds most IETF language tags:
    private const string IETF_LANGUAGE_TAG_PATTERN = @"^[a-z]{2,3}-[A-Z]{2,3}$";

    internal static string UnMask(this string value, StringBuilder sb, VCdVersion version)
    {
        Debug.Assert(value != null);
        Debug.Assert(sb is not null);

        if (!value.Contains('\\'))
        {
            return value;
        }

        _ = sb.Clear().Append(value).UnMask(version);

        return sb.Length != value.Length
                 ? sb.ToString()
                 : version == VCdVersion.V2_1
                       ? value
                       : sb.ToString();
    }


    /// <summary> Indicates whether <paramref name="s" /> needs 
    /// Quoted-Printable encoding.</summary>
    /// <param name="s">The <see cref="string" /> to check, or <c>null</c>.</param>
    /// <returns><c>true</c>, if <paramref name="s"/> contains NON-ASCII
    /// characters or line breaks, otherwise <c>false</c>.
    /// If <paramref name="s"/> is <c>null</c>, the method returns <c>false</c>.</returns>
    public static bool NeedsToBeQpEncoded(this string? s)
    {
        if (s is null)
        {
            return false;
        }

        var span = s.AsSpan();

        if (span.ContainsNewLine())
        {
            return true;
        }

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] > 126)
            {
                return true;
            }
        }

        return false;
    }

    [ExcludeFromCodeCoverage]
    internal static bool IsIetfLanguageTag(this string value)
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0 || NET6_0
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
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    [GeneratedRegex(IETF_LANGUAGE_TAG_PATTERN, RegexOptions.CultureInvariant, 50)]
    private static partial Regex IetfLanguageTagRegex();
#endif

}
