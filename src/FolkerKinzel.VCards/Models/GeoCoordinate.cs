using System.Globalization;
using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Kapselt Informationen über die geographische Position.
/// </summary>
public sealed class GeoCoordinate : IEquatable<GeoCoordinate?>
{
    /// <summary>
    /// Initialisiert ein neues <see cref="GeoCoordinate"/>-Objekt.
    /// </summary>
    /// <param name="latitude">Breitengrad (muss zwischen -90 und 90 liegen).</param>
    /// <param name="longitude">Längengrad (muss zwischen -180 und 180 liegen).</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="latitude"/> oder <paramref name="longitude"/> hat keinen gültigen Wert.</exception>
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

    /// <summary>
    /// Breitengrad
    /// </summary>
    public double Latitude { get; }

    /// <summary>
    /// Längengrad
    /// </summary>
    public double Longitude { get; }


    /// <inheritdoc/>
    public bool Equals(GeoCoordinate? other)
    {
        const double _6 = 0.000001;

        return other is not null && (Math.Abs(this.Latitude - other.Latitude) < _6) && (Math.Abs(this.Longitude - other.Longitude) < _6);
    }


    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as GeoCoordinate);


    /// <inheritdoc/>
    public override int GetHashCode()
    {
        const int prec = 1000000;

        return HashCode.Combine(Math.Floor(Latitude * prec), Math.Floor(Longitude * prec));
    }


    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts. 
    /// (Nur zum Debugging.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts.</returns>
    public override string ToString()
    {
        string latitude = Latitude.ToString("F6");
        string longitude = Longitude.ToString("F6");


        return $"Latitude:  {latitude,11}{ Environment.NewLine }Longitude: {longitude,11}";
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
