using FolkerKinzel.VCards.Intls;

/* Unmerged change from project 'FolkerKinzel.VCards (netstandard2.1)'
Before:
using FolkerKinzel.VCards.Resources;
After:
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
*/
using FolkerKinzel.VCards.Resources;
using OneOf;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates a globally unique identifier corresponding to the entity associated 
/// with the vCard.
/// </summary>
public sealed class ContactID : IEquatable<ContactID>
{
    private readonly OneOf<Guid, Uri, string> _oneOf;

    private ContactID(OneOf<Guid, Uri, string> oneOf)
        => _oneOf = oneOf;

    internal static ContactID Create() => new(System.Guid.NewGuid());

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
    /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is not an absolute <see cref="Uri"/>.</exception>
    public static ContactID Create(Uri uri)
    {
        _ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        return uri.IsAbsoluteUri ? new ContactID(uri) 
                                 : throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)));
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
        _ArgumentNullException.ThrowIfNull(text, nameof(text));

        return string.IsNullOrWhiteSpace(text)
            ? throw new ArgumentException(string.Format(Res.Whitespace, nameof(text)))
            : new ContactID(text);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't reference anything, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    internal static ContactID Empty { get; } = new ContactID(System.Guid.Empty);

    /// <summary>
    /// Gets the encapsulated <see cref="Guid"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public Guid? Guid => IsGuid ? AsGuid : null;

    /// <summary>
    /// Gets the encapsulated <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public Uri? Uri => IsUri ? AsUri : null;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => IsString ? AsString : null;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Value => this._oneOf.Value;

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="guidAction">The <see cref="Action{T}"/> to perform if the encapsulated value
    /// is a <see cref="Guid"/>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<Guid> guidAction,
                       Action<Uri> uriAction,
                       Action<string> stringAction)
        => _oneOf.Switch(guidAction, uriAction, stringAction);

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
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<Guid, TResult>? guidFunc,
                                    Func<Uri, TResult> uriFunc,
                                    Func<string, TResult> stringFunc)
        => _oneOf.Match(guidFunc, uriFunc, stringFunc);

    /// <inheritdoc/>
    public override string ToString() => _oneOf.ToString();

    /// <inheritdoc/>
    public bool Equals(ContactID? other)
    {
        return other is not null
          && (IsGuid ? other.IsGuid && Guid.Equals(other.Guid)
            : IsUri ? other.IsUri && Uri.Equals(other.Uri)
            : StringComparer.Ordinal.Equals(String, other.String));
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as ContactID);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_oneOf);

    /// <summary>
    /// Overloads the equality operator for <see cref="ContactID"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(ContactID? left, ContactID? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="ContactID"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="ContactID"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the <see cref="Value"/> of <paramref name="left"/> and 
    /// <paramref name="right"/> is not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(ContactID? left, ContactID? right)
        => !(left == right);

    [MemberNotNullWhen(true, nameof(Guid))]
    internal bool IsGuid => _oneOf.IsT0;

    private Guid AsGuid => _oneOf.AsT0;

    [MemberNotNullWhen(true, nameof(Uri))]
    internal bool IsUri => _oneOf.IsT1;

    private Uri AsUri => _oneOf.AsT1;

    [MemberNotNullWhen(true, nameof(String))]
    internal bool IsString => _oneOf.IsT2;

    private string AsString => _oneOf.AsT2;
}
