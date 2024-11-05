using FolkerKinzel.VCards.Intls;
using System;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Resources;
using OneOf;
using FolkerKinzel.VCards.Enums;

/* Unmerged change from project 'FolkerKinzel.VCards (netstandard2.0)'
Added:
using FolkerKinzel;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Models;
*/
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates the data that describes a relation with a person or
/// organization.
/// This can be a <see cref="string"/>, a 
/// <see cref="VCards.VCard"/>, a <see cref="Guid"/>,
/// or an absolute <see cref="Uri"/>.
/// </summary>
/// <seealso cref="RelationProperty"/>
public sealed class Relation
{
    private readonly OneOf<VCard, ContactID> _oneOf;

    private Relation(OneOf<VCard, ContactID> oneOf) => _oneOf = oneOf;

    /// <summary>
    /// <c>true</c> if the instance doesn't contain usable data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    internal static Relation Empty { get; } = new Relation(ContactID.Empty);

    /// <summary>
    /// Creates a new <see cref="Relation"/> instance, which is newly initialized using the 
    /// <see cref="VCard"/>-object that represents a person or organization.
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/>-object that represents a person or organization
    /// to whom there is a relationship.</param>
    /// 
    /// <returns>The newly created <see cref="Relation"/> instance.</returns>
    /// 
    /// <remarks>
    /// <note type="important">
    /// This method clones <paramref name="vCard"/> in order to avoid circular references.
    /// Changing the <paramref name="vCard"/> instance AFTER assigning it to this constructor 
    /// leads to unexpected results!
    /// </note>
    /// <para>
    /// vCard&#160;2.1 and vCard&#160;3.0 can embed nested vCards if the flag <see cref="Rel.Agent"/> is 
    /// set in their <see cref="ParameterSection.RelationType"/> property. When serializing a vCard&#160;4.0, 
    /// embedded <see cref="VCard"/>s will be automatically replaced by their <see cref="VCard.ID"/>
    /// references and appended as separate vCards to the VCF file.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\EmbeddedVCardExample.cs" />
    /// </example>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="vCard"/> is <c>null</c>.</exception>
    public static Relation Create(VCard vCard)
    {
        _ArgumentNullException.ThrowIfNull(vCard, nameof(vCard));

        // Clone vCard in order to avoid circular references:
        return new Relation((VCard)vCard.Clone());
    }

    internal static Relation CreateNoClone(VCard vCard)
    {
        Debug.Assert(vCard != null);

        // Clone vCard in order to avoid circular references:
        return new Relation(vCard);
    }

    /// <summary>
    /// Creates a new <see cref="Relation"/> instance, which is newly initialized using the 
    /// <see cref="ContactID"/>-object that identifies a person or organization.
    /// </summary>
    /// <param name="id">The <see cref="ContactID"/>-object that identifies a person or organization
    /// to whom there is a relationship.</param>
    /// 
    /// <returns>The newly created <see cref="Relation"/> instance.</returns>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="id"/> is <c>null</c>.</exception>
    internal static Relation Create(ContactID id)
    {
        _ArgumentNullException.ThrowIfNull(id, nameof(id));
        return id.IsEmpty ? Empty : new Relation(id);
    }

    /// <summary>
    /// Gets the encapsulated <see cref="VCards.VCard"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public VCard? VCard => Value as VCard;

    /// <summary>
    /// Gets the encapsulated <see cref="Models.ContactID"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Models.ContactID"/> references another <see cref="VCard"/> with
    /// its <see cref="VCard.ID"/> property.
    /// </remarks>
    public ContactID? ContactID => Value as ContactID;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Value => this._oneOf.Value;

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
    public override string ToString() => $"{Value.GetType().FullName}: {Value}";

}

