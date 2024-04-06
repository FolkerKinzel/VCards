using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.Access"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this structure in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct AccessBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal AccessBuilder(VCardBuilder builder) => _builder = builder;


    /// <summary>
    /// Allows to edit the content of the <see cref="VCard.Access"/> property with a specified delegate.
    /// </summary>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.Access"/> property as argument. Its return value will be the new content of the 
    /// <see cref="VCard.Access"/> property.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AccessBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<AccessProperty?, AccessProperty?> func)
    {
        var prop = Builder.VCard.Access;
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.Access = func.Invoke(prop);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Access"/> property to an <see cref="AccessProperty"/> instance that is 
    /// newly initialized using the specified arguments.
    /// </summary>
    /// <param name="value">A member of the <see cref="Access" /> enum.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is 
    /// called with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AccessBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been
    /// initialized using the default constructor.</exception>
    public VCardBuilder Set(Access value,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Access, 
                          new AccessProperty(value, group?.Invoke(_builder.VCard)));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Access"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AccessBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been
    /// initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Access, null);
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
