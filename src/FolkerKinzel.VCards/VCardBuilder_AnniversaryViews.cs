using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddAnniversaryView(int year, int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.AnniversaryViews = Add(DateAndOrTimeProperty.FromDate(year, month, day, group),
                                              _vCard.AnniversaryViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddAnniversaryView(int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.AnniversaryViews = Add(DateAndOrTimeProperty.FromDate(month, day, group),
                                              _vCard.AnniversaryViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddAnniversaryView(DateOnly date, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.AnniversaryViews = Add(DateAndOrTimeProperty.FromDate(date, group),
                                              _vCard.AnniversaryViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder AddAnniversaryView(DateTimeOffset dateTime, string? group = null, Action<ParameterSection>? parameters = null)
    {
        _vCard.AnniversaryViews = Add(DateAndOrTimeProperty.FromDateTime(dateTime, group),
                                              _vCard.AnniversaryViews,
                                              parameters,
                                              false);
        return this;
    }

    public VCardBuilder ClearAnniversaryViews()
    {
        _vCard.AnniversaryViews = null;
        return this;
    }

    public VCardBuilder RemoveAnniversaryView(DateAndOrTimeProperty? prop)
    {
        _vCard.AnniversaryViews = _vCard.AnniversaryViews.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveAnniversaryView(Func<DateAndOrTimeProperty?, bool> predicate)
    {
        _vCard.AnniversaryViews = _vCard.AnniversaryViews.Remove(predicate);
        return this;
    }

}
