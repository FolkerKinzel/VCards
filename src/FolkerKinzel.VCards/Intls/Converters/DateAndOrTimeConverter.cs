using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters;

/// <threadsafety static="true" instance="false" />
internal sealed class DateAndOrTimeConverter
{
    internal const int FIRST_LEAP_YEAR = 4;
    private const int MAX_DATE_TIME_STRING_LENGTH = 64;

    private static readonly DateTimeOffset _minYear = new DateTime(5, 1, 1, 14, 0, 0);

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

    internal bool TryParse(ReadOnlySpan<char> roSpan, [NotNullWhen(true)] out DateAndOrTime? dateAndOrTime)
    {
        Debug.Assert(!roSpan.StartsWith('T'));
        Debug.Assert(roSpan.Trim().Length == roSpan.Length);

        dateAndOrTime = null;

        // Test the length to avoid a StackOverflowException when stackalloc
        if (roSpan.Length > MAX_DATE_TIME_STRING_LENGTH)
        {
            return false;
        }

        // allowed only with vCard 4.0
        if (roSpan.StartsWith("---", StringComparison.Ordinal))
        {
            return TryParseDateNoReduc(roSpan, ref dateAndOrTime);
        }

        // allowed only with vCard 4.0
        if (roSpan.StartsWith("--", StringComparison.Ordinal))
        {
            // "--MM" to "0004-MM":
            // Note the use of YYYY-MM in the second example above. YYYYMM is
            // disallowed to prevent confusion with YYMMDD.
            if (roSpan.Length == 4)
            {
                return TryParseMonthWithoutYear(roSpan, ref dateAndOrTime);
            }

            // "--MMdd" to "0004MMdd" ("0004" + s.Substring(2))
            // Note also that --MM-DD is disallowed since vCard 4.0 uses the ISO 8601
            // basic format instead of the extended format.
            return TryParseMonthDayWithoutYear(roSpan, ref dateAndOrTime);
        }

        return TryParseInternal(roSpan,
                                ignoreYear: false,
                                ignoreMonth: roSpan.Length == 4, // yyyy (vCard 4.0 only)
                                ignoreDay: roSpan.Length is 4 or 7, // yyyy or yyyy-MM (vCard 4.0 only)
                                ref dateAndOrTime);
        
        //////////////////////////////////////////////////////////////////////////////////////

        bool TryParseDateNoReduc(ReadOnlySpan<char> roSpan, ref DateAndOrTime? dateAndOrTime)
        {
            roSpan = roSpan.Slice(3);
            ReadOnlySpan<char> firstLeapYearJanuary = "000401".AsSpan();

            Span<char> span = stackalloc char[firstLeapYearJanuary.Length + roSpan.Length];

            firstLeapYearJanuary.CopyTo(span);
            Span<char> slice = span.Slice(firstLeapYearJanuary.Length);
            roSpan.CopyTo(slice);

            return TryParseInternal(span,
                                    ignoreYear: true,
                                    ignoreMonth: true,
                                    ignoreDay: false,
                                    ref dateAndOrTime);
        }

        bool TryParseMonthWithoutYear(ReadOnlySpan<char> roSpan, ref DateAndOrTime? dateAndOrTime)
        {
            Debug.Assert(roSpan.StartsWith("--", StringComparison.Ordinal));
            Debug.Assert(roSpan.Length == 4);

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
                dateAndOrTime = DateAndOrTime.Create(dateOnly,
                                                     ignoreYear: true,
                                                     ignoreMonth: false,
                                                     ignoreDay: true);
                return true;
            }

            return false;
        }

        bool TryParseMonthDayWithoutYear(ReadOnlySpan<char> roSpan, ref DateAndOrTime? dateAndOrTime)
        {
            Debug.Assert(roSpan.StartsWith("--", StringComparison.Ordinal));

            roSpan = roSpan.Slice(2);
            ReadOnlySpan<char> leapYear = "0004".AsSpan();

            Span<char> span = stackalloc char[leapYear.Length + roSpan.Length];

            leapYear.CopyTo(span);
            Span<char> slice = span.Slice(leapYear.Length);
            roSpan.CopyTo(slice);

            return TryParseInternal(span,
                                    ignoreYear: true,
                                    ignoreMonth: false,
                                    ignoreDay: false,
                                    ref dateAndOrTime);
        }
    }

    private bool TryParseInternal(ReadOnlySpan<char> span,
                                  bool ignoreYear,
                                  bool ignoreMonth,
                                  bool ignoreDay,
                                  [NotNullWhen(true)] ref DateAndOrTime? daot)
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
                daot = DateAndOrTime.Create(dateOnly, ignoreYear, ignoreMonth, ignoreDay);
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
                daot = DateAndOrTime.Create(offset, ignoreYear, ignoreMonth, ignoreDay);
                return true;
            }
        }

        return false;

        //////////////////////////////////////////////////////////////

        static bool IsDateOnly(ReadOnlySpan<char> span) => !span.Contains('T');
    }

    internal static void AppendDateTo(StringBuilder builder,
                                      DateOnly dt,
                                      VCdVersion version,
                                      bool hasYear,
                                      bool hasMonth,
                                      bool hasDay)
    {
        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture,
                                                       "{0:0000}-{1:00}-{2:00}",
                                                        dt.Year, dt.Month, dt.Day);
                    break;
                }
            default: // vCard 4.0
                {
                    _ = hasYear
                        ? hasDay
                                ? builder.AppendFormat(CultureInfo.InvariantCulture,
                                                       "{0:0000}{1:00}{2:00}",
                                                       dt.Year, dt.Month, dt.Day)
                                : hasMonth
                                        ? builder.AppendFormat(CultureInfo.InvariantCulture,
                                                               "{0:0000}-{1:00}",
                                                               dt.Year, dt.Month)
                                        : builder.AppendFormat(CultureInfo.InvariantCulture,
                                                               "{0:0000}",
                                                               dt.Year)
                        : hasMonth
                                ? hasDay 
                                    ? builder.AppendFormat(CultureInfo.InvariantCulture,
                                                           "--{0:00}{1:00}",
                                                           dt.Month, dt.Day)
                                    : builder.AppendFormat(CultureInfo.InvariantCulture,
                                                           "--{0:00}",
                                                           dt.Month)
                                : builder.AppendFormat(CultureInfo.InvariantCulture,
                                                       "---{0:00}",
                                                       dt.Day);
                    break;
                }
        }//switch
    }

    internal static void AppendDateTimeOffsetTo(StringBuilder builder,
                                                DateTimeOffset dt,
                                                VCdVersion version,
                                                bool hasYear,
                                                bool hasMonth,
                                                bool hasDay)
    {
        if (hasMonth || hasDay || hasYear)
        {
            AppendDateTo(builder, DateOnly.FromDateTime(dt.Date), version, hasYear, hasMonth, hasDay);
        }

        if (HasTime(dt))
        {
            builder.Append('T');
            TimeConverter.AppendTimeTo(builder, dt, version);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasYear(DateTimeOffset dt) => dt > _minYear;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasYear(DateOnly value)
        => value.Year > DateAndOrTimeConverter.FIRST_LEAP_YEAR;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasTime(DateTimeOffset dt)
        => dt.TimeOfDay != TimeSpan.Zero || dt.Offset != TimeSpan.Zero;
}
