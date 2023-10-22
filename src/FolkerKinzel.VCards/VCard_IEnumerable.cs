using System.Collections;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>, IEnumerable<VCard>
{
    /// <summary>Returns an enumerator, which iterates through the properties of the
    /// <see cref="VCard" /> object.</summary>
    /// <returns>An Enumerator for the <see cref="VCard" />.</returns>
    public IEnumerator<KeyValuePair<VCdProp, object>> GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();

    /// <summary>Explicit implementation of <see cref="IEnumerable{T}"> IEnumerable&lt;VCard&gt;</see>,
    /// which enables to pass a single <see cref="VCard" /> object to a method or property
    /// that requires a collection of <see cref="VCard" /> objects.</summary>
    /// <returns>An enumerator, which returns the <see cref="VCard" /> object itself.</returns>
    IEnumerator<VCard> IEnumerable<VCard>.GetEnumerator()
    {
        yield return this;
    }
}
