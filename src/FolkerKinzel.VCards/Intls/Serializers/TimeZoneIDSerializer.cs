using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal static class TimeZoneIDSerializer
{
    internal static void AppendTo(
        StringBuilder builder, TimeZoneID tzID, VCdVersion version, ITimeZoneIDConverter? converter, bool asParameter)
    {
        if (tzID.IsEmpty)
        {
            return;
        }

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    if (tzID.TryGetUtcOffset(out TimeSpan utcOffset, converter))
                    {
                        string format = utcOffset < TimeSpan.Zero ? @"\-hh\:mm" : @"\+hh\:mm";
                        _ = builder.Append(utcOffset.ToString(format, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        _ = builder.Append(tzID.Value);
                    }
                    break;
                }
            default:
                {
                    if (tzID.IsUtcOffset() && tzID.TryGetUtcOffset(out TimeSpan utcOffset))
                    {
                        string format = utcOffset < TimeSpan.Zero ? @"\-hhmm" : @"\+hhmm";
                        _ = builder.Append(utcOffset.ToString(format, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        _ = asParameter ? builder.AppendParameterValueEscapedAndQuoted(tzID.Value, version)
                                        : builder.Append(tzID.Value);
                    }
                    break;
                }
        }
    }
}