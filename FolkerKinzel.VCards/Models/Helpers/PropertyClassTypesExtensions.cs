using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit der <see cref="PropertyClassTypes"/>-Enum erleichtern.
    /// </summary>
    public static class PropertyClassTypesExtensions
    {
        /// <summary>
        /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="PropertyClassTypes"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="PropertyClassTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="PropertyClassTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="PropertyClassTypes"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
        /// gesetzten Flags gesetzt sind. Wenn <paramref name="value"/> null ist, wird <paramref name="flags"/> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [return: NotNull]
        public static PropertyClassTypes? Set(this PropertyClassTypes? value, PropertyClassTypes flags)
        {
            // Check inlining:
            // Trace.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            return value.HasValue ? (value.Value | flags) : flags;
        }

        /// <summary>
        /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind.
        /// </summary>
        /// <param name="value">Der <see cref="PropertyClassTypes"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
        /// Flags auf ihm gesetzt sind.</param>
        /// <param name="flags">Ein einzelner <see cref="PropertyClassTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="PropertyClassTypes"/>-Werten.</param>
        /// <returns>True, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind. Wenn <paramref name="value"/> null ist, wird false zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this PropertyClassTypes? value, PropertyClassTypes flags)
        {
            return (value & flags) == flags;
        }


        /// <summary>
        /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="PropertyClassTypes"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="PropertyClassTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="PropertyClassTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="PropertyClassTypes"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
        /// gesetzten Flags entfernt sind oder null, wenn sämtliche Flags entfernt wurden. Wenn <paramref name="value"/>
        /// null ist, wird null zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static PropertyClassTypes? Unset(this PropertyClassTypes? value, PropertyClassTypes flags)
        {
            value &= ~flags;
            return value == (PropertyClassTypes)0 ? null : value;
        }

    }
}
