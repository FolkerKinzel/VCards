using FolkerKinzel.VCards.Intls;
using System;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;
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
    private readonly OneOf<VCard, ContactID> _oneOf;

    private Relation(OneOf<VCard, ContactID> oneOf) => _oneOf = oneOf;

    /// <summary>
    /// <c>true</c> if the instance doesn't contain usable data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => object.ReferenceEquals(this, Empty);

    internal static Relation Empty { get; } = new Relation(ContactID.Empty);

    internal static Relation Create(VCard vCard)
    {
        _ArgumentNullException.ThrowIfNull(vCard, nameof(vCard));

        // Clone vCard in order to avoid circular references:
        return new Relation((VCard)vCard.Clone());
    }

    //internal static Relation CreateNoClone(VCard vCard)
    //{
    //    Debug.Assert(vCard != null);

    //    // Clone vCard in order to avoid circular references:
    //    return new Relation(vCard);
    //}

    internal static Relation Create(ContactID id)
    {
        _ArgumentNullException.ThrowIfNull(id, nameof(id));
        return id.IsEmpty ? Empty : new Relation(id);
    }

    /// <summary>
    /// Gets the encapsulated <see cref="VCards.VCard"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public VCard? VCard => IsVCard ? AsVCardIntl : null;

    /// <summary>
    /// Gets the encapsulated <see cref="VCards.ContactID"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="VCards.ContactID"/> references another <see cref="VCard"/> with
    /// its <see cref="VCard.ID"/> property.
    /// </remarks>
    public ContactID? ContactID => IsContactID ? AsContactIDIntl : null;

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
    /// <item>if the instance encapsulates a <see cref="ContactID"/></item>
    /// <item>if the instance contains a <see cref="VCards.VCard"/> and if this <see cref="VCards.VCard"/>
    /// contains no data that can be displayed as its name.</item>
    /// </list>
    /// <para>Encapsulated <see cref="System.Uri"/>s will be converted using <see cref="System.Uri.ToString"/>
    /// in order to get a more readable <see cref="string"/> than with <see cref="System.Uri.AbsoluteUri"/>.</para>
    /// </remarks>
    public bool TryAsString([NotNullWhen(true)] out string? str)
    {
        str = Convert
            (
            static vc => vc.DisplayNames?.PrefOrNullIntl(ignoreEmptyItems: true)?.Value ??
                         ToDisplayName(vc.NameViews?.FirstOrNullIntl(ignoreEmptyItems: true), vc) ??
                         vc.Organizations?.PrefOrNullIntl(ignoreEmptyItems: true)?.Value.OrganizationName,
            static contactID => contactID.Convert<string?>
                                (
                                guid => null,
                                uri => uri.AbsoluteUri,
                                str => str
                                )
            );
        return str is not null;

        static string? ToDisplayName(NameProperty? nameProperty, VCard vCard)
            => nameProperty is null ? null :  NameFormatter.Default.ToDisplayName(nameProperty, vCard);
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="vCardAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="VCards.VCard"/>.</param>
    /// <param name="contactIDAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="ContactID"/>.</param>
    /// 
    /// 
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<VCard> vCardAction,
                       Action<ContactID> contactIDAction)
        => _oneOf.Switch(vCardAction, contactIDAction);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="vCardFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="VCards.VCard"/>.</param>
    /// <param name="contactIDFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="ContactID"/>.</param>
    /// 
    /// 
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<VCard, TResult> vCardFunc,
                                    Func<ContactID, TResult> contactIDFunc)
        => _oneOf.Match(vCardFunc, contactIDFunc);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _oneOf.ToString();

    [MemberNotNullWhen(true, nameof(VCard))]
    internal bool IsVCard => _oneOf.IsT0;

    [MemberNotNullWhen(true, nameof(ContactID))]
    internal bool IsContactID => _oneOf.IsT1;

    private VCard AsVCardIntl => _oneOf.AsT0;

    private ContactID AsContactIDIntl => _oneOf.AsT1;

}

