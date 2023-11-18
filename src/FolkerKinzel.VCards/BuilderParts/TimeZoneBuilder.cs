﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct TimeZoneBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal TimeZoneBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(TimeZoneID? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop.TimeZones, VCardBuilder.Add(new TimeZoneProperty(value, group),
                                                  Builder.VCard.Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones),
                                                  parameters,
                                                  pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.TimeZones, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<TimeZoneProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.TimeZones, Builder.VCard.Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones).Remove(predicate));
        return _builder!;
    }
}
