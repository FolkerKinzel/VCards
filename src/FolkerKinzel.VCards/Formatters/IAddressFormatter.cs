using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Formatters;

/// <summary>
/// Interface that allows implementing classes to convert <see cref="AddressProperty"/> instances
/// to <see cref="string"/>s, which represent corresponding address labels.
/// </summary>
public interface IAddressFormatter
{
    /// <summary>
    /// Converts an <see cref="AddressProperty"/> instance to an address label.
    /// </summary>
    /// <param name="addressProperty">The <see cref="AddressProperty"/> to convert.</param>
    /// <returns>A <see cref="string"/> that represents an address label, or <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="addressProperty"/> is <c>null</c>.</exception>
    string? ToLabel(AddressProperty addressProperty);
}


