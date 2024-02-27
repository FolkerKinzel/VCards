using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct XmlBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal XmlBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(XElement? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Xmls,
                          VCardBuilder.Add(new XmlProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls),
                                           parameters,
                                           pref));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Xmls"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Xmls, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="XmlProperty"/> objects that match a specified predicate from the <see cref="VCard.Xmls"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="XmlProperty"/> objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<XmlProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Xmls,
                          _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls).Remove(predicate));
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

