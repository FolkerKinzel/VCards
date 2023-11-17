using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddTitle(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Titles = Add(new TextProperty(value, group),
                                              _vCard.Titles,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearTitles()
    {
        _vCard.Titles = null;
        return this;
    }

    public VCardBuilder RemoveTitle(TextProperty? prop)
    {
        _vCard.Titles = _vCard.Titles.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveTitle(Func<TextProperty, bool> predicate)
    {
        _vCard.Titles = _vCard.Titles.Remove(predicate);
        return this;
    }

}
