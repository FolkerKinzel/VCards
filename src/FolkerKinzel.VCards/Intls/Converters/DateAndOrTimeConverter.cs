using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.CompilerServices;

#if !NET40
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Intls.Converters
{
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
            "THH",
            "THHmm",
            "THHmmss",
            "THHzz",
            "THHzzz",
            "THHmmzz",
            "THHmmzzz",
            "THHmmsszz",
            "THHmmsszzz",
            "T-mmss",
            "T-mmsszz",
            "T-mmsszzz",
            "T--ss",
            "T--sszz",
            "T--sszzz"
        };

#if !NET40
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
#endif
        internal bool TryParse(string? s, out DateTimeOffset offset)
        {

            offset = DateTimeOffset.MinValue;

            // Test auf Länge nötig, um StackOverflowException auszuschließen
            if (s is null || s.Length > MAX_DATE_TIME_STRING_LENGTH)
            {
                return false;
            }

            DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

#if NET40
            if (s.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(0, s.Length - 1);

                styles |= DateTimeStyles.AssumeUniversal;
            }
            else
            {
                styles |= DateTimeStyles.AssumeLocal;
            }

            // date-noreduc zu date-complete
            if (s.StartsWith("---", StringComparison.Ordinal))
            {
                s = "000401" + s.Substring(3); // 4 ist das erste Schaltjahr!
            }
            else if (s.StartsWith("--", StringComparison.Ordinal))
            {
                s = s.Length == 4 
                ? "0004-" + s.Substring(2) // "--MM" zu "0004-MM"
                : "0004" + s.Substring(2); // "--MMdd" zu "0004MMdd"
            }


            return DateTimeOffset.TryParseExact(s, _modelStrings, CultureInfo.InvariantCulture, styles, out offset);

#else
            Debug.Assert(s.Length <= MAX_DATE_TIME_STRING_LENGTH);

            ReadOnlySpan<char> roSpan = s.AsSpan();
            if (s.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
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

#if NET461 || NETSTANDARD2_0
                return DateTimeOffset.TryParseExact(span.ToString(), _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#else
                return DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#endif
            }
            else if (s.StartsWith("--", StringComparison.Ordinal))
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

#if NET461 || NETSTANDARD2_0
                    return DateTimeOffset.TryParseExact(span.ToString(), _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#else
                    return DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#endif
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

#if NET461 || NETSTANDARD2_0
                    return DateTimeOffset.TryParseExact(span.ToString(), _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#else
                    return DateTimeOffset.TryParseExact(span, _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#endif
                }
            }
            else
            {
#if NET461 || NETSTANDARD2_0
                return DateTimeOffset.TryParseExact(roSpan.ToString(), _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#else
                return DateTimeOffset.TryParseExact(roSpan, _modelStrings, CultureInfo.InvariantCulture, styles, out offset);
#endif
            }
#endif
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

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static bool HasTimeComponent(DateTimeOffset dt)
            => dt.TimeOfDay != TimeSpan.Zero;
        //|| utcOffset != TimeSpan.Zero //nicht konsequent, aber sonst bei Geburtstagen meist komisch

    }
}
