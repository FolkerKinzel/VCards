using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Converters
{
    static class GeoCoordinateConverter
    {
        const string GEO_SCHEME = "geo:";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Keine allgemeinen Ausnahmetypen abfangen", Justification = "<Ausstehend>")]
        internal static GeoCoordinate? Parse(string? value)
        {
            if (value is null) return null;

            value = value.Trim();

            if (value.StartsWith(GEO_SCHEME, true, CultureInfo.InvariantCulture))
            {
                value = value.Substring(GEO_SCHEME.Length);
            }

            value = value.Replace(';', ','); // vCard 3.0

#if NET40
            var arr = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // vCard 4.0
#else
            var arr = value.Split(',', StringSplitOptions.RemoveEmptyEntries); // vCard 4.0
#endif

            try
            {
                var numStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
                var culture = CultureInfo.InvariantCulture;

                return new GeoCoordinate(
                    double.Parse(arr[0].Trim(), numStyle, culture),
                    double.Parse(arr[1].Trim(), numStyle, culture));
            }
            catch
            {
                return null;
            }

        }


        internal static void AppendTo(StringBuilder builder, GeoCoordinate coordinate, VCdVersion version)
        {
            Debug.Assert(builder != null);
            Debug.Assert(coordinate != null);

            var culture = CultureInfo.InvariantCulture;

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
