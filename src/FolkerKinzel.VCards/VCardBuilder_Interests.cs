using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddInterest(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Interests = Add(new TextProperty(value, group),
                                              _vCard.Interests,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearInterests()
    {
        _vCard.Interests = null;
        return this;
    }

    public VCardBuilder RemoveInterest(TextProperty? prop)
    {
        _vCard.Interests = _vCard.Interests.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveInterest(Func<TextProperty?, bool> predicate)
    {
        _vCard.Interests = _vCard.Interests.Remove(predicate);
        return this;
    }
}
