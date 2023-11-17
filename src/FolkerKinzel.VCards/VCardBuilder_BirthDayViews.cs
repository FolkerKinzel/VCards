using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddBirthDayView(int year, int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.BirthDayViews = Add(DateAndOrTimeProperty.FromDate(year, month, day, group),
                                              _vCard.BirthDayViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddBirthDayView(int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.BirthDayViews = Add(DateAndOrTimeProperty.FromDate(month, day, group),
                                              _vCard.BirthDayViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddBirthDayView(DateOnly date, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.BirthDayViews = Add(DateAndOrTimeProperty.FromDate(date, group),
                                              _vCard.BirthDayViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddBirthDayView(DateTimeOffset dateTime, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.BirthDayViews = Add(DateAndOrTimeProperty.FromDateTime(dateTime, group),
                                              _vCard.BirthDayViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearBirthDayViews()
    {
        _vCard.BirthDayViews = null;
        return this;
    }

    public VCardBuilder RemoveBirthDayView(DateAndOrTimeProperty? prop)
    {
        _vCard.BirthDayViews = _vCard.BirthDayViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveBirthDayView(Func<DateAndOrTimeProperty, bool> predicate)
    {
        _vCard.BirthDayViews = _vCard.BirthDayViews.Remove(predicate);
        return this;
    }

}
