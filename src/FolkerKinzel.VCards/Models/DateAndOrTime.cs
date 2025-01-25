using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// A union that encapsulates data that describes a date and/or a time.
/// The encapsulated data can be either a <see cref="System.DateOnly"/> value, a 
/// <see cref="System.DateTimeOffset"/> value, a <see cref="System.TimeOnly"/>
/// value, or a <see cref="string"/>.
/// </summary>
/// <seealso cref="DateAndOrTimeProperty"/>
public abstract class DateAndOrTime : IEquatable<DateAndOrTime>
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

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a <see cref="DateOnly"/> 
    /// value.
    /// </summary>
    /// <param name="value">The <see cref="DateOnly"/> value.</param>
    /// <param name="ignoreYear">Pass <c>true</c> to ignore the <see cref="DateOnly.Year"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreMonth">Pass <c>true</c> to ignore the <see cref="DateOnly.Month"/> part, otherwise <c>false</c>.</param>
    /// <param name="ignoreDay">Pass <c>true</c> to ignore the <see cref="DateOnly.Day"/> part, otherwise <c>false</c>.</param>
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static DateAndOrTime Create(DateOnly value,
                                       bool ignoreYear = false,
                                       bool ignoreMonth = false,
                                       bool ignoreDay = false)
    {
        ignoreYear = ignoreYear || !DateAndOrTimeConverter.HasYear(value);

        return ignoreYear && ignoreMonth && ignoreDay
            ? Empty
            : new VcfDateOnly(value, ignoreYear, ignoreMonth, ignoreDay);
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
    /// If <paramref name="ignoreDay"/> is <c>true</c> and <paramref name="ignoreMonth"/> or <paramref name="ignoreYear"/> 
    /// is <c>false</c>, the time component is ignored and the newly created instance will encapsulate a <see cref="DateOnly"/> value.
    /// </para>
    /// </remarks>
    public static DateAndOrTime Create(DateTimeOffset value,
                                       bool ignoreYear = false,
                                       bool ignoreMonth = false,
                                       bool ignoreDay = false)
    {
        ignoreYear = ignoreYear || !DateAndOrTimeConverter.HasYear(value);
        return (ignoreYear && ignoreMonth && ignoreDay && !DateAndOrTimeConverter.HasTime(value))
            ? Empty
            : ignoreDay && !(ignoreYear || ignoreMonth)
                       ? Create(new DateOnly(value.Year, value.Month, value.Day), ignoreYear, ignoreMonth, ignoreDay)
                       : new VcfDateTimeOffset(value, ignoreYear, ignoreMonth, ignoreDay);
    }

    /// <summary>
    /// Creates a new <see cref="DateAndOrTime"/> instance from a <see cref="TimeOnly"/> 
    /// value.
    /// </summary>
    /// <param name="value">The <see cref="TimeOnly"/> value.</param>
    /// 
    /// <returns>The newly created <see cref="DateAndOrTime"/> instance.</returns>
    public static DateAndOrTime Create(TimeOnly value) => new VcfTimeOnly(value);

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
        => string.IsNullOrWhiteSpace(value) ? Empty : new VcfDateString(value);

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
    internal static DateAndOrTime Empty { get; } = new VcfDateString("");

    /// <summary>
    /// <c>true</c> if the instance contains no data, otherwise <c>false</c>.
    /// </summary>
    public bool IsEmpty => ReferenceEquals(this, Empty);

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the year.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the year, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the year in a date but the .NET data types don't. This property
    /// gets the information whether the year component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public abstract bool HasYear { get; }

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the month.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the month, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the month in a date but the .NET data types don't. This property
    /// gets the information whether the month component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public abstract bool HasMonth { get; }

    /// <summary>
    /// Gets a value indicating whether the encapsulated data contains information about the day.
    /// </summary>
    /// <value><c>true</c> if the instance contains information about the day, otherwise <c>false</c>.</value>
    /// <remarks>
    /// The vCard 4.0 specification allows to omit the day in a date but the .NET data types don't. This property
    /// gets the information whether the day component of an encapsulated <see cref="DateOnly"/> or <see cref="DateTimeOffset"/>
    /// value should be treated as irrelevant or not.
    /// </remarks>
    public abstract bool HasDay { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="System.DateOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public abstract DateOnly? DateOnly { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="System.DateTimeOffset"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="System.DateTimeOffset"/> value may contain either a 
    /// <see cref="Data.DateTime"/> or a <see cref="Data.Time"/> with a 
    /// <see cref="Data.UtcOffset"/>.
    /// </remarks>
    public abstract DateTimeOffset? DateTimeOffset { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="System.TimeOnly"/> value,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public abstract TimeOnly? TimeOnly { get; }

    /// <summary>
    /// Gets the encapsulated <see cref="string"/>,
    /// or <c>null</c>, if the encapsulated value has a different <see cref="Type"/>.
    /// </summary>
    public abstract string? String { get; }

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
    /// </remarks>
    /// 
    public abstract bool TryAsDateOnly(out DateOnly value);

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
    /// If the encapsulated value is a <see cref="System.DateOnly"/>, 12 hours
    /// are added to the resulting <see cref="System.DateTimeOffset"/> in order
    /// to avoid wrapping the date.
    /// </remarks>
    public abstract bool TryAsDateTimeOffset(out DateTimeOffset value);

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
    /// <see cref="System.TimeOnly"/>, a <see cref="DateTimeOffset"/>, or a <see cref="string"/> that represents a 
    /// <see cref="System.TimeOnly"/>. If the encapsulated value is a <see cref="DateTimeOffset"/>,
    /// its time will be converted to local time.
    /// </remarks>
    public abstract bool TryAsTimeOnly(out TimeOnly value);

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
    /// If the encapsulated value is a <see cref="string"/>, 
    /// <paramref name="formatProvider"/> is ignored.
    /// </remarks>
    public abstract string AsString(IFormatProvider? formatProvider = null);

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
    public abstract void Switch(Action<DateOnly>? dateAction = null,
                       Action<DateTimeOffset>? dtoAction = null,
                       Action<TimeOnly>? timeAction = null,
                       Action<string>? stringAction = null);

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
    public abstract void Switch<TArg>(TArg arg,
                             Action<DateOnly, TArg>? dateAction = null,
                             Action<DateTimeOffset, TArg>? dtoAction = null,
                             Action<TimeOnly, TArg>? timeAction = null,
                             Action<string, TArg>? stringAction = null);

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
    public abstract TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                    Func<DateTimeOffset, TResult> dtoFunc,
                                    Func<TimeOnly, TResult> timeFunc,
                                    Func<string, TResult> stringFunc);

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
    public abstract TResult Convert<TArg, TResult>(TArg arg,
                                          Func<DateOnly, TArg, TResult> dateFunc,
                                          Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                          Func<TimeOnly, TArg, TResult> timeFunc,
                                          Func<string, TArg, TResult> stringFunc);

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="other"/> is a <see cref="DateAndOrTime"/>
    /// instance, and if the content of <paramref name="other"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public abstract bool Equals([NotNullWhen(true)] DateAndOrTime? other);

    /// <inheritdoc/>
    /// <remarks>Equality is given if <paramref name="obj"/> is a <see cref="DateAndOrTime"/>
    /// instance, and if the content of <paramref name="obj"/> has the same <see cref="Type"/>
    /// and is equal.</remarks>
    public override bool Equals(object? obj) => obj is DateAndOrTime dto && Equals(dto);

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => throw new NotImplementedException();

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
