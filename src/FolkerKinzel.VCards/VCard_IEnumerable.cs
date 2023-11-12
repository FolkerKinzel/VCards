using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEnumerable<KeyValuePair<Prop, object>>, IEnumerable<VCard>
{
    /// <summary>Returns an enumerator, which iterates through the properties of the
    /// <see cref="VCard" /> object.</summary>
    /// <returns>An Enumerator for the <see cref="VCard" />.</returns>
    public IEnumerator<KeyValuePair<Prop, object>> GetEnumerator() => ((IEnumerable<KeyValuePair<Prop, object>>)_propDic).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<Prop, object>>)_propDic).GetEnumerator();

    /// <summary>Explicit implementation of <see cref="IEnumerable{T}"> IEnumerable&lt;VCard&gt;</see>,
    /// which enables to pass a single <see cref="VCard" /> object to a method or property
    /// that requires a collection of <see cref="VCard" /> objects.</summary>
    /// <returns>An enumerator, which returns the <see cref="VCard" /> object itself.</returns>
    IEnumerator<VCard> IEnumerable<VCard>.GetEnumerator()
    {
        yield return this;
    }

    /// <summary>
    /// Gets this instance as <see cref="IEnumerable{T}"/> that allows to iterate over the stored
    /// properties.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> that allows to iterate over the stored
    /// properties.</returns>
    /// <remarks>
    /// <note type="tip">
    /// Each <see cref="KeyValuePair{TKey, TValue}.Value"/> is either a <see cref="VCardProperty"/>
    /// or an <see cref="IEnumerable{T}">IEnumerable&lt;VCardProperty?&gt;</see>.
    /// </note>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<KeyValuePair<Prop, object>> AsProperties() 
        => this as IEnumerable<KeyValuePair<Prop, object>>;

}
