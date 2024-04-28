using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Intls.Converters;

/// <threadsafety static="true" instance="false" />
internal sealed class DateTimeConverter
{
    internal const int FIRST_LEAP_YEAR = 4;
    private const int MAX_DATE_TIME_STRING_LENGTH = 64;

    private static readonly DateTimeOffset _minYear = new DateTime(5, 1, 1, 14, 0, 0);
    private static readonly DateTimeOffset _minDate = new DateTime(3, 12, 31, 10, 0, 0);


    private readonly string[] _dateOnlyFormats =
    [
        "yyyyMMdd",
        "yyyy",
        "yyyy-MM",
        "yyyy-MM-dd"
    ];

    private readonly string[] _modelStrings =
    [
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

            // Handled by TimeConverter:

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
    ];

#if NET5_0_OR_GREATER
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
#endif
    internal bool TryParse(ReadOnlySpan<char> roSpan, out OneOf<DateOnly, DateTimeOffset> oneOf)
    {
        Debug.Assert(!roSpan.StartsWith('T'));
        Debug.Assert(roSpan.Trim().Length == roSpan.Length);

        oneOf = default;

        // Test the length to avoid a StackOverflowException when stackalloc
        if (roSpan.Length > MAX_DATE_TIME_STRING_LENGTH)
        {
            return false;
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

            if (TryParseInternal(span, ref oneOf))
            {
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

                if (DateOnly.TryParseExact(span,
                                           _dateOnlyFormats,
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.AllowWhiteSpaces,
                                           out DateOnly dateOnly))
                {
                    oneOf = dateOnly;
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

                if (TryParseInternal(span, ref oneOf))
                {
                    return true;
                }
            }
        }
        else
        {
            if (TryParseInternal(roSpan, ref oneOf))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryParseInternal(ReadOnlySpan<char> span, ref OneOf<DateOnly, DateTimeOffset> oneOf)
    {
        DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

        if (IsDateOnly(span))
        {
            if (DateOnly.TryParseExact(span,
                                       _dateOnlyFormats,
                                       CultureInfo.InvariantCulture,
                                       styles,
                                       out DateOnly dateOnly))
            {
                oneOf = dateOnly;
                return true;
            }
        }
        else
        {
            if (span.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                span = span.Slice(0, span.Length - 1);
                styles |= DateTimeStyles.AssumeUniversal;
            }
            else
            {
                styles |= DateTimeStyles.AssumeLocal;
            }

            if (_DateTimeOffset.TryParseExact(span,
                                              _modelStrings,
                                              CultureInfo.InvariantCulture,
                                              styles,
                                              out DateTimeOffset offset))
            {
                oneOf = offset;
                return true;
            }
        }

        return false;

        //////////////////////////////////////////////////////////////

        static bool IsDateOnly(ReadOnlySpan<char> span) => !span.Contains('T');
    }

    internal static void AppendTimeStampTo(StringBuilder builder,
        DateTimeOffset dto, VCdVersion version)
    {
        DateTimeOffset dt = dto.ToUniversalTime();

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                         "{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}Z",
                                          dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                break;
            default:
                _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                         "{0:0000}{1:00}{2:00}T{3:00}{4:00}{5:00}Z",
                                          dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                break;
        }

    }

    internal static void AppendDateTo(StringBuilder builder,
                                      DateOnly dt,
                                      VCdVersion version)
    {
        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    _ = dt.HasYear()
                        ? builder.AppendFormat(CultureInfo.InvariantCulture,
                                               "{0:0000}-{1:00}-{2:00}",
                                                dt.Year, dt.Month, dt.Day)
                        : builder.AppendFormat(CultureInfo.InvariantCulture,
                                               "--{0:00}-{1:00}",
                                               dt.Month, dt.Day);
                    break;
                }
            default: // vCard 4.0
                {
                    _ = dt.HasYear()
                        ? builder.AppendFormat(CultureInfo.InvariantCulture,
                                               "{0:0000}{1:00}{2:00}",
                                               dt.Year, dt.Month, dt.Day)
                        : builder.AppendFormat(CultureInfo.InvariantCulture,
                                               "--{0:00}{1:00}",
                                               dt.Month, dt.Day);
                    break;
                }
        }//switch
    }

    internal static void AppendDateAndOrTimeTo(StringBuilder builder,
                                               DateTimeOffset dt,
                                               VCdVersion version)
    {
        if (HasDate(dt))
        {
            AppendDateTo(builder, DateOnly.FromDateTime(dt.Date), version);
        }

        if (HasTime(dt))
        {
            builder.Append('T');
            TimeConverter.AppendTimeTo(builder, dt, version);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasDate(DateTimeOffset dt) => dt > _minDate;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasYear(DateTimeOffset dt) => dt > _minYear;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasTime(DateTimeOffset dt)
        => dt.TimeOfDay != TimeSpan.Zero || dt.Offset != TimeSpan.Zero;

}
