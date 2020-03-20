using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FolkerKinzel.VCards.Intls.Converters
{
    static class TimeZoneInfoConverter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Keine allgemeinen Ausnahmetypen abfangen", Justification = "<Ausstehend>")]
        internal static TimeZoneInfo? Parse(string? value)
        {
            if (value is null) return null;

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(value);
            }
            catch { }


            if (
#if NET40
                value.Contains(":") 
#else
                value.Contains(':', StringComparison.Ordinal)
#endif
                && TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan timeSpan)) // vCard 3.0
            {
                goto done;
            }
            else if (Regex.IsMatch(value, @"^[\+-][01][0-4]([0-5][0-9])?")) // UTC-Timespan
            {
                if (value.StartsWith("-", StringComparison.Ordinal))
                {
                    if (TimeSpan.TryParseExact(
                        value,
                        new string[] { @"\-hhmm", @"\-hh" },
                        CultureInfo.InvariantCulture,
                        TimeSpanStyles.AssumeNegative, out timeSpan)) goto done;
                }
                else if (TimeSpan.TryParseExact(
                        value,
                        new string[] { @"\+hh", @"\+hhmm" },
                        CultureInfo.InvariantCulture,
                        TimeSpanStyles.None, out timeSpan))
                {
                    goto done;
                }
            }

            return null;


        done:
            return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.BaseUtcOffset == timeSpan);
        }



        internal static void AppendTo(StringBuilder builder, TimeZoneInfo tzInfo, VCdVersion version)
        {
            Debug.Assert(builder != null);
            Debug.Assert(tzInfo != null);

            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    var timeSpan = tzInfo.BaseUtcOffset;
                    string format = timeSpan < TimeSpan.Zero ? @"\-hh\:mm" : @"hh\:mm";
                    builder.Append(timeSpan.ToString(format, CultureInfo.InvariantCulture));
                    break;
                default:
                    builder.Append(tzInfo.Id); // Wenn ein URI-Schema eingeführt wird, muss ParameterSerializer4_0.AppendTz() geändert werden
                    break;
            }
        }
    }
}
