namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IntExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool ValidateID(this int id) => id > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool ValidateID(this int? id) => !id.HasValue || id.Value.ValidateID();

}
