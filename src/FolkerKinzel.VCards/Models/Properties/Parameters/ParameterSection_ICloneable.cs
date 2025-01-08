namespace FolkerKinzel.VCards.Models.Properties.Parameters;

public sealed partial class ParameterSection : ICloneable
{
    /// <inheritdoc />
    public object Clone() => new ParameterSection(this);
}
