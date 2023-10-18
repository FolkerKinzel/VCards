using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls;


internal static class ReadOnlyCollectionString
{
    private static readonly ReadOnlyCollection<string> _singleton = new(Array.Empty<string>());
    internal static ReadOnlyCollection<string> Empty => _singleton;
}



