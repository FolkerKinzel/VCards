using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct DateAndOrTimeBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal DateAndOrTimeBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(int year, int month, int day, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(year, month, day, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(int month, int day, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(month, day, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(DateOnly date, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDate(date, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(DateTimeOffset dateTime, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromDateTime(dateTime, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(TimeOnly time, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromTime(time, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(string? text, Action<ParameterSection>? parameters = null, Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(DateAndOrTimeProperty.FromText(text, group?.Invoke(_builder.VCard)),
                                                  Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<DateAndOrTimeProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(Prop).Remove(predicate));
        return _builder;
    }
}
