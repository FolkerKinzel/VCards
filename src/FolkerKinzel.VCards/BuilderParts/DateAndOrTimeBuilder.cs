using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct DateAndOrTimeBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal DateAndOrTimeBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(int year, int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(year, month, day, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(int month, int day, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(month, day, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(DateOnly date, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(date, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(DateTimeOffset dateTime, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDateTime(dateTime, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(TimeOnly time, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromTime(time, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Add(string? text, string? group = null, Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromText(text, group),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<DateAndOrTimeProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop).Remove(predicate));
        return _builder!;
    }
}
