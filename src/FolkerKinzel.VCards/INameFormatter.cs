using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

/// <summary>
/// Interface that allows implementing classes to convert <see cref="NameProperty"/> instances
/// to corresponding <see cref="string"/>s, which can by displayed to users.
/// </summary>
public interface INameFormatter
{
    /// <summary>
    /// Converts the content of a <see cref="NameProperty"/> instance to a formatted <see cref="string"/>.
    /// </summary>
    /// <param name="nameProperty">The <see cref="NameProperty"/> instance to convert.</param>
    /// <param name="vCard">The <see cref="VCard"/> instance <paramref name="nameProperty"/> belongs to.</param>
    /// <returns>A <see cref="string"/> that represents the content of <paramref name="nameProperty"/>, or <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="nameProperty"/> or <paramref name="vCard"/> is <c>null</c>.
    /// </exception>
    string? ToDisplayName(NameProperty nameProperty, VCard vCard);
}


