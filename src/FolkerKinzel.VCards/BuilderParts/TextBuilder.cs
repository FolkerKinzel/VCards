using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TextBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal TextBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(string? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop, VCardBuilder.Add(new TextProperty(value, group),
                                                  Builder._vCard.Get<IEnumerable<TextProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<TextProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop, Builder._vCard.Get<IEnumerable<TextProperty?>?>(Prop).Remove(predicate));
        return _builder!;
    }
}
