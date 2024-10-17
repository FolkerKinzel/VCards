namespace FolkerKinzel.VCards.Intls.Formatters;

internal enum NameOrder
{
    /// <summary>
    /// Prefixes + GivenNames + AdditionalNames + Surnames2 + FamilyNames + Generations + Suffixes
    /// </summary>
    Default,

    /// <summary>
    /// FamilyNames + GivenNames
    /// </summary>
    Hungarian,

    /// <summary>
    /// GivenNames + FamilyNames + Surnames2
    /// </summary>
    Spanish,

    /// <summary>
    /// FamilyNames + AdditionalNames + GivenNames
    /// </summary>
    Vietnamese
}