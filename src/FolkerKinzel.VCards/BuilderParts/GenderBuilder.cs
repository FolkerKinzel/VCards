using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct GenderBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal GenderBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(Sex? sex,
                          string? identity = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder._vCard.Set(Prop.GenderViews, VCardBuilder.Add(new GenderProperty(sex, identity, group),
                                                  Builder._vCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.GenderViews, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<GenderProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.GenderViews, Builder._vCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews).Remove(predicate));
        return _builder!;
    }
}
