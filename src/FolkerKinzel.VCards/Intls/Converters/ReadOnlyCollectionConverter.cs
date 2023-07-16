using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ReadOnlyCollectionConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(string? s)
        => string.IsNullOrWhiteSpace(s) ? ReadOnlyCollectionString.Empty : new ReadOnlyCollection<string>(new string[] { s });


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(IEnumerable<string?>? coll)
    {
        if (coll is null || !coll.Any(x => !string.IsNullOrWhiteSpace(x)))
        {
            return ReadOnlyCollectionString.Empty;
        }

        if (coll.All(x => !string.IsNullOrWhiteSpace(x)))
        {
            if (coll is ReadOnlyCollection<string> roc)
            {
                return roc;
            }
            else if (coll is IList<string> list)
            {
                return new ReadOnlyCollection<string>(list);
            }
            else
            {
                return new ReadOnlyCollection<string>(coll.ToArray()!);
            }
        }
        else
        {
            return new ReadOnlyCollection<string>(coll.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()!);
        }
    }
}
