using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Globalization;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    /// <summary>
    /// Konvertiert das vCard-Value "Time".
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    internal class TimeConverter
    {
        private readonly string[] modelStrings = new string[]
        {
            "HH",
            "HHmm",
            "HHmmss",
            "HH:mm:ss", //vCard 2.1
            "HH:mm:sszzz", //vCard 2.1
            "HH:mm:sszz",
            "HHzz",
            "HHzzz",
            "HHmmzz",
            "HHmmzzz",
            "HHmmsszz",
            "HHmmsszzz",
            "-mmss",
            "-mmsszz",
            "-mmsszzz",
            "--ss",
            "--sszz",
            "--sszzz"
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

            return (DateTimeOffset.TryParseExact(s, modelStrings, CultureInfo.InvariantCulture, styles, out offset));
            //{
            //    var tmp = new DateTimeOffset();
            //    tmp += offset.Subtract(offset.Date);
            //    offset = tmp.ToLocalTime();
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


        internal static string ToTimeString(DateTimeOffset dt, VCdVersion version)
        {
            var builder = new StringBuilder();

            AppendTo(builder, dt, version);

            return builder.ToString();
        }


        internal static void AppendTo(StringBuilder builder, DateTimeOffset dt, VCdVersion version)
        {
            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}:{2:00}",
                            dt.Hour, dt.Minute, dt.Second);

                        TimeSpan utcOffset = dt.Offset;

                        if (utcOffset == TimeSpan.Zero)
                        {
                            builder.Append('Z');
                        }
                        else
                        {
                            string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                        }

                        break;
                    }
                default: // vCard 4.0
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, "{0:00}{1:00}{2:00}",
                            dt.Hour, dt.Minute, dt.Second);

                        TimeSpan utcOffset = dt.Offset;

                        if (utcOffset == TimeSpan.Zero)
                        {
                            builder.Append('Z');
                        }
                        else
                        {
                            string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                        }
                        break;
                    }
            }
        }
    }
}
