using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddNickName(string? value,
                                            string? group = null,
                                            Action<ParameterSection>? parameters = null,
                                            bool pref = false)
    {
        _vCard.NickNames = Add(new StringCollectionProperty(value, group),
                                                   _vCard.NickNames,
                                                   parameters,
                                                   pref);
        return this;
    }

    public VCardBuilder AddNickName(IEnumerable<string?>? value,
                                               string? group = null,
                                               Action<ParameterSection>? parameters = null,
                                               bool pref = false)
    {
        _vCard.NickNames = Add(new StringCollectionProperty(value, group),
                                                   _vCard.NickNames,
                                                   parameters,
                                                   pref);
        return this;
    }

    public VCardBuilder ClearNickNames()
    {
        _vCard.NickNames = null;
        return this;
    }

    public VCardBuilder RemoveNickName(StringCollectionProperty? prop)
    {
        _vCard.NickNames = _vCard.NickNames.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveNickName(Func<StringCollectionProperty?, bool> predicate)
    {
        _vCard.NickNames = _vCard.NickNames.Remove(predicate);
        return this;
    }

}
