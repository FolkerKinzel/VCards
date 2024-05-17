using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls;

internal static class ReadOnlyStringCollection
{
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1

    [SuppressMessage("Style", "IDE0301:Simplify collection initialization",
        Justification = "Performance: Collection initializer initializes a new List<string>")]
    private static readonly ReadOnlyCollection<string> _singleton = new(Array.Empty<string>());

    internal static ReadOnlyCollection<string> Empty => _singleton;
#else
    internal static ReadOnlyCollection<string> Empty => ReadOnlyCollection<string>.Empty;
#endif
}



