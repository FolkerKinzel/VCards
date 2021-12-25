using System.Collections;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>, IEnumerable<VCard>
{
    /// <summary>
    /// Gibt einen Enumerator zurück, der die Eigenschaften <see cref="VCard"/> durchläuft.
    /// </summary>
    /// <returns>Ein Enumerator für die <see cref="VCard"/>.</returns>
    public IEnumerator<KeyValuePair<VCdProp, object>> GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();


    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<VCdProp, object>>)_propDic).GetEnumerator();

    /// <summary>
    /// Explizite Implementierung von <see cref="IEnumerable{T}">IEnumerable&lt;VCard&gt;</see>, die es erlaubt,
    /// ein einzelnes <see cref="VCard"/>-Objekt an eine Methode oder Eigenschaft zu übergeben, die eine Sammlung
    /// von <see cref="VCard"/>-Objekten verlangt.
    /// </summary>
    /// <returns>Ein Enumerator, der das <see cref="VCard"/>-Objekt selbst zurückgibt.</returns>
    IEnumerator<VCard> IEnumerable<VCard>.GetEnumerator()
    {
        yield return this;
    }
}
