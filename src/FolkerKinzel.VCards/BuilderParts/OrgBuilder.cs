using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct OrgBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal OrgBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string? organizationName,
                            IEnumerable<string?>? organizationalUnits = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop.Organizations,
                           VCardBuilder.Add(new OrgProperty(organizationName, organizationalUnits, group),
                                            Builder._vCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations),
                                            parameters,
                                            pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.Organizations, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<OrgProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.Organizations,
                           Builder._vCard.Get<IEnumerable<OrgProperty?>?>(Prop.Organizations).Remove(predicate));
        return _builder!;
    }
}

