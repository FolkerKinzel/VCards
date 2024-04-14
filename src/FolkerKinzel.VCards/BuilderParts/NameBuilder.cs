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
/// Provides methods for editing the <see cref="VCard.NameViews"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NameBuilder(VCardBuilder builder) => _builder = builder;


    /// <summary>
    /// Sets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.NameViews"/> property ascending depending on their 
    /// position in that collection and allows to specify whether to skip empty items in that 
    /// process.
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to reset the <see cref="ParameterSection.Index"/> 
    /// of empty <see cref="VCardProperty"/> objects to <c>null</c>, or <c>false</c> to treat 
    /// empty <see cref="VCardProperty"/> objects like any other. (<c>null</c> references are 
    /// always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetIndexes(bool skipEmptyItems = true) =>
        Edit(static (props, skip) =>
        {
            props.SetIndexes(skip);
            return props;
        }, skipEmptyItems);

    /// <summary>
    /// Resets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.NameViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetIndexes() =>
        Edit(static props =>
        {
            props.UnsetIndexes();
            return props;
        });

    /// <summary>
    /// Edits the content of the <see cref="VCard.NameViews"/> property with a delegate and 
    /// allows to pass <paramref name="data"/> to this delegate.
    /// </summary>
    /// <typeparam name="TData">The type of <paramref name="data"/>.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.NameViews"/> property and <paramref name="data"/> as arguments. Its return value 
    /// will be the new content of the <see cref="VCard.NameViews"/> property.</param>
    /// <param name="data">The data to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TData>(Func<IEnumerable<NameProperty>, TData, IEnumerable<NameProperty?>?> func,
                                    TData data)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NameViews = func.Invoke(props, data);
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.NameViews"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.NameViews"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.NameViews"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<NameProperty>, IEnumerable<NameProperty?>?> func)
    {
        var props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NameViews = func.Invoke(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<NameProperty> GetProperty() =>
        Builder.VCard.NameViews?.WhereNotNull() ?? [];

    /// <summary>
    /// Adds a <see cref="NameProperty"/> instance, which is newly 
    /// initialized using the specified arguments, to the <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="familyNames">Family Names (also known as surnames)</param>
    /// <param name="givenNames">Given Names (first names)</param>
    /// <param name="additionalNames">Additional Names (middle names)</param>
    /// <param name="prefixes">Honorific Prefixes</param>
    /// <param name="suffixes">Honorific Suffixes</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <param name="displayName">An <see cref="Action{T1, T2}"/> delegate that's invoked with the 
    /// <see cref="TextBuilder"/> the <see cref="VCardBuilder.DisplayNames"/> property returns and 
    /// the newly created <see cref="NameProperty"/> instance as arguments.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
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
               parameters));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="NameProperty"/> instance, which is newly 
    /// initialized using the specified arguments, to the <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="familyName">Family Name (also known as surname)</param>
    /// <param name="givenName">Given Name (first name)</param>
    /// <param name="additionalName">Additional Name (middle name)</param>
    /// <param name="prefix">Honorific Prefix</param>
    /// <param name="suffix">Honorific Suffix</param>
    /// <param name="displayName">An <see cref="Action{T1, T2}"/> delegate that's invoked with the 
    /// <see cref="TextBuilder"/> the <see cref="VCardBuilder.DisplayNames"/> property returns and the newly
    /// created <see cref="NameProperty"/> instance as arguments.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string? familyName,
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null,
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
                                parameters));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.NameViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NameViews, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="NameProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="NameProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<NameProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NameViews, 
                          _builder.VCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews)
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

