using System.ComponentModel;
using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using DateTimeConverter = FolkerKinzel.VCards.Intls.Converters.DateTimeConverter;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Encapsulates data that describes a date and/or a time.
/// This can be a <see cref="System.DateOnly"/> value, a 
/// <see cref="System.DateTimeOffset"/> value, a <see cref="System.TimeOnly"/>
/// value, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DateAndOrTimeProperty"/>
public sealed class DateAndOrTime
{
    private DateAndOrTime(DateOnly value) => DateOnly = value;

    private DateAndOrTime(DateTimeOffset value) => DateTimeOffset = value;

    private DateAndOrTime(TimeOnly value) => TimeOnly = value;

    private DateAndOrTime(string value) => String = value;

    public static DateAndOrTime Create(DateOnly value) => new(value);

    public static DateAndOrTime Create(DateTimeOffset value) 
        => !DateTimeConverter.HasDate(value) && !DateTimeConverter.HasTime(value) 
            ? Empty 
            : new(value);

    public static DateAndOrTime Create(TimeOnly value) => new(value);

    public static DateAndOrTime Create(string? value) => string.IsNullOrWhiteSpace(value) ? Empty : new(value);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a recurring date in the
    /// Gregorian calendar.
    /// </summary>
    /// <param name="month">The month (1 bis 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month"/> -
    /// a leap year may be assumed.)</param>
    /// 
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    /// <remarks>
    /// This overload is intended to be used for recurring dates, like, e.g., birthdays, or 
    /// if the year is unknown.
    /// </remarks>
    /// 
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="month"/> is less than 1 or greater than 12.</para>
    /// <para>-or-</para>
    /// <para><paramref name="day"/> is less than 1 or greater than the number of days 
    /// that <paramref name="month"/> has in a leap year.</para>
    /// </exception>
    public static DateAndOrTime Create(int month, int day)
        => new(new DateOnly(DateTimeConverter.FIRST_LEAP_YEAR, month, day));

    public static implicit operator DateAndOrTime(DateOnly value) => Create(value);

    public static implicit operator DateAndOrTime(DateTimeOffset value) => Create(value);

    public static implicit operator DateAndOrTime(TimeOnly value) => Create(value);

    public static implicit operator DateAndOrTime(string? value) => Create(value);

    public static DateAndOrTime Empty { get; } = new DateAndOrTime("");

    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// Gets the encapsulated <see cref="System.DateOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// If the <see cref="DateOnly.Year"/> is less than 5, it should be treated a
    /// irrelevant. Use the extension method <see cref="DateOnlyExtension.HasYear"/>
    /// to check this.
    /// </remarks>
    public DateOnly? DateOnly { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="System.DateTimeOffset"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="System.DateTimeOffset"/> value may contain either a 
    /// <see cref="Data.DateTime"/> or a <see cref="Data.Time"/> with a 
    /// <see cref="Data.UtcOffset"/>.
    /// </para>
    /// <para>
    /// Parts of the contained data may be irrelevant. Use the extension methods
    /// <see cref="DateTimeOffsetExtension.HasYear(System.DateTimeOffset)"/> and
    /// <see cref="DateTimeOffsetExtension.HasDate(System.DateTimeOffset)"/> to
    /// check this.
    /// </para>
    /// </remarks>
    public DateTimeOffset? DateTimeOffset { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="System.TimeOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public TimeOnly? TimeOnly { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String { get; }


    [Obsolete("Use Object instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public object Value => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Tries to convert the encapsulated data to a <see cref="System.DateOnly"/> value.
    /// </summary>
    /// <param name="value">When the method
    /// returns <c>true</c>, contains a <see cref="System.DateOnly"/> value that
    /// represents the data that is encapsulated in the instance. The
    /// parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the conversion was successful, otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The conversion fails in any case if the encapsulated value is a
    /// <see cref="System.TimeOnly"/>.
    /// </para>
    /// <para>
    /// The <see cref="DateOnly.Year"/> part of <paramref name="value"/> may
    /// be irrelevant. Check this with the extension method <see cref="DateOnlyExtension.HasYear"/>.
    /// </para>
    /// </remarks>
    /// 
    public bool TryAsDateOnly(out DateOnly value)
    {
        (DateOnly parsed, bool result) = Convert<(DateOnly Value, bool Result)>
         (
          static dateOnly => (dateOnly, true),
          static dtOffset => dtOffset.HasDate()
                        ? (new DateOnly(dtOffset.Year, dtOffset.Month, dtOffset.Day), true)
                        : (default, false),
          static timeOnly => (default, false),
          static str => System.DateOnly.TryParse(str,
                                          CultureInfo.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces,
                                          out DateOnly dOnly) ? (dOnly, true)
                                                              : (default, false)
         );

        value = parsed;
        return result;
    }

    /// <summary>
    /// Tries to convert the encapsulated data to a <see cref="System.DateTimeOffset"/> value.
    /// </summary>
    /// <param name="value">When the method
    /// returns <c>true</c>, contains a <see cref="System.DateTimeOffset"/> value that
    /// represents the data that is encapsulated in the instance. The
    /// parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the conversion was successful, otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If the encapsulated value is a <see cref="System.DateOnly"/>, 12 hours
    /// are added to the resulting <see cref="System.DateTimeOffset"/> in order
    /// to avoid wrapping the date.
    /// </para>
    /// <para>Parts of the information stored in <paramref name="value"/> may be
    /// irrelevant. Check this with the extension methods 
    /// <see cref="DateTimeOffsetExtension.HasYear(System.DateTimeOffset)"/> and
    /// <see cref="DateTimeOffsetExtension.HasDate(System.DateTimeOffset)"/>.</para>
    /// </remarks>
    public bool TryAsDateTimeOffset(out DateTimeOffset value)
    {
        (DateTimeOffset parsed, bool result) = Convert<(DateTimeOffset Value, bool Result)>
         (
          static dateOnly => (new DateTimeOffset(dateOnly.Year, dateOnly.Month, dateOnly.Day, 12, 0, 0, TimeSpan.Zero), true),
          static dtOffset => (dtOffset, true),
          static timeOnly => (new DateTimeOffset().AddTicks(timeOnly.Ticks), true),
          static str => System.DateTimeOffset.TryParse(str,
                                                CultureInfo.CurrentCulture,
                                                DateTimeStyles.AllowWhiteSpaces,
                                                out DateTimeOffset dto) ? (dto, true)
                                                                        : (default, false)
         );

        value = parsed;
        return result;
    }

    public bool TryAsTimeOnly(out TimeOnly value)
    {
        if (TimeOnly.HasValue)
        {
            value = TimeOnly.Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool TryAsString([NotNullWhen(true)] out string? value)
    {
        value = String;
        return value is not null;
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="dateAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateOnly"/>.</param>
    /// <param name="dtOffsetAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/>.</param>
    /// <param name="timeAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.TimeOnly"/>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<DateOnly>? dateAction,
                       Action<DateTimeOffset>? dtOffsetAction,
                       Action<TimeOnly>? timeAction,
                       Action<string>? stringAction)
    {
        if (DateOnly.HasValue)
        {
            dateAction?.Invoke(DateOnly.Value);
        }
        else if (DateTimeOffset.HasValue)
        {
            dtOffsetAction?.Invoke(DateTimeOffset.Value);
        }
        else if (TimeOnly.HasValue)
        {
            timeAction?.Invoke(TimeOnly.Value);
        }
        else
        {
            stringAction?.Invoke(String!);
        }
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="dateFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateOnly"/> value.</param>
    /// <param name="dtOffsetFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/> value.</param>
    /// <param name="timeFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.TimeOnly"/> value.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                    Func<DateTimeOffset, TResult> dtOffsetFunc,
                                    Func<TimeOnly, TResult> timeFunc,
                                    Func<string, TResult> stringFunc)
        => DateOnly.HasValue
            ? dateFunc is null ? throw new ArgumentNullException(nameof(dateFunc)) : dateFunc(DateOnly.Value)
            : DateTimeOffset.HasValue
                ? dtOffsetFunc is null ? throw new ArgumentNullException(nameof(dtOffsetFunc)) : dtOffsetFunc(DateTimeOffset.Value)
                : TimeOnly.HasValue
                            ? timeFunc is null ? throw new ArgumentNullException(nameof(timeFunc)) : timeFunc(TimeOnly.Value)
                            : stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc(String!);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        if (IsEmpty)
        {
            return "<Empty>";
        }

        object box = Convert<object>
        (
            date => date,
            dateTimeOffset => dateTimeOffset,
            time => time,
            str => str
        );

        return $"{box.GetType().FullName}: {box}";
    }
}
