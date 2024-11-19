using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties.Parameters;

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
#if !(NET462 || NETSTANDARD2_0 || NETSTANDARD2_1)
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
#endif
public readonly struct NameViewsBuilder
{
    #region Remove this code with version 8.0.0

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use Add(Name?, ...) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null,
                            Action<TextBuilder, NameProperty>? displayName = null) => throw new NotImplementedException();


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use Add(Name?, ...) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public VCardBuilder Add(string? familyName,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null,
                            Action<TextBuilder, NameProperty>? displayName = null) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use Add(Name?, ...) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public VCardBuilder Add(FolkerKinzel.VCards.NameBuilder builder,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null) => throw new NotImplementedException();

    #endregion

    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NameViewsBuilder(VCardBuilder builder) => _builder = builder;

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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/>
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/>
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
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.NameViews"/> property and <paramref name="arg"/> as arguments. Its return value 
    /// will be the new content of the <see cref="VCard.NameViews"/> property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(Func<IEnumerable<NameProperty>, TArg, IEnumerable<NameProperty?>?> func,
                                   TArg arg)
    {
        IEnumerable<NameProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NameViews = func(props, arg);
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<NameProperty>, IEnumerable<NameProperty?>?> func)
    {
        IEnumerable<NameProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NameViews = func(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<NameProperty> GetProperty() =>
        Builder.VCard.NameViews?.OfType<NameProperty>() ?? [];

    /// <summary>
    /// Adds a <see cref="NameProperty"/> instance, which is newly 
    /// initialized with the specified <see cref="Name"/>.
    /// </summary>
    /// <param name="value">The <see cref="Name"/> instance, or <c>null</c>.</param>
    /// 
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <seealso cref="NameBuilder"/>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(Name? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        VCard vc = Builder.VCard;
        var prop = new NameProperty(value ?? new Name(),
                                    group?.Invoke(vc));
        vc.Set(Prop.NameViews,
               VCardBuilder.Add(prop,
                                vc.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                                parameters));

        return _builder;
    }

    /// <summary>
    /// Adds automatically corresponding <see cref="TextProperty"/> instances for each <see cref="NameProperty"/> that is 
    /// currently in the <see cref="VCard"/> to <see cref="VCard.DisplayNames"/>.
    /// </summary>
    /// <param name="nameFormatter"></param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <para>
    /// The <see cref="NameProperty"/> instances are processed ordered by their <see cref="ParameterSection.Preference"/> value.
    /// Empty <see cref="NameProperty"/> instances will be skipped.
    /// If a <see cref="TextProperty"/> instance that is not empty and has the same <see cref="ParameterSection.Language"/>
    /// is still in <see cref="VCard.DisplayNames"/>, the corresponding <see cref="NameProperty"/> instance will be skipped.
    /// </para>
    /// <para>
    /// The newly created <see cref="TextProperty"/> instances will have their <see cref="ParameterSection.Derived"/> property set
    /// to <c>true</c>. The values of their <see cref="VCardProperty.Group"/> property will be taken from the 
    /// <see cref="NameProperty"/> instances.
    /// </para>
    /// <para>
    /// Empty <see cref="NameProperty"/> instances will be skipped.
    /// </para>
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <seealso cref="NameFormatter"/>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="nameFormatter"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder ToDisplayNames(INameFormatter nameFormatter)
    {
        VCard vc = Builder.VCard;
        _ArgumentNullException.ThrowIfNull(nameFormatter, nameof(nameFormatter));

        IEnumerable<NameProperty?>? names = vc.NameViews;

        if(names is null)
        {
            return _builder;
        }

        foreach (NameProperty nameProp in names.OrderByPref())
        {
            string? language = nameProp.Parameters.Language;

            if (vc.DisplayNames?.WhereNotEmpty().Any(x => x.Parameters.Language == language) ?? false)
            {
                continue;
            }

            _builder.DisplayNames.Add(
                nameFormatter.ToDisplayName(nameProp, vc), 
                parameters: p => { p.Language = language; p.Derived = true; },
                group: v => nameProp.Group
                ); 
        }

        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.NameViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/> 
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
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameViewsBuilder"/>
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

