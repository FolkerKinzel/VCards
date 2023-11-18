using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal NameBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder._vCard.Set(Prop.NameViews,
                           VCardBuilder.Add(new NameProperty(familyNames,
                                                             givenNames,
                                                             additionalNames,
                                                             prefixes,
                                                             suffixes, group),
                           Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                           parameters,
                           false));
        return _builder!;
    }

    public VCardBuilder Add(string? familyName,
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        Builder._vCard.Set(Prop.NameViews,
                           VCardBuilder.Add(new NameProperty(familyName,
                                                             givenName,
                                                             additionalName,
                                                             prefix,
                                                             suffix,
                                                             group),
                           Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                           parameters,
                           false));
        return _builder!;
    }

    public VCardBuilder Clear()
    {
        Builder._vCard.Set(Prop.NameViews, null);
        return _builder!;
    }

    public VCardBuilder Remove(Func<NameProperty, bool> predicate)
    {
        Builder._vCard.Set(Prop.NameViews, Builder._vCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews).Remove(predicate));
        return _builder!;
    }
}

