namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection : ICloneable
{
    /// <inheritdoc/>
    public object Clone() => new ParameterSection(this);
}
