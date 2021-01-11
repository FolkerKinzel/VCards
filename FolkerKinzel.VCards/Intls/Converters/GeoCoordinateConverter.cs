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

        internal static GeoCoordinate? Parse(string? value)
        {
            if (value is null)
            {
                return null;
            }

#if NET40
            const string GEO_SCHEME = "geo:";

            value = value.Trim();

            if (value.StartsWith(GEO_SCHEME, true, CultureInfo.InvariantCulture))
            {
                value = value.Substring(GEO_SCHEME.Length);
            }

            value = value.Replace(';', ','); // vCard 3.0

            string[] arr = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // vCard 4.0

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
#else
            ReadOnlySpan<char> roSpan = value.AsSpan();

            int startIndex = 0;

            while(startIndex < roSpan.Length)
            {
                char c = roSpan[startIndex];

                if(char.IsDigit(c) || c == '.')
                {
                    break;
                }

                startIndex++;
            }

            if(startIndex != 0)
            {
                roSpan = roSpan.Slice(startIndex);
            }

            int splitIndex = MemoryExtensions.IndexOf(roSpan, ','); // vCard 4.0

            if(splitIndex == -1)
            {
                splitIndex = MemoryExtensions.IndexOf(roSpan, ';'); // vCard 3.0
            }

            try
            {
                NumberStyles numStyle = NumberStyles.AllowDecimalPoint
                                      | NumberStyles.AllowLeadingSign 
                                      | NumberStyles.AllowLeadingWhite 
                                      | NumberStyles.AllowTrailingWhite;

                CultureInfo culture = CultureInfo.InvariantCulture;

                return new GeoCoordinate(
                    double.Parse(roSpan.Slice(0, splitIndex), numStyle, culture),
                    double.Parse(roSpan.Slice(splitIndex + 1), numStyle, culture));
            }
            catch
            {
                return null;
            }

#endif

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
                    _ = builder.Append(coordinate.Latitude.ToString(culture));
                    _ = builder.Append(';');
                    _ = builder.Append(coordinate.Longitude.ToString(culture));
                    break;
                default:
                    _ = builder.Append(GEO_SCHEME);
                    _ = builder.Append(coordinate.Latitude.ToString(culture));
                    _ = builder.Append(',');
                    _ = builder.Append(coordinate.Longitude.ToString(culture));
                    break;
            }//switch
        }
    }
}
