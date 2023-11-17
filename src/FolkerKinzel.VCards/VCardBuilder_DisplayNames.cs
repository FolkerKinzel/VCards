using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddDisplayName(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.DisplayNames = Add(new TextProperty(value, group),
                                              _vCard.DisplayNames,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearDisplayNames()
    {
        _vCard.DisplayNames = null;
        return this;
    }

    public VCardBuilder RemoveDisplayName(TextProperty? prop)
    {
        _vCard.DisplayNames = _vCard.DisplayNames.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveDisplayName(Func<TextProperty, bool> predicate)
    {
        _vCard.DisplayNames = _vCard.DisplayNames.Remove(predicate);
        return this;
    }

}
