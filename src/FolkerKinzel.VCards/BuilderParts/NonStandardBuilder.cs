using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct NonStandardBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NonStandardBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(string xName,
                            string? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.NonStandards,
                           VCardBuilder.Add(new NonStandardProperty(xName, value, group?.Invoke(_builder.VCard)),
                                            _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards),
                                            parameters,
                                            pref));
        return _builder;
    }

    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NonStandards, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<NonStandardProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NonStandards,
                          _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards).Remove(predicate));
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

