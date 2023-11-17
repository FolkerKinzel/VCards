using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddTimeZone(TimeZoneID? value,
                                         string? group = null,
                                         Action<ParameterSection>? parameters = null,
                                         bool pref = false)
    {
        _vCard.TimeZones = Add(new TimeZoneProperty(value, group),
                                              _vCard.TimeZones,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearTimeZones()
    {
        _vCard.TimeZones = null;
        return this;
    }

    public VCardBuilder RemoveTimeZone(TimeZoneProperty? prop)
    {
        _vCard.TimeZones = _vCard.TimeZones.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveTimeZone(Func<TimeZoneProperty?, bool> predicate)
    {
        _vCard.TimeZones = _vCard.TimeZones.Remove(predicate);
        return this;
    }

}
