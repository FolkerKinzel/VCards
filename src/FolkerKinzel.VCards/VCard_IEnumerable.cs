using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEnumerable<VCard>
{
    /// <summary>Merges all the data stored in the properties of the <see cref="VCard" /> 
    /// instance into a single <see cref="IEnumerable{T}"/>.</summary>
    /// <returns>A collection that contains the 
    /// data that is stored in the <see cref="VCard" /> instance.</returns>
    /// <example>
    /// <code language="cs" source="..\Examples\ExtensionMethodExample.cs"/>
    /// </example>
    public IEnumerable<KeyValuePair<Prop, VCardProperty>> AsEnumerable()
    {
        foreach (var item in _propDic)
        {
            if (item.Value is VCardProperty prop)
            {
                yield return new KeyValuePair<Prop, VCardProperty>(item.Key, prop);
            }
            else
            {
                var coll = (IEnumerable<VCardProperty?>)item.Value;

                foreach(VCardProperty? p in coll)
                {
                    if(p is not null)
                    {
                        yield return new KeyValuePair<Prop, VCardProperty>(item.Key, p);
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<VCard>)this).GetEnumerator();

    /// <summary>Explicit implementation of <see cref="IEnumerable{T}"> IEnumerable&lt;VCard&gt;</see>,
    /// which enables to pass a single <see cref="VCard" /> object to a method or property
    /// that requires a collection of <see cref="VCard" /> objects.</summary>
    /// <returns>An enumerator that returns the <see cref="VCard" /> instance itself.</returns>
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
    internal IEnumerable<KeyValuePair<Prop, object>> AsProperties()
        => this._propDic;

}
