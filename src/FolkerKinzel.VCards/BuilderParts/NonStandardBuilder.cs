using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct NonStandardBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal NonStandardBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string xName,
                            string? value,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder._vCard.Set(Prop.NonStandards,
                           VCardBuilder.Add(new NonStandardProperty(xName, value, group),
                                            Builder._vCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards),
                                            parameters,
                                            pref));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.NonStandards, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<NonStandardProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.NonStandards,
                           Builder._vCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards).Remove(predicate));
        return _builder!;
    }
}

