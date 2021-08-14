using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models
{
    public class NamedTimeZone
    {
        private readonly string[] _patterns = new string[] { @"hh\:mm", @"hhmm", @"hh" };

        /// <summary>
        /// Initialisiert ein neues <see cref="NamedTimeZone"/>-Objekt.
        /// </summary>
        /// <param name="timeZoneID">Bezeichner der Zeitzone. Es sollte sich um einen Bezeichner aus der 
        /// "IANA Time Zone Database" handeln. (Siehe https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .)</param>
        /// <exception cref="ArgumentNullException"><paramref name="timeZoneID"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="timeZoneID"/> ist ein leerer <see cref="string"/> oder 
        /// enthält nur Leerraum.</exception>
        public NamedTimeZone(string timeZoneID)
        {
            TimeZoneID = timeZoneID ?? throw new ArgumentNullException(nameof(timeZoneID));

            if(string.IsNullOrWhiteSpace(TimeZoneID))
            {
                throw new ArgumentException(Res.NoData, nameof(timeZoneID));
            }

            TimeZoneID = Regex.Replace(TimeZoneID, @"\s+", "");

        }


        /// <summary>
        /// ID der Zeitzone.
        /// </summary>
        /// <remarks>
        /// Das ist der <see cref="string"/>, mit dem das <see cref="NamedTimeZone"/>-Objekt initialisiert wurde - von Leerraum befreit.
        /// </remarks>
        public string TimeZoneID { get; }


        public bool TryGetUtcOffset(out TimeSpan utcOffset, INamedTimeZoneConverter? converter = null)
        {
            if (IsUtcOffset())
            {
                int startIndex = 0;

                TimeSpanStyles styles = TimeSpanStyles.None;


                if (TimeZoneID.StartsWith("-", StringComparison.Ordinal))
                {
                    startIndex = 1;
                    styles = TimeSpanStyles.AssumeNegative;
                }
                else if (TimeZoneID.StartsWith("+", StringComparison.Ordinal))
                {
                    startIndex = 1;
                }

#if NET40
                string input = TimeZoneID.Substring(startIndex);
#else
                ReadOnlySpan<char> input = TimeZoneID.AsSpan(startIndex);
#endif

                return TimeSpan.TryParseExact(
                    input,
                    _patterns,
                    CultureInfo.InvariantCulture,
                    styles, out utcOffset);
            }

            if(converter is not null && converter.TryGetUtcOffset(TimeZoneID, out utcOffset))
            {
                return true;
            }

            try
            {
                var tzInfo =  TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                utcOffset = tzInfo.BaseUtcOffset;
                return true;
            }
            catch { }

            utcOffset = default;
            return false;
        }


        internal void AppendTo(StringBuilder builder, VCdVersion version, INamedTimeZoneConverter? converter)
        {
            Debug.Assert(builder != null);

            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    {
                        if (TryGetUtcOffset(out TimeSpan timeSpan, converter))
                        {
                            string format = timeSpan < TimeSpan.Zero ? @"\-hh\:mm" : @"\+hh\:mm";
                            _ = builder.Append(timeSpan.ToString(format, CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            _ = builder.Append(TimeZoneID);
                        }
                        break;
                    }
                default:
                    {
                        if (IsUtcOffset() && TryGetUtcOffset(out TimeSpan timeSpan))
                        {
                            string format = timeSpan < TimeSpan.Zero ? @"\-hhmm" : @"\+hhmm";
                            _ = builder.Append(timeSpan.ToString(format, CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            _ = builder.Append(TimeZoneID);
                        }
                        break;
                    }
            }
        }

        


        private bool IsUtcOffset()
        {
            const string pattern = @"^[-\+]?[01][0-9]:?([0-5][0-9])?";
            const RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline;

#if NET40
                return Regex.IsMatch(TimeZoneID, pattern, options);
#else
            try
            {
                return Regex.IsMatch(TimeZoneID, pattern, options, TimeSpan.FromMilliseconds(50));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
#endif
        }
    }
}
