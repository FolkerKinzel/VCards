using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddDeathDateView(int year, int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.DeathDateViews = VCardBuilder.Add(DateAndOrTimeProperty.FromDate(year, month, day, group),
                                              _vCard.DeathDateViews,
                                              parameters,
                                              false);
        return this;
    }



    public VCardBuilder AddDeathDateView(DateOnly date, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.DeathDateViews = VCardBuilder.Add(DateAndOrTimeProperty.FromDate(date, group),
                                              _vCard.DeathDateViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddDeathDateView(DateTimeOffset dateTime, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.DeathDateViews = VCardBuilder.Add(DateAndOrTimeProperty.FromDateTime(dateTime, group),
                                              _vCard.DeathDateViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearDeathDateViews()
    {
        _vCard.DeathDateViews = null;
        return this;
    }

    //public VCardBuilder RemoveDeathDateView(DateAndOrTimeProperty? prop)
    //{
    //    _vCard.DeathDateViews = _vCard.DeathDateViews.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveDeathDateView(Func<DateAndOrTimeProperty, bool> predicate)
    {
        _vCard.DeathDateViews = _vCard.DeathDateViews.Remove(predicate);
        return this;
    }

}
