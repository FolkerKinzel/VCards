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

}

