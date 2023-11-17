using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddLanguage(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Languages = Add(new TextProperty(value, group),
                                              _vCard.Languages,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearLanguages()
    {
        _vCard.Languages = null;
        return this;
    }

    public VCardBuilder RemoveLanguage(TextProperty? prop)
    {
        _vCard.Languages = _vCard.Languages.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveLanguage(Func<TextProperty?, bool> predicate)
    {
        _vCard.Languages = _vCard.Languages.Remove(predicate);
        return this;
    }
}
