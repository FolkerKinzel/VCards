// It's recommended to use a namespace-alias for better readability of
// your code and better usability of Visual Studio IntelliSense:
using VC = FolkerKinzel.VCards.Models;

namespace Examples;

/// <summary>
/// Example implementation of FolkerKinzel.VCards.Models.ITimeZoneIDConverter to convert IANA time zone names
/// into UTC offsets in order to be compatible to older vCard clients when writing vCard 2.1 or vCard 3.0.
/// This example uses the nuget package TimeZoneConverter. ( https://www.nuget.org/packages/TimeZoneConverter/ )
/// </summary>
public class TimeZoneIDConverter : VC::ITimeZoneIDConverter
{
    // Converts, e.g., "Europe/Berlin" to +01:00
    public bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset)
    {
        // Convert at least IANA time zone names:
        if (TimeZoneConverter.TZConvert.TryGetTimeZoneInfo(timeZoneID, out TimeZoneInfo tzInfo))
        {
            utcOffset = tzInfo.BaseUtcOffset;
            return true;
        }

        utcOffset = default;
        return false;
    }
}


