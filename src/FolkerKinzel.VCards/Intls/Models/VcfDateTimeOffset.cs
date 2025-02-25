using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class VcfDateTimeOffset : DateAndOrTime
{
    internal VcfDateTimeOffset(DateTimeOffset value, bool ignoreYear, bool ignoreMonth, bool ignoreDay)
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

        DateTimeOffset = year == value.Year && month == value.Month && day == value.Day
            ? value
            : new DateTimeOffset(year, month, day, value.Hour, value.Minute, value.Second, value.Offset);
    }

    public override bool HasYear { get; }

    public override bool HasMonth { get; }

    public override bool HasDay { get; }

    public override DateOnly? DateOnly => null;

    [NotNull]
    public override DateTimeOffset? DateTimeOffset { get; }

    public override TimeOnly? TimeOnly => null;

    public override string? String => null;

    public override bool TryAsDateOnly(out DateOnly value)
    {
        if (HasMonth || HasDay || HasYear)
        {
            DateTimeOffset dtOffset = DateTimeOffset.Value;
            value = new DateOnly(dtOffset.Year, dtOffset.Month, dtOffset.Day);
            return true;
        }

        value = default;
        return false;
    }

    public override bool TryAsDateTimeOffset(out DateTimeOffset value)
    {
        value = DateTimeOffset.Value;
        return true;
    }

    public override bool TryAsTimeOnly(out TimeOnly value)
    {
        DateTimeOffset dto = DateTimeOffset.Value.ToLocalTime();
        value = new TimeOnly(dto.Hour, dto.Minute, dto.Second);
        return true;
    }

    public override string AsString(IFormatProvider? formatProvider = null)
    {
        if (HasYear && HasMonth && HasDay)
        {
            return DateTimeOffset.Value.ToString(formatProvider);
        }

        var sb = new StringBuilder();
        DateAndOrTimeConverter.AppendDateTimeOffsetTo(sb, DateTimeOffset.Value, VCdVersion.V4_0, HasYear, HasMonth, HasDay);
        return sb.ToString();
    }

    public override TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                             Func<DateTimeOffset, TResult> dtoFunc,
                                             Func<TimeOnly, TResult> timeFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(dtoFunc, nameof(dtoFunc));
        return dtoFunc(DateTimeOffset.Value);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<DateOnly, TArg, TResult> dateFunc,
                                                   Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                                   Func<TimeOnly, TArg, TResult> timeFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(dtoFunc, nameof(dtoFunc));
        return dtoFunc(DateTimeOffset.Value, arg);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch(Action<DateOnly>? dateAction = null,
                                Action<DateTimeOffset>? dtoAction = null,
                                Action<TimeOnly>? timeAction = null,
                                Action<string>? stringAction = null) => dtoAction?.Invoke(DateTimeOffset.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Switch<TArg>(TArg arg,
                                      Action<DateOnly, TArg>? dateAction = null,
                                      Action<DateTimeOffset, TArg>? dtoAction = null,
                                      Action<TimeOnly, TArg>? timeAction = null,
                                      Action<string, TArg>? stringAction = null) => dtoAction?.Invoke(DateTimeOffset.Value, arg);

    public override bool Equals([NotNullWhen(true)] DateAndOrTime? other)
        => other is VcfDateTimeOffset vcfDateTimeOffset
        && HasYear == other.HasYear
        && HasMonth == other.HasMonth
        && HasDay == other.HasDay
        && DateTimeOffset.Value.Equals(vcfDateTimeOffset.DateTimeOffset.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(HasYear, HasMonth, HasDay, DateTimeOffset.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"DateTimeOffset: {AsString()}";
}
