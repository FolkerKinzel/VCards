using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddDeathPlaceView(string? value, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.DeathPlaceViews = Add(new TextProperty(value, group),
                                              _vCard.DeathPlaceViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearDeathPlaceViews()
    {
        _vCard.DeathPlaceViews = null;
        return this;
    }

    public VCardBuilder RemoveDeathPlaceView(TextProperty? prop)
    {
        _vCard.DeathPlaceViews = _vCard.DeathPlaceViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveDeathPlaceView(Func<TextProperty?, bool> predicate)
    {
        _vCard.DeathPlaceViews = _vCard.DeathPlaceViews.Remove(predicate);
        return this;
    }

}
