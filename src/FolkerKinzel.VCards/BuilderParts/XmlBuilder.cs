using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.Xmls"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", 
    Justification = "Overriding does not change the default behavior.")]
public readonly struct XmlBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal XmlBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder SetPreferences(bool skipEmptyItems = true) =>
        Edit(props =>
        {
            props.SetPreferences(skipEmptyItems);
            return props;
        });

    public VCardBuilder UnsetPreferences() =>
        Edit(props =>
        {
            props.UnsetPreferences();
            return props;
        });

    public VCardBuilder Edit<TData>(Func<IEnumerable<XmlProperty>, TData, IEnumerable<XmlProperty?>?> func, TData data)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.Xmls = func.Invoke(props, data);
        return _builder;
    }

    /// <summary>
    /// Allows to edit the items of the <see cref="VCard.Xmls"/> property with a specified delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.Xmls"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.Xmls"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<XmlProperty>, IEnumerable<XmlProperty?>?> func)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.Xmls = func.Invoke(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<XmlProperty> GetProperty() =>
        Builder.VCard.Xmls?.WhereNotNull() ?? [];

    /// <summary>
    /// Adds an <see cref="XmlProperty"/> instance, which is newly initialized using the 
    /// specified arguments, to the <see cref="VCard.Xmls"/> property.
    /// </summary>
    /// <param name="value">A <see cref="XElement" /> or <c>null</c>. The element must be 
    /// explicitly assigned to an XML namespace (xmlns attribute). This namespace must not 
    /// be the VCARD 4.0 namespace <c>urn:ietf:params:xml:ns:vcard-4.0</c>!</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of 
    /// <see cref="VCardProperty" /> objects, which the <see cref="VCardProperty" /> should belong to,
    /// or <c>null</c> to indicate that the <see cref="VCardProperty" /> does not belong to any 
    /// group. The function is called with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentException"> <paramref name="value" /> is not assigned to an XML 
    /// namespace - or - <paramref name="value" /> is in the reserved
    /// namespace <c>urn:ietf:params:xml:ns:vcard-4.0</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(XElement? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Xmls,
                          VCardBuilder.Add(new XmlProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Xmls"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Xmls, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="XmlProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.Xmls"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="XmlProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="XmlBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<XmlProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Xmls,
                          _builder.VCard.Get<IEnumerable<XmlProperty?>?>(Prop.Xmls)
                                        .Remove(predicate)
                          );
        return _builder;
    }

    // Overriding Equals, GetHashCode and ToString to hide these methods in IntelliSense:

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

