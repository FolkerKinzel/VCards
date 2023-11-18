﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct AddressBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal AddressBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string? street,
                            string? locality,
                            string? region,
                            string? postalCode,
                            string? country = null,
                            bool autoLabel = true,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop.Addresses,
                           VCardBuilder.Add(new AddressProperty(street, locality, region, postalCode, country, autoLabel, group),
                           Builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses),
                           parameters,
                           pref));
        return _builder!;
    }

    public VCardBuilder Add(IEnumerable<string?>? street,
                            IEnumerable<string?>? locality,
                            IEnumerable<string?>? region,
                            IEnumerable<string?>? postalCode,
                            IEnumerable<string?>? country = null,
                            bool autoLabel = true,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop.Addresses,
                           VCardBuilder.Add(new AddressProperty(street, locality, region, postalCode, country, autoLabel, group),
                           Builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses),
                           parameters,
                           pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Addresses, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<AddressProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Addresses, Builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses).Remove(predicate));
        return _builder!;
    }

}
