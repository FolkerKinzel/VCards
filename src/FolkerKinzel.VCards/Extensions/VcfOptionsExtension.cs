using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods that make working with the <see cref="VcfOptions"
/// /> enum easier.</summary>
public static class VcfOptionsExtension
{
    /// <summary>Sets all the flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value to which all the flags set in <paramref name="flags"
    /// /> are added.</param>
    /// <param name="flags">A single <see cref="VcfOptions" /> value or a combination
    /// of several <see cref="VcfOptions" /> values.</param>
    /// <returns>A value, which has all flags set that are set in <paramref name="value"
    /// /> and <paramref name="flags" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VcfOptions Set(this VcfOptions value, VcfOptions flags) => value | flags;

    /// <summary>Checks whether all flags set in <paramref name="flags" /> are also
    /// set in <paramref name="value" />.</summary>
    /// <param name="value">The value, which is checked to see whether all flags set
    /// in <paramref name="flags" /> are set on it.</param>
    /// <param name="flags">A single <see cref="VcfOptions" /> value or a combination
    /// of several <see cref="VcfOptions" /> values.</param>
    /// <returns> 
    /// Returns <c>true</c>, if all flags set in <paramref name="flags" /> are also set in 
    /// <paramref name="value" />. If <paramref name="value" /> is null, <c>false</c> is returned. 
    /// (If flags is <see cref="VcfOptions.None" />, <c>true</c> is only returned if 
    /// <paramref name="value" /> is also <see cref="VcfOptions.None" />. The same applies 
    /// to <see cref="VcfOptions.All" />.)
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSet(this VcfOptions value, VcfOptions flags)
        => flags == VcfOptions.None ? value == flags : (value & flags) == flags;

    /// <summary>Unsets all flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value from which the flags set in <paramref name="flags"
    /// /> are removed.</param>
    /// <param name="flags">A single <see cref="VcfOptions" /> value or a combination
    /// of several <see cref="VcfOptions" /> values.</param>
    /// <returns>
    /// A value from which all the flags set in <paramref name="flags" /> are removed. 
    /// If <paramref name="flags" /> is <see cref="VcfOptions.All" />, 
    /// <see cref="VcfOptions.None" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VcfOptions Unset(this VcfOptions value, VcfOptions flags) => value & ~flags;
}
