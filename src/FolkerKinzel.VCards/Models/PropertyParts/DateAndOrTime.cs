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


    
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _oneOf.ToString();

    private bool IsDateOnly => _oneOf.IsT0;

    private DateOnly AsDateOnly => _oneOf.AsT0;

    private bool IsDateTimeOffset => _oneOf.IsT1;

    private DateTimeOffset AsDateTimeOffset => _oneOf.AsT1;

    private bool IsTimeOnly => _oneOf.IsT2;

    private TimeOnly AsTimeOnly => _oneOf.AsT2;

    private bool IsString => _oneOf.IsT3;

    private string AsString => _oneOf.AsT3;

}
