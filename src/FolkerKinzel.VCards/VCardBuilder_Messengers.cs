using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddMessenger(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Messengers = Add(new TextProperty(value, group),
                                              _vCard.Messengers,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearMessengers()
    {
        _vCard.Messengers = null;
        return this;
    }

    //public VCardBuilder RemoveMessenger(TextProperty? prop)
    //{
    //    _vCard.Messengers = _vCard.Messengers.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveMessenger(Func<TextProperty, bool> predicate)
    {
        _vCard.Messengers = _vCard.Messengers.Remove(predicate);
        return this;
    }

}
