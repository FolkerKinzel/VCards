using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Formatters;

[Obsolete()]
internal static class DisplayNameFormatter
{
    internal static string? ToDisplayName(Name name)
    {
        Debug.Assert(name != null);

        return name.IsEmpty
            ? null
#if NETSTANDARD2_0 || NET462
            : string.Join(" "
#else
            : string.Join(' '
#endif
        , name.Prefixes
        .Concat(name.Given)
        .Concat(name.Given2)
        .Concat(name.Surnames)
        .Concat(name.Surnames2)
        .Concat(name.Suffixes)
        .Concat(name.Generations));
    }
}