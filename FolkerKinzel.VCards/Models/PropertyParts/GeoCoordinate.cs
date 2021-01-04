using System;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Informationen über die geographische Position.
    /// </summary>
    public class GeoCoordinate : IEquatable<GeoCoordinate?>
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
        /// True, wenn das <see cref="GeoCoordinate"/>-Objekt keine gültige geographische Position beschreibt.
        /// </summary>
        public bool IsUnknown => (double.IsNaN(Latitude) || double.IsNaN(Longitude) ||
            Latitude < -90 || Latitude > 90 || Longitude < -180 || Longitude > 180);

        


        ///// <summary>
        ///// Stellt fest, ob das <see cref="GeoCoordinate"/>-Objekt dem Parameter entspricht.
        ///// </summary>
        ///// <param name="other">Ein anderes <see cref="GeoCoordinate"/>-Objekt, mit dem die Instanz verglichen wird.</param>
        ///// <returns>True, wenn das <see cref="GeoCoordinate"/>-Objekt dem Parameter entspricht.</returns>
        /// <inheritdoc/>
        public bool Equals(GeoCoordinate? other)
        {
            return !(other is null) && (other.Latitude == this.Latitude) && (other.Longitude == this.Longitude);
        }

        ///// <summary>
        ///// Stellt fest, ob der Parameter ein <see cref="GeoCoordinate"/>-Objekt ist, dessen Werte mit denen
        ///// der vergleichenden Instanz übereinstimmen.
        ///// </summary>
        ///// <param name="obj">Ein beliebiges <see cref="object"/>.</param>
        ///// <returns>True, wenn der Parameter ein <see cref="GeoCoordinate"/>-Objekt ist, dessen Werte mit denen
        ///// der vergleichenden Instanz übereinstimmen.</returns>
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as GeoCoordinate);
        }

        ///// <summary>
        ///// Hashfunktion für das <see cref="GeoCoordinate"/>-Objekt.
        ///// </summary>
        ///// <returns>Ein Hashcode für das aktuelle <see cref="GeoCoordinate"/>-Objekt.</returns>
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return -1 ^ Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts.</returns>
        public override string ToString()
        {
            return $"Latitude:  {Latitude}" + Environment.NewLine +
                   $"Longitude: {Longitude}";
        }


    }
}
