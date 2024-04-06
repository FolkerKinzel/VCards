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
/// Provides methods for editing the <see cref="VCard.GeoCoordinates"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this structure in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct GeoBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal GeoBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Allows to edit the items of the <see cref="VCard.GeoCoordinates"/> property with a specified delegate.
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
        var props = Builder.VCard.GeoCoordinates?.WhereNotNull() ?? [];
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.GeoCoordinates = func.Invoke(props);
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="GeoProperty"/> instance, which is newly initialized using the specified 
    /// arguments, to the <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="latitude">Latitude (value between -90 and 90).</param>
    /// <param name="longitude">Longitude (value between -180 and 180).</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> 
    /// the highest preference <c>(1)</c> and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" /> 
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="latitude" />
    /// or <paramref name="longitude" /> does not have a valid value.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(double latitude,
                            double longitude,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, 
                          VCardBuilder.Add(new GeoProperty(latitude, longitude, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                           parameters,
                                           pref)
                          );
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="GeoProperty"/> instance, which is newly initialized using the specified
    /// arguments, to the <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="value">A <see cref="GeoCoordinate" /> object or <c>null</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> 
    /// the highest preference <c>(1)</c> and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called
    /// with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to be 
    /// able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(GeoCoordinate? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates,
                          VCardBuilder.Add(new GeoProperty(value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                           parameters,
                                           pref)
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
