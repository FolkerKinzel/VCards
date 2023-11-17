using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddExpertise(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.Expertises = Add(new TextProperty(value, group),
                                              _vCard.Expertises,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearExpertises()
    {
        _vCard.Expertises = null;
        return this;
    }

    public VCardBuilder RemoveExpertise(TextProperty? prop)
    {
        _vCard.Expertises = _vCard.Expertises.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveExpertise(Func<TextProperty, bool> predicate)
    {
        _vCard.Expertises = _vCard.Expertises.Remove(predicate);
        return this;
    }

}
