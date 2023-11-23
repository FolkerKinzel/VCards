using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct RelationBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal RelationBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    public VCardBuilder Add(Guid uuid,
                            Rel? relationType = null,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(RelationProperty.FromGuid(uuid, relationType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(string? text,
                            Rel? relationType = null,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(RelationProperty.FromText(text, relationType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(VCard? vCard,
                            Rel? relationType = null,
                            bool pref = false, 
                            Action<ParameterSection>? parameters = null, 
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(RelationProperty.FromVCard(vCard, relationType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Add(Uri? uri,
                            Rel? relationType = null,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null, 
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(RelationProperty.FromUri(uri, relationType, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<RelationProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop, _builder.VCard.Get<IEnumerable<RelationProperty?>?>(_prop).Remove(predicate));
        return _builder;
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}

