using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal class DefaultAddressFormatter : IAddressFormatter
{
    public string? ToLabel(AddressProperty addressProperty)
    {
        _ArgumentNullException.ThrowIfNull(addressProperty, nameof(addressProperty));
        return AddressLabelFormatter.ToLabel(addressProperty);
    }
}
