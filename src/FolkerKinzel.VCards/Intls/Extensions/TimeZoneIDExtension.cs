using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static partial class TimeZoneIDExtension
{
    private const string UTC_OFFSET_PATTERN = @"^[-\+]?[01][0-9]:?([0-5][0-9])?";

    [ExcludeFromCodeCoverage]
    public static bool IsUtcOffset(this TimeZoneID id)
    {
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0 || NET6_0
        try
        {
            return Regex.IsMatch(id.Value,
                                 UTC_OFFSET_PATTERN,
                                 RegexOptions.CultureInvariant,
                                 TimeSpan.FromMilliseconds(50));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
#else
        try
        {
            return UtcOffsetRegex().IsMatch(id.Value);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }


    [GeneratedRegex(UTC_OFFSET_PATTERN, RegexOptions.CultureInvariant, 50)]
    private static partial Regex UtcOffsetRegex();
#endif
}
