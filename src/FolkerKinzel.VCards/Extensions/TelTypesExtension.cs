using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>
/// Erweiterungsmethoden, die die Arbeit mit der <see cref="TelTypes"/>-Enum erleichtern.
/// </summary>
public static class TelTypesExtension
{
    /// <summary>
    /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Der <see cref="TelTypes"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
    /// <param name="flags">Ein einzelner <see cref="TelTypes"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="TelTypes"/>-Werten.</param>
    /// <returns>Ein <see cref="TelTypes"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
    /// gesetzten Flags gesetzt sind. Wenn <paramref name="value"/>&#160;<c>null</c> ist, wird <paramref name="flags"/> zurückgegeben.</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [return: NotNull]
    public static TelTypes? Set(this TelTypes? value, TelTypes flags) => value.HasValue ? (value.Value | flags) : flags;


    /// <summary>
    /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
    /// gesetzt sind.
    /// </summary>
    /// <param name="value">Der <see cref="TelTypes"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
    /// Flags auf ihm gesetzt sind.</param>
    /// <param name="flags">Ein einzelner <see cref="TelTypes"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="TelTypes"/>-Werten.</param>
    /// <returns><c>true</c>, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
    /// gesetzt sind. Wenn <paramref name="value"/>&#160;<c>null</c> ist, wird <c>false</c> zurückgegeben.</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsSet(this TelTypes? value, TelTypes flags) => (value & flags) == flags;


    /// <summary>
    /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Der <see cref="TelTypes"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
    /// <param name="flags">Ein einzelner <see cref="TelTypes"/>-Wert oder eine Kombination aus mehreren 
    /// <see cref="TelTypes"/>-Werten.</param>
    /// <returns>Ein <see cref="TelTypes"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
    /// gesetzten Flags entfernt sind oder <c>null</c>, wenn sämtliche Flags entfernt wurden. Wenn 
    /// <paramref name="value"/>&#160;<c>null</c> ist, wird <c>null</c> zurückgegeben.</returns>
#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static TelTypes? Unset(this TelTypes? value, TelTypes flags)
    {
        value &= ~flags;
        return value == (TelTypes)0 ? null : value;
    }
}
