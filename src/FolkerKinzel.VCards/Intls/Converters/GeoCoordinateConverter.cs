using System.Globalization;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GeoCoordinateConverter
{
    internal static void AppendTo(StringBuilder builder, GeoCoordinate? coordinate, VCdVersion version)
    {
        Debug.Assert(builder != null);

        if (coordinate is null)
        {
            return;
        }

        CultureInfo culture = CultureInfo.InvariantCulture;

        string latitude = coordinate.Latitude.ToString("F6", culture);
        string longitude = coordinate.Longitude.ToString("F6", culture);

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                _ = builder.Append(latitude).Append(';').Append(longitude);
                break;
            default:
                _ = builder.Append("geo:").Append(latitude).Append(',').Append(longitude);
                break;
        }//switch
    }
}
