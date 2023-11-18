using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddCalendarUserAddress(string? value,
                                               string? group = null,
                                               Action<ParameterSection>? parameters = null,
                                               bool pref = false)
    {
        _vCard.CalendarUserAddresses = VCardBuilder.Add(new TextProperty(value, group),
                                                   _vCard.CalendarUserAddresses,
                                                   parameters,
                                                   pref);
        return this;
    }

    public VCardBuilder ClearCalendarUserAddresses()
    {
        _vCard.CalendarUserAddresses = null;
        return this;
    }

    //public VCardBuilder RemoveCalendarUserAddress(TextProperty? prop)
    //{
    //    _vCard.CalendarUserAddresses = _vCard.CalendarUserAddresses.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveCalendarUserAddress(Func<TextProperty, bool> predicate)
    {
        _vCard.CalendarUserAddresses = _vCard.CalendarUserAddresses.Remove(predicate);
        return this;
    }

}
