using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class VcfDateOnly : DateAndOrTime
{
    internal VcfDateOnly(DateOnly value, bool ignoreYear, bool ignoreMonth, bool ignoreDay)
    {
        int year = value.Year;
        int month = value.Month;
        int day = value.Day;

        if (ignoreYear)
        {
            year = DateAndOrTimeConverter.FIRST_LEAP_YEAR;
        }
        else
        {
            HasYear = true;
        }

        if (ignoreMonth)
        {
            month = 1;
        }
        else
        {
            HasMonth = true;
        }

        if (ignoreDay)
        {
            day = 1;
        }
        else
        {
            HasDay = true;
        }

        DateOnly = year == value.Year && month == value.Month && day == value.Day
            ? value
            : new DateOnly(year, month, day);
    }

    public override bool HasYear { get; }

    public override bool HasMonth { get; }

    public override bool HasDay { get; }

    [NotNull]
    public override DateOnly? DateOnly { get; }

    public override DateTimeOffset? DateTimeOffset => null;

    public override TimeOnly? TimeOnly => null;

    public override string? String => null;

    public override bool TryAsDateOnly(out DateOnly value)
    {
        value = DateOnly.Value;
        return true;
    }

    public override bool TryAsDateTimeOffset(out DateTimeOffset value)
    {
        DateOnly dateOnly = DateOnly.Value;
        value = new DateTimeOffset(dateOnly.Year, dateOnly.Month, dateOnly.Day, 12, 0, 0, TimeSpan.Zero);
        return true;
    }

    public override bool TryAsTimeOnly(out TimeOnly value)
    {
        value = default;
        return false;
    }

    public override string AsString(IFormatProvider? formatProvider = null)
    {
        if (HasYear && HasMonth && HasDay)
        {
            return DateOnly.Value.ToString(formatProvider);
        }

        var sb = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTo(sb, DateOnly.Value, VCdVersion.V4_0, HasYear, HasMonth, HasDay);
        return sb.ToString();
    }

    public override TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                             Func<DateTimeOffset, TResult> dtoFunc,
                                             Func<TimeOnly, TResult> timeFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(dateFunc, nameof(dateFunc));
        return dateFunc(DateOnly.Value);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<DateOnly, TArg, TResult> dateFunc,
                                                   Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                                   Func<TimeOnly, TArg, TResult> timeFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(dateFunc, nameof(dateFunc));
        return dateFunc(DateOnly.Value, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch(Action<DateOnly>? dateAction = null,
                                Action<DateTimeOffset>? dtoAction = null,
                                Action<TimeOnly>? timeAction = null,
                                Action<string>? stringAction = null) => dateAction?.Invoke(DateOnly.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch<TArg>(TArg arg,
                                      Action<DateOnly, TArg>? dateAction = null,
                                      Action<DateTimeOffset, TArg>? dtoAction = null,
                                      Action<TimeOnly, TArg>? timeAction = null,
                                      Action<string, TArg>? stringAction = null) => dateAction?.Invoke(DateOnly.Value, arg);

    public override bool Equals([NotNullWhen(true)] DateAndOrTime? other)
        => other is VcfDateOnly vcfDateOnly
           && HasYear == other.HasYear
           && HasMonth == other.HasMonth
           && HasDay == other.HasDay
           && DateOnly.Value.Equals(vcfDateOnly.DateOnly.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(HasYear, HasMonth, HasDay, DateOnly.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"DateOnly: {AsString()}";
}
