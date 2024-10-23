using System.ComponentModel;
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

        if(JSCompsFormatter.TryFormat(nameProperty, out string? formatted))
        {
            return formatted;
        }

        NameOrder order = GetNameOrder(nameProperty, vCard);
        List<string> list = [];

        switch (order)
        {
            case NameOrder.Hungarian:
                FillHungarianList(list, nameProperty.Value);
                break;
            case NameOrder.Vietnamese:
                FillVietnameseList(list, nameProperty.Value);
                break;
            case NameOrder.Spanish:
                FillSpanishList(list, nameProperty.Value);
                break;
            default:
                FillDefaultList(list, nameProperty.Value);
                break;
        }

        return string.Join(" ", list);
    }

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString() ?? "";

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

    private static void FillDefaultList(List<string> list, Name name)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.GivenNames);
        list.AddRange(name.AdditionalNames);
        list.AddRange(name.Surnames2);
        list.AddRange(name.FamilyNames);
        list.AddRange(name.Generations);
        list.AddRange(name.Suffixes);
    }

    private static void FillSpanishList(List<string> list, Name name)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.GivenNames);
        list.AddRange(name.AdditionalNames);
        list.AddRange(name.FamilyNames);
        list.AddRange(name.Surnames2);
        list.AddRange(name.Generations);
        list.AddRange(name.Suffixes);
    }

    private static void FillHungarianList(List<string> list, Name name)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Surnames2);
        list.AddRange(name.FamilyNames);
        list.AddRange(name.Generations);
        list.AddRange(name.GivenNames);
        list.AddRange(name.AdditionalNames);
        list.AddRange(name.Suffixes);
    }

    private static void FillVietnameseList(List<string> list, Name name)
    {
        list.AddRange(name.Prefixes);
        list.AddRange(name.Surnames2);
        list.AddRange(name.FamilyNames);
        list.AddRange(name.Generations);
        list.AddRange(name.AdditionalNames);
        list.AddRange(name.GivenNames);
        list.AddRange(name.Suffixes);
    }
}
