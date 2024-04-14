using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods that make working with the <see cref="PCl" />
/// enum easier.</summary>
public static class PClExtension
{
    /// <summary>Sets all the flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value to which all the flags set in <paramref name="flags"
    /// /> are added.</param>
    /// <param name="flags">A single <see cref="PCl" /> value or a combination
    /// of several <see cref="PCl" /> values.</param>
    /// <returns>A value, which has all flags set that are set in <paramref name="value"
    /// /> and <paramref name="flags" />. If <paramref name="value" /> is <c>null</c>,
    /// <paramref name="flags" /> is returned.</returns>
    /// <example>
    /// <code language="cs" source="..\Examples\ExtensionMethodExample.cs"/>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static PCl? Set(
        this PCl? value, PCl flags)
        => value.HasValue ? (value.Value | flags) : flags;
    // Check inlining:
    // Trace.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

    /// <summary>Checks whether all flags set in <paramref name="flags" /> are also
    /// set in <paramref name="value" />.</summary>
    /// <param name="value">The value, which is checked to see whether all flags set
    /// in <paramref name="flags" /> are set on it.</param>
    /// <param name="flags">A single <see cref="PCl" /> value or a combination
    /// of several <see cref="PCl" /> values.</param>
    /// <returns>Returns <c>true</c>, if all flags set in <paramref name="flags" />
    /// are also set in <paramref name="value" />. If <paramref name="value" /> is <c>null</c>,
    /// <c>false</c> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSet(this PCl? value, PCl flags) 
        => (value & flags) == flags;

    /// <summary>Unsets all flags set in <paramref name="flags" /> in <paramref name="value"
    /// />.</summary>
    /// <param name="value">The value from which the flags set in <paramref name="flags"
    /// /> are removed.</param>
    /// <param name="flags">A single <see cref="PCl" /> value or a combination
    /// of several <see cref="PCl" /> values.</param>
    /// <returns>A value from which all the flags set in <paramref name="flags" /> are
    /// removed, respectively <c>null</c> if all flags are unset. If <paramref name="value"
    /// /> is <c>null</c>, <c>null</c> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PCl? Unset(this PCl? value, PCl flags)
    {
        value &= ~flags;
        return value == (PCl)0 ? null : value;
    }
}
