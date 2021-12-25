namespace FolkerKinzel.VCards;

public sealed partial class VCard : ICloneable
{
    /// <inheritdoc/>
    public object Clone()
        => new VCard(this);
}
