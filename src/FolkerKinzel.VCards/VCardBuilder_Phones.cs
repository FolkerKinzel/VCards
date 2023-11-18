using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddPhone(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Phones = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.Phones,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearPhones()
    {
        _vCard.Phones = null;
        return this;
    }

    //public VCardBuilder RemovePhone(TextProperty? prop)
    //{
    //    _vCard.Phones = _vCard.Phones.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemovePhone(Func<TextProperty, bool> predicate)
    {
        _vCard.Phones = _vCard.Phones.Remove(predicate);
        return this;
    }

}
