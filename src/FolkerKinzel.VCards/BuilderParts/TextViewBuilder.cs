using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TextViewBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal TextViewBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(string? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(new TextProperty(value, group),
                                                  Builder.VCard.Get<IEnumerable<TextProperty?>?>(Prop),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<TextProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, Builder.VCard.Get<IEnumerable<TextProperty?>?>(Prop).Remove(predicate));
        return _builder;
    }
}
