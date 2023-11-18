using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct AddressBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal AddressBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string? street,
                            string? locality,
                            string? region,
                            string? postalCode,
                            string? country = null,
                            bool autoLabel = true,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Addresses,
                           VCardBuilder.Add(new AddressProperty(
                               street, locality, region, postalCode, country, autoLabel, group?.Invoke(_builder.VCard)),
                           _builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses),
                           parameters,
                           pref));
        return _builder;
    }

    public VCardBuilder Add(IEnumerable<string?>? street,
                            IEnumerable<string?>? locality,
                            IEnumerable<string?>? region,
                            IEnumerable<string?>? postalCode,
                            IEnumerable<string?>? country = null,
                            bool autoLabel = true,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Addresses,
                           VCardBuilder.Add(new AddressProperty(street, locality, region, postalCode, country, autoLabel, group?.Invoke(_builder.VCard)),
                           _builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses),
                           parameters,
                           pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Addresses, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<AddressProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Addresses, _builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses).Remove(predicate));
        return _builder;
    }

}
