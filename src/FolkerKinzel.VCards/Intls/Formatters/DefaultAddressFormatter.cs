using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal sealed class DefaultAddressFormatter : IAddressFormatter
{
    public string? ToLabel(AddressProperty addressProperty)
    {
        _ArgumentNullException.ThrowIfNull(addressProperty, nameof(addressProperty));
        return addressProperty.Value.IsEmpty
            ? null
            : JSCompsFormatter.TryFormat(addressProperty, out string? formatted)
                ? formatted
                : AddressLabelFormatter.ToLabel(addressProperty);
    }
}
