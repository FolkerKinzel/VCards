using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Encapsulates the data that describes a relation with a person or
/// organization.
/// This can be a <see cref="string"/>, a 
/// <see cref="VCards.VCard"/>, a <see cref="System.Guid"/>,
/// or an absolute <see cref="System.Uri"/>.
/// </summary>
/// <seealso cref="RelationProperty"/>
public sealed class Relation
{
    private readonly OneOf<string, VCard, Guid, Uri> _oneOf;

    internal Relation(OneOf<string, VCard, Guid, Uri> oneOf) => _oneOf = oneOf;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => IsString ? AsStringIntl : null;

    /// <summary>
    /// Gets the encapsulated <see cref="VCards.VCard"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public VCard? VCard => IsVCard ? AsVCardIntl : null;

    /// <summary>
    /// Gets the encapsulated <see cref="System.Guid"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Guid"/> references another <see cref="VCard"/> with
    /// its <see cref="VCard.ID"/> property.
    /// </remarks>
    public Guid? Guid => IsGuid ? AsGuidIntl : null;

    /// <summary>
    /// Gets the encapsulated <see cref="System.Uri"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <value>An absolute <see cref="System.Uri"/> or <c>null</c>.</value>
    public Uri? Uri => IsUri ? AsUriIntl : null;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Value => this._oneOf.Value;

    /// <summary>
    /// Tries to convert the encapsulated data to a <see cref="string"/> that can be
    /// displayed to users.
    /// </summary>
    /// <param name="str">When the method
    /// returns <c>true</c>, contains a <see cref="string"/> that
    /// represents the data that is encapsulated in the instance. The
    /// parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the conversion was successful, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    /// The method differs from the <see cref="ToString"/> method in that it tries to convert 
    /// the information contained in the encapsulated data to a human-readable <see cref="string"/>,
    /// whereas the <see cref="ToString"/> method provides meta-information about the 
    /// <see cref="Relation"/> object itself.
    /// </para>
    /// The method fails
    /// <list type="bullet">
    /// <item>if the instance encapsulates a <see cref="System.Guid"/></item>
    /// <item>if the instance contains a <see cref="VCards.VCard"/> and if this <see cref="VCards.VCard"/>
    /// contains no data that can be displayed as its name.</item>
    /// </list>
    /// <para>Encapsulated <see cref="System.Uri"/>s will be converted using <see cref="System.Uri.ToString"/>
    /// in order to get a more readable <see cref="string"/> than with <see cref="System.Uri.AbsoluteUri"/>.</para>
    /// </remarks>
    public bool TryAsString([NotNullWhen(true)] out string? str)
    {
        str = Convert(
            static s => s,
            static vc => vc.DisplayNames?.PrefOrNullIntl(ignoreEmptyItems: true)?.Value ??
                          vc.NameViews?.FirstOrNullIntl(ignoreEmptyItems: true)?.ToDisplayName() ??
                          vc.Organizations?.PrefOrNullIntl(ignoreEmptyItems: true)?.Value.OrganizationName,
            static guid => null,
            static uri => uri.ToString()
            );
        return str != null;
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <param name="vCardAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="VCards.VCard"/>.</param>
    /// <param name="guidAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Guid"/>.</param>
    /// <param name="uriAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<string> stringAction,
                       Action<VCard> vCardAction,
                       Action<Guid> guidAction,
                       Action<Uri> uriAction)
        => _oneOf.Switch(stringAction, vCardAction, guidAction, uriAction);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <param name="vCardFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="VCards.VCard"/>.</param>
    /// <param name="guidFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Guid"/>.</param>
    /// <param name="uriFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.Uri"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<string, TResult> stringFunc,
                                  Func<VCard, TResult> vCardFunc,
                                  Func<Guid, TResult> guidFunc,
                                  Func<Uri, TResult> uriFunc)
        => _oneOf.Match(stringFunc, vCardFunc, guidFunc, uriFunc);
    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _oneOf.ToString();

    [MemberNotNullWhen(true, nameof(String))]
    private bool IsString => _oneOf.IsT0;

    [MemberNotNullWhen(true, nameof(VCard))]
    private bool IsVCard => _oneOf.IsT1;

    [MemberNotNullWhen(true, nameof(Guid))]
    private bool IsGuid => _oneOf.IsT2;

    [MemberNotNullWhen(true, nameof(Uri))]
    private bool IsUri => _oneOf.IsT3;

    private string AsStringIntl => _oneOf.AsT0;

    private VCard AsVCardIntl => _oneOf.AsT1;
    
    private Guid AsGuidIntl => _oneOf.AsT2;

    private Uri AsUriIntl => _oneOf.AsT3;
}

