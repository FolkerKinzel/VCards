﻿using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// A union that encapsulates a globally unique identifier corresponding to the entity associated 
/// with the vCard. The value of this identifier can be either a <see cref="System.Guid"/>,
/// a <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
public abstract class ContactID : IEquatable<ContactID>
{
    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a newly
    /// created <see cref="System.Guid"/>.
    /// </summary>
    /// <returns>A newly created <see cref="ContactID"/> instance that 
    /// has been initialized with a newly created <see cref="Guid"/>.</returns>
    public static ContactID Create()
#if NET9_0_OR_GREATER
        => new ContactIDGuid(System.Guid.CreateVersion7());
#else
        => new ContactIDGuid(System.Guid.NewGuid());
#endif

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a specified
    /// <see cref="System.Guid"/>.
    /// </summary>
    /// <param name="guid">A <see cref="System.Guid"/>.</param>
    /// <returns>The newly created <see cref="ContactID"/> instance.</returns>
    public static ContactID Create(Guid guid)
        => guid == System.Guid.Empty ? Empty : new ContactIDGuid(guid);

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a specified
    /// absolute <see cref="System.Uri"/>.
    /// </summary>
    /// <param name="uri">An absolute <see cref="System.Uri"/>.</param>
    /// <returns>The newly created <see cref="ContactID"/> instance.</returns>
    /// <remarks>
    /// If <paramref name="uri"/> is a valid "uuid" URN, a <see cref="ContactID"/> instance
    /// with this <see cref="Guid"/> value will be created.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is not an absolute <see cref="Uri"/>.</exception>
    public static ContactID Create(Uri uri)
    {
        _ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        return !uri.IsAbsoluteUri
            ? throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)))
            : uri.AbsoluteUri.StartsWith("urn:uuid:", StringComparison.OrdinalIgnoreCase)
                ? UuidConverter.TryAsGuid(uri.AbsoluteUri.AsSpan(), out Guid uuid)
                    ? Create(uuid)
                    : new ContactIDUri(uri)
                : new ContactIDUri(uri);
    }

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a specified
    /// <see cref="string"/>.
    /// </summary>
    /// <param name="text">A <see cref="string"/> that can be used as identifier.</param>
    /// <returns>The newly created <see cref="ContactID"/> instance.</returns>
    /// <remarks>
    /// If <paramref name="text"/> represents a <see cref="Guid"/>, a <see cref="ContactID"/> instance
    /// with this <see cref="Guid"/> value will be created. If <paramref name="text"/> represents an absolute
    /// <see cref="Uri"/>, a <see cref="ContactID"/> instance containing a <see cref="Uri"/> will
    /// be created.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> is an empty <see cref="string"/> 
    /// or consists only of white space.</exception>
    public static ContactID Create(string text)
    {
        _ArgumentNullException.ThrowIfNull(text, nameof(text));

        return string.IsNullOrWhiteSpace(text)
            ? throw new ArgumentException(string.Format(Res.Whitespace, nameof(text)))
            : UuidConverter.TryAsGuid(text.AsSpan().Trim(), out Guid uuid)
                ? Create(uuid)
                : Uri.TryCreate(text, UriKind.Absolute, out Uri? uri)
                    ? new ContactIDUri(uri)
                    : new ContactIDString(text);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't identify anything, otherwise <c>false</c>.
    /// </summary>
    /// <remarks>
    /// <see cref="ContactID.Empty"/> is a singleton that encapsulates an empty 
    /// <see cref="string"/>.
    /// </remarks>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// A singleton whose <see cref="IsEmpty"/> property returns <c>true</c>.
    /// </summary>
    /// <remarks>
    /// The singleton instance encapsulates an empty <see cref="string"/>.
    /// </remarks>
    public static ContactID Empty { get; } = new ContactIDString("");

    /// <summary>
    /// Gets the encapsulated <see cref="Guid"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public abstract Guid? Guid { get; }

    /// <summary>
    /// Gets the encapsulated absolute <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public abstract Uri? Uri { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public abstract string? String { get; }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="guidAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="Guid"/>.</param>
    /// <param name="uriAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// 
    public abstract void Switch(Action<Guid>? guidAction = null,
                                Action<Uri>? uriAction = null,
                                Action<string>? stringAction = null);

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value and allows to pass an argument to the delegates.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="guidAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="Guid"/>.</param>
    /// <param name="uriAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    public abstract void Switch<TArg>(TArg arg,
                             Action<Guid, TArg>? guidAction = null,
                             Action<Uri, TArg>? uriAction = null,
                             Action<string, TArg>? stringAction = null);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="guidFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is a <see cref="Guid"/>.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public abstract TResult Convert<TResult>(Func<Guid, TResult> guidFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/> and allows to specify an
    /// argument for the conversion.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// 
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="guidFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is a <see cref="Guid"/>.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public abstract TResult Convert<TArg, TResult>(TArg arg,
                                          Func<Guid, TArg, TResult> guidFunc,
                                          Func<Uri, TArg, TResult> uriFunc,
                                          Func<string, TArg, TResult> stringFunc);

    /// <inheritdoc/>
    ///
    /// <remarks>Equality is given if <paramref name="other"/> is a <see cref="ContactID"/>
    /// instance, and if the content of <paramref name="other"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public abstract bool Equals([NotNullWhen(true)] ContactID? other);

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="obj"/> is a <see cref="ContactID"/>
    /// instance, and if the content of <paramref name="obj"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as ContactID);

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() // Must be overridden in derived classes.
        => throw new NotImplementedException();

    /// <summary>
    /// Overloads the equality operator for <see cref="ContactID"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(ContactID? left, ContactID? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="ContactID"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(ContactID? left, ContactID? right)
        => !(left == right);
}
