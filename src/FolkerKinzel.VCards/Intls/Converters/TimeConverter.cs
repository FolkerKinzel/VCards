using System.Globalization;
using FolkerKinzel.VCards.Intls.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Intls.Converters;

/// <summary>
/// Konvertiert das vCard-Value "Time".
/// </summary>
/// <threadsafety static="true" instance="false" />
internal sealed class TimeConverter
{
    private readonly string[] _modelStrings = new string[]
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


    internal bool TryParse(ReadOnlySpan<char> s, out OneOf<TimeOnly, DateTimeOffset> oneOf)
    {
        oneOf = default;

        DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

        ReadOnlySpan<char> roSpan = s.Trim();

        if(roSpan.StartsWith('T'))
        {
            roSpan = roSpan.Slice(1);
        }

        bool hasUtcIndicator = roSpan.EndsWith("Z", StringComparison.OrdinalIgnoreCase);

        if (hasUtcIndicator)
        {
            roSpan = roSpan.Slice(0, roSpan.Length - 1);
            styles |= DateTimeStyles.AssumeUniversal;
        }
        else
        {
            styles |= DateTimeStyles.AssumeLocal;
        }

        if (_DateTimeOffset.TryParseExact(roSpan, _modelStrings, CultureInfo.InvariantCulture, styles, out DateTimeOffset dtOffset))
        {
            oneOf = hasUtcIndicator || dtOffset.Offset != TimeSpan.Zero || roSpan.ContainsUtcOffset()
                   ? dtOffset 
                   : TimeOnly.FromTimeSpan(dtOffset.TimeOfDay);
            return true;
        }
        return false;

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
                    _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}:{2:00}",
                        dt.Hour, dt.Minute, dt.Second);

                    TimeSpan utcOffset = dt.Offset;

                    if (utcOffset == TimeSpan.Zero)
                    {
                        _ = builder.Append('Z');
                    }
                    else
                    {
                        string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                        _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                    }

                    break;
                }
            default: // vCard 4.0
                {
                    _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0:00}{1:00}{2:00}",
                        dt.Hour, dt.Minute, dt.Second);

                    TimeSpan utcOffset = dt.Offset;

                    if (utcOffset == TimeSpan.Zero)
                    {
                        _ = builder.Append('Z');
                    }
                    else
                    {
                        string sign = utcOffset < TimeSpan.Zero ? "" : "+";

                        _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1:00}:{2:00}", sign, utcOffset.Hours, utcOffset.Minutes);
                    }
                    break;
                }
        }
    }
}
