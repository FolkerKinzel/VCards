using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddCalendarAddress(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.CalendarAddresses = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.CalendarAddresses,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearCalendarAddresses()
    {
        _vCard.CalendarAddresses = null;
        return this;
    }

    //public VCardBuilder RemoveCalendarAddress(TextProperty? prop)
    //{
    //    _vCard.CalendarAddresses = _vCard.CalendarAddresses.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveCalendarAddress(Func<TextProperty, bool> predicate)
    {
        _vCard.CalendarAddresses = _vCard.CalendarAddresses.Remove(predicate);
        return this;
    }
}
