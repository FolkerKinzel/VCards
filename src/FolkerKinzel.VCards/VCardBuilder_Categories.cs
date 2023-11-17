using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddCategory(string? value,
                                               string? group = null,
                                               Action<ParameterSection>? parameters = null,
                                               bool pref = false)
    {
        _vCard.Categories = Add(new StringCollectionProperty(value, group),
                                                   _vCard.Categories,
                                                   parameters,
                                                   pref);
        return this;
    }

    public VCardBuilder AddCategory(IEnumerable<string?>? value,
                                               string? group = null,
                                               Action<ParameterSection>? parameters = null,
                                               bool pref = false)
    {
        _vCard.Categories = Add(new StringCollectionProperty(value, group),
                                                   _vCard.Categories,
                                                   parameters,
                                                   pref);
        return this;
    }

    public VCardBuilder ClearCategories()
    {
        _vCard.Categories = null;
        return this;
    }

    public VCardBuilder RemoveCategory(StringCollectionProperty? prop)
    {
        _vCard.Categories = _vCard.Categories.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveCategory(Func<StringCollectionProperty, bool> predicate)
    {
        _vCard.Categories = _vCard.Categories.Remove(predicate);
        return this;
    }

}
