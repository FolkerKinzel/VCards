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

[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "<Pending>")]
public readonly struct NonStandardBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))] 
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NonStandardBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Allows to edit the items of the <see cref="VCard.NonStandards"/> property with a specified delegate.
    /// </summary>
    /// <param name="action">An <see cref="Action{T}"/> delegate that's invoked with the items of the <see cref="VCard.NonStandards"/> property 
    /// that are not <c>null</c>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Action<IEnumerable<NonStandardProperty>> action)
    {
        var props = Builder.VCard.NonStandards?.WhereNotNull() ?? [];
        _ArgumentNullException.ThrowIfNull(action, nameof(action));
        action.Invoke(props);
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="NonStandardProperty"/> instance, which is newly 
    /// initialized using the specified arguments, to the <see cref="VCard.NonStandards"/> property.
    /// </summary>
    /// <param name="xName">The key ("name") of the non-standard vCard property
    /// (format: <c>X-NAME</c>).</param>
    /// <param name="value">The value of the vCard property: any data encoded as <see
    /// cref="string" /> or <c>null</c>.</param>
    /// <param name="pref">Pass <c>true</c> to give the newly created <see cref="VCardProperty"/> the highest preference <c>(1)</c>
    /// and to downgrade the other instances in the collection.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the <see cref="ParameterSection"/> of the newly 
    /// created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="xName" /> is
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="xName" /> is not
    /// a valid X-NAME.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Add(string xName,
                            string? value,
                            bool pref = false,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.NonStandards,
                          VCardBuilder.Add(new NonStandardProperty(xName, value, group?.Invoke(_builder.VCard)),
                                           _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards),
                                           parameters,
                                           pref));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.NonStandards"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NonStandards, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="NonStandardProperty"/> objects that match a specified predicate from the <see cref="VCard.NonStandards"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="NonStandardProperty"/> objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NonStandardBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<NonStandardProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NonStandards,
                          _builder.VCard.Get<IEnumerable<NonStandardProperty?>?>(Prop.NonStandards).Remove(predicate));
        return _builder;
    }

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

