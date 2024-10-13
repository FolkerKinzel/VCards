using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls.Formatters;

namespace FolkerKinzel.VCards;

/// <summary>
/// Static class that provides <see cref="INameFormatter"/> implementations.
/// </summary>
/// <remarks>
/// Since this is a partial class, it can be extended with custom implementations of 
/// <see cref="INameFormatter"/>.
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
public static partial class NameFormatter
{
    /// <summary>
    /// The default <see cref="INameFormatter"/> implementation.
    /// </summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public static INameFormatter Default { get; } = new DefaultNameFormatter();
}
