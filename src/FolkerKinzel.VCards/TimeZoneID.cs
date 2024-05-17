using System.Globalization;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards;

/// <summary>Represents the standardized name of a time zone.</summary>
/// <seealso cref="Models.TimeZoneProperty"/>
/// <seealso cref="VCard.TimeZones"/>
/// <seealso cref="ParameterSection.TimeZone"/>
public sealed partial class TimeZoneID
{
    private enum TzError { None, Null, Empty }

    private const string UTC_OFFSET_PATTERN = @"^[-\+]?[01][0-9]:?([0-5][0-9])?";

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

    /// <summary> Tries to parse a <see cref="string" /> as <see cref="TimeZoneID" /> object. </summary>
    /// <param name="value">Identifier of the time zone. It should be an identifier
    /// from the "IANA Time Zone Database". (See https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
    /// .)</param>
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

    /// <summary>Standardized name of the time zone.</summary>
    /// <value> That's the String with which the <see cref="TimeZoneID"
    /// /> object was initialized.</value>
    public string Value { get; }

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
        if (IsUtcOffset())
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


    internal void AppendTo(
        StringBuilder builder, VCdVersion version, ITimeZoneIDConverter? converter)
    {
        Debug.Assert(builder is not null);

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                {
                    if (TryGetUtcOffset(out TimeSpan utcOffset, converter))
                    {
                        string format = utcOffset < TimeSpan.Zero ? @"\-hh\:mm" : @"\+hh\:mm";
                        _ = builder.Append(utcOffset.ToString(format, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        _ = builder.Append(Value);
                    }
                    break;
                }
            default:
                {
                    if (IsUtcOffset() && TryGetUtcOffset(out TimeSpan utcOffset))
                    {
                        string format = utcOffset < TimeSpan.Zero ? @"\-hhmm" : @"\+hhmm";
                        _ = builder.Append(utcOffset.ToString(format, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        _ = builder.Append(Value);
                    }
                    break;
                }
        }
    }

    [ExcludeFromCodeCoverage]
    private bool IsUtcOffset()
    {
#if NET462 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0 || NET6_0
        try
        {
            return Regex.IsMatch(Value,
                                 UTC_OFFSET_PATTERN,
                                 RegexOptions.CultureInvariant,
                                 TimeSpan.FromMilliseconds(50));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
#else
        try
        {
            return UtcOffsetRegex().IsMatch(Value);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }


    [GeneratedRegex(UTC_OFFSET_PATTERN, RegexOptions.CultureInvariant, 50)]
    private static partial Regex UtcOffsetRegex();
#endif
}


