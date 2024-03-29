using System.Globalization;

namespace FolkerKinzel.VCards.Intls.Converters;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _DateTimeOffset
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryParseExact(ReadOnlySpan<char> input,
                                       string?[]? formats,
                                       IFormatProvider? formatProvider,
                                       DateTimeStyles styles,
                                       out DateTimeOffset result)
#if NET461 || NETSTANDARD2_0
        => DateTimeOffset.TryParseExact(input.ToString(), formats, formatProvider, styles, out result);
#else
        => DateTimeOffset.TryParseExact(input, formats, formatProvider, styles, out result);
#endif
}


[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _Guid
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryParse(ReadOnlySpan<char> input, out Guid result)
#if NET461 || NETSTANDARD2_0
        => Guid.TryParse(input.ToString(), out result);
#else
        => Guid.TryParse(input, out result);
#endif
}


[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _Double
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
#if NET461 || NETSTANDARD2_0
        => double.Parse(s.ToString(), style, provider);
#else
        => double.Parse(s, style, provider);
#endif
}


[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
internal static class _TimeSpan
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryParseExact(ReadOnlySpan<char> input,
                                       string? format,
                                       IFormatProvider? formatProvider,
                                       TimeSpanStyles styles,
                                       out TimeSpan result)
#if NET461 || NETSTANDARD2_0
        => TimeSpan.TryParseExact(input.ToString(), format, formatProvider, styles, out result);
#else
        => TimeSpan.TryParseExact(input, format, formatProvider, styles, out result);
#endif
}




