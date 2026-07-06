using System.ComponentModel;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// A union that encapsulates a globally unique identifier corresponding to the entity associated 
/// with the vCard. The value of this identifier can be either a <see cref="System.Guid"/>,
/// a <see cref="System.Uri"/>, or a <see cref="string"/>.
/// </summary>
/// <remarks>
/// This class supports semantic comparability. For example, UUIDs are always compared as 128-bit 
/// numbers - regardless of whether they are represented as a <see cref="Guid"/> instance, a <see cref="Uri"/>, 
/// or a <see cref="string"/>.
/// </remarks>
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
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is not an absolute <see cref="Uri"/>.</exception>
    public static ContactID Create(Uri uri)
    {
        _ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        if (!uri.IsAbsoluteUri)
        {
            throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)));
        }

        ContactID? comparer = uri.AbsoluteUri.StartsWith("urn:uuid:", StringComparison.OrdinalIgnoreCase)
                              && UuidConverter.TryAsGuid(uri.AbsoluteUri.AsSpan(), out Guid uuid)
                ? Create(uuid)
                : null;
        
        return new ContactIDUri(uri, comparer);
    }

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a specified
    /// <see cref="string"/>.
    /// </summary>
    /// <param name="text">A <see cref="string"/> that can be used as identifier.</param>
    /// <returns>The newly created <see cref="ContactID"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> is an empty <see cref="string"/> 
    /// or consists only of white space.</exception>
    public static ContactID Create(string text)
    {
        ContactID? comparer = UuidConverter.TryAsGuid(text.AsSpan().Trim(), out Guid uuid)
                            ? Create(uuid)
                            : Uri.TryCreate(text, UriKind.Absolute, out Uri? uri)
                                ? new ContactIDUri(uri, null)
                                : null;

        return new ContactIDString(text, comparer);
    }

    /// <summary>
    /// Gets the <see cref="ContactID"/> instance that is used for comparison.
    /// </summary>
    /// <remarks>
    /// This is not necessarily the same instance as the parent object.
    /// For example, <see cref="Comparer"/> will encapsulate a <see cref="Guid"/> 
    /// if the parent instance encapsulates a <see cref="string"/> representing a
    /// <see cref="Guid"/>, or a <see cref="Uri"/> that is a UUID-URN.
    /// </remarks>
    public ContactID Comparer { get; protected set; } = Empty;

    /// <summary>
    /// <c>true</c> if the instance doesn't identify anything, otherwise <c>false</c>.
    /// </summary>
    /// <remarks>
    /// <see cref="Empty"/> is a singleton that encapsulates an empty
    /// <see cref="string"/>.
    /// </remarks>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// A singleton whose <see cref="IsEmpty"/> property returns <c>true</c>.
    /// </summary>
    /// <remarks>
    /// The singleton instance encapsulates an empty <see cref="string"/>.
    /// </remarks>
    public static ContactID Empty { get; } = new ContactIDString();

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
    /// instance, and if the content of <paramref name="other"/> is semantically equivalent.
    /// (This does not necessarily require the content of <paramref name="other"/> to have the 
    /// same <see cref="Type"/>.)</remarks>
    public abstract bool Equals([NotNullWhen(true)] ContactID? other);

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="obj"/> is a <see cref="ContactID"/>
    /// instance, and if the content of <paramref name="obj"/> is semantically equivalent.
    /// (This does not necessarily require the content of <paramref name="obj"/> to have the 
    /// same <see cref="Type"/>.)</remarks>
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
