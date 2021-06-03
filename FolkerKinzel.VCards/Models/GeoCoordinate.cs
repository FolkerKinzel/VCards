using System;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt Informationen über die geographische Position.
    /// </summary>
    public sealed class GeoCoordinate : IEquatable<GeoCoordinate?>
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="GeoCoordinate"/>-Objekt.
        /// </summary>
        /// <param name="latitude">Breitengrad (sollte zwischen -90 und 90 liegen).</param>
        /// <param name="longitude">Längengrad (sollte zwischen -180 und 180 liegen).</param>
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Breitengrad
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Längengrad
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// <c>true</c>, wenn das <see cref="GeoCoordinate"/>-Objekt keine gültige geographische Position beschreibt.
        /// </summary>
        public bool IsUnknown => (Latitude != 0 && !Latitude.IsNormal()) 
                              || (Longitude != 0 && !Longitude.IsNormal())
                              ||  Latitude < -90 || Latitude > 90 || Longitude < -180 || Longitude > 180;


        /// <inheritdoc/>
        public bool Equals(GeoCoordinate? other)
        {
            const double _6 = 0.000001;

            return other is not null
                   && (this.IsUnknown
                        ? other.IsUnknown
                        : !other.IsUnknown && (Math.Abs(this.Latitude - other.Latitude) < _6) && (Math.Abs(this.Longitude - other.Longitude) < _6));
        }


        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as GeoCoordinate);


        /// <inheritdoc/>
        public override int GetHashCode()
        {
            const int prec = 1000000;

            return this.IsUnknown
                ? double.NaN.GetHashCode()
                : -1 ^ Math.Floor(Latitude * prec).GetHashCode() ^ Math.Floor(Longitude * prec).GetHashCode();
        }


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts.</returns>
        public override string ToString()
        {
            if (IsUnknown)
            {
                return Res.UnknownPosition;
            }

            string latitude = Latitude.ToString("F7");
            string longitude = Longitude.ToString("F7");

            
            return $"Latitude:  {latitude.Substring(0, latitude.Length - 1), 11}{ Environment.NewLine }Longitude: {longitude.Substring(0, longitude.Length-1), 11}";
        }
    }
}
