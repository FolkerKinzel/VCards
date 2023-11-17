using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{

    public VCardBuilder AddOrgDirectory(string? value,
                                           string? group = null,
                                           Action<ParameterSection>? parameters = null,
                                           bool pref = false)
    {
        _vCard.OrgDirectories = Add(new TextProperty(value, group),
                                              _vCard.OrgDirectories,
                                              parameters,
                                              pref);
        return this;
    }

    public VCardBuilder ClearOrgDirectories()
    {
        _vCard.OrgDirectories = null;
        return this;
    }

    public VCardBuilder RemoveOrgDirectory(TextProperty? prop)
    {
        _vCard.OrgDirectories = _vCard.OrgDirectories.Remove(prop);
        return this;
    }

    public VCardBuilder RemoveOrgDirectory(Func<TextProperty, bool> predicate)
    {
        _vCard.OrgDirectories = _vCard.OrgDirectories.Remove(predicate);
        return this;
    }

}
