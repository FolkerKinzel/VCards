using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressToUSLabelConverter
{
    private const int BUILDER_CAPACITY = 256;
    private const int WORKER_CAPACITY = 64;

    internal static string ConvertToUSLabel(this Address address)
    {
        StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
        StringBuilder worker = new StringBuilder(WORKER_CAPACITY);

        throw new NotImplementedException();
    }
}
