using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Encodings;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class StringExtension
{
    /// <summary> Indicates whether <paramref name="s" /> needs 
    /// Quoted-Printable encoding.</summary>
    /// <param name="s">The <see cref="string" /> to check or <c>null</c>.</param>
    /// <returns><c>true</c>, if <paramref name="s"/> contains NON-ASCII
    /// characters or control characters, otherwise <c>false</c>.
    /// If <paramref name="s"/> is <c>null</c>, the method returns <c>false</c>.</returns>
    internal static bool NeedsToBeQpEncoded(this string? s)
    {
        ReadOnlySpan<char> span = s.AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            int c = span[i];

            if (c is > 126 or < 32)
            {
                return true;
            }
        }

        return false;
    }

    internal static bool ContainsAnyThatNeedsQpEncoding(this string[] strings)
    {
        foreach (string s in strings.AsSpan())
        {
            if (NeedsToBeQpEncoded(s))
            {
                return true;
            }
        }

        return false;
    }
}
