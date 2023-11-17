using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddGenderView(Sex? sex,
                          string? identity = null,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null)
    {
        _vCard.GenderViews = Add(new GenderProperty(sex, identity, group),
                                              _vCard.GenderViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearGenderViews()
    {
        _vCard.GenderViews = null;
        return this;
    }

    public VCardBuilder RemoveGenderView(GenderProperty? prop)
    {
        _vCard.GenderViews = _vCard.GenderViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveGenderView(Func<GenderProperty, bool> predicate)
    {
        _vCard.GenderViews = _vCard.GenderViews.Remove(predicate);
        return this;
    }

}
