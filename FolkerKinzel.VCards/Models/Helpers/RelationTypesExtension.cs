using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FolkerKinzel.VCards.Models.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden, die die Arbeit mit der <see cref="RelationTypes"/>-Enum erleichtern.
    /// </summary>
    public static class RelationTypesExtension
    {
        /// <summary>
        /// Setzt sämtliche in <paramref name="flags"/> gesetzten Flags in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="RelationTypes"/>?-Wert, auf dem die in <paramref name="flags"/> gesetzten Flags gesetzt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="RelationTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="RelationTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="RelationTypes"/>-Wert, auf dem sämtliche in <paramref name="value"/> und <paramref name="flags"/>
        /// gesetzten Flags gesetzt sind. Wenn <paramref name="value"/> <c>null</c> ist, wird <paramref name="flags"/> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [return: NotNull]
        public static RelationTypes? Set(this RelationTypes? value, RelationTypes flags) => value.HasValue ? (value.Value | flags) : flags;


        /// <summary>
        /// Untersucht, ob sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind.
        /// </summary>
        /// <param name="value">Der <see cref="RelationTypes"/>?-Wert, der daraufhin überprüft wird, ob sämtliche in <paramref name="flags"/> gesetzten 
        /// Flags auf ihm gesetzt sind.</param>
        /// <param name="flags">Ein einzelner <see cref="RelationTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="RelationTypes"/>-Werten.</param>
        /// <returns>True, wenn sämtliche in <paramref name="flags"/> gesetzten Flags auch in <paramref name="value"/>
        /// gesetzt sind. Wenn <paramref name="value"/> <c>null</c> ist, wird false zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this RelationTypes? value, RelationTypes flags) => (value & flags) == flags;


        /// <summary>
        /// Entfernt sämtliche in <paramref name="flags"/> gesetzten Flags aus <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Der <see cref="RelationTypes"/>?-Wert, aus dem die in <paramref name="flags"/> gesetzten Flags entfernt werden.</param>
        /// <param name="flags">Ein einzelner <see cref="RelationTypes"/>-Wert oder eine Kombination aus mehreren 
        /// <see cref="RelationTypes"/>-Werten.</param>
        /// <returns>Ein <see cref="RelationTypes"/>-Wert, auf dem sämtliche in <paramref name="flags"/>
        /// gesetzten Flags entfernt sind oder <c>null</c>, wenn sämtliche Flags entfernt wurden. Wenn <paramref name="value"/>
        /// <c>null</c> ist, wird <c>null</c> zurückgegeben.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static RelationTypes? Unset(this RelationTypes? value, RelationTypes flags)
        {
            value &= ~flags;
            return value == (RelationTypes)0 ? null : value;
        }

    }
}
