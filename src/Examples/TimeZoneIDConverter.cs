using FolkerKinzel.VCards;

namespace Examples;

/// <summary>
/// Example implementation of <see cref="ITimeZoneIDConverter"/> to convert IANA time zone 
/// names into UTC offsets in order to be compatible to older vCard clients when writing 
/// vCard 2.1 or vCard 3.0.
/// This example uses the nuget package TimeZoneConverter 
/// ( https://www.nuget.org/packages/TimeZoneConverter/ ).
/// </summary>
public class TimeZoneIDConverter : ITimeZoneIDConverter
{
    // Converts, e.g., "Europe/Berlin" to +01:00
    public bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset)
    {
        // Convert at least IANA time zone names:
        if (TimeZoneConverter.TZConvert.TryGetTimeZoneInfo(timeZoneID, out TimeZoneInfo? tzInfo))
        {
            utcOffset = tzInfo.BaseUtcOffset;
            return true;
        }

        utcOffset = default;
        return false;
    }
}


