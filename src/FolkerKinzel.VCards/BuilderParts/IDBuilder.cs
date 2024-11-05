﻿using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.ID"/> property.
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
public readonly struct IDBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal IDBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Edits the content of the <see cref="VCard.ID"/> property with a delegate and 
    /// allows to pass <paramref name="data"/> to this delegate.
    /// </summary>
    /// <typeparam name="TData">The type of <paramref name="data"/>.</typeparam>
    /// <param name="func">A function called with the content of the 
    /// <see cref="VCard.ID"/> property and <paramref name="data"/> as arguments. Its return value 
    /// will be the new content of the <see cref="VCard.ID"/> property.</param>
    /// <param name="data">The data to pass to <paramref name="func"/>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/>
    /// to be able to chain calls.</returns>
    /// <remarks>
    /// This overload allows to pass external data to the delegate without having to use closures.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit<TData>(Func<IDProperty?, TData, IDProperty?> func, TData data)
    {
        IDProperty? prop = Builder.VCard.ID;
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.ID = func(prop, data);
        return _builder;
    }

    /// <summary>
    /// Edits the content of the <see cref="VCard.ID"/> property with a delegate.
    /// </summary>
    /// <param name="func">
    /// A function called with the content of the 
    /// <see cref="VCard.ID"/> property as argument. Its return value will be the new content of the 
    /// <see cref="VCard.ID"/> property.
    /// </param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Func<IDProperty?, IDProperty?> func)
    {
        IDProperty? prop = Builder.VCard.ID;
        _ArgumentNullException.ThrowIfNull(func, nameof(func));
        _builder.VCard.ID = func(prop);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized with a new <see cref="Guid"/>.
    /// </summary>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is 
    /// called with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Set(Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Set(ContactID.Create(), parameters, group);

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized using a specified <see cref="ContactID"/>.
    /// </summary>
    /// <param name="id">A <see cref="ContactID" /> instance, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// The method sets <see cref="VCard.ID"/> to an empty <see cref="IDProperty"/> instance if
    /// <paramref name="id"/> is <c>null</c>. 
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Set(ContactID? id,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        VCard vc = Builder.VCard;
        var property = new IDProperty(id ?? ContactID.Empty, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.ID, property);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized using a specified absolute <see cref="Uri"/>.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri" />, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <para>If <paramref name="uri"/> is a valid "uuid" URN, a <see cref="ContactID"/> instance
    /// with this <see cref="Guid"/> value will be created.</para>
    /// <para>
    /// The method sets <see cref="VCard.ID"/> to an empty <see cref="IDProperty"/> instance if
    /// <paramref name="uri"/> is <c>null</c>. If <paramref name="uri"/> is a relative <see cref="Uri"/>,
    /// its <see cref="Uri.OriginalString"/> will be preserved.
    /// </para>
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Set(Uri? uri,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Set(ContactIDFromUri(uri), parameters, group);

    internal static ContactID ContactIDFromUri(Uri? uri)
    {
        return uri is null
                   ? ContactID.Empty
                   : uri.IsAbsoluteUri
                      ? ContactID.Create(uri)
                      : string.IsNullOrEmpty(uri.OriginalString)
                          ? ContactID.Empty
                          : ContactID.Create(uri.OriginalString);
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized using free-form text.
    /// </summary>
    /// <param name="text">Free-form text, or <c>null</c>.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// <remarks>
    /// <para>If <paramref name="text"/> represents a <see cref="Guid"/>, a <see cref="ContactID"/> instance
    /// with this <see cref="Guid"/> value will be created.</para>
    /// <para>
    /// The method sets <see cref="VCard.ID"/> to an empty <see cref="IDProperty"/> instance if
    /// <paramref name="text"/> is <c>null</c>, or an empty <see cref="string"/>, or if it consists only of 
    /// white space.
    /// </para>
    /// </remarks>
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Set(string? text,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Set(string.IsNullOrWhiteSpace(text)
                         ? ContactID.Empty
                         : ContactID.Create(text), parameters, group);

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized using a specified <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">A <see cref="Guid" /> value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// 
    /// 
    /// 
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VCardBuilder Set(Guid guid,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
        => Set(ContactID.Create(guid), parameters, group);

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.ID, null);
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
