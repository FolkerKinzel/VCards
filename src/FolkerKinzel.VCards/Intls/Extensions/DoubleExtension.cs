namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class DoubleExtension
{
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    internal static bool IsNormal(this double d)
    {
#if NET40 || NET461 || NETSTANDARD2_0
            long bits = BitConverter.DoubleToInt64Bits(d);

            bits &= 0x7FFFFFFFFFFFFFFF;
            return (bits < 0x7FF0000000000000) && (bits != 0) && ((bits & 0x7FF0000000000000) != 0);
#else
#pragma warning disable IDE0022 // Ausdruckskörper für Methoden verwenden
        return double.IsNormal(d);
#pragma warning restore IDE0022 // Ausdruckskörper für Methoden verwenden
#endif
    }

}
