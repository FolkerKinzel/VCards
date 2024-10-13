using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal class DefaultNameFormatter : INameFormatter
{
    public string? ToDisplayName(NameProperty nameProperty, VCard vCard)
    {
        _ArgumentNullException.ThrowIfNull(nameProperty, nameof(nameProperty));
        _ArgumentNullException.ThrowIfNull(vCard, nameof(vCard));

        return DisplayNameFormatter.ToDisplayName(nameProperty.Value);
    }
}
