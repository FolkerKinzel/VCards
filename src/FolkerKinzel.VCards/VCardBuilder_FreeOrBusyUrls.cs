using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddFreeOrBusyUrl(string? value,
                                          string? group = null,
                                          Action<ParameterSection>? parameters = null,
                                          bool pref = false)
    {
        _vCard.FreeOrBusyUrls = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.FreeOrBusyUrls,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearFreeOrBusyUrls()
    {
        _vCard.FreeOrBusyUrls = null;
        return this;
    }

    //public VCardBuilder RemoveFreeOrBusyUrl(TextProperty? prop)
    //{
    //    _vCard.FreeOrBusyUrls = _vCard.FreeOrBusyUrls.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveFreeOrBusyUrl(Func<TextProperty, bool> predicate)
    {
        _vCard.FreeOrBusyUrls = _vCard.FreeOrBusyUrls.Remove(predicate);
        return this;
    }

}
