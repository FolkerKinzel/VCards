using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Formatters;

/// <summary>
/// Interface implemented by <see cref="VCardProperty"/> instances containing a compound value.
/// </summary>
/// <seealso cref="AddressProperty"/>
/// <seealso cref="NameProperty"/>
public interface ICompoundProperty
{
    /// <summary>Gets the data of the parameter section of a vCard property.</summary>
    ParameterSection Parameters { get; }

    /// <summary>
    /// Gets the element at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is less than zero, or equal or greater than
    /// <see cref="IReadOnlyCollection{T}.Count"/></exception>
    IReadOnlyList<string> this[int index] { get; }

    /// <summary>
    /// Gets the number of elements in the <see cref="ICompoundProperty"/>.
    /// </summary>
    int Count { get; }
}


