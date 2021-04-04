﻿using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class TimeZoneInfoConverter
    {
        internal static TimeZoneInfo? Parse(string? value)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(value);
            }
            catch { }

            TimeSpan timeSpan;

            if (
#if NET40
                value.Contains(":"))
#else
             
                value.Contains(':', StringComparison.Ordinal)) // vCard 3.0
#endif
            {
                if (value.StartsWith("-", StringComparison.Ordinal))
                {
                    if (TimeSpan.TryParseExact(
                        value,
                        new string[] { @"\-hh\:mm" },
                        CultureInfo.InvariantCulture,
                        TimeSpanStyles.AssumeNegative, out timeSpan))
                    {
                        goto done;
                    }
                }
                else if (TimeSpan.TryParseExact(
                        value,
                        new string[] { @"\+hh\:mm" },
                        CultureInfo.InvariantCulture,
                        TimeSpanStyles.None, out timeSpan))
                {
                    goto done;
                }
                
            }
            else if (IsUtcTimespan(value)) // UTC-Timespan
            {
                if (value.StartsWith("-", StringComparison.Ordinal))
                {
                    if (TimeSpan.TryParseExact(
                        value,
                        new string[] { @"\-hhmm", @"\-hh" },
                        CultureInfo.InvariantCulture,
                        TimeSpanStyles.AssumeNegative, out timeSpan))
                    {
                        goto done;
                    }
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

        private static bool IsUtcTimespan(string value)
        {
            const string pattern = @"^[-\+][01][0-9]([0-5][0-9])?";
            const RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline;

#if NET40
                return Regex.IsMatch(value, pattern, options);
#else
            try
            {
                return Regex.IsMatch(value, pattern, options, TimeSpan.FromMilliseconds(50));
            }
            catch(RegexMatchTimeoutException)
            {
                return false;
            }
#endif
        }

        internal static void AppendTo(StringBuilder builder, TimeZoneInfo tzInfo, VCdVersion version)
        {
            Debug.Assert(builder != null);
            Debug.Assert(tzInfo != null);

            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    TimeSpan timeSpan = tzInfo.BaseUtcOffset;
                    string format = timeSpan < TimeSpan.Zero ? @"\-hh\:mm" : @"hh\:mm";
                    _ = builder.Append(timeSpan.ToString(format, CultureInfo.InvariantCulture));
                    break;
                default:
                    _ = builder.Append(tzInfo.Id); // Wenn ein URI-Schema eingeführt wird, muss ParameterSerializer4_0.AppendTz() geändert werden
                    break;
            }
        }
    }
}
