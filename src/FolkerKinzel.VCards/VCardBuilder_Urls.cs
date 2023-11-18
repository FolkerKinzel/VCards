using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddUrl(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Urls = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.Urls,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearUrls()
    {
        _vCard.Urls = null;
        return this;
    }

    //public VCardBuilder RemoveUrl(TextProperty? prop)
    //{
    //    _vCard.Urls = _vCard.Urls.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveUrl(Func<TextProperty, bool> predicate)
    {
        _vCard.Urls = _vCard.Urls.Remove(predicate);
        return this;
    }
}
