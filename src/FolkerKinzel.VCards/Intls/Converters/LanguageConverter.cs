namespace FolkerKinzel.VCards.Intls.Converters;

internal static class LanguageConverter
{
    private const string EN = "en";

    internal static string ToString(ReadOnlySpan<char> language) => language.Equals(EN, StringComparison.Ordinal) ? EN : language.ToString();
}
