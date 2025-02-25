using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.GeoCoordinates"/> property.
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
public readonly struct GeoBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal GeoBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Sets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in the <see cref="VCard.GeoCoordinates"/> property depending on their position
    /// in that collection and allows to specify whether to skip empty items in that process.
    /// (The first item gets the highest preference <c>1</c>.)
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to give empty <see cref="VCardProperty"/> 
    /// objects always the lowest <see cref="ParameterSection.Preference"/> (100), independently
    /// of their position in the collection, or <c>false</c> to treat empty <see cref="VCardProperty"/> 
    /// objects like any other. (<c>null</c> references are always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetPreferences(bool skipEmptyItems = true)
    {
        Builder.VCard.GeoCoordinates.SetPreferences(skipEmptyItems);
        return _builder;
    }

    /// <summary>
    /// Resets the <see cref="ParameterSection.Preference"/> properties of 
    /// the items in in the <see cref="VCard.GeoCoordinates"/> property to the lowest value (100).
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetPreferences()
    {
        Builder.VCard.GeoCoordinates.UnsetPreferences();
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.GeoCoordinates"/> property ascending depending on their 
    /// position in that collection and allows to specify whether to skip empty items in that 
    /// process.
    /// </summary>
    /// <param name="skipEmptyItems"><c>true</c> to reset the <see cref="ParameterSection.Index"/> 
    /// of empty <see cref="VCardProperty"/> objects to <c>null</c>, or <c>false</c> to treat 
    /// empty <see cref="VCardProperty"/> objects like any other. (<c>null</c> references are 
    /// always skipped.)</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder SetIndexes(bool skipEmptyItems = true)
    {
        Builder.VCard.GeoCoordinates.SetIndexes(skipEmptyItems);
        return _builder;
    }

    /// <summary>
    /// Resets the <see cref="ParameterSection.Index"/> properties of 
    /// the items in in the <see cref="VCard.GeoCoordinates"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder UnsetIndexes()
    {
        Builder.VCard.GeoCoordinates.UnsetIndexes();
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.GeoCoordinates"/> property with a delegate and 
    /// allows to pass an argument to this delegate.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.GeoCoordinates"/> property and <paramref name="arg"/> as arguments. Its return value 
    /// will be the new content of the <see cref="VCard.GeoCoordinates"/> property.</param>
    /// <param name="arg">The argument to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TArg>(Func<IEnumerable<GeoProperty>, TArg, IEnumerable<GeoProperty?>?> func,
                                   TArg arg)
    {
        IEnumerable<GeoProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.GeoCoordinates = func(props, arg);
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.GeoCoordinates"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with a collection of the non-<c>null</c> items of the <see cref="VCard.GeoCoordinates"/>
    /// property as argument.
    /// Its return value will be the 
    /// new content of the <see cref="VCard.GeoCoordinates"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IEnumerable<GeoProperty>, IEnumerable<GeoProperty?>?> func)
    {
        IEnumerable<GeoProperty> props = GetProperty();
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.GeoCoordinates = func(props);
        return _builder;
    }

    [MemberNotNull(nameof(_builder))]
    private IEnumerable<GeoProperty> GetProperty()
        => Builder.VCard.GeoCoordinates?.OfType<GeoProperty>() ?? [];

    /// <summary>
    /// Adds a <see cref="GeoProperty"/> instance, which is newly initialized using the specified 
    /// arguments, to the <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="latitude">Latitude (value between -90 and 90).</param>
    /// <param name="longitude">Longitude (value between -180 and 180).</param>
    /// <param name="uncertainty">The amount of uncertainty in the location as a 
    /// value in meters, or <c>null</c> to leave this unspecified.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" /> 
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <remarks>If the arguments are out of range, an empty <see cref="GeoProperty"/> instance is added.</remarks>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Add(double latitude,
                            double longitude,
                            float? uncertainty = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Add(GeoCoordinate.TryCreate(latitude, longitude, uncertainty), parameters, group);

    /// <summary>
    /// Adds a <see cref="GeoProperty"/> instance, which is newly initialized using the specified
    /// arguments, to the <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="value">A <see cref="GeoCoordinate" /> object or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// 
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to be 
    /// able to chain calls.</returns>
    /// 
    /// <remarks>If <paramref name="value"/> is <c>null</c>, an empty <see cref="GeoProperty"/> instance is added.</remarks>
    /// 
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(GeoCoordinate? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates,
                          VCardBuilder.Add(new GeoProperty(value ?? GeoCoordinate.Empty, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                           parameters)
                          );
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.GeoCoordinates"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.GeoCoordinates, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="GeoProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="GeoProperty"/> objects 
    /// that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<GeoProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.GeoCoordinates,
                          _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates)
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
