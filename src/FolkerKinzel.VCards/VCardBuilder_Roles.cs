using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder AddRole(string? value,
                                          string? group = null,
                                          Action<ParameterSection>? parameters = null,
                                          bool pref = false)
    {
        _vCard.Roles = Add(new TextProperty(value, group),
                                              _vCard.Roles,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearRoles()
    {
        _vCard.Roles = null;
        return this;
    }

    public VCardBuilder RemoveRole(TextProperty? prop)
    {
        _vCard.Roles = _vCard.Roles.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveRole(Func<TextProperty, bool> predicate)
    {
        _vCard.Roles = _vCard.Roles.Remove(predicate);
        return this;
    }
}
