using System.Globalization;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates information about the geographical position.</summary>
public sealed class GeoCoordinate : IEquatable<GeoCoordinate?>
{
    private const double _6 = 0.000001;

    /// <summary>
    /// Distance in meters for 1° at the Equator.
    /// </summary>
    private const double ONE_DEGREE_DISTANCE = 111300;

    /// <summary>
    /// Minimum recognized distance (11,13 cm).
    /// </summary>
    private const double MIN_DISTANCE = ONE_DEGREE_DISTANCE * _6;

    /// <summary> Initializes a new <see cref="GeoCoordinate" /> objekt. </summary>
    /// <param name="latitude">Latitude (value between -90 and 90).</param>
    /// <param name="longitude">Longitude (value between -180 and 180).</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="latitude" />
    /// or <paramref name="longitude" /> does not have a valid value.</exception>
    /// <seealso cref="GeoProperty"/>
    /// <seealso cref="VCard.GeoCoordinates"/>
    /// <seealso cref="ParameterSection.GeoPosition"/>
    public GeoCoordinate(double latitude, double longitude)
    {
        if (double.IsNaN(latitude) || latitude < -100.0 || latitude > 100.0)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude));
        }

        if (double.IsNaN(longitude) || longitude < -200.0 || longitude > 200.0)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude));
        }

        // Don't change the order: longitude MUST be normalized first.
        longitude = NormalizeLongitude(longitude);
        NormalizeLatitude(ref latitude, ref longitude);

        Latitude = Math.Round(latitude, 6, MidpointRounding.ToEven);
        Longitude = Math.Round(longitude, 6, MidpointRounding.ToEven);

        static double NormalizeLongitude(double longitude)
        {
            if (longitude < -180.0)
            {
                longitude += 360.0;
            }
            else if (longitude > 180.0)
            {
                longitude -= 360.0;
            }

            return longitude;
        }

        static void NormalizeLatitude(ref double latitude, ref double longitude)
        {
            Debug.Assert(longitude <= 180.0);

            // fly over the Pole
            if (latitude > 90.0)
            {
                latitude = 180.0 - latitude;
                longitude = 180.0 - Math.Abs(longitude);
            }
            else if (latitude < -90.0)
            {
                latitude = -180.0 - latitude;
                longitude = 180.0 - Math.Abs(longitude);
            }
        }
    }

    /// <summary>Latitude.</summary>
    public double Latitude { get; }

    /// <summary>Longitude.</summary>
    public double Longitude { get; }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as GeoCoordinate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals([NotNullWhen(true)] GeoCoordinate? other) => Equals(other, 0);

    /// <summary>
    /// Indicates whether the current object is equal to <paramref name="other"/>
    /// and allows to define equality by specifying a distance within that geographical
    /// positions are considered equal.
    /// </summary>
    /// <param name="other">A <see cref="GeoCoordinate"/> object to compare with the 
    /// current instance, or <c>null</c>.</param>
    /// <param name="minDistance">A distance in <c>m</c> within that geographical
    /// positions are considered equal. (The recognized minimum is about 12 cm.)</param>
    /// <returns><c>true</c> if the geographical position of <paramref name="other"/>
    /// is no further away than <paramref name="minDistance"/> meters from that of the 
    /// current instance, otherwise <c>false</c>.</returns>
    public bool Equals([NotNullWhen(true)] GeoCoordinate? other, int minDistance)
    {
        if (other != null)
        {
            double minDist = minDistance < 1 ? MIN_DISTANCE : minDistance;
            return ComputeDistance(other, minDist) < minDist;
        }

        return false;
    }

    /// <summary>
    /// Computes the distance between this and <paramref name="other"/> in <c>m</c> simply with 
    /// Pythagoras (that's precisely enough for very short distances).
    /// </summary>
    /// <param name="other">The <see cref="GeoCoordinate"/> to compare with.</param>
    /// <param name="minDistance">Minimum distance in <c>m</c> for which <see cref="GeoCoordinate"/>
    /// objects are considered different.</param>
    /// <returns>The distance in <c>m</c> between this and <paramref name="other"/>.</returns>
    private double ComputeDistance(GeoCoordinate other, double minDistance)
    {
        double diffLat = ONE_DEGREE_DISTANCE * (Latitude - other.Latitude);

        // radians of the average latitude
        double latRad = (Latitude + other.Latitude) * (Math.PI / 360);
        
        double diffAngleLong = Math.Abs(Longitude - other.Longitude);

        // take the shortest direction around the globe:
        if (diffAngleLong > 180.0)
        {
            diffAngleLong = 360.0 - diffAngleLong;
        }

        double diffLong = ONE_DEGREE_DISTANCE * Math.Cos(latRad) * diffAngleLong;

        return Math.Sqrt(diffLat * diffLat + diffLong * diffLong);
    }

    /// <summary>
    /// Overloads the equality operator for <see cref="GeoCoordinate"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/>
    /// are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(GeoCoordinate? left, GeoCoordinate? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="GeoCoordinate"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/>
    /// are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(GeoCoordinate? left, GeoCoordinate? right)
        => !(left == right);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => GetHashCode(0);

    /// <summary>
    /// Generates a hash code for the current instance and allows to specify
    /// a distance within that differences between geographical positions are
    /// ignored.
    /// </summary>
    /// <param name="minDistance">A distance in <c>m</c> within that geographical
    /// positions are considered equal. (The recognized minimum is about 12 cm.)</param>
    /// <returns>A hash code for the current object.</returns>
    public int GetHashCode(int minDistance)
    {
        double minDist = minDistance < 1 ? MIN_DISTANCE : minDistance;

        double lati = Math.Floor(Latitude * ONE_DEGREE_DISTANCE / minDist);

        double oneDegreeLongitudeDistance = ONE_DEGREE_DISTANCE * Math.Cos(Latitude * (Math.PI / 180));

        double longi = oneDegreeLongitudeDistance < minDist 
                                 ? 0 
                                 : Math.Floor(Longitude * oneDegreeLongitudeDistance / minDist);

        return HashCode.Combine(lati, longi);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string latitude = Latitude.ToString("F6");
        string longitude = Longitude.ToString("F6");

        return $"""
                Latitude:  {latitude,11}
                Longitude: {longitude,11}
                """;
    }

    internal static bool TryParse(ReadOnlySpan<char> value, out GeoCoordinate? coordinate)
    {
        coordinate = null;

        if (value.IsEmpty)
        {
            return false;
        }

        int startIndex = 0;

        while (startIndex < value.Length)
        {
            char c = value[startIndex];

            if (char.IsDigit(c) || c == '.') // ".8" == "0.8"
            {
                break;
            }

            startIndex++;
        }

        if (startIndex != 0)
        {
            value = value.Slice(startIndex);
        }

        int splitIndex = value.IndexOf(','); // vCard 4.0

        if (splitIndex == -1)
        {
            splitIndex = value.IndexOf(';'); // vCard 3.0
        }

        try
        {
            NumberStyles numStyle = NumberStyles.AllowDecimalPoint
                                  | NumberStyles.AllowLeadingSign
                                  | NumberStyles.AllowLeadingWhite
                                  | NumberStyles.AllowTrailingWhite;

            CultureInfo culture = CultureInfo.InvariantCulture;


            coordinate = new GeoCoordinate(
                _Double.Parse(value.Slice(0, splitIndex), numStyle, culture),
                _Double.Parse(value.Slice(splitIndex + 1), numStyle, culture));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
