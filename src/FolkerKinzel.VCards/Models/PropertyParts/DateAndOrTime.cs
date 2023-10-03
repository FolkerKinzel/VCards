using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class DateAndOrTime
{
    private readonly OneOf<DateOnly, DateTimeOffset, TimeOnly, string> _oneOf;

    internal DateAndOrTime(OneOf<DateOnly, DateTimeOffset, TimeOnly, string> oneOf) => _oneOf = oneOf;

    public DateOnly? DateOnly => IsDateOnly ? AsDateOnly : null;

    public DateTimeOffset? DateTimeOffset => IsDateTimeOffset ? AsDateTimeOffset : null;

    public TimeOnly? TimeOnly => IsTimeOnly ? AsTimeOnly : null;

    public string? String => IsString ? AsString : null;

    public object Value => this._oneOf.Value;


    public bool TryAsDateOnly(out DateOnly value)
    {
        var result = _oneOf.Match<(DateOnly Value, bool Result)>
         (
          dateOnly => (dateOnly, true),
          dtOffset => (new DateOnly(dtOffset.Year, dtOffset.Month, dtOffset.Day), true),
          timeOnly => (default, false),
          str => (default, false)
         );

        value = result.Value;
        return result.Result;
    }

    public bool TryAsDateTimeOffset(out DateTimeOffset value)
    {
        var result = _oneOf.Match<(DateTimeOffset Value, bool Result)>
         (
          dateOnly => (new DateTimeOffset(dateOnly.Year, dateOnly.Month, dateOnly.Day, 12, 0, 0, TimeSpan.Zero), true),
          dtOffset => (dtOffset, true),
          timeOnly => (new DateTimeOffset().AddTicks(timeOnly.Ticks), true),
          str => (default, false)
         );

        value = result.Value;
        return result.Result;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<DateOnly>? f0,
                       Action<DateTimeOffset>? f1,
                       Action<TimeOnly>? f2,
                       Action<string>? f3) => _oneOf.Switch(f0, f1, f2, f3);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<DateOnly, TResult>? f0,
                                  Func<DateTimeOffset, TResult>? f1,
                                  Func<TimeOnly, TResult>? f2,
                                  Func<string, TResult>? f3) => _oneOf.Match(f0, f1, f2, f3);

    private bool IsDateOnly => _oneOf.IsT0;

    private DateOnly AsDateOnly => _oneOf.AsT0;

    private bool IsDateTimeOffset => _oneOf.IsT1;

    private DateTimeOffset AsDateTimeOffset => _oneOf.AsT1;

    private bool IsTimeOnly => _oneOf.IsT2;

    private TimeOnly AsTimeOnly => _oneOf.AsT2;

    private bool IsString => _oneOf.IsT3;

    private string AsString => _oneOf.AsT3;


    //public static implicit operator DateAndOrTime(DateOnly _) => new DateAndOrTime(_);
    //public static explicit operator DateOnly(DateAndOrTime _) => _.AsDateOnly;

    //public static implicit operator DateAndOrTime(DateTimeOffset _) => new DateAndOrTime(_);
    //public static explicit operator DateTimeOffset(DateAndOrTime _) => _.AsDateTimeOffset;

    //public static implicit operator DateAndOrTime(TimeOnly _) => new DateAndOrTime(_);
    //public static explicit operator TimeOnly(DateAndOrTime _) => _.AsTimeOnly;

    //public static implicit operator DateAndOrTime(string _) => new DateAndOrTime(_);
    //public static explicit operator string(DateAndOrTime _) => _.AsString;


    //public bool TryGetTimeOnly(out TimeOnly value)
    //{
    //    if (IsTimeOnly)
    //    {
    //        value = AsTimeOnly;
    //        return true;
    //    }
    //    else
    //    {
    //        value = default;
    //        return false;
    //    }
    //}

    //public bool TryGetString([NotNullWhen(true)] out string? value)
    //{
    //    if (IsString)
    //    {
    //        value = AsString;
    //        return true;
    //    }
    //    else
    //    {
    //        value = default;
    //        return false;
    //    }
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickDateOnly(out DateOnly value,
    //                            out OneOf<DateTimeOffset, TimeOnly, string> remainder) 
    //    => _oneOf.TryPickT0(out value, out remainder);


    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickDateTimeOffset(out DateTimeOffset value,
    //                                  out OneOf<DateOnly, TimeOnly, string> remainder)
    //    => _oneOf.TryPickT1(out value, out remainder);


    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickTimeOnly(out TimeOnly value,
    //                            out OneOf<DateOnly, DateTimeOffset, string> remainder)
    //    => _oneOf.TryPickT2(out value, out remainder);


    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickString([NotNullWhen(true)] out string? value,
    //                          out OneOf<DateOnly, DateTimeOffset, TimeOnly> remainder) 
    //    => _oneOf.TryPickT3(out value, out remainder);



    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<TResult, DateTimeOffset, TimeOnly, string> MapDateOnly<TResult>(Func<DateOnly, TResult> mapFunc) 
    //    => _oneOf.MapT0(mapFunc);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<DateOnly, TResult, TimeOnly, string> MapDateTimeOffset<TResult>(Func<DateTimeOffset, TResult> mapFunc)
    //    => _oneOf.MapT1(mapFunc);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<DateOnly, DateTimeOffset, TResult, string> MapTimeOnly<TResult>(Func<TimeOnly, TResult> mapFunc)
    //    => _oneOf.MapT2(mapFunc);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<DateOnly, DateTimeOffset, TimeOnly, TResult> MapString<TResult>(Func<string, TResult> mapFunc)
    //    => _oneOf.MapT3(mapFunc);




    //public int Index => this._oneOf.Index;

}
