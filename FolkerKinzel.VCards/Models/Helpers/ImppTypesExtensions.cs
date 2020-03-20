using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit der <see cref="ImppTypes"/>-Enum erleichtern.
    /// </summary>
    public static class ImppTypesExtensions
    {
        /// <summary>
        /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="ImppTypes"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="ImppTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="ImppTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="ImppTypes"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
        /// gesetzten Flags gesetzt sind. Wenn <paramref name="value"/> null ist, wird <paramref name="flags"/> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [return: NotNull]
        public static ImppTypes? Set(this ImppTypes? value, ImppTypes flags)
        {
            return value.HasValue ? (value.Value | flags) : flags;
        }

        /// <summary>
        /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind.
        /// </summary>
        /// <param name="value">Der <see cref="ImppTypes"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
        /// Flags auf ihm gesetzt sind.</param>
        /// <param name="flags">Ein einzelner <see cref="ImppTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="ImppTypes"/>-Werten.</param>
        /// <returns>True, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind. Wenn <paramref name="value"/> null ist, wird false zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this ImppTypes? value, ImppTypes flags)
        {
            return (value & flags) == flags;
        }


        /// <summary>
        /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="ImppTypes"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="ImppTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="ImppTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="ImppTypes"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
        /// gesetzten Flags entfernt sind oder null, wenn sämtliche Flags entfernt wurden. Wenn <paramref name="value"/>
        /// null ist, wird null zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImppTypes? Unset(this ImppTypes? value, ImppTypes flags)
        {
            value &= ~flags;
            return value == (ImppTypes)0 ? null : value;
        }
    }
}
