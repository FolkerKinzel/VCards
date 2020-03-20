using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Model.Enums;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Model.Helpers
{
    /// <summary>
    /// Erweiterungsmethoden für Enums, die mit dem <see cref="FlagsAttribute"/> ausgezeichnet sind.
    /// </summary>
    public static class FlagsEnumExtensions
    {
        
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static PropertyClassTypes? Set(this PropertyClassTypes? value, PropertyClassTypes flag)
        {
            // Check inlining:
            // Trace.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
            return value.HasValue ? (value.Value | flag) : flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this PropertyClassTypes? value, PropertyClassTypes flag)
        {
            return (value & flag) == flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static PropertyClassTypes? Unset(this PropertyClassTypes? value, PropertyClassTypes flag)
        {
            value &= ~flag;
            return value == (PropertyClassTypes)0 ? null : value;
        }


        /////////////////////

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TelTypes? Set(this TelTypes? value, TelTypes flag)
        {
            return value.HasValue ? (value.Value | flag) : flag;
        }


#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this TelTypes? value, TelTypes flag)
        {
            return (value & flag) == flag;
        }


#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TelTypes? Unset(this TelTypes? value, TelTypes flag)
        {
            value &= ~flag;
            return value == (TelTypes)0 ? null : value;
        }


        /////////////////////

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static AddressTypes? Set(this AddressTypes? value, AddressTypes flag)
        {
            return value.HasValue ? (value.Value | flag) : flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this AddressTypes? value, AddressTypes flag)
        {
            return (value & flag) == flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static AddressTypes? Unset(this AddressTypes? value, AddressTypes flag)
        {
            value &= ~flag;
            return value == (AddressTypes)0 ? null : value;
        }


        // /////////////////////////////////

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static RelationTypes? Set(this RelationTypes? value, RelationTypes flag)
        {
            return value.HasValue ? (value.Value | flag) : flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this RelationTypes? value, RelationTypes flag)
        {
            return (value & flag) == flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static RelationTypes? Unset(this RelationTypes? value, RelationTypes flag)
        {
            value &= ~flag;
            return value == (RelationTypes)0 ? null : value;
        }

        // /////////////////////////////////


#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImppTypes? Set(this ImppTypes? value, ImppTypes flag)
        {
            return value.HasValue ? (value.Value | flag) : flag;
        }


#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this ImppTypes? value, ImppTypes flag)
        {
            return (value & flag) == flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImppTypes? Unset(this ImppTypes? value, ImppTypes flag)
        {
            value &= ~flag;
            return value == (ImppTypes)0 ? null : value;
        }

        // /////////////////////////////////

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static VcfOptions Set(this VcfOptions value, VcfOptions flag)
        {
            return (value | flag);
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsSet(this VcfOptions value, VcfOptions flag)
        {
            return flag == VcfOptions.None ? value == flag : (value & flag) == flag;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static VcfOptions Unset(this VcfOptions value, VcfOptions flag)
        {
            return value & ~flag;
        }


        
    }
}
