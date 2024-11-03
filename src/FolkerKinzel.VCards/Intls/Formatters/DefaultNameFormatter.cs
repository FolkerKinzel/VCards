using System.Globalization;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal sealed class DefaultNameFormatter : INameFormatter
{
    public string? ToDisplayName(NameProperty nameProperty, VCard vCard)
    {
        _ArgumentNullException.ThrowIfNull(nameProperty, nameof(nameProperty));
        _ArgumentNullException.ThrowIfNull(vCard, nameof(vCard));

        if (nameProperty.IsEmpty)
        {
            return null;
        }

        if (JSCompsFormatter.TryFormat(nameProperty, out string? formatted))
        {
            return formatted;
        }

        NameOrder order = GetNameOrder(nameProperty, vCard);

        return order switch
        {
            NameOrder.Hungarian => FormatHungarian(nameProperty.Value),
            NameOrder.Vietnamese => FormatVietnamese(nameProperty.Value),
            NameOrder.Spanish => FormatSpanish(nameProperty.Value),
            _ => FormatDefault(nameProperty.Value)
        };
    }

    private static NameOrder GetNameOrder(NameProperty nameProperty, VCard vCard)
    {
        string? language = nameProperty.Parameters.Language;

        if (language != null)
        {
            return NameOrderConverter.ParseIetfLanguageTag(language);
        }

        language = vCard.Language?.Value;

        return language != null
            ? NameOrderConverter.ParseIetfLanguageTag(language)
            : NameOrderConverter.ParseIetfLanguageTag(CultureInfo.CurrentCulture.Name);
    }

    private static string FormatDefault(Name name) => string.Join(" ",
        name.Prefixes
        .Concat(name.GivenNames)
        .Concat(name.AdditionalNames)
        .Concat(name.Surnames2)
        .Concat(name.FamilyNames)
        .Concat(name.Generations)
        .Concat(name.Suffixes));


    private static string FormatSpanish(Name name) => string.Join(" ",
        name.Prefixes
        .Concat(name.GivenNames)
        .Concat(name.AdditionalNames)
        .Concat(name.FamilyNames)
        .Concat(name.Surnames2)
        .Concat(name.Generations)
        .Concat(name.Suffixes));

    private static string FormatHungarian(Name name) => string.Join(" ",
        name.Prefixes
        .Concat(name.Surnames2)
        .Concat(name.FamilyNames)
        .Concat(name.Generations)
        .Concat(name.GivenNames)
        .Concat(name.AdditionalNames)
        .Concat(name.Suffixes));


    private static string FormatVietnamese(Name name) => string.Join(" ",
        name.Prefixes
        .Concat(name.Surnames2)
        .Concat(name.FamilyNames)
        .Concat(name.Generations)
        .Concat(name.AdditionalNames)
        .Concat(name.GivenNames)
        .Concat(name.Suffixes));
}
