using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ReadOnlyCollectionConverter
{
#if NET40
        private static readonly string[] _emptyStringArray = new string[0];
#endif

    private static readonly ReadOnlyCollection<string> _emptyColl = new ReadOnlyCollection<string>(
#if NET40
            _emptyStringArray);
#else
            Array.Empty<string>());
#endif

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static ReadOnlyCollection<string> Empty() => _emptyColl;

#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(string? s)
        => string.IsNullOrWhiteSpace(s) ? _emptyColl : new ReadOnlyCollection<string>(new string[] { s });

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(IEnumerable<string?>? coll)
    {
        if (coll is null || !coll.Any(x => !string.IsNullOrWhiteSpace(x)))
        {
            return _emptyColl;
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
