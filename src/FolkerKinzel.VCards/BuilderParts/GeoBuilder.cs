using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

//[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct GeoBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal GeoBuilder(VCardBuilder builder) => _builder = builder;

    public VCardBuilder Add(double latitude,
                            double longitude,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, VCardBuilder.Add(new GeoProperty(latitude, longitude, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                                  parameters,
                                                  false));
        return _builder;
    }

    public VCardBuilder Add(GeoCoordinate? value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, VCardBuilder.Add(new GeoProperty(value, group?.Invoke(_builder.VCard)),
                                                  _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates),
                                                  parameters,
                                                  false));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.GeoCoordinates"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.GeoCoordinates, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="GeoProperty"/> objects that match a specified predicate from the <see cref="VCard.GeoCoordinates"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="GeoProperty"/> objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="GeoBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<GeoProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.GeoCoordinates, _builder.VCard.Get<IEnumerable<GeoProperty?>?>(Prop.GeoCoordinates).Remove(predicate));
        return _builder;
    }

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override int GetHashCode() => base.GetHashCode();

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override string ToString() => base.ToString()!;

}
