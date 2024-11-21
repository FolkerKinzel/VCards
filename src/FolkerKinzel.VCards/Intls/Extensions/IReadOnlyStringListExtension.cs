namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IReadOnlyStringListExtension
{
    internal static string? FirstOrNull(this IReadOnlyList<string>? values)
        => values is null || values.Count == 0 ? null : values[0];
}
