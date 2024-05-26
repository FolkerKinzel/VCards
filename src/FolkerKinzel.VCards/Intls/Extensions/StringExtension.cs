using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class StringExtension
{
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

        ReadOnlySpan<char> span = s.AsSpan();

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
}
