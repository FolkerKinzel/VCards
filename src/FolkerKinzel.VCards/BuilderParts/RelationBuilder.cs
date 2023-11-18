using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct RelationBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException();

    public Prop Prop { get; }

    internal RelationBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        Prop = prop;
    }

    public VCardBuilder Add(Guid uuid,
                            Rel? relationType = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromGuid(uuid, relationType, group),
                                                  Builder.VCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(string? text,
                            Rel? relationType = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromText(text, relationType, group),
                                                  Builder.VCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(VCard? vCard,
                            Rel? relationType = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromVCard(vCard, relationType, group),
                                                  Builder.VCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(Uri? uri,
                            Rel? relationType = null,
                            string? group = null,
                            Action<ParameterSection>? parameters = null,
                            bool pref = false)
    {
        Builder.VCard.Set(Prop, VCardBuilder.Add(RelationProperty.FromUri(uri, relationType, group),
                                                  Builder.VCard.Get<IEnumerable<RelationProperty?>?>(Prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<RelationProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop, Builder.VCard.Get<IEnumerable<RelationProperty?>?>(Prop).Remove(predicate));
        return _builder;
    }
}

