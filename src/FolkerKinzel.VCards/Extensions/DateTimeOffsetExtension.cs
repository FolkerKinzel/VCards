using FolkerKinzel.VCards.Intls.Converters;

namespace FolkerKinzel.VCards.Extensions;

/// <summary>Extension methods for the <see cref="DateTimeOffset"
/// /> struct.</summary>
public static class DateTimeOffsetExtension
{
    /// <summary>
    /// Indicates whether <paramref name="value"/> contains relevant
    /// information in its the <see cref="DateTimeOffset.Year"/> property.
    /// </summary>
    /// <param name="value">The <see cref="DateTimeOffset"/> value to 
    /// examine.</param>
    /// <returns><c>true</c> if <paramref name="value"/> has a
    /// relevant <see cref="DateTimeOffset.Year"/> property, otherwise
    /// <c>false</c>.</returns>
    /// <remarks>
    /// The library sets the <see cref="DateTimeOffset.Year"/> property to an extremely low
    /// value to indicate that it's not relevant. The method returns <c>false</c> if the value 
    /// of the <see cref="DateTimeOffset.Year"/>
    /// property is smaller than 5, otherwise it returns <c>true</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasYear(this DateTimeOffset value) => DateAndOrTimeConverter.HasYear(value);

    /// <summary>
    /// Indicates whether the date part of <paramref name="value"/> contains relevant
    /// information (<see cref="DateTimeOffset.Year"/>, <see cref="DateTimeOffset.Month"/>, 
    /// <see cref="DateTimeOffset.Day"/>)
    /// </summary>
    /// <param name="value">The <see cref="DateTimeOffset"/> value to 
    /// examine.</param>
    /// <returns><c>true</c> if <paramref name="value"/> has a
    /// relevant date part, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// The library sets the <see cref="DateTimeOffset.Date"/> property to an extremely low
    /// value to indicate that it's not relevant. The method returns <c>false</c> if the value 
    /// of the <see cref="DateTimeOffset.Year"/>
    /// property is smaller than 4, otherwise it returns <c>true</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasDate(this DateTimeOffset value) => DateAndOrTimeConverter.HasDate(value);
}
