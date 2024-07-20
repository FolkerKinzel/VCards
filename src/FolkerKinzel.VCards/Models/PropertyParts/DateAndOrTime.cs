using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Encapsulates data that describes a date and/or a time.
/// This can be a <see cref="System.DateOnly"/> value, a 
/// <see cref="System.DateTimeOffset"/> value, a <see cref="System.TimeOnly"/>
/// value, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DateAndOrTimeProperty"/>
public sealed partial class DateAndOrTime
{
    private readonly OneOf<DateOnly, DateTimeOffset, TimeOnly, string> _oneOf;

    internal DateAndOrTime(OneOf<DateOnly, DateTimeOffset, TimeOnly, string> oneOf)
        => _oneOf = oneOf;

    /// <summary>
    /// Gets the encapsulated <see cref="System.DateOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// If the <see cref="DateOnly.Year"/> is less than 5, it should be treated a
    /// irrelevant. Use the extension method <see cref="DateOnlyExtension.HasYear"/>
    /// to check this.
    /// </remarks>
    public DateOnly? DateOnly => IsDateOnly ? AsDateOnly : null;

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
    public DateTimeOffset? DateTimeOffset => IsDateTimeOffset ? AsDateTimeOffset : null;

    /// <summary>
    /// Gets the encapsulated <see cref="System.TimeOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public TimeOnly? TimeOnly => IsTimeOnly ? AsTimeOnly : null;

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public string? String => IsString ? AsString : null;

    /// <summary>
    /// Gets the encapsulated value.
    /// </summary>
    public object Value => this._oneOf.Value;

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
    /// The <see cref="System.DateOnly.Year"/> part of <paramref name="value"/> may
    /// be irrelevant. Check this with the extension method <see cref="DateOnlyExtension.HasYear"/>.
    /// </para>
    /// </remarks>
    /// 
    public bool TryAsDateOnly(out DateOnly value)
    {
        (DateOnly Value, bool Result) result = _oneOf.Match<(DateOnly Value, bool Result)>
         (
          dateOnly => (dateOnly, true),
          dtOffset => dtOffset.HasDate()
                        ? (new DateOnly(dtOffset.Year, dtOffset.Month, dtOffset.Day), true)
                        : (default, false),
          timeOnly => (default, false),
          str => System.DateOnly.TryParse(str,
                                          CultureInfo.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces,
                                          out DateOnly dOnly) ? (dOnly, true)
                                                              : (default, false)
         );

        value = result.Value;
        return result.Result;
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
        var result = _oneOf.Match<(DateTimeOffset Value, bool Result)>
         (
          dateOnly => (new DateTimeOffset(dateOnly.Year, dateOnly.Month, dateOnly.Day, 12, 0, 0, TimeSpan.Zero), true),
          dtOffset => (dtOffset, true),
          timeOnly => (new DateTimeOffset().AddTicks(timeOnly.Ticks), true),
          str => System.DateTimeOffset.TryParse(str,
                                                CultureInfo.CurrentCulture,
                                                DateTimeStyles.AllowWhiteSpaces,
                                                out DateTimeOffset dto) ? (dto, true)
                                                                        : (default, false)
         );

        value = result.Value;
        return result.Result;
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="dateAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateOnly"/>.</param>
    /// <param name="dtoAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/>.</param>
    /// <param name="timeAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.TimeOnly"/>.</param>
    /// <param name="stringAction">The <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<DateOnly> dateAction,
                       Action<DateTimeOffset> dtoAction,
                       Action<TimeOnly> timeAction,
                       Action<string> stringAction)
        => _oneOf.Switch(dateAction, dtoAction, timeAction, stringAction);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter.</typeparam>
    /// <param name="dateFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateOnly"/> value.</param>
    /// <param name="dtoFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/> value.</param>
    /// <param name="timeFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.TimeOnly"/> value.</param>
    /// <param name="stringFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// One of the arguments is <c>null</c> and the encapsulated value is of that <see cref="Type"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                    Func<DateTimeOffset, TResult> dtoFunc,
                                    Func<TimeOnly, TResult> timeFunc,
                                    Func<string, TResult> stringFunc)
        => _oneOf.Match(dateFunc, dtoFunc, timeFunc, stringFunc);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _oneOf.ToString();

    private bool IsDateOnly => _oneOf.IsT0;

    private DateOnly AsDateOnly => _oneOf.AsT0;

    private bool IsDateTimeOffset => _oneOf.IsT1;

    private DateTimeOffset AsDateTimeOffset => _oneOf.AsT1;

    private bool IsTimeOnly => _oneOf.IsT2;

    private TimeOnly AsTimeOnly => _oneOf.AsT2;

    private bool IsString => _oneOf.IsT3;

    private string AsString => _oneOf.AsT3;
}
