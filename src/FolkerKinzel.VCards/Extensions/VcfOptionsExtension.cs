namespace FolkerKinzel.VCards.Extensions;

/// <summary>
/// Erweiterungsmethoden, die die Arbeit mit der <see cref="VcfOptions"/>-Enum erleichtern.
/// </summary>
public static class VcfOptionsExtension
{
    /// <summary>
    /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Der <see cref="VcfOptions"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
    /// <param name="flags">Ein einzelner <see cref="VcfOptions"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="VcfOptions"/>-Werten.</param>
    /// <returns>Ein <see cref="VcfOptions"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
    /// gesetzten Flags gesetzt sind.</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static VcfOptions Set(this VcfOptions value, VcfOptions flags) => value | flags;

    /// <summary>
    /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
    /// gesetzt sind.
    /// </summary>
    /// <param name="value">Der <see cref="VcfOptions"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
    /// Flags auf ihm gesetzt sind.</param>
    /// <param name="flags">Ein einzelner <see cref="VcfOptions"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="VcfOptions"/>-Werten.</param>
    /// <returns><c>true</c>, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
    /// gesetzt sind. (Wenn <paramref name="flags"/>&#160;<see cref="VcfOptions.None"/> ist, wird nur dann <c>true</c> zurückgegeben,
    /// wenn auch <paramref name="value"/>&#160;<see cref="VcfOptions.None"/> ist. Dasselbe gilt auch für <see cref="VcfOptions.All"/>.)</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsSet(this VcfOptions value, VcfOptions flags) => flags == VcfOptions.None ? value == flags : (value & flags) == flags;

    /// <summary>
    /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Der <see cref="VcfOptions"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
    /// <param name="flags">Ein einzelner <see cref="VcfOptions"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="VcfOptions"/>-Werten.</param>
    /// <returns>Ein <see cref="VcfOptions"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
    /// gesetzten Flags entfernt sind. (Wenn <paramref name="flags"/>&#160;<see cref="VcfOptions.All"/> ist, wird 
    /// <see cref="VcfOptions.None"/> zurückgegeben.)</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static VcfOptions Unset(this VcfOptions value, VcfOptions flags) => value & ~flags;
}
