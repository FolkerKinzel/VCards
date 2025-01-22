using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.NonStandards"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
#if !(NET462 || NETSTANDARD2_0 || NETSTANDARD2_1)
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
#endif
public readonly struct NonStandardBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NonStandardBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Edits the content of the <see cref="VCard.NonStandards"/> property with a delegate and 
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.NonStandards"/> property and <paramref name="arg"/> as arguments. Its return 
    /// value will be the new content of the <see cref="VCard.NonStandards"/> property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this 
    /// <see cref="NonStandardBuilder"/> to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(
        Func<IEnumerable<NonStandardProperty>, TArg, IEnumerable<NonStandardProperty?>?> func,
        TArg arg)
    {
        IEnumerable<NonStandardProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NonStandards = func(props, arg);
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.NonStandards"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.NonStandards"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.NonStandards"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<NonStandardProperty>, IEnumerable<NonStandardProperty?>?> func)
    {
        IEnumerable<NonStandardProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.NonStandards = func(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<NonStandardProperty> GetProperty() =>
        Builder.VCard.NonStandards?.OfType<NonStandardProperty>() ?? [];

    /// <summary>
    /// Adds a <see cref="NonStandardProperty"/> instance, which is newly initialized using the 
    /// specified arguments, to the <see cref="VCard.NonStandards"/> property.
    /// </summary>
    /// <param name="key">The key ("name") of the non-standard vCard property (format: <c>X-NAME</c>).</param>
    /// <param name="value">The value of the vCard property: any data encoded as <see
    /// cref="string" /> or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/>
    /// to be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// If the arguments are not valid, an empty <see cref="NonStandardProperty"/> with the key "Empty" will be 
    /// added. In any case such a <see cref="NonStandardProperty"/> won't be written to a VCF file.
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string key,
                            string? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        VCard vCard = Builder.VCard;
        NonStandardProperty prop;

        try
        {
            prop = new NonStandardProperty(key, value, group?.Invoke(vCard));
        }
        catch
        {
            prop = new NonStandardProperty(group?.Invoke(vCard));
        }

        vCard.Set(Prop.NonStandards,
                  VCardBuilder.Add(prop,
                                   _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards),
                                   parameters)
                  );

        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.NonStandards"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NonStandards, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="NonStandardProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.NonStandards"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="NonStandardProperty"/>
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<NonStandardProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NonStandards,
                          _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards)
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

