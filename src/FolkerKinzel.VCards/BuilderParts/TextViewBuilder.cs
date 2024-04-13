using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing <see cref="VCard"/> properties that contain 
/// <see cref="TextProperty"/> instances that are different views of the same information.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct TextViewBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal TextViewBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    public VCardBuilder SetIndexes(bool skipEmptyItems = true) =>
        Edit(static (props, skip) =>
        {
            props.SetIndexes(skip);
            return props;
        }, skipEmptyItems);

    public VCardBuilder UnsetIndexes() =>
        Edit(static props =>
        {
            props.UnsetIndexes();
            return props;
        });

    public VCardBuilder Edit<TData>(Func<IEnumerable<TextProperty>, TData, IEnumerable<TextProperty?>?> func, TData data)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func.Invoke(props, data));
        return _builder;
    }

    /// <summary>
    /// Allows to edit the items of the specified property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the specified property
    /// as argument.
    /// Its return value will be the new content of the specified property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TextViewBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<TextProperty>, IEnumerable<TextProperty?>?> func)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func.Invoke(props));
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<TextProperty> GetProperty() =>
        Builder.VCard.Get<IEnumerable<TextProperty?>?>(_prop)?
                                 .WhereNotNull() ?? [];

    private void SetProperty(IEnumerable<TextProperty?>? value)
    {
        Debug.Assert(_builder != null);
        _builder.VCard.Set(_prop, value);
    }

    /// <summary>
    /// Adds a <see cref="TextProperty"/> instance, which is newly initialized from a 
    /// <see cref="string"/>, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="value">A <see cref="string" /> or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of 
    /// <see cref="VCardProperty" /> objects, which the <see cref="VCardProperty" /> should belong to, 
    /// or <c>null</c> to indicate that the <see cref="VCardProperty" /> does not belong to any group. The 
    /// function is called with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TextViewBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, 
                          VCardBuilder.Add(new TextProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<TextProperty?>?>(_prop),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the specified property of the <see cref="VCardBuilder.VCard"/> to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TextViewBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="TextProperty"/> objects that match a specified predicate from the specified 
    /// property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="TextProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TextBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<TextProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop, 
                          _builder.VCard.Get<IEnumerable<TextProperty?>?>(_prop)
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
