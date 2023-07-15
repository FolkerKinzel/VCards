using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Repräsentiert den standardisierten Namen einer Zeitzone.
/// </summary>
public class TimeZoneID
{
    /// <summary>
    /// Initialisiert ein neues <see cref="TimeZoneID"/>-Objekt.
    /// </summary>
    /// <param name="timeZoneID">Bezeichner der Zeitzone. Es sollte sich um einen Bezeichner aus der 
    /// "IANA Time Zone Database" handeln. (Siehe https://en.wikipedia.org/wiki/List_of_tz_database_time_zones .)</param>
    /// <exception cref="ArgumentNullException"><paramref name="timeZoneID"/> ist <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="timeZoneID"/> ist ein leerer <see cref="string"/> oder 
    /// enthält nur Leerraum.</exception>
    public TimeZoneID(string timeZoneID)
    {
        Value = timeZoneID ?? throw new ArgumentNullException(nameof(timeZoneID));

        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException(Res.NoData, nameof(timeZoneID));
        }
    }


    /// <summary>
    /// Standardisierter Name der Zeitzone.
    /// </summary>
    /// <remarks>
    /// Das ist der <see cref="string"/>, mit dem das <see cref="TimeZoneID"/>-Objekt initialisiert wurde.
    /// </remarks>
    public string Value { get; }


    /// <summary>
    /// Versucht, einen entsprechenden UTC-Offset für das <see cref="TimeZoneID"/>-Objekt zu finden.
    /// </summary>
    /// <param name="utcOffset">Enthält nach erfolgreicher Beendigung der Methode den UTC-Offset. Das Argument
    /// wird uninitialisiert übergeben.</param>
    /// <param name="converter">Ein Objekt, das <see cref="ITimeZoneIDConverter"/> implementiert, um IANA time zone IDs
    /// in UTC-Offsets umzuwandeln, oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn ein geeigneter UTC-Offset gefunden werden konnte, andernfalls <c>false</c>.</returns>
    /// <seealso cref="ITimeZoneIDConverter"/>
    public bool TryGetUtcOffset(out TimeSpan utcOffset, ITimeZoneIDConverter? converter = null)
    {
        if (IsUtcOffset())
        {
            int startIndex = 0;

            TimeSpanStyles styles = TimeSpanStyles.None;


            if (Value.StartsWith("-", StringComparison.Ordinal))
            {
                startIndex = 1;
                styles = TimeSpanStyles.AssumeNegative;
            }
            else if (Value.StartsWith("+", StringComparison.Ordinal))
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

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="TimeZoneID"/>-Objekts. (Nur zum Debuggen.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="TimeZoneID"/>-Objekts.</returns>
    public override string ToString() => Value;

    internal void AppendTo(StringBuilder builder, VCdVersion version, ITimeZoneIDConverter? converter)
    {
        Debug.Assert(builder != null);

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


    private bool IsUtcOffset()
    {
        const string pattern = @"^[-\+]?[01][0-9]:?([0-5][0-9])?";
        const RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline;

        try
        {
            return Regex.IsMatch(Value, pattern, options, TimeSpan.FromMilliseconds(50));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
