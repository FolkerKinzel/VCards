using System.Globalization;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

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
        List<string> list = [];

        switch (order)
        {
            case NameOrder.Hungarian:
                FillHungarian(nameProperty.Value, list);
                break;
            case NameOrder.Vietnamese:
                FillVietnamese(nameProperty.Value, list);
                break;
            case NameOrder.Spanish:
                FillSpanish(nameProperty.Value, list);
                break;
            default:
                FillDefault(nameProperty.Value, list);
                break;
        }

        return string.Join(" ", list);
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

    private static void FillDefault(Name name, List<string> list)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Given);
        list.AddRange(name.Given2);
        list.AddRange(name.Surnames2);
        list.AddRange(name.Surnames);
        list.AddRange(name.Generations);
        list.AddRange(name.Suffixes);
    }

    private static void FillSpanish(Name name, List<string> list)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Given);
        list.AddRange(name.Given2);
        list.AddRange(name.Surnames);
        list.AddRange(name.Surnames2);
        list.AddRange(name.Generations);
        list.AddRange(name.Suffixes);
    }

    private static void FillHungarian(Name name, List<string> list)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Surnames2);
        list.AddRange(name.Surnames);
        list.AddRange(name.Generations);
        list.AddRange(name.Given);
        list.AddRange(name.Given2);
        list.AddRange(name.Suffixes);
    }

    private static void FillVietnamese(Name name, List<string> list)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Surnames2);
        list.AddRange(name.Surnames);
        list.AddRange(name.Generations);
        list.AddRange(name.Given2);
        list.AddRange(name.Given);
        list.AddRange(name.Suffixes);
    }
}
