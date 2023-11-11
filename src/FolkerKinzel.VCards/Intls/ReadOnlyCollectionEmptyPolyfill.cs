using System.Collections.ObjectModel;
using System.Globalization;

namespace FolkerKinzel.VCards.Intls;

internal static class ReadOnlyStringCollection
{
    private static readonly ReadOnlyCollection<string> _singleton = new(Array.Empty<string>());
    internal static ReadOnlyCollection<string> Empty => _singleton;
}



