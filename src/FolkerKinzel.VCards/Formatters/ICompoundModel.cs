using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Formatters;

/// <summary>
/// Interface implemented by model classes containing a compound value.
/// </summary>
/// <seealso cref="Address"/>
/// <seealso cref="Name"/>
public interface ICompoundModel
{
    /// <summary>
    /// Gets the element at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is less
    /// than zero, or equal or greater than <see cref="Count"/></exception>
    IReadOnlyList<string> this[int index] { get; }

    /// <summary>
    /// Gets the number of elements in the <see cref="ICompoundModel"/>.
    /// </summary>
    int Count { get; }
}
