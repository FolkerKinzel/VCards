using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates the data that describes a person or organization 
/// with whom a relationship exists.
/// This can be either a 
/// <see cref="VCards.VCard"/>, or a <see cref="ContactID"/>.
/// </summary>
/// <seealso cref="RelationProperty"/>
/// <seealso cref="VCards.VCard"/>
/// <seealso cref="ContactID"/>
public sealed class Relation
{
    private Relation(object obj) => Object = obj;

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
    /// vCard&#160;2.1 and vCard&#160;3.0 can embed nested vCards if the flag <see cref="Rel.Agent"/> is 
    /// set in their <see cref="ParameterSection.RelationType"/> property. When serializing a vCard&#160;4.0, 
    /// embedded <see cref="VCard"/>s will be automatically replaced by their <see cref="VCard.ContactID"/>
    /// references and appended as separate vCards to the VCF file.
    /// </remarks>
    /// 
    /// <exception cref="ArgumentNullException"> <paramref name="vCard"/> is <c>null</c>.</exception>
    public static Relation Create(VCard vCard)
    {
        _ArgumentNullException.ThrowIfNull(vCard, nameof(vCard));

        return new Relation(vCard);
    }

    /// <summary>
    /// Creates a new <see cref="Relation"/> instance, which is newly initialized using a 
    /// <see cref="ContactID"/> instance.
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
    public VCard? VCard => Object as VCard;

    /// <summary>
    /// Gets the encapsulated <see cref="Models.ContactID"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Models.ContactID"/> references another <see cref="VCard"/> with
    /// its <see cref="VCard.ContactID"/> property.
    /// </remarks>
    public ContactID? ContactID => Object as ContactID;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Object { get; }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="vCardAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="VCards.VCard"/>, or <c>null</c>.</param>
    /// <param name="contactIDAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="ContactID"/>, or <c>null</c>.</param>
    ///
    public void Switch(Action<VCard>? vCardAction,
                       Action<ContactID>? contactIDAction)
    {
        if (Object is VCard vc)
        {
            vCardAction?.Invoke(vc);
        }
        else
        {
            contactIDAction?.Invoke((ContactID)Object);
        }
    }

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
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    public TResult Convert<TResult>(Func<VCard, TResult> vCardFunc,
                                    Func<ContactID, TResult> contactIDFunc)
        => Object is VCard vCard
            ? vCardFunc is null ? throw new ArgumentNullException(nameof(vCardFunc))
                                : vCardFunc(vCard)
            : contactIDFunc is null ? throw new ArgumentNullException(nameof(contactIDFunc))
                                    : contactIDFunc((ContactID)Object);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Object is VCard vc
                                            ? $"{{ {nameof(VCard)}: {vc.DisplayNames.FirstOrNull()?.Value} }}"
                                            : $"{Object.GetType().FullName}: {Object}";
}

