using System.Globalization;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class VcfDateString : DateAndOrTime
{
    internal VcfDateString(string value) => String = value;

    public override bool HasYear => false;

    public override bool HasMonth => false;

    public override bool HasDay => false;

    public override DateOnly? DateOnly => null;

    public override DateTimeOffset? DateTimeOffset => null;

    public override TimeOnly? TimeOnly => null;

    [NotNull]
    public override string? String { get; }


    public override bool TryAsDateOnly(out DateOnly value)
        => System.DateOnly.TryParse(String,
                                    CultureInfo.CurrentCulture,
                                    DateTimeStyles.AllowWhiteSpaces,
                                    out value);


    public override bool TryAsDateTimeOffset(out DateTimeOffset value)
        => System.DateTimeOffset.TryParse(String,
                                          CultureInfo.CurrentCulture,
                                          DateTimeStyles.AllowWhiteSpaces,
                                          out value);

    public override bool TryAsTimeOnly(out TimeOnly value) => System.TimeOnly.TryParse(String,
                                                              CultureInfo.CurrentCulture,
                                                              DateTimeStyles.AllowWhiteSpaces,
                                                              out value);

    public override string AsString(IFormatProvider? formatProvider = null) => String;

    public override TResult Convert<TResult>(Func<DateOnly, TResult> dateFunc,
                                             Func<DateTimeOffset, TResult> dtoFunc,
                                             Func<TimeOnly, TResult> timeFunc,
                                             Func<string, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(stringFunc, nameof(stringFunc));
        return stringFunc(String);
    }

    public override TResult Convert<TArg, TResult>(TArg arg,
                                                   Func<DateOnly, TArg, TResult> dateFunc,
                                                   Func<DateTimeOffset, TArg, TResult> dtoFunc,
                                                   Func<TimeOnly, TArg, TResult> timeFunc,
                                                   Func<string, TArg, TResult> stringFunc)
    {
        _ArgumentNullException.ThrowIfNull(stringFunc, nameof(stringFunc));
        return stringFunc(String, arg);
    }

    public override void Switch(Action<DateOnly>? dateAction = null,
                                Action<DateTimeOffset>? dtoAction = null,
                                Action<TimeOnly>? timeAction = null,
                                Action<string>? stringAction = null) => stringAction?.Invoke(String);

    public override void Switch<TArg>(TArg arg,
                                      Action<DateOnly, TArg>? dateAction = null,
                                      Action<DateTimeOffset, TArg>? dtoAction = null,
                                      Action<TimeOnly, TArg>? timeAction = null,
                                      Action<string, TArg>? stringAction = null) => stringAction?.Invoke(String, arg);

    public override bool Equals([NotNullWhen(true)] DateAndOrTime? other)
        => StringComparer.CurrentCultureIgnoreCase.Equals(String, other?.String);

    public override int GetHashCode() => StringComparer.CurrentCultureIgnoreCase.GetHashCode(String);

    public override string ToString() => IsEmpty ? "<Empty>" : $"String: {String}";
}
