using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters;

/// <threadsafety static="true" instance="false" />
internal sealed class TimeConverter
{
    private readonly string[] _timeOnlyPatterns =
    [
            "HH",
            "HHmm",
            "HHmmss",
            "HH:mm:ss", //vCard 2.1
            "-mmss",
            "--ss",
    ];

    private readonly string[] _modelStrings =
    [
        // These patterns are needed if the input ends with 'Z'
        "HH",
        "HHmm",
        "HHmmss",
        "HH:mm:ss", //vCard 2.1
        "-mmss",
        "--ss",


        "HH:mm:sszzz", //vCard 2.1
        "HH:mm:sszz",
        "HHzz",
        "HHzzz",
        "HHmmzz",
        "HHmmzzz",
        "HHmmsszz",
        "HHmmsszzz",
        "-mmsszz",
        "-mmsszzz",
        "--sszz",
        "--sszzz"
    ];

    internal bool TryParse(ReadOnlySpan<char> s, [NotNullWhen(true)] out DateAndOrTime? dateAndOrTime)
    {
        dateAndOrTime = null;
        ReadOnlySpan<char> roSpan = s.Trim();

        if (roSpan.StartsWith('T'))
        {
            roSpan = roSpan.Slice(1);
        }

        DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;
        bool hasUtcIndicator = roSpan.EndsWith("Z", StringComparison.OrdinalIgnoreCase);

        if (hasUtcIndicator || ContainsUtcOffset(roSpan))
        {
            if (hasUtcIndicator)
            {
                roSpan = roSpan.Slice(0, roSpan.Length - 1);
                styles |= DateTimeStyles.AssumeUniversal;
            }
            else
            {
                styles |= DateTimeStyles.AssumeLocal;
            }

            if (_DateTimeOffset.TryParseExact(roSpan,
                                              _modelStrings,
                                              CultureInfo.InvariantCulture,
                                              styles,
                                              out DateTimeOffset dtOffset))
            {
                dtOffset = new DateTimeOffset(2, 1, 1, dtOffset.Hour, dtOffset.Minute, dtOffset.Second, dtOffset.Offset);
                dateAndOrTime = dtOffset;
                dateAndOrTime.HasDay = false;
                dateAndOrTime.HasMonth = false;
                return true;
            }
        }
        else
        {
            if (TimeOnly.TryParseExact(roSpan,
                                       _timeOnlyPatterns,
                                       CultureInfo.InvariantCulture,
                                       styles,
                                       out TimeOnly timeOnly))
            {
                dateAndOrTime = timeOnly;
                dateAndOrTime.HasDay = false;
                dateAndOrTime.HasMonth = false;
                return true;
            }
        }

        return false;

        static bool ContainsUtcOffset(ReadOnlySpan<char> span)
            => span.TrimStart('-').ContainsAny("+-");
    }

    internal static void AppendTimeTo(StringBuilder builder, TimeOnly dt, VCdVersion version)
    {
        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                             "{0:00}:{1:00}:{2:00}",
                                              dt.Hour, dt.Minute, dt.Second);

                    break;
                }
            default: // vCard 4.0
                {
                    _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                             "{0:00}{1:00}{2:00}",
                                             dt.Hour, dt.Minute, dt.Second);
                    break;
                }
        }
    }

    internal static void AppendTimeTo(StringBuilder builder, DateTimeOffset dt, VCdVersion version)
    {
        AppendTimeTo(builder, TimeOnly.FromTimeSpan(dt.TimeOfDay), version);

        TimeSpan utcOffset = dt.Offset;

        if (utcOffset == TimeSpan.Zero)
        {
            _ = builder.Append('Z');
        }
        else
        {
            // The '-' sign is added by the TimeSpan.ToString() method.
            string sign = utcOffset < TimeSpan.Zero ? "" : "+";

            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    {
                        _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                                 "{0}{1:00}:{2:00}",
                                                 sign, utcOffset.Hours, utcOffset.Minutes);
                        break;
                    }
                default: // vCard 4.0
                    {
                        _ = builder.AppendFormat(CultureInfo.InvariantCulture,
                                                 "{0}{1:00}:{2:00}",
                                                 sign, utcOffset.Hours, utcOffset.Minutes);
                        break;
                    }
            }
        }
    }
}
