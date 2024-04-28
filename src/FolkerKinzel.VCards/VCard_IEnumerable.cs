using System.Collections;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEnumerable<VCard>
{
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
}
