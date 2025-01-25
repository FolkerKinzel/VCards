using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class VcfTimeOnly : DateAndOrTime
{
    internal VcfTimeOnly(TimeOnly value) => TimeOnly = value;

    public override bool HasYear => false;

    public override bool HasMonth => false;

    public override bool HasDay => false;

    public override DateOnly? DateOnly => null;

    public override DateTimeOffset? DateTimeOffset => null;

    [NotNull]
    public override TimeOnly? TimeOnly { get; }

    public override string? String => null;

    public override bool TryAsDateOnly(out DateOnly value)
    {
        value = default;
        return false;
    }

    public override bool TryAsDateTimeOffset(out DateTimeOffset value)
    {
        value = new DateTimeOffset().AddTicks(TimeOnly.Value.Ticks);
        return true;
    }

    public override bool TryAsTimeOnly(out TimeOnly value)
    {
        value = TimeOnly.Value;
        return true;
    }

    public override string AsString(IFormatProvider? formatProvider = null) => TimeOnly.Value.ToString(formatProvider);

    public override TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                             Func<DateTimeOffset, TResult> dtoFunc,
                                             Func<TimeOnly, TResult> timeFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(timeFunc, nameof(timeFunc));
        return timeFunc(TimeOnly.Value);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<DateOnly, TArg, TResult> dateFunc,
                                                   Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                                   Func<TimeOnly, TArg, TResult> timeFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(timeFunc, nameof(timeFunc));
        return timeFunc(TimeOnly.Value, arg);
    }

    public override void Switch(Action<DateOnly>? dateAction = null,
                                Action<DateTimeOffset>? dtoAction = null,
                                Action<TimeOnly>? timeAction = null,
                                Action<string>? stringAction = null) => timeAction?.Invoke(TimeOnly.Value);

    public override void Switch<TArg>(TArg arg,
                                      Action<DateOnly, TArg>? dateAction = null,
                                      Action<DateTimeOffset, TArg>? dtoAction = null,
                                      Action<TimeOnly, TArg>? timeAction = null,
                                      Action<string, TArg>? stringAction = null) => timeAction?.Invoke(TimeOnly.Value, arg);

    public override bool Equals([NotNullWhen(true)] DateAndOrTime? other)
        => other is VcfTimeOnly vcfTimeOnly && TimeOnly.Value.Equals(vcfTimeOnly.TimeOnly.Value);

    public override int GetHashCode() => TimeOnly.Value.GetHashCode();

    public override string ToString() => $"TimeOnly: {AsString()}";
}
