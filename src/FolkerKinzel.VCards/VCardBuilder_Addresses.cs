using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddAddress(string? street,
                                   string? locality,
                                   string? region,
                                   string? postalCode,
                                   string? country = null,
                                   string? group = null,
                                   Action<ParameterSection>? parameters = null,
                                   bool pref = false,
                                   bool autoLabel = true)
    {
        _vCard.Addresses = Add(new AddressProperty(street, locality, region, postalCode, country, group, autoLabel),
                                          _vCard.Addresses,
                                          parameters,
                                          pref);

        return this;
    }

    public VCardBuilder AddAddress(IEnumerable<string?>? street,
                                   IEnumerable<string?>? locality,
                                   IEnumerable<string?>? region,
                                   IEnumerable<string?>? postalCode,
                                   IEnumerable<string?>? country = null,
                                   string? group = null,
                                   Action<ParameterSection>? parameters = null,
                                   bool pref = false,
                                   bool autoLabel = true)
    {
        _vCard.Addresses = Add(new AddressProperty(street, locality, region, postalCode, country, group, autoLabel),
                                          _vCard.Addresses,
                                          parameters,
                                          pref);

        return this;
    }

    public VCardBuilder ClearAddresses()
    {
        _vCard.Addresses = null;
        return this;
    }

    //public VCardBuilder RemoveAddress(AddressProperty? prop)
    //{
    //    _vCard.Addresses = _vCard.Addresses.Remove(prop);
    //    return this;
    //}

    public VCardBuilder RemoveAddress(Func<AddressProperty, bool> predicate)
    {
        _vCard.Addresses = _vCard.Addresses.Remove(predicate);
        return this;
    }

}
