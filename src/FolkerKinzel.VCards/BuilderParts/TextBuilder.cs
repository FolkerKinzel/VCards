using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TextBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    public Prop Prop { get; }

    internal TextBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(string? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null, 
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(new TextProperty(value, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<TextProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<TextProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, _builder.VCard.Get<IEnumerable<TextProperty?>?>(Prop).Remove(predicate));
        return _builder;
    }
}
