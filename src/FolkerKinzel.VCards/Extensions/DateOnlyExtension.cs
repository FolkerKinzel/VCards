using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods for the <see cref="DateOnly"
/// /> struct.</summary>
public static class DateOnlyExtension
{
    /// <summary>
    /// Indicates whether <paramref name="value"/> contains relevant
    /// information in its the <see cref="DateOnly.Year"/> property.
    /// </summary>
    /// <param name="value">The <see cref="DateOnly"/> value to 
    /// examine.</param>
    /// <returns><c>true</c> if <paramref name="value"/> has a
    /// relevant <see cref="DateOnly.Year"/> property, otherwise
    /// <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasYear(this DateOnly value)
        => value.Year > DateAndOrTimeConverter.FIRST_LEAP_YEAR;
}
