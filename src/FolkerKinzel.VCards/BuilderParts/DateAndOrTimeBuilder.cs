﻿using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing <see cref="VCard"/> properties that contain 
/// <see cref="DateAndOrTimeProperty"/> instances.
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
public readonly struct DateAndOrTimeBuilder
{
    private readonly VCardBuilder? _builder;
    private readonly Prop _prop;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal DateAndOrTimeBuilder(VCardBuilder builder, Prop prop)
    {
        _builder = builder;
        _prop = prop;
    }

    /// <summary>
    /// Sets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the specified <see cref="VCard"/> property ascending depending on their 
    /// position in that collection and allows to specify whether to skip empty items in that 
    /// process.
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to reset the <see cref="ParameterSection.Index"/> 
    /// of empty <see cref="VCardProperty"/> objects to <c>null</c>, or <c>false</c> to treat 
    /// empty <see cref="VCardProperty"/> objects like any other. (<c>null</c> references are 
    /// always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetIndexes(bool skipEmptyItems = true)
    {
        GetProperty().SetIndexes(skipEmptyItems);
        return _builder;
    }

    /// <summary>
    /// Resets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the specified <see cref="VCard"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetIndexes()
    {
        GetProperty().UnsetIndexes();
        return _builder;
    }

    /// <summary>
    /// Edits the content of the specified <see cref="VCard"/> property with a delegate and 
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// specified <see cref="VCard"/> property and <paramref name="arg"/> as arguments. Its return value 
    /// will be the new content of the specified <see cref="VCard"/> property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this 
    /// <see cref="DateAndOrTimeBuilder"/> to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(
        Func<IEnumerable<DateAndOrTimeProperty>, TArg, IEnumerable<DateAndOrTimeProperty?>?> func,
        TArg arg)
    {
        IEnumerable<DateAndOrTimeProperty> props = GetNonNullableProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props, arg));
        return _builder;
    }

    /// <summary>
    /// Edits the content of the specified <see cref="VCard"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the specified <see cref="VCard"/> property
    /// as argument.
    /// Its return value will be the new content of the specified <see cref="VCard"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="RawDataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(
        Func<IEnumerable<DateAndOrTimeProperty>, IEnumerable<DateAndOrTimeProperty?>?> func)
    {
        IEnumerable<DateAndOrTimeProperty> props = GetNonNullableProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        SetProperty(func(props));
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<DateAndOrTimeProperty?>? GetProperty()
        => Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop);

    [MemberNotNull(nameof(_builder))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<DateAndOrTimeProperty> GetNonNullableProperty()
        => GetProperty()?.OfType<DateAndOrTimeProperty>() ?? [];

    private void SetProperty(IEnumerable<DateAndOrTimeProperty?>? value)
    {
        Debug.Assert(_builder != null);
        _builder.VCard.Set(_prop, value);
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly 
    /// initialized from a date in the Gregorian
    /// calendar, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="year">The year (1 bis 9999).</param>
    /// <param name="month">The month (1 bis 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month"/>).</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(int year,
                            int month,
                            int day,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        DateAndOrTime val;

        try
        {
            val = new DateOnly(year, month, day);
        }
        catch
        {
            val = DateAndOrTime.Empty;
        }

        return Add(val, parameters, group);
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly 
    /// initialized from a recurring date in the Gregorian
    /// calendar, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="month">The month (1 bis 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month"/> -
    /// a leap year may be assumed.)</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload is intended to be used for recurring dates, like, e.g., birthdays, or 
    /// if the year is unknown.
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(int month,
                            int day,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        DateAndOrTime val;

        try
        {
            val = DateAndOrTime.Create(new DateOnly(DateAndOrTimeConverter.FIRST_LEAP_YEAR, month, day), ignoreYear: true);
        }
        catch
        {
            val = DateAndOrTime.Empty;
        }

        return Add(val, parameters, group);
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly initialized with a specified
    /// <see cref="DateAndOrTime"/> instance, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="value">A <see cref="DateAndOrTime"/> instance, or <c>null</c>. <see cref="DateOnly"/>, 
    /// <see cref="System.DateTime"/>, <see cref="DateTimeOffset"/> and <see cref="TimeOnly"/> values can be passed
    /// directly because <see cref="DateAndOrTime"/> overloads the <c>implicit</c> operators.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(DateAndOrTime? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop,
                          VCardBuilder.Add(new DateAndOrTimeProperty(value ?? DateAndOrTime.Empty, group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the specified property of the <see cref="VCardBuilder.VCard"/> to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(_prop, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="DateAndOrTimeProperty"/> objects that match a specified predicate from 
    /// the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="DateAndOrTimeProperty"/>
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this 
    /// <see cref="DateAndOrTimeBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<DateAndOrTimeProperty, bool> predicate)
    {
        Builder.VCard.Set(_prop,
                          Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop)
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
