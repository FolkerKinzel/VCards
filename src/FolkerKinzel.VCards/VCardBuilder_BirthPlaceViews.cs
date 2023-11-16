using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddBirthPlaceView(string? value, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.BirthPlaceViews = Add(new TextProperty(value, group),
                                              _vCard.BirthPlaceViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearBirthPlaceViews()
    {
        _vCard.BirthPlaceViews = null;
        return this;
    }

    public VCardBuilder RemoveBirthPlaceView(TextProperty? prop)
    {
        _vCard.BirthPlaceViews = _vCard.BirthPlaceViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveBirthPlaceView(Func<TextProperty?, bool> predicate)
    {
        _vCard.BirthPlaceViews = _vCard.BirthPlaceViews.Remove(predicate);
        return this;
    }

}
