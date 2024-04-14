namespace FolkerKinzel.VCards.Intls;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _ArgumentNullException
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull([NotNull] object? argument, string? paramName)
#if NET461 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
    { if (argument is null) { throw new ArgumentNullException(paramName); } }
#else
        => ArgumentNullException.ThrowIfNull(argument, paramName);
#endif
}



