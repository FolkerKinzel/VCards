using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal class TimeZoneConverter
    {
        private readonly string[] _patterns = new string[] { @"hh\:mm", @"hhmm", @"hh" };

        internal TimeZoneInfo? Parse(string? value)
        {
            if (value is null)
            {
                return null;
            }

            if (IsUtcOffset(value))
            {
                int startIndex = 0;

                TimeSpanStyles styles = TimeSpanStyles.None;


                if (value.StartsWith("-", StringComparison.Ordinal))
                {
                    startIndex = 1;
                    styles = TimeSpanStyles.AssumeNegative;
                }
                else if (value.StartsWith("+", StringComparison.Ordinal))
                {
                    startIndex = 1;
                }

#if NET40
                string input = value.Substring(startIndex);
#else
                ReadOnlySpan<char> input = value.AsSpan(startIndex);
#endif

                if (TimeSpan.TryParseExact(
                    input,
                    _patterns,
                    CultureInfo.InvariantCulture,
                    styles, out TimeSpan timeSpan))
                {
                    return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.BaseUtcOffset == timeSpan);
                }
            }


            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(value);
            }
            catch { }

            return null;
        }


        internal static bool IsUtcOffset(string value)
        {
            const string pattern = @"^[-\+]?[01][0-9]:?([0-5][0-9])?";
            const RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline;

#if NET40
                return Regex.IsMatch(value, pattern, options);
#else
            try
            {
                return Regex.IsMatch(value, pattern, options, TimeSpan.FromMilliseconds(50));
            }
            catch (RegexMatchTimeoutException)
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
                    string format = timeSpan < TimeSpan.Zero ? @"\-hh\:mm" : @"\+hh\:mm";
                    _ = builder.Append(timeSpan.ToString(format, CultureInfo.InvariantCulture));
                    break;
                default:
                    _ = builder.Append(tzInfo.Id); // Wenn ein URI-Schema eingeführt wird, muss ParameterSerializer4_0.AppendTz() geändert werden
                    break;
            }
        }
    }
}
