using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods that make working with the <see cref="VcfOpts"
/// /> enum easier.</summary>
public static class VcfOptsExtension
{
    /// <summary>Sets all the flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value to which all the flags set in <paramref name="flags"
    /// /> are added.</param>
    /// <param name="flags">A single <see cref="VcfOpts" /> value or a combination
    /// of several <see cref="VcfOpts" /> values.</param>
    /// <returns>A value, which has all flags set that are set in <paramref name="value"
    /// /> and <paramref name="flags" />.</returns>
    /// <example>
    /// <code language="cs" source="..\Examples\VCardExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VcfOpts Set(this VcfOpts value, VcfOpts flags) => value | flags;

    /// <summary>Checks whether all flags set in <paramref name="flags" /> are also
    /// set in <paramref name="value" />.</summary>
    /// <param name="value">The value, which is checked to see whether all flags set
    /// in <paramref name="flags" /> are set on it.</param>
    /// <param name="flags">A single <see cref="VcfOpts" /> value or a combination
    /// of several <see cref="VcfOpts" /> values.</param>
    /// <returns> 
    /// Returns <c>true</c>, if all flags set in <paramref name="flags" /> are also set in 
    /// <paramref name="value" />. If <paramref name="value" /> is null, <c>false</c> is returned. 
    /// (If flags is <see cref="VcfOpts.None" />, <c>true</c> is only returned if 
    /// <paramref name="value" /> is also <see cref="VcfOpts.None" />. The same applies 
    /// to <see cref="VcfOpts.All" />.)
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSet(this VcfOpts value, VcfOpts flags)
        => flags == VcfOpts.None ? value == flags : (value & flags) == flags;

    /// <summary>Unsets all flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value from which the flags set in <paramref name="flags"
    /// /> are removed.</param>
    /// <param name="flags">A single <see cref="VcfOpts" /> value or a combination
    /// of several <see cref="VcfOpts" /> values.</param>
    /// <returns>
    /// A value from which all the flags set in <paramref name="flags" /> are removed. 
    /// If <paramref name="flags" /> is <see cref="VcfOpts.All" />, 
    /// <see cref="VcfOpts.None" /> is returned.</returns>
    /// <example>
    /// <code language="cs" source="..\Examples\ExtensionMethodExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VcfOpts Unset(this VcfOpts value, VcfOpts flags) => value & ~flags;
}
