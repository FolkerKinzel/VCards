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
/// <see cref="DateAndOrTimeProperty"/> instances.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this structure in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
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
    /// Allows to edit the items of the specified property with a delegate.
    /// </summary>
    /// <param name="action">An <see cref="Action{T}"/> delegate that's invoked with the items 
    /// of the specified property that are not <c>null</c>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DataBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Action<IEnumerable<DateAndOrTimeProperty>> action)
    {
        var props = 
            Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop)?.WhereNotNull() ?? [];
        _ArgumentNullException.ThrowIfNull(action, nameof(action));
        action.Invoke(props);
        return _builder;
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
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="year"/> is less than 1 or greater than 9999.</para>
    /// <para>-or-</para>
    /// <para><paramref name="month"/> is less than 1 or greater than 12.</para>
    /// <para>-or-</para>
    /// <para><paramref name="day"/> is less than 1 or greater than the number of days in 
    /// <paramref name="month"/>.</para>
    /// </exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(int year,
                            int month,
                            int day,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, 
                          VCardBuilder.Add(DateAndOrTimeProperty.FromDate(year,
                                                                          month,
                                                                          day,
                                                                          group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false)
                          );
        return _builder;
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
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="month"/> is less than 1 or greater than 12.</para>
    /// <para>-or-</para>
    /// <para><paramref name="day"/> is less than 1 or greater than the number of days 
    /// that <paramref name="month"/> has in a leap year.</para>
    /// </exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(int month,
                            int day,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop,
                          VCardBuilder.Add(DateAndOrTimeProperty.FromDate(month,
                                                                          day,
                                                                          group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false)
                          );
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly 
    /// initialized from a <see cref="DateOnly"/> 
    /// value, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly"/> value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been 
    /// initialized using the default constructor.</exception>
    public VCardBuilder Add(DateOnly date,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop,
                          VCardBuilder.Add(DateAndOrTimeProperty.FromDate(date, group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false)
                          );
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly initialized from 
    /// a <see cref="System.DateTime"/> or <see cref="DateTimeOffset"/> value, to the specified property 
    /// of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="dateTime">A <see cref="System.DateTime"/> or <see cref="DateTimeOffset"/> value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/>
    /// of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" /> 
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(DateTimeOffset dateTime,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, 
                          VCardBuilder.Add(DateAndOrTimeProperty.FromDateTime(dateTime, group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false));
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly initialized from a 
    /// <see cref="TimeOnly"/> value, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="time">A <see cref="TimeOnly"/> value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(TimeOnly time,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop, 
                          VCardBuilder.Add(DateAndOrTimeProperty.FromTime(time, group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false)
                          );
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="DateAndOrTimeProperty"/> instance, which is newly initialized from a 
    /// <see cref="TimeOnly"/> value, to the specified property of the <see cref="VCardBuilder.VCard"/>.
    /// </summary>
    /// <param name="text">Any text or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called 
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="DateAndOrTimeBuilder"/>
    /// to be able to chain calls.</returns>
    /// <example>
    /// <code language="none">
    /// After midnight.
    /// </code>
    /// </example>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string? text,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(_prop,
                          VCardBuilder.Add(DateAndOrTimeProperty.FromText(text, group?.Invoke(_builder.VCard)),
                                           Builder.VCard.Get<IEnumerable<DateAndOrTimeProperty?>?>(_prop),
                                           parameters,
                                           false)
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
