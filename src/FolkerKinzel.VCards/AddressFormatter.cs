using FolkerKinzel.VCards.Intls.Formatters;

namespace FolkerKinzel.VCards;

/// <summary>
/// Static class that provides <see cref="IAddressFormatter"/> implementations.
/// </summary>
public static partial class AddressFormatter
{
    /// <summary>
    /// The default <see cref="IAddressFormatter"/> implementation.
    /// </summary>
    public static IAddressFormatter Default { get; } = new DefaultAddressFormatter();
}