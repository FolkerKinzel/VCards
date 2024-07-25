using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class NameConverter
{

    internal static string? ToDisplayName(Name name)
    {
        Debug.Assert(name != null);

        const int stringBuilderInitialCapacity = 32;

        return name.IsEmpty
            ? null
            : new StringBuilder(stringBuilderInitialCapacity)
            .AppendReadableProperty(name.Prefixes)
            .AppendReadableProperty(name.GivenNames)
            .AppendReadableProperty(name.AdditionalNames)
            .AppendReadableProperty(name.FamilyNames)
            .AppendReadableProperty(name.Surname2)
            .AppendReadableProperty(name.Suffixes)
            .AppendReadableProperty(name.Generation)
            .ToString();
    }
}