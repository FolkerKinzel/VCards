using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit der <see cref="AddressTypes"/>-Enum erleichtern.
    /// </summary>
    public static class AddressTypesExtension
    {
        /// <summary>
        /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="AddressTypes"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="AddressTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="AddressTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="AddressTypes"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
        /// gesetzten Flags gesetzt sind. Wenn <paramref name="value"/> <c>null</c> ist, wird <paramref name="flags"/> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [return: NotNull]
        public static AddressTypes? Set(this AddressTypes? value, AddressTypes flags) => value.HasValue ? (value.Value | flags) : flags;


        /// <summary>
        /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind.
        /// </summary>
        /// <param name="value">Der <see cref="AddressTypes"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
        /// Flags auf ihm gesetzt sind.</param>
        /// <param name="flags">Ein einzelner <see cref="AddressTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="AddressTypes"/>-Werten.</param>
        /// <returns>True, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind. Wenn <paramref name="value"/> <c>null</c> ist, wird false zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this AddressTypes? value, AddressTypes flags) => (value & flags) == flags;


        /// <summary>
        /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="AddressTypes"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="AddressTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="AddressTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="AddressTypes"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
        /// gesetzten Flags entfernt sind oder <c>null</c>, wenn sämtliche Flags entfernt wurden. Wenn <paramref name="value"/>
        /// <c>null</c> ist, wird <c>null</c> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static AddressTypes? Unset(this AddressTypes? value, AddressTypes flags)
        {
            value &= ~flags;
            return value == (AddressTypes)0 ? null : value;
        }
    }
}
