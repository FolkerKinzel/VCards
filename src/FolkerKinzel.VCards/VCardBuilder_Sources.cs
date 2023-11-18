using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddSource(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Sources = VCardBuilder.Add(new TextProperty(value, group),
                                              _vCard.Sources,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearSources()
    {
        _vCard.Sources = null;
        return this;
    }

    //public VCardBuilder RemoveSource(TextProperty? prop)
    //{
    //    _vCard.Sources = _vCard.Sources.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveSource(Func<TextProperty, bool> predicate)
    {
        _vCard.Sources = _vCard.Sources.Remove(predicate);
        return this;
    }
}
