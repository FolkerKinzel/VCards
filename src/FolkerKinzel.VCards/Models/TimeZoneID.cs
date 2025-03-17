using System.Globalization;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the standardized name of a time zone.</summary>
/// <seealso cref="Models.Properties.TimeZoneProperty"/>
/// <seealso cref="VCard.TimeZones"/>
/// <seealso cref="ParameterSection.TimeZone"/>
public sealed class TimeZoneID
{
    private enum TzError { None, Null, Empty }

    private TimeZoneID()
    {
        Value = "";
        IsEmpty = true;
    }

    private TimeZoneID(string timeZoneID) => Value = timeZoneID.Trim();

    /// <summary> Parses a <see cref="string" /> as <see cref="TimeZoneID" /> object.
    /// </summary>
    /// <param name="value">Identifier of the time zone. It should be an identifier
    /// from the "IANA Time Zone Database".
    /// (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .)</param>
    /// <returns>The parsed <see cref="TimeZoneID"/>.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="value" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException"> <paramref name="value" /> is an empty
    /// <see cref="string" /> or consists only of white space characters.</exception>
    public static TimeZoneID Parse(string value)
    {
        if (!Validate(value, out TzError error))
        {
            if (error == TzError.Null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            throw new ArgumentException(Res.NoData, nameof(value));
        }

        return new TimeZoneID(value);
    }

    /// <summary> Tries to parse a <see cref="string" /> as <see cref="TimeZoneID" /> object.
    /// </summary>
    /// <param name="value">Identifier of the time zone. It should be an identifier
    /// from the "IANA Time Zone Database". 
    /// (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .)</param>
    /// <param name="timeZoneID">If parsing was successful, the parameter contains a new 
    /// <see cref="TimeZoneID" /> object. The parameter is passed uninitialized.</param>
    /// <returns> <c>true</c> if parsing was successful, otherwise <c>false</c>.</returns>
    public static bool TryParse(string? value, [NotNullWhen(true)] out TimeZoneID? timeZoneID)
    {
        if (Validate(value, out _))
        {
            timeZoneID = new TimeZoneID(value);
            return true;
        }

        timeZoneID = null;
        return false;
    }

    private static bool Validate([NotNullWhen(true)] string? value, out TzError error)
    {
        if (value is null)
        {
            error = TzError.Null;
            return false;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            error = TzError.Empty;
            return false;
        }

        error = TzError.None;
        return true;
    }

    /// <summary> Tries to create a new <see cref="TimeZoneID" /> instance from
    /// <paramref name="value"/> and returns <c>null</c> if the creation fails. </summary>
    /// <param name="value">Identifier of the time zone. It should be an identifier
    /// from the "IANA Time Zone Database". (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
    /// .)</param>
    /// <returns>The newly created <see cref="TimeZoneID"/> instance, or <c>null</c>
    /// if one of the arguments is out of range.</returns>
    /// <seealso cref="TimeZoneProperty"/>
    /// <seealso cref="VCard.TimeZones"/>
    /// <seealso cref="ParameterSection.TimeZone"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeZoneID? TryCreate(string? value)
        => TryParse(value, out TimeZoneID? timeZoneID) ? timeZoneID : null;

    /// <summary>Standardized name of the time zone.</summary>
    /// <value> That's the String with which the <see cref="TimeZoneID"
    /// /> object was initialized.</value>
    public string Value { get; }

    /// <summary>
    /// If <c>true</c>, the instance doesn't represent an existing time zone, otherwise, <c>false</c>.
    /// </summary>
    /// <remarks>
    /// The possibility to create an empty <see cref="TimeZoneID"/> exists because <see cref="VCardBuilder"/> 
    /// needs the ability to create an empty <see cref="TimeZoneProperty"/>. 
    /// The value of of an empty <see cref="TimeZoneID"/> is never written to a VCF file and is not taken
    /// into account in conversions.
    /// </remarks>
    public bool IsEmpty { get; }

    internal static TimeZoneID Empty => new(); // Not a singleton

    /// <summary>Tries to find a corresponding UTC offset for the <see cref="TimeZoneID" />
    /// object.</summary>
    /// <param name="utcOffset">Contains the UTC offset after the method has been successfully
    /// returned. The argument is passed uninitialized.</param>
    /// <param name="converter">An object that implements <see cref="ITimeZoneIDConverter"
    /// /> to convert IANA time zone names to UTC offsets, or <c>null</c>.</param>
    /// <returns> <c>true</c> if a suitable UTC offset could be found, otherwise <c>false</c>.</returns>
    /// <seealso cref="ITimeZoneIDConverter" />
    public bool TryGetUtcOffset(out TimeSpan utcOffset, ITimeZoneIDConverter? converter = null)
    {
        if (IsEmpty)
        {
            utcOffset = default;
            return false;
        }

        if (this.IsUtcOffset())
        {
            int startIndex = 0;

            TimeSpanStyles styles = TimeSpanStyles.None;

            if (Value.StartsWith('-'))
            {
                startIndex = 1;
                styles = TimeSpanStyles.AssumeNegative;
            }
            else if (Value.StartsWith('+'))
            {
                startIndex = 1;
            }

            ReadOnlySpan<char> input = Value.AsSpan(startIndex);

            return _TimeSpan.TryParseExact(
                input,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                styles, out utcOffset) ||

                _TimeSpan.TryParseExact(
                input,
                @"hhmm",
                CultureInfo.InvariantCulture,
                styles, out utcOffset) ||

                _TimeSpan.TryParseExact(
                input,
                @"hh",
                CultureInfo.InvariantCulture,
                styles, out utcOffset);
        }

        if (converter is not null && converter.TryConvertToUtcOffset(Value, out utcOffset))
        {
            return true;
        }

        try
        {
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(Value);
            utcOffset = tzInfo.BaseUtcOffset;
            return true;
        }
        catch { }

        utcOffset = default;
        return false;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}


