using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class GeoCoordinateConverter
    {
        private const string GEO_SCHEME = "geo:";

        internal static GeoCoordinate? Parse(string? value)
        {
            if (value is null)
            {
                return null;
            }

            value = value.Trim();

            if (value.StartsWith(GEO_SCHEME, true, CultureInfo.InvariantCulture))
            {
                value = value.Substring(GEO_SCHEME.Length);
            }

            value = value.Replace(';', ','); // vCard 3.0

#if NET40
            string[] arr = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // vCard 4.0
#else
            string[] arr = value.Split(',', StringSplitOptions.RemoveEmptyEntries); // vCard 4.0
#endif

            try
            {
                NumberStyles numStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
                CultureInfo culture = CultureInfo.InvariantCulture;

                return new GeoCoordinate(
                    double.Parse(arr[0].Trim(), numStyle, culture),
                    double.Parse(arr[1].Trim(), numStyle, culture));
            }
            catch
            {
                return null;
            }

        }


        internal static void AppendTo(StringBuilder builder, GeoCoordinate? coordinate, VCdVersion version)
        {
            Debug.Assert(builder != null);

            if (coordinate is null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;

            switch (version)
            {
                case VCdVersion.V2_1:
                case VCdVersion.V3_0:
                    builder.Append(coordinate.Latitude.ToString(culture));
                    builder.Append(';');
                    builder.Append(coordinate.Longitude.ToString(culture));
                    break;
                default:
                    builder.Append(GEO_SCHEME);
                    builder.Append(coordinate.Latitude.ToString(culture));
                    builder.Append(',');
                    builder.Append(coordinate.Longitude.ToString(culture));
                    break;
            }//switch
        }
    }
}
