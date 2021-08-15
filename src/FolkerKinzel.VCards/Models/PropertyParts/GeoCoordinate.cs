using System;
using System.ComponentModel;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Informationen über die geographische Position.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Obsolete("Use FolkerKinzel.VCards.Models.GeoCoordinate instead.", true)]
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
        public bool IsUnknown => double.IsNaN(Latitude) || double.IsNaN(Longitude) ||
            Latitude < -90 || Latitude > 90 || Longitude < -180 || Longitude > 180;


        /// <inheritdoc/>
        public bool Equals(GeoCoordinate? other) => !(other is null) && (other.Latitude == this.Latitude) && (other.Longitude == this.Longitude);

        
        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as GeoCoordinate);

        
        /// <inheritdoc/>
        public override int GetHashCode() => -1 ^ Latitude.GetHashCode() ^ Longitude.GetHashCode();


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="GeoCoordinate"/>-Objekts.</returns>
        public override string ToString() => $"Latitude:  {Latitude}{ Environment.NewLine}Longitude: {Longitude}";
    }
}
