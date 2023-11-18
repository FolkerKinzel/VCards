using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    internal NameBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null,
                            Action<TextBuilder, NameProperty>? displayName = null)
    {
        var vc = Builder.VCard;
        var prop = new NameProperty(familyNames,
                                    givenNames,
                                    additionalNames,
                                    prefixes,
                                    suffixes, group?.Invoke(vc));
        vc.Set(Prop.NameViews,
               VCardBuilder.Add(prop,
               vc.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
               parameters,
               false));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    public VCardBuilder Add(string? familyName,
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null,
                            Action<TextBuilder, NameProperty>? displayName = null)
    {
        var vc = Builder.VCard;
        var prop = new NameProperty(familyName,
                                    givenName,
                                    additionalName,
                                    prefix,
                                    suffix,
                                    group?.Invoke(vc));
        vc.Set(Prop.NameViews,
               VCardBuilder.Add(prop,
                                vc.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                                parameters,
                                false));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NameViews, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<NameProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NameViews, _builder.VCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews).Remove(predicate));
        return _builder;
    }
}

