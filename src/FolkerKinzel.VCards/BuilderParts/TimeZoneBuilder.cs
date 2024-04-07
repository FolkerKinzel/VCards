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
/// Provides methods for editing the <see cref="VCard.TimeZones"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this struct in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct TimeZoneBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal TimeZoneBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Allows to edit the items of the <see cref="VCard.TimeZones"/> property with a specified delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.TimeZones"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.TimeZones"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TimeZoneBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<TimeZoneProperty>, IEnumerable<TimeZoneProperty?>?> func)
    {
        var props = Builder.VCard.TimeZones?.WhereNotNull() ?? [];
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.TimeZones = func.Invoke(props);
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="TimeZoneProperty"/> instance, which is newly 
    /// initialized using the specified <see cref="TimeZoneID"/> instance, to the 
    /// <see cref="VCard.TimeZones"/> property.
    /// </summary>
    /// <param name="value">A <see cref="TimeZoneID" /> object or <c>null</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> 
    /// the highest preference <c>(1)</c> and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TimeZoneBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(TimeZoneID? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.TimeZones,
                          VCardBuilder.Add(new TimeZoneProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones),
                                           parameters,
                                           pref)
                          );
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="TimeZoneProperty"/> instance, which is newly 
    /// initialized using the specified <see cref="string"/>, which represents an identifier
    /// from the "IANA Time Zone Database", to the <see cref="VCard.TimeZones"/> property.
    /// (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .)
    /// </summary>
    /// <param name="value">A <see cref="string"/> that represents an identifier
    /// from the "IANA Time Zone Database".</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> 
    /// the highest preference <c>(1)</c> and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TimeZoneBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// This method initializes a new <see cref="TimeZoneID"/> instance. Use the overload
    /// <see cref="TimeZoneBuilder.Add(TimeZoneID?, bool, Action{ParameterSection}?, Func{VCard, string?}?)"/>
    /// to reuse an existing one.
    /// </remarks>
    /// 
    /// <example>
    /// <code language="cs" source="..\Examples\ExtensionMethodExample.cs"/>
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="value" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"> <paramref name="value" /> is an empty
    /// <see cref="string" /> or consists only of white space characters.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.TimeZones,
                          VCardBuilder.Add(new TimeZoneProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones),
                                           parameters,
                                           pref)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.TimeZones"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TimeZoneBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.TimeZones, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="TimeZoneProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="TimeZoneProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="TimeZoneBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<TimeZoneProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.TimeZones,
                          _builder.VCard.Get<IEnumerable<TimeZoneProperty?>?>(Prop.TimeZones)
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
