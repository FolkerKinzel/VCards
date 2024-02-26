using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NameBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null,
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

    /// <summary>
    /// Sets the <see cref="VCard.NameViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
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

