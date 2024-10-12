using FolkerKinzel.VCards.Intls.Formatters;

namespace FolkerKinzel.VCards;

/// <summary>
/// Static class that provides <see cref="INameFormatter"/> implementations.
/// </summary>
public static partial class NameFormatter
{
    /// <summary>
    /// The default <see cref="INameFormatter"/> implementation.
    /// </summary>
    public static INameFormatter Default { get; } = new DefaultNameFormatter();
}
