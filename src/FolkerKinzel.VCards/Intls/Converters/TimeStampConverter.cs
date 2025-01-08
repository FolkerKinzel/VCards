using System.Globalization;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal sealed class TimeStampConverter
{
    private readonly string[] _timeStampFormats =
        [
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyyMMddTHHmmss",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmsszzz"
        ];

    internal bool TryParse(ReadOnlySpan<char> span, out DateTimeOffset dto)
    {
        DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces;

        if (span.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
        {
            span = span.Slice(0, span.Length - 1);
            styles |= DateTimeStyles.AssumeUniversal;
        }
        else
        {
            styles |= DateTimeStyles.AssumeLocal;
        }

        return _DateTimeOffset.TryParseExact(span,
                                             _timeStampFormats,
                                             CultureInfo.InvariantCulture,
                                             styles,
                                             out dto);
    }

    internal static void AppendTo(StringBuilder builder,
        DateTimeOffset dto, VCdVersion version)
    {
        DateTimeOffset dt = dto.ToUniversalTime();

        _ = version switch
        {
            VCdVersion.V2_1 or VCdVersion.V3_0 => builder.AppendFormat(CultureInfo.InvariantCulture,
                                                     "{0:0000}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}Z",
                                                      dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second),
            _ => builder.AppendFormat(CultureInfo.InvariantCulture,
                                                     "{0:0000}{1:00}{2:00}T{3:00}{4:00}{5:00}Z",
                                                      dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second),
        };
    }
}
