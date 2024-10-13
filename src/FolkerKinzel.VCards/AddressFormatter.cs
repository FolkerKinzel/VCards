using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls.Formatters;

namespace FolkerKinzel.VCards;

/// <summary>
/// Static class that provides <see cref="IAddressFormatter"/> implementations.
/// </summary>
/// <remarks>
/// Since this is a partial class, it can be extended with custom implementations of 
/// <see cref="IAddressFormatter"/>.
/// </remarks>
/// <example>
/// <code language="cs" source="..\Examples\VCardExample.cs"/>
/// </example>
public static partial class AddressFormatter
{
    /// <summary>
    /// The default <see cref="IAddressFormatter"/> implementation.
    /// </summary>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    public static IAddressFormatter Default { get; } = new DefaultAddressFormatter();
}