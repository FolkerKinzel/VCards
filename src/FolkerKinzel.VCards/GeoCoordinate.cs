using System.Globalization;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates information about the geographical position.</summary>
public sealed class GeoCoordinate : IEquatable<GeoCoordinate?>
{
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
        if (double.IsNaN(latitude) || latitude < -90.0000001 || latitude > 90.0000001)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude));
        }

        if (double.IsNaN(longitude) || longitude < -180.0000001 || longitude > 180.0000001)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude));
        }

        Latitude = Math.Round(latitude, 6, MidpointRounding.ToEven);
        Longitude = Math.Round(longitude, 6, MidpointRounding.ToEven);
    }

    /// <summary>Latitude.</summary>
    public double Latitude { get; }

    /// <summary>Longitude.</summary>
    public double Longitude { get; }

    /// <inheritdoc />
    public bool Equals(GeoCoordinate? other)
    {
        const double _6 = 0.000001;
        return other is not null && Math.Abs(Latitude - other.Latitude) < _6 && Math.Abs(Longitude - other.Longitude) < _6;
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
    public override bool Equals(object? obj) => Equals(obj as GeoCoordinate);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        const int prec = 1000000;
        return HashCode.Combine(Math.Floor(Latitude * prec), Math.Floor(Longitude * prec));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string latitude = Latitude.ToString("F6");
        string longitude = Longitude.ToString("F6");


        return $"Latitude:  {latitude,11}{Environment.NewLine}Longitude: {longitude,11}";
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
