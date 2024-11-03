using FolkerKinzel.VCards.Resources;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

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

    internal static ContactID Create(Guid guid) 
        => guid == System.Guid.Empty ? Empty : new ContactID(guid);

    internal static ContactID Create(Uri uri)
    {
        Debug.Assert(uri != null);
        Debug.Assert(uri.IsAbsoluteUri);

        return new ContactID(uri);
    }

    internal static ContactID Create(String str)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(str));

        return new ContactID(str);
    }

    /// <summary>
    /// <c>true</c> if the instance doesn't reference anything, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => IsGuid && Guid.Value == System.Guid.Empty;

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
            : IsUri  ? other.IsUri && Uri.Equals(other.Uri)
            : StringComparer.Ordinal.Equals(String, other.String));
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as ContactID);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_oneOf);
    

    [MemberNotNullWhen(true, nameof(Guid))]
    private bool IsGuid => _oneOf.IsT0;

    private Guid AsGuid => _oneOf.AsT0;

    [MemberNotNullWhen(true, nameof(Uri))]
    private bool IsUri => _oneOf.IsT1;

    private Uri AsUri => _oneOf.AsT1;

    [MemberNotNullWhen(true, nameof(String))]
    private bool IsString => _oneOf.IsT2;

    private string AsString => _oneOf.AsT2;
}
