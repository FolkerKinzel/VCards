using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddGeoCoordinate(double latitude,
                                            double longitude,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.GeoCoordinates = Add(new GeoProperty(latitude, longitude, group),
                                              _vCard.GeoCoordinates,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder AddGeoCoordinate(GeoCoordinate? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.GeoCoordinates = Add(new GeoProperty(value, group),
                                              _vCard.GeoCoordinates,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearGeoCoordinates()
    {
        _vCard.GeoCoordinates = null;
        return this;
    }

    public VCardBuilder RemoveGeoCoordinate(GeoProperty? prop)
    {
        _vCard.GeoCoordinates = _vCard.GeoCoordinates.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveGeoCoordinate(Func<GeoProperty?, bool> predicate)
    {
        _vCard.GeoCoordinates = _vCard.GeoCoordinates.Remove(predicate);
        return this;
    }

}
