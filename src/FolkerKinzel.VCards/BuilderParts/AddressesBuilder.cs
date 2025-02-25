using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.Addresses"/> property.
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
public readonly struct AddressesBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal AddressesBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Sets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in the <see cref="VCard.Addresses"/> property depending on their position
    /// in that collection and allows to specify whether to skip empty items in that process.
    /// (The first item gets the highest preference <c>1</c>.)
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to give empty <see cref="VCardProperty"/> 
    /// objects always the lowest <see cref="ParameterSection.Preference"/> (100), independently
    /// of their position in the collection, or <c>false</c> to treat empty <see cref="VCardProperty"/> 
    /// objects like any other. (<c>null</c> references are always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetPreferences(bool skipEmptyItems = true)
    {
        Builder.VCard.Addresses.SetPreferences(skipEmptyItems);
        return _builder;
    }

    /// <summary>
    /// Resets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in in the <see cref="VCard.Addresses"/> property to the lowest value (100).
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetPreferences()
    {
        Builder.VCard.Addresses.UnsetPreferences();
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.Addresses"/> property ascending depending on their 
    /// position in that collection and allows to specify whether to skip empty items in that 
    /// process.
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to reset the <see cref="ParameterSection.Index"/> 
    /// of empty <see cref="VCardProperty"/> objects to <c>null</c>, or <c>false</c> to treat 
    /// empty <see cref="VCardProperty"/> objects like any other. (<c>null</c> references are 
    /// always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetIndexes(bool skipEmptyItems = true)
    {
        Builder.VCard.Addresses.SetIndexes(skipEmptyItems);
        return _builder;
    }

    /// <summary>
    /// Resets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.Addresses"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetIndexes()
    {
        Builder.VCard.Addresses.UnsetIndexes();
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.Addresses"/> property with a delegate and 
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.Addresses"/> property and <paramref name="arg"/> as arguments. Its return value 
    /// will be the new content of the <see cref="VCard.Addresses"/> property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(Func<IEnumerable<AddressProperty>, TArg, IEnumerable<AddressProperty?>?> func,
                                   TArg arg)
    {
        IEnumerable<AddressProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.Addresses = func(props, arg);
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.Addresses"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.Addresses"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.Addresses"/> property.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been 
    /// initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<AddressProperty>, IEnumerable<AddressProperty?>?> func)
    {
        IEnumerable<AddressProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.Addresses = func(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<AddressProperty> GetProperty()
        => Builder.VCard.Addresses?.OfType<AddressProperty>() ?? [];

    /// <summary>
    /// Adds an <see cref="AddressProperty"/> instance, which is newly 
    /// initialized with the specified <see cref="Name"/>.
    /// </summary>
    /// <param name="value">The <see cref="Address"/> instance, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <note type="tip">
    /// Use an <see cref="AddressesBuilder"/> to create a new <see cref="Address"/>.
    /// </note>
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <seealso cref="VCards.AddressBuilder"/>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(Address? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        VCard vc = Builder.VCard;
        var prop = new AddressProperty(value ?? Address.Empty, group?.Invoke(vc));
        vc.Set(Prop.Addresses,
               VCardBuilder.Add(prop,
                                vc.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses),
                                parameters));

        return _builder;
    }

    /// <summary>
    /// Attaches <c>LABEL</c> parameters (<see cref="ParameterSection.Label"/>) to all 
    /// <see cref="AddressProperty"/> instances that are currently in the <see cref="VCard"/>.
    /// </summary>
    /// <param name="addressFormatter">The <see cref="IAddressFormatter"/> instance to
    /// use for the conversion.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks><see cref="AddressProperty"/> instances whose <see cref="ParameterSection.Label"/> property
    /// is not <c>null</c> will be skipped.</remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <seealso cref="AddressFormatter"/>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="addressFormatter"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder AttachLabels(IAddressFormatter addressFormatter)
    {
        VCard vc = Builder.VCard;
        _ArgumentNullException.ThrowIfNull(addressFormatter, nameof(addressFormatter));
        IEnumerable<AddressProperty?>? addresses = vc.Addresses;

        if (addresses is null)
        {
            return _builder;
        }

        foreach (AddressProperty? adrProp in addresses)
        {
            if (adrProp is null)
            {
                continue;
            }

            if (adrProp.Parameters.Label is null)
            {
                adrProp.Parameters.Label = addressFormatter.ToLabel(adrProp);
            }
        }

        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Addresses"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Addresses, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="AddressProperty"/> objects that match a specified predicate from 
    /// the <see cref="VCard.Addresses"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="AddressProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AddressesBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<AddressProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.Addresses,
                          _builder.VCard.Get<IEnumerable<AddressProperty?>?>(Prop.Addresses)
                                        .Remove(predicate));
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
