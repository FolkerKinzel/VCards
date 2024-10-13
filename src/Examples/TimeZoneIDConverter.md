# Example implementation of ITimeZoneIDConverter
vCard 4.0 [(RFC6350)](https://datatracker.ietf.org/doc/html/rfc6350) requires IANA time zone names to specify timezones, while older vCard standards use UTC offsets for this task. To preserve vCard 4.0 time zone information when converting data to a lower vCard version, IANA time zone names must be converted to UTC offsets. (Converting UTC offsets to IANA time zone names is not possible because the mapping is not unique.)

It's beyond the scope of `FolkerKinzel.VCards` to perform such a conversion. The `ITimeZoneIDConverter` interface allows to use third-party libraries for this task.

The example shows an implementation using the nuget package [TimeZoneConverter](https://www.nuget.org/packages/TimeZoneConverter/).

```csharp
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
```