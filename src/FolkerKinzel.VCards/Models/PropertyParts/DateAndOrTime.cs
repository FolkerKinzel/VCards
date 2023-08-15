using System.Collections.ObjectModel;
using System.ComponentModel;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class DateAndOrTime : IOneOf
{
    private readonly OneOf<DateOnly, DateTimeOffset, TimeOnly, string> _oneOf;

    public DateAndOrTime(OneOf<DateOnly, DateTimeOffset, TimeOnly, string> oneOf) => _oneOf = oneOf;

    public bool IsDateOnly => _oneOf.IsT0;

    public DateOnly AsDateOnly => _oneOf.AsT0;

    public bool IsDateTimeOffset => _oneOf.IsT1;

    public DateTimeOffset AsDateTimeOffset => _oneOf.AsT1;

    public bool IsTimeOnly => _oneOf.IsT2;

    public TimeOnly AsTimeOnly => _oneOf.AsT2;

    public bool IsString => _oneOf.IsT3;

    public string AsString => _oneOf.AsT3;

    public DateOnly? DateOnly => IsDateOnly ? AsDateOnly : null;

    public DateTimeOffset? DateTimeOffset => IsDateTimeOffset ? AsDateTimeOffset : null;

    public TimeOnly? TimeOnly => IsTimeOnly ? AsTimeOnly : null;

    public string? String => IsString ? AsString : null;

    public bool TryPickDateOnly(out DateOnly value, out OneOf<DateTimeOffset, TimeOnly, string> remainder) => _oneOf.TryPickT0(out value, out remainder);

    public bool TryPickDateTimeOffset(out DateTimeOffset value, out OneOf<DateOnly, TimeOnly, string> remainder) => _oneOf.TryPickT1(out value, out remainder);


    public bool TryPickTimeOnly(out TimeOnly value, out OneOf<DateOnly, DateTimeOffset, string> remainder) => _oneOf.TryPickT2(out value, out remainder);

    public bool TryPickString(out string value, out OneOf<DateOnly, DateTimeOffset, TimeOnly> remainder) => _oneOf.TryPickT3(out value, out remainder);

    public void Switch(Action<DateOnly> f0, Action<DateTimeOffset> f1, Action<TimeOnly> f2, Action<string> f3) => _oneOf.Switch(f0, f1, f2, f3);

    public TResult Match<TResult>(Func<DateOnly, TResult> f0, Func<DateTimeOffset, TResult> f1, Func<TimeOnly, TResult> f2, Func<string, TResult> f3) => _oneOf.Match(f0, f1, f2, f3);

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool IsT0 => base.IsT0;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool IsT1 => base.IsT1;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool IsT2 => base.IsT2;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool IsT3 => base.IsT2;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new DateOnly AsT0 => base.AsT0;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new DateTimeOffset AsT1 => base.AsT1;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new TimeOnly AsT2 => base.AsT2;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new string AsT3 => base.AsT3;

    public object Value => this._oneOf.Value;

    public int Index => this._oneOf.Index;
}
