using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct TextBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal TextBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    public VCardBuilder Add(string? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null, 
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, VCardBuilder.Add(new TextProperty(value, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<TextProperty?>?>(_prop),
                                                  parameters,
                                                  pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<TextProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop, _builder.VCard.Get<IEnumerable<TextProperty?>?>(_prop).Remove(predicate));
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
