using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Globalization;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    /// <summary>
    /// Konvertiert die vCard-Values Date, Date-Time, Date-And-Or-Time und Timestamp.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    internal class DateAndOrTimeConverter
    {
        private readonly string[] modelStrings = new string[]
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



        internal bool TryParse(string? s, out DateTimeOffset offset)
        {
            offset = DateTimeOffset.MinValue;
            
            if (s is null)
            {
                return false;
            }

            //s = s.Trim();

            DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

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
                if (s.Length == 4) // "--MM" zu "yyyy-MM"
                {
                    s = "0004-" + s.Substring(2);
                }
                else
                {
                    s = "0004" + s.Substring(2);
                }
            }


            return DateTimeOffset.TryParseExact(s, modelStrings, CultureInfo.InvariantCulture, styles, out offset);
        }


        internal static string ToDateTimeString(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();

            AppendDateTimeStringTo(builder, dt, version);

            return builder.ToString();
        }

        internal static string ToTimestamp(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();

            AppendTimestampTo(builder, dt, version);

            return builder.ToString();
        }


        internal static void AppendTimestampTo(StringBuilder builder,
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
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}Z",
                        dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                    break;
                default:
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}{1:00}{2:00}T{3:00}{4:00}{5:00}Z",
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
                        if (dt.Year > 4) // erstes Schaltjahr
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}-{1:00}-{2:00}", dt.Year, dt.Month, dt.Day);
                        }
                        else
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "--{0:00}-{1:00}", dt.Month, dt.Day);
                        }

                        TimeSpan utcOffset = dt.Offset;

                        if (HasTimeComponent(dt))
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "T{0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);

                            if (utcOffset == TimeSpan.Zero)
                            {
                                builder.Append('Z');
                            }
                            else
                            {
                                string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                                builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                            }
                        }
                        break;
                    }
                default: // vCard 4.0
                    {
                        if (dt.Year > 4) // erstes Schaltjahr
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "{0:0000}{1:00}{2:00}", dt.Year, dt.Month, dt.Day);
                        }
                        else
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "--{0:00}{1:00}", dt.Month, dt.Day);
                        }

                        TimeSpan utcOffset = dt.Offset;

                        if (HasTimeComponent(dt))
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "T{0:00}{1:00}{2:00}", dt.Hour, dt.Minute, dt.Second);

                            if (utcOffset == TimeSpan.Zero)
                            {
                                builder.Append('Z');
                            }
                            else
                            {
                                string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                                builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                            }
                        }
                        break;
                    }
            }//switch
        }


        internal static bool HasTimeComponent(DateTimeOffset? dt) 
            => dt.HasValue && (dt.Value.TimeOfDay != TimeSpan.Zero);
              //|| utcOffset != TimeSpan.Zero //nicht konsequent, aber sonst bei Geburtstagen meist komisch

    }
}
