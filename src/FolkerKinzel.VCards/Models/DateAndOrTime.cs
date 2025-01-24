using System;
using System.ComponentModel;
using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// A union that encapsulates data that describes a date and/or a time.
/// The encapsulated data can be either a <see cref="System.DateOnly"/> value, a 
/// <see cref="System.DateTimeOffset"/> value, a <see cref="System.TimeOnly"/>
/// value, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DateAndOrTimeProperty"/>
public sealed class DateAndOrTime : IEquatable<DateAndOrTime>
{
    #region Remove with 8.0.1

    [Obsolete("Use the TryAsXXX methods instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public object Value => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use Create(DateOnly, bool, bool, bool) instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DateAndOrTime Create(int month, int day)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        => throw new NotImplementedException();

    #endregion

    private DateAndOrTime(DateOnly value, bool ignoreMonth, bool ignoreDay)
    {
        if (ignoreMonth)
        {
            HasMonth = false;

            if (value.Month != 1)
            { 
                value = new DateOnly(value.Year, 1, value.Day); 
            }
        }

        if (ignoreDay)
        {
            HasDay = false;

            if (value.Day != 1) 
            {
                value = new DateOnly(value.Year, value.Month, 1); 
            }
        }

        DateOnly = value;
    }

    private DateAndOrTime(DateTimeOffset value, bool ignoreMonth, bool ignoreDay)
    {
        if(ignoreMonth)
        {
            HasMonth = false;

            if (value.Month != 1)
            {
                value = new DateTimeOffset(value.Year, 1, value.Day, value.Hour, value.Minute, value.Second, value.Offset);
            }
        }

        if (ignoreDay)
        {
            HasDay = false;

            if (value.Day != 1)
            {
                value = new DateTimeOffset(value.Year, value.Month, 1, value.Hour, value.Minute, value.Second, value.Offset);
            }
        }

        DateTimeOffset = value;
    }

    private DateAndOrTime(TimeOnly value)
    {
        TimeOnly = value;
        HasMonth = false;
        HasDay = false;
    }

    private DateAndOrTime(string value)
    {
        String = value;
        HasMonth = false;
        HasDay = false;
    }

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a <see cref="DateOnly"/> 
    /// value.
    /// </summary>
    /// <param name="value">The <see cref="DateOnly"/> value.</param>
    /// <param name="ignoreYear">Pass <c>true</c> to ignore the <see cref="DateOnly.Year"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreMonth">Pass <c>true</c> to ignore the <see cref="DateOnly.Month"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreDay">Pass <c>true</c> to ignore the <see cref="DateOnly.Day"/> part, otherwise <c>false</c>.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static DateAndOrTime Create(DateOnly value, bool ignoreYear = false, bool ignoreMonth = false, bool ignoreDay = false)
    {
        return !ignoreYear && value.HasYear()
                    ? new(value, ignoreMonth, ignoreDay)
                    : ignoreMonth && ignoreDay
                         ? Empty
                         : value.Year == DateAndOrTimeConverter.FIRST_LEAP_YEAR
                              ? new(value, ignoreMonth, ignoreDay)
                              : new(new DateOnly(DateAndOrTimeConverter.FIRST_LEAP_YEAR, value.Month, value.Day), ignoreMonth, ignoreDay);
    }

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a <see cref="DateTimeOffset"/> 
    /// value.
    /// </summary>
    /// <param name="value">The <see cref="DateTimeOffset"/> value.</param>
    /// <param name="ignoreYear">Pass <c>true</c> to ignore the <see cref="DateTimeOffset.Year"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreMonth">Pass <c>true</c> to ignore the <see cref="DateTimeOffset.Month"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreDay">Pass <c>true</c> to ignore the <see cref="DateTimeOffset.Day"/> part, otherwise <c>false</c>.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    /// <remarks>
    /// <para>
    /// If <paramref name="value"/> has neither a date nor a time, the method returns
    /// an empty instance.
    /// </para>
    /// <para>
    /// If <paramref name="ignoreDay"/> is <c>true</c> and <paramref name="ignoreMonth"/> or <paramref name="ignoreYear"/> is <c>true</c>,
    /// the time component is ignored and the newly created instance will encapsulate a <see cref="DateOnly"/> value.
    /// </para>
    /// </remarks>
    public static DateAndOrTime Create(DateTimeOffset value,
                                       bool ignoreYear = false,
                                       bool ignoreMonth = false,
                                       bool ignoreDay = false)
        => (ignoreYear && ignoreMonth && ignoreDay) || !DateAndOrTimeConverter.HasDate(value)
            ? DateAndOrTimeConverter.HasTime(value)
                ? new(new DateTimeOffset(2, 1, 1, value.Hour, value.Minute, value.Second, value.Offset), true, true)
                : Empty
            : ignoreDay && (ignoreYear || ignoreMonth) 
                   ? Create(new DateOnly(value.Year, value.Month, value.Day), ignoreYear, ignoreMonth, ignoreDay) 
                   : ignoreYear || !DateAndOrTimeConverter.HasYear(value)
                        ? (ignoreMonth && ignoreDay)
                               ? Empty
                               : value.Year == DateAndOrTimeConverter.FIRST_LEAP_YEAR
                                   ? new(value, ignoreMonth, ignoreDay)
                                   : new(new DateTimeOffset(DateAndOrTimeConverter.FIRST_LEAP_YEAR,
                                                            value.Month,
                                                            value.Day,
                                                            value.Hour,
                                                            value.Minute,
                                                            value.Second,
                                                            value.Offset), ignoreMonth, ignoreDay)
                        : new(value, ignoreMonth, ignoreDay);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a <see cref="TimeOnly"/> 
    /// value.
    /// </summary>
    /// <param name="value">The <see cref="TimeOnly"/> value.</param>
    /// 
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static DateAndOrTime Create(TimeOnly value) => new(value);

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from free-form text.
    /// </summary>
    /// <param name="value">Free-form text, or <c>null</c>.</param>
    /// 
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    /// 
    /// <example>
    /// <code language="cs">
    /// var result = DateAndOrTime.Create("after midnight");
    /// </code>
    /// </example>
    public static DateAndOrTime Create(string? value) 
        => string.IsNullOrWhiteSpace(value) ? Empty : new(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="System.DateOnly"/> value to a 
    /// <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static implicit operator DateAndOrTime(DateOnly value) => Create(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="System.DateTimeOffset"/> value to a 
    /// <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static implicit operator DateAndOrTime(DateTimeOffset value) => Create(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="System.DateTime"/> value to a 
    /// <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static implicit operator DateAndOrTime(DateTime value) => Create(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="System.TimeOnly"/> value to a 
    /// <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static implicit operator DateAndOrTime(TimeOnly value) => Create(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="string"/> to a 
    /// <see cref="DateAndOrTime"/> object.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to convert, or <c>null</c>.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static implicit operator DateAndOrTime(string? value) => Create(value);

    /// <summary>
    /// A singleton whose <see cref="IsEmpty"/> property returns <c>true</c>.
    /// </summary>
    internal static DateAndOrTime Empty { get; } = new DateAndOrTime("");

    /// <summary>
    /// <c>true</c> if the instance contains no data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the year.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the year, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the year in a date but the .NET data types doesn't. This property
    /// gets the information whether the year component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public bool HasYear => (DateOnly.HasValue && DateOnly.Value.HasYear()) ||
                           (DateTimeOffset.HasValue && DateAndOrTimeConverter.HasYear(DateTimeOffset.Value));

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the month.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the month, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the month in a date but the .NET data types doesn't. This property
    /// gets the information whether the month component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public bool HasMonth { get; internal set; } = true;

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the day.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the day, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the day in a date but the .NET data types doesn't. This property
    /// gets the information whether the day component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public bool HasDay { get; internal set; } = true;

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
                                                 out DateOnly dOnly)
                                                    ? (dOnly, true)
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
                                                       out DateTimeOffset dto)
                                                           ? (dto, true)
                                                           : (default, false)
         );

        value = parsed;
        return result;
    }

    /// <summary>
    /// Tries to convert the encapsulated data to a <see cref="System.TimeOnly"/> value.
    /// </summary>
    /// <param name="value">When the method
    /// returns <c>true</c>, contains a <see cref="System.TimeOnly"/> value that
    /// represents the data that is encapsulated in the instance. The
    /// parameter is passed uninitialized.</param>
    /// <returns><c>true</c> if the conversion was successful, otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// The conversion succeeds only if the encapsulated value is a
    /// <see cref="System.TimeOnly"/>, or a <see cref="string"/> that represents a 
    /// <see cref="System.TimeOnly"/>.
    /// </remarks>
    public bool TryAsTimeOnly(out TimeOnly value)
    {
        if (TimeOnly.HasValue)
        {
            value = TimeOnly.Value;
            return true;
        }

        return System.TimeOnly.TryParse(String,
                                        CultureInfo.CurrentCulture,
                                        DateTimeStyles.AllowWhiteSpaces,
                                        out value);
    }

    /// <summary>
    /// Converts the encapsulated data to a <see cref="string"/>.
    /// </summary>
    /// <param name="formatProvider">
    /// An object that supplies culture-specific formatting information, or
    /// <c>null</c> for <see cref="CultureInfo.CurrentCulture"/>.
    /// </param>
    /// <returns>The encapsulated data as <see cref="string"/>.
    /// </returns>
    /// <remarks>
    /// If the encapsulated value is a <see cref="DateTimeOffset"/>, or a <see cref="string"/>, 
    /// <paramref name="formatProvider"/> is ignored.
    /// </remarks>
    public string AsString(IFormatProvider? formatProvider = null)
    {
        return Convert<(IFormatProvider FormatProvider, DateAndOrTime DateAndOrTime), string>
            (
                (formatProvider ?? CultureInfo.CurrentCulture, this),

                static (date, tuple) => DateOnlyToString(date, tuple.FormatProvider, tuple.DateAndOrTime),
                static (dtOffset, tuple) => DateTimeOffsetToString(dtOffset, tuple.DateAndOrTime),
                static (time, tuple) => time.ToString(tuple.FormatProvider),
                static (str, tuple) => str
            );

        static string DateOnlyToString(DateOnly date, IFormatProvider formatProvider, DateAndOrTime daot)
        {
            if (date.HasYear() && daot.HasMonth && daot.HasDay)
            {
                return date.ToString(formatProvider);
            }

            var sb = new StringBuilder();
            DateAndOrTimeConverter.AppendDateTo(sb, date, VCdVersion.V3_0, daot.HasMonth, daot.HasDay);
            return sb.ToString();
        }

        static string DateTimeOffsetToString(DateTimeOffset dtOffset, DateAndOrTime daot)
        {
            var sb = new StringBuilder();

            DateAndOrTimeConverter.AppendDateTimeOffsetTo(sb, dtOffset, VCdVersion.V3_0, daot.HasMonth, daot.HasDay);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value.
    /// </summary>
    /// <param name="dateAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateOnly"/>.</param>
    /// <param name="dtoAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/>.</param>
    /// <param name="timeAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.TimeOnly"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    /// 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<DateOnly>? dateAction = null,
                       Action<DateTimeOffset>? dtoAction = null,
                       Action<TimeOnly>? timeAction = null,
                       Action<string>? stringAction = null)
    {
        if (DateOnly.HasValue)
        {
            dateAction?.Invoke(DateOnly.Value);
        }
        else if (DateTimeOffset.HasValue)
        {
            dtoAction?.Invoke(DateTimeOffset.Value);
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
    /// Performs an <see cref="Action{T}"/> depending on the <see cref="Type"/> of the 
    /// encapsulated value and allows to pass an argument to the delegates.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="dateAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateOnly"/>.</param>
    /// <param name="dtoAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.DateTimeOffset"/>.</param>
    /// <param name="timeAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="System.TimeOnly"/>.</param>
    /// <param name="stringAction"><c>null</c>, or the <see cref="Action{T}"/> to perform if the encapsulated
    /// value is a <see cref="string"/>.</param>
    public void Switch<TArg>(TArg arg,
                             Action<DateOnly, TArg>? dateAction = null,
                             Action<DateTimeOffset, TArg>? dtoAction = null,
                             Action<TimeOnly, TArg>? timeAction = null,
                             Action<string, TArg>? stringAction = null)
    {
        if (DateOnly.HasValue)
        {
            dateAction?.Invoke(DateOnly.Value, arg);
        }
        else if (DateTimeOffset.HasValue)
        {
            dtoAction?.Invoke(DateTimeOffset.Value, arg);
        }
        else if (TimeOnly.HasValue)
        {
            timeAction?.Invoke(TimeOnly.Value, arg);
        }
        else
        {
            stringAction?.Invoke(String!, arg);
        }
    }

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// <param name="dateFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateOnly"/> value.</param>
    /// <param name="dtoFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
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
                                    Func<DateTimeOffset, TResult> dtoFunc,
                                    Func<TimeOnly, TResult> timeFunc,
                                    Func<string, TResult> stringFunc)
        => DateOnly.HasValue
            ? dateFunc is null ? throw new ArgumentNullException(nameof(dateFunc)) : dateFunc(DateOnly.Value)
            : DateTimeOffset.HasValue
                ? dtoFunc is null ? throw new ArgumentNullException(nameof(dtoFunc)) : dtoFunc(DateTimeOffset.Value)
                : TimeOnly.HasValue
                    ? timeFunc is null ? throw new ArgumentNullException(nameof(timeFunc)) : timeFunc(TimeOnly.Value)
                    : stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc(String!);

    /// <summary>
    /// Converts the encapsulated value to <typeparamref name="TResult"/> and allows to specify an
    /// argument for the conversion.
    /// </summary>
    /// <typeparam name="TArg">Generic type parameter for the type of the argument to pass
    /// to the delegates.</typeparam>
    /// <typeparam name="TResult">Generic type parameter for the return type of the delegates.</typeparam>
    /// <param name="arg">The argument to pass to the delegates.</param>
    /// <param name="dateFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
    /// value is a <see cref="System.DateOnly"/> value.</param>
    /// <param name="dtoFunc">The <see cref="Func{T, TResult}"/> to call if the encapsulated
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
    public TResult Convert<TArg, TResult>(TArg arg,
                                          Func<DateOnly, TArg, TResult> dateFunc,
                                          Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                          Func<TimeOnly, TArg, TResult> timeFunc,
                                          Func<string, TArg, TResult> stringFunc)
        => DateOnly.HasValue
            ? dateFunc is null ? throw new ArgumentNullException(nameof(dateFunc)) : dateFunc(DateOnly.Value, arg)
            : DateTimeOffset.HasValue
                ? dtoFunc is null ? throw new ArgumentNullException(nameof(dtoFunc)) : dtoFunc(DateTimeOffset.Value, arg)
                : TimeOnly.HasValue
                    ? timeFunc is null ? throw new ArgumentNullException(nameof(timeFunc)) : timeFunc(TimeOnly.Value, arg)
                    : stringFunc is null ? throw new ArgumentNullException(nameof(stringFunc)) : stringFunc(String!, arg);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        if (IsEmpty)
        {
            return "<Empty>";
        }

        string? type = Convert
        (
            static date => typeof(System.DateOnly).Name,
            static dto => typeof(System.DateTimeOffset).Name,
            static time => typeof(System.TimeOnly).Name,
            static str => typeof(string).Name
        );

        return $"{type}: {AsString()}";
    }

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="other"/> is a <see cref="DateAndOrTime"/>
    /// instance, and if the content of <paramref name="other"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public bool Equals(DateAndOrTime? other)
       => other is not null
        && (ReferenceEquals(this, other)
            || (HasYear == other.HasYear && HasMonth == other.HasMonth && HasDay == other.HasDay
                && (DateOnly.HasValue
                    ? DateOnly == other.DateOnly
                    : DateTimeOffset.HasValue
                       ? DateTimeOffset == other.DateTimeOffset
                       : TimeOnly.HasValue
                           ? TimeOnly == other.TimeOnly
                           : String!.Equals(other.String, StringComparison.Ordinal))
                )
            );

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="obj"/> is a <see cref="DateAndOrTime"/>
    /// instance, and if the content of <paramref name="obj"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public override bool Equals(object? obj) => obj is DateAndOrTime dto && Equals(dto);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(DateOnly, DateTimeOffset, TimeOnly, String);

    /// <summary>
    /// Overloads the equality operator for <see cref="DateAndOrTime"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="DateAndOrTime"/> object, or <c>null</c>.</param>
    /// <param name="right">The right <see cref="DateAndOrTime"/> object, or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(DateAndOrTime? left, DateAndOrTime? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="DateAndOrTime"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="DateAndOrTime"/> object, or <c>null</c>.</param>
    /// <param name="right">The right <see cref="DateAndOrTime"/> object, or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(DateAndOrTime? left, DateAndOrTime? right)
        => !(left == right);
}
