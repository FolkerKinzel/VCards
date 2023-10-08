using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class DataPropertyValue
{
    private readonly OneOf<ReadOnlyCollection<byte>, Uri, string> _oneOf;

    internal DataPropertyValue(OneOf<ReadOnlyCollection<byte>, Uri, string> oneOf) => _oneOf = oneOf;

    public ReadOnlyCollection<byte>? Bytes => IsBytes ? AsBytes : null;

    public Uri? Uri => IsUri ? AsUri : null;

    public string? String => IsString ? AsString : null;

    public object Value => this._oneOf.Value;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Switch(Action<ReadOnlyCollection<byte>>? f0,
                       Action<Uri>? f1,
                       Action<string>? f2)
        => _oneOf.Switch(f0, f1, f2);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<ReadOnlyCollection<byte>, TResult>? f0,
                                  Func<Uri, TResult>? f1,
                                  Func<string, TResult>? f2)
        => _oneOf.Match(f0, f1, f2);

    public override string ToString()
     => Match
        (
         bytes => $"{bytes.Count} Bytes",
         uri => uri.ToString(),
         str => str
        );

    private bool IsBytes => _oneOf.IsT0;

    private ReadOnlyCollection<byte> AsBytes => _oneOf.AsT0;

    private bool IsUri => _oneOf.IsT1;

    private Uri AsUri => _oneOf.AsT1;

    private bool IsString => _oneOf.IsT2;

    private string AsString => _oneOf.AsT2;


    //public static implicit operator DataPropertyValue(ReadOnlyCollection<byte> _) => new DataPropertyValue(_);
    //public static explicit operator ReadOnlyCollection<byte>(DataPropertyValue _) => _.AsBytes;

    //public static implicit operator DataPropertyValue(Uri _) => new DataPropertyValue(_);
    //public static explicit operator Uri(DataPropertyValue _) => _.AsUri;

    //public static implicit operator DataPropertyValue(string _) => new DataPropertyValue(_);
    //public static explicit operator string(DataPropertyValue _) => _.AsString;


    //public bool TryGetBytes([NotNullWhen(true)] out ReadOnlyCollection<byte>? value)
    //{

    //    if (IsBytes)
    //    {
    //        value = AsBytes;
    //        return true;
    //    }
    //    else
    //    {
    //        value = default;
    //        return false;
    //    }
    //}

    //public bool TryGetUri([NotNullWhen(true)] out Uri? value)
    //{
    //    if (IsUri)
    //    {
    //        value = AsUri;
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
    //public bool TryPickReadOnlyByteCollection([NotNullWhen(true)] out ReadOnlyCollection<byte>? value,
    //                                          out OneOf<Uri, string> remainder)
    //    => _oneOf.TryPickT0(out value, out remainder);


    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickUri([NotNullWhen(true)] out Uri? value,
    //                       out OneOf<ReadOnlyCollection<byte>, string> remainder)
    //    => _oneOf.TryPickT1(out value, out remainder);


    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public bool TryPickString([NotNullWhen(true)] out string? value,
    //                          out OneOf<ReadOnlyCollection<byte>, Uri> remainder)
    //    => _oneOf.TryPickT2(out value, out remainder);



    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<TResult, Uri, string> MapReadOnlyByteCollection<TResult>(
    //    Func<ReadOnlyCollection<byte>, TResult> mapFunc)
    //    => _oneOf.MapT0(mapFunc);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<ReadOnlyCollection<byte>, TResult, string> MapUri<TResult>(Func<Uri, TResult> mapFunc)
    //    => _oneOf.MapT1(mapFunc);

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public OneOf<ReadOnlyCollection<byte>, Uri, TResult> MapString<TResult>(Func<string, TResult> mapFunc)
    //    => _oneOf.MapT2(mapFunc);


    //public int Index => this._oneOf.Index;


}

