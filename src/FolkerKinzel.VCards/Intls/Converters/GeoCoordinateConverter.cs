using System.Globalization;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GeoCoordinateConverter
{
    private const string FLOAT_FORMAT = "0.0#####";
    internal const string U_PARAMETER = ";u=";

    internal static void AppendTo(StringBuilder builder, GeoCoordinate? coordinate, VCdVersion version)
    {
        Debug.Assert(builder is not null);

        if (coordinate is null)
        {
            return;
        }

        CultureInfo culture = CultureInfo.InvariantCulture;

        string latitude = coordinate.Latitude.ToString(FLOAT_FORMAT, culture);
        string longitude = coordinate.Longitude.ToString(FLOAT_FORMAT, culture);

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                _ = builder.Append(latitude).Append(';').Append(longitude);
                break;
            default:
                // "geo" URI contains a comma and sometimes a semicolon and should be masked.
                // In RFC 6350 the example is unmasked. (This is a verified error.)
                // The "Verifier notes" to https://www.rfc-editor.org/errata/eid3845
                // note that "the ABNF does not support escaping for URIs."
                // That's why the "geo" URI will remain unmasked.
                _ = builder.Append("geo:").Append(latitude).Append(',').Append(longitude);

                if (coordinate.Uncertainty.HasValue)
                {
                    _ = builder.Append(U_PARAMETER).Append(coordinate.Uncertainty.Value.ToString("0.", culture));
                }

                break;
        }//switch
    }
}
