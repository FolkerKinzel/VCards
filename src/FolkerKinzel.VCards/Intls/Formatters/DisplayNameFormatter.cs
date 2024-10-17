using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

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
        .Concat(name.GivenNames)
        .Concat(name.AdditionalNames)
        .Concat(name.FamilyNames)
        .Concat(name.Surnames2)
        .Concat(name.Suffixes)
        .Concat(name.Generations));
    }
}