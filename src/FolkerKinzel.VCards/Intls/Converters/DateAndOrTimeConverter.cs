using System;
using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Intls.Converters;

/// <summary>
/// Konvertiert die vCard-Values Date, Date-Time, Date-And-Or-Time und Timestamp.
/// </summary>
/// <threadsafety static="true" instance="false" />
internal sealed class DateAndOrTimeConverter
{
    private const int FIRST_LEAP_YEAR = 4;
    private const int MAX_DATE_TIME_STRING_LENGTH = 64;

    private readonly string[] _modelStrings = new string[]
    {
            "yyyyMMdd",
            "yyyy",
            "yyyy-MM",
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyyMMddTHH",
            "yyyyMMddTHHmm",
            "yyyyMMddTHHmmss",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmsszzz",
            //"THH",
            //"THHmm",
            //"THHmmss",
            //"THHzz",
            //"THHzzz",
            //"THHmmzz",
            //"THHmmzzz",
            //"THHmmsszz",
            //"THHmmsszzz",
            //"T-mmss",
            //"T-mmsszz",
            //"T-mmsszzz",
            //"T--ss",
            //"T--sszz",
            //"T--sszzz"
    };

#if NET5_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
#endif
    internal bool TryParse(ReadOnlySpan<char> roSpan, out OneOf<DateOnly, DateTime, DateTimeOffset> oneOf)
    {
        oneOf = default;

        // Test auf Länge nötig, um StackOverflowException auszuschließen
        if (roSpan.Length > MAX_DATE_TIME_STRING_LENGTH)
        {
            return false;
        }

        DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

        Debug.Assert(roSpan.Length <= MAX_DATE_TIME_STRING_LENGTH);

        bool hasUtcIndicator = roSpan.EndsWith("Z", StringComparison.OrdinalIgnoreCase);

        if (hasUtcIndicator)
        {
            roSpan = roSpan.Slice(0, roSpan.Length - 1);

            styles |= DateTimeStyles.AssumeUniversal;
        }
        else
        {
            styles |= DateTimeStyles.AssumeLocal;
        }

        // date-noreduc zu date-complete
        if (roSpan.StartsWith("---", StringComparison.Ordinal))
        {
            roSpan = roSpan.Slice(3);
            ReadOnlySpan<char> firstLeapYearJanuary = "000401".AsSpan();

            Span<char> span = stackalloc char[firstLeapYearJanuary.Length + roSpan.Length];

            firstLeapYearJanuary.CopyTo(span);
            Span<char> slice = span.Slice(firstLeapYearJanuary.Length);
            roSpan.CopyTo(slice);

            if (_DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out DateTimeOffset offset))
            {
                oneOf = hasUtcIndicator || offset.Offset != TimeSpan.Zero || roSpan.ContainsUtcOffset()
                       ? offset
                       : IsDateOnly(span) ? DateOnly.FromDateTime(offset.Date) : offset.DateTime;
                return true;
            }
        }
        else if (roSpan.StartsWith("--", StringComparison.Ordinal))
        {
            // "--MM" zu "0004-MM":
            // Note the use of YYYY-MM in the second example above.  YYYYMM is
            // disallowed to prevent confusion with YYMMDD.
            if (roSpan.Length == 4)
            {
                roSpan = roSpan.Slice(2);
                ReadOnlySpan<char> leapYear = "0004-".AsSpan();

                Span<char> span = stackalloc char[leapYear.Length + roSpan.Length];

                leapYear.CopyTo(span);
                Span<char> slice = span.Slice(leapYear.Length);
                roSpan.CopyTo(slice);

                if(_DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out DateTimeOffset offset))
                {
                    oneOf = hasUtcIndicator || offset.Offset != TimeSpan.Zero || roSpan.ContainsUtcOffset()
                           ? offset
                           : IsDateOnly(span) ? DateOnly.FromDateTime(offset.Date) : offset.DateTime;
                    return true;
                }
            }
            else
            {
                // "--MMdd" zu "0004MMdd" ("0004" + s.Substring(2))
                // Note also that YYYY-MM-DD is disallowed since we are using the basic format instead
                // of the extended format.
                roSpan = roSpan.Slice(2);
                ReadOnlySpan<char> leapYear = "0004".AsSpan();

                Span<char> span = stackalloc char[leapYear.Length + roSpan.Length];

                leapYear.CopyTo(span);
                Span<char> slice = span.Slice(leapYear.Length);
                roSpan.CopyTo(slice);

                if (_DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out DateTimeOffset offset))
                {
                    oneOf = hasUtcIndicator || offset.Offset != TimeSpan.Zero || roSpan.ContainsUtcOffset()
                           ? offset
                           : IsDateOnly(span) ? DateOnly.FromDateTime(offset.Date) : offset.DateTime;
                    return true;
                }
            }
        }
        else
        {
            if (_DateTimeOffset.TryParseExact(roSpan, _modelStrings, CultureInfo.InvariantCulture, styles, out DateTimeOffset offset))
            {
                oneOf = hasUtcIndicator || offset.Offset != TimeSpan.Zero || roSpan.ContainsUtcOffset()
                       ? offset
                       : IsDateOnly(roSpan) ? DateOnly.FromDateTime(offset.Date) : offset.DateTime;
                return true;
            }
        }

        return false;
    }


    private bool IsDateOnly(ReadOnlySpan<char> span)
    {
        const int maxDateOnly = 8;
        int counter = 0;
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i].IsDecimalDigit())
            {
                counter++;
            }

            if(counter > maxDateOnly)
            {
                return false;
            }
        }
        return true;
    }


    internal static void AppendTimeStampTo(StringBuilder builder,
        DateTimeOffset? dto, VCdVersion version)
    {
        if (!dto.HasValue)
        {
            return;
        }

        DateTimeOffset dt = dto.Value.ToUniversalTime();

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}Z",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                break;
            default:
                _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}{1:00}{2:00}T{3:00}{4:00}{5:00}Z",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                break;
        }

    }


    internal static void AppendDateTimeStringTo(StringBuilder builder,
        DateTimeOffset? dto, VCdVersion version)
    {
        if (!dto.HasValue)
        {
            return;
        }

        DateTimeOffset dt = dto.Value;

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    _ = dt.Year >= FIRST_LEAP_YEAR
                        ? builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day)
                        : builder.AppendFormat(CultureInfo.InvariantCulture, "--{0:00}-{1:00}", dt.Month, dt.Day);

                    TimeSpan utcOffset = dt.Offset;

                    if (HasTimeComponent(dt))
                    {
                        _ = builder.AppendFormat(CultureInfo.InvariantCulture, "T{0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);

                        if (utcOffset == TimeSpan.Zero)
                        {
                            _ = builder.Append('Z');
                        }
                        else
                        {
                            string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                            _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                        }
                    }
                    break;
                }
            default: // vCard 4.0
                {
                    _ = dt.Year >= FIRST_LEAP_YEAR
                        ? builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}{1:00}{2:00}", dt.Year, dt.Month, dt.Day)
                        : builder.AppendFormat(CultureInfo.InvariantCulture, "--{0:00}{1:00}", dt.Month, dt.Day);

                    TimeSpan utcOffset = dt.Offset;

                    if (HasTimeComponent(dt))
                    {
                        _ = builder.AppendFormat(CultureInfo.InvariantCulture, "T{0:00}{1:00}{2:00}", dt.Hour, dt.Minute, dt.Second);

                        if (utcOffset == TimeSpan.Zero)
                        {
                            _ = builder.Append('Z');
                        }
                        else
                        {
                            string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                            _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                        }
                    }
                    break;
                }
        }//switch
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasTimeComponent(DateTimeOffset dt)
        => dt.TimeOfDay != TimeSpan.Zero;
    //|| utcOffset != TimeSpan.Zero //nicht konsequent, aber sonst bei Geburtstagen meist komisch

}
