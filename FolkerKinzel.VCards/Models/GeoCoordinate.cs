using System;
using System.ComponentModel;
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
        /// <param name="latitude">Breitengrad (muss zwischen -90 und 90 liegen).</param>
        /// <param name="longitude">Längengrad (muss zwischen -180 und 180 liegen).</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="latitude"/> oder <paramref name="longitude"/> hat keinen gültigen Wert.</exception>
        public GeoCoordinate(double latitude, double longitude)
        {
            if((latitude != 0 && !latitude.IsNormal()) ||  latitude < -90.0000001 || latitude > 90.0000001)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude));
            }

            if((longitude != 0 && !longitude.IsNormal()) || longitude < -180.0000001 || longitude > 180.0000001)
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

        ///// <summary>
        ///// <c>true</c>, wenn das <see cref="GeoCoordinate"/>-Objekt keine gültige geographische Position beschreibt.
        ///// </summary>
        [Obsolete("This property has been removed.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public bool IsUnknown => false;


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

            return -1 ^ Math.Floor(Latitude * prec).GetHashCode() ^ Math.Floor(Longitude * prec).GetHashCode();
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

            
            return $"Latitude:  {latitude, 11}{ Environment.NewLine }Longitude: {longitude, 11}";
        }
    }
}
