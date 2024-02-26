using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct GenderBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal GenderBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(Sex? sex,
                          string? identity = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GenderViews, VCardBuilder.Add(new GenderProperty(sex, identity, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews),
                                                  parameters,
                                                  false));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.GenderViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GenderBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.GenderViews, null);
        return _builder;
    }

    public VCardBuilder Remove(Func<GenderProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.GenderViews, _builder.VCard.Get<IEnumerable<GenderProperty?>?>(Prop.GenderViews).Remove(predicate));
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
