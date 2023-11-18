﻿using FolkerKinzel.VCards.Enums;
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
        Builder.VCard.Set(Prop.GenderViews, VCardBuilder.Add(new GenderProperty(sex, identity, group),
                                                  Builder.VCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews),
                                                  parameters,
                                                  false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.GenderViews, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<GenderProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.GenderViews, Builder.VCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews).Remove(predicate));
        return _builder!;
    }
}
