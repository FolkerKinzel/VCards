using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates a globally unique identifier corresponding to the entity associated 
/// with the vCard.
/// </summary>
public sealed class ContactID : IEquatable<ContactID>
{
    private object? _object;
    private bool _boxed;

    private ContactID(Guid guid) => Guid = guid;

    private ContactID(object value)
    {
        _object = value;
        _boxed = true;
    }

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a newly
    /// created <see cref="System.Guid"/>.
    /// </summary>
    /// <returns>A newly created <see cref="ContactID"/> instance that 
    /// has been initialized with a newly created <see cref="Guid"/>.</returns>
    public static ContactID Create() => new(System.Guid.NewGuid());

    /// <summary>
    /// Creates a new <see cref="ContactID"/> instance from a specified
    /// <see cref="System.Guid"/>.
    /// </summary>
    /// <param name="guid">A <see cref="System.Guid"/>.</param>
    /// <returns>The newly created <see cref="ContactID"/> instance.</returns>
    public static ContactID Create(Guid guid)
        => guid == System.Guid.Empty ? Empty : new ContactID(guid);

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
                    : new ContactID(uri)
                : new ContactID(uri);
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
                    ? new ContactID(uri)
                    : new ContactID(text);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't identify anything, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// A singleton whose <see cref="IsEmpty"/> property returns <c>true</c>.
    /// </summary>
    public static ContactID Empty { get; } = new ContactID("");

    /// <summary>
    /// Gets the encapsulated <see cref="Guid"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public Guid? Guid { get; }

    /// <summary>
    /// Gets the encapsulated absolute <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public Uri? Uri => Object as Uri;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => Object as string;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Object
    {
        get
        {
            // Box the Guid only once, and only if
            // needed:
            if (!_boxed)
            {
                _object = Guid!.Value;
                _boxed = true;
            }

            return _object!;
        }
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="guidAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="Guid"/>, or <c>null</c>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>, or <c>null</c>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>, or <c>null</c>.</param>
    /// 
    public void Switch(Action<Guid>? guidAction = null,
                       Action<Uri>? uriAction = null,
                       Action<string>? stringAction = null)
    {
        if (Guid.HasValue)
        {
            guidAction?.Invoke(Guid.Value);
        }
        else if (Object is Uri uri)
        {
            uriAction?.Invoke(uri);
        }
        else
        {
            stringAction?.Invoke((string)Object);
        }
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value and allows to pass an argument to the delegates.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <param name="guidAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="Guid"/>, or <c>null</c>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>, or <c>null</c>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>, or <c>null</c>.</param>
    /// <param name="arg">The argument to pass to the delegates.</param>
    public void Switch<TArg>(Action<Guid, TArg>? guidAction,
                             Action<Uri, TArg>? uriAction,
                             Action<string, TArg>? stringAction,
                             TArg arg)
    {
        if (Guid.HasValue)
        {
            guidAction?.Invoke(Guid.Value, arg);
        }
        else if (Object is Uri uri)
        {
            uriAction?.Invoke(uri, arg);
        }
        else
        {
            stringAction?.Invoke((string)Object, arg);
        }
    }

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
    public TResult Convert<TResult>(Func<Guid, TResult> guidFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
        => Guid.HasValue ? guidFunc is null 
                              ? throw new ArgumentNullException(nameof(guidFunc))
                              : guidFunc(Guid.Value)
                         : Object is Uri uri 
                              ? uriFunc is null 
                                      ? throw new ArgumentNullException(nameof(uriFunc))
                                      : uriFunc(uri)
                              : stringFunc is null 
                                      ? throw new ArgumentNullException(nameof(stringFunc))
                                      : stringFunc((string)Object);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/> and allows to specify an
    /// argument for the conversion.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// 
    /// <param name="guidFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated 
    /// value is a <see cref="Guid"/>.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public TResult Convert<TArg, TResult>(Func<Guid, TArg, TResult> guidFunc,
                                          Func<Uri, TArg, TResult> uriFunc,
                                          Func<string, TArg, TResult> stringFunc,
                                          TArg arg)
        => Guid.HasValue ? guidFunc is null 
                             ? throw new ArgumentNullException(nameof(guidFunc))
                             : guidFunc(Guid.Value, arg)
                         : Object is Uri uri 
                                 ? uriFunc is null 
                                      ? throw new ArgumentNullException(nameof(uriFunc))
                                      : uriFunc(uri, arg)
                                 : stringFunc is null 
                                      ? throw new ArgumentNullException(nameof(stringFunc))
                                      : stringFunc((string)Object, arg);

    /// <inheritdoc/>
    public override string ToString() => IsEmpty ? "<Empty>" : $"{Object.GetType().FullName}: {Object}";

    /// <inheritdoc/>
    ///
    /// <remarks>Equality is given if <paramref name="other"/> is a <see cref="ContactID"/>
    /// instance, and if the content of <paramref name="other"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public bool Equals(ContactID? other)
    {
        return other is not null
          && (Guid.HasValue
                     ? other.Guid.HasValue && Guid.Value.Equals(other.Guid.Value)
                     : _object!.Equals(other._object));
    }

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="obj"/> is a <see cref="ContactID"/>
    /// instance, and if the content of <paramref name="obj"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public override bool Equals(object? obj) => Equals(obj as ContactID);

    /// <inheritdoc/>
    public override int GetHashCode() => Object.GetHashCode();

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
