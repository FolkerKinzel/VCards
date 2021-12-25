using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard : IEquatable<VCard?>
{
    /// <summary>
    /// Bestimmt, ob zwei <see cref="VCard"/>-Referenzen als gleich betrachtet werden.
    /// </summary>
    /// <param name="other">Die zu vergleichende <see cref="VCard"/>-Referenz oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> auf eine identische vCard
    /// verweist.</returns>
    /// <remarks>
    /// <paramref name="other"/> wird als identisch betrachtet, wenn
    /// <list type="bullet">
    /// <item><c>this</c> und <paramref name="other"/> auf dasselbe <see cref="VCard"/>-Objekt
    /// verweisen</item>
    /// </list>
    /// <para>- oder -</para>
    /// <list type="number">
    /// <item>wenn sowohl <c>this</c> als auch <paramref name="other"/> eine gesetzte <see cref="UniqueIdentifier"/>-
    /// Eigenschaft haben und wenn die Werte dieser Eigenschaft übereinstimmen, und
    /// </item>
    /// <item>wenn außerdem <c>this</c> als auch <paramref name="other"/> eine gesetzte <see cref="TimeStamp"/>-
    /// Eigenschaft haben und wenn die Werte dieser Eigenschaft übereinstimmen. (Zum Vergleich wird
    /// <see cref="DateTimeOffset.EqualsExact(DateTimeOffset)"/> verwendet.)
    /// 
    /// </item>
    /// </list>
    /// </remarks>
    public bool Equals(VCard? other)
        => object.ReferenceEquals(this, other) ||
            (other is not null &&
            this.UniqueIdentifier is UuidProperty thisUuidProp &&
            other.UniqueIdentifier is UuidProperty otherUuidProp &&
            !thisUuidProp.IsEmpty &&
            thisUuidProp.Value == otherUuidProp.Value &&
            this.TimeStamp is TimeStampProperty thisTimeStampProp &&
            other.TimeStamp is TimeStampProperty otherTimeStampProp &&
            !thisTimeStampProp.IsEmpty &&
            thisTimeStampProp.Value.EqualsExact(otherTimeStampProp.Value));


    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is VCard vcard && Equals(vcard);

    /// <inheritdoc/>
    public override int GetHashCode()
        => this.UniqueIdentifier is UuidProperty uuidProp &&
            !uuidProp.IsEmpty &&
            this.TimeStamp is TimeStampProperty timeStampProp &&
            !timeStampProp.IsEmpty
                ? -1 ^ uuidProp.Value.GetHashCode() ^ timeStampProp.Value.GetHashCode()
                : base.GetHashCode();


}
