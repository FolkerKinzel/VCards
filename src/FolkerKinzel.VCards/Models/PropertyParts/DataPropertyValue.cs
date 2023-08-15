using System.Collections.ObjectModel;
using System.ComponentModel;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class DataPropertyValue : IOneOf
{
    private readonly OneOf<ReadOnlyCollection<byte>, Uri, string> _oneOf;

    public DataPropertyValue(OneOf<ReadOnlyCollection<byte>, Uri, string> oneOf) => _oneOf = oneOf;

    public static implicit operator DataPropertyValue(ReadOnlyCollection<byte> _) => new DataPropertyValue(_);
    public static explicit operator ReadOnlyCollection<byte>(DataPropertyValue _) => _.AsReadOnlyByteCollection;

    public static implicit operator DataPropertyValue(Uri _) => new DataPropertyValue(_);
    public static explicit operator Uri(DataPropertyValue _) => _.AsUri;

    public static implicit operator DataPropertyValue(string _) => new DataPropertyValue(_);
    public static explicit operator string(DataPropertyValue _) => _.AsString;

    public bool IsReadOnlyByteCollection => _oneOf.IsT0;

    public ReadOnlyCollection<byte> AsReadOnlyByteCollection => _oneOf.AsT0;

    public bool IsUri => _oneOf.IsT1;

    public Uri AsUri => _oneOf.AsT1;

    public bool IsString => _oneOf.IsT2;

    public string AsString => _oneOf.AsT2;

    public ReadOnlyCollection<byte>? ReadOnlyByteCollection => IsReadOnlyByteCollection ? AsReadOnlyByteCollection : null;

    public Uri? Uri => IsUri ? AsUri : null;

    public string? String => IsString ? AsString : null;

    public bool TryPickReadOnlyByteCollection(out ReadOnlyCollection<byte> value, out OneOf<Uri, string> remainder) => _oneOf.TryPickT0(out value, out remainder);

    public bool TryPickUri(out Uri value, out OneOf<ReadOnlyCollection<byte>, string> remainder) => _oneOf.TryPickT1(out value, out remainder);

    public bool TryPickString(out string value, out OneOf<ReadOnlyCollection<byte>, Uri> remainder) => _oneOf.TryPickT2(out value, out remainder);

    public void Switch(Action<ReadOnlyCollection<byte>> f0, Action<Uri> f1, Action<string> f2) => _oneOf.Switch(f0, f1, f2);

    public TResult Match<TResult>(Func<ReadOnlyCollection<byte>, TResult> f0, Func<Uri, TResult> f1, Func<string, TResult> f2) => _oneOf.Match(f0, f1, f2);

    public OneOf<TResult, Uri, string> MapReadOnlyByteCollection<TResult>(Func<ReadOnlyCollection<byte>, TResult> mapFunc) => _oneOf.MapT0(mapFunc);

    public OneOf<ReadOnlyCollection<byte>, TResult, string> MapUri<TResult>(Func<Uri, TResult> mapFunc) => _oneOf.MapT1(mapFunc);

    public OneOf<ReadOnlyCollection<byte>, Uri, TResult> MapString<TResult>(Func<string, TResult> mapFunc) => _oneOf.MapT2(mapFunc);

    public object Value => this._oneOf.Value;

    public int Index => this._oneOf.Index;


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
    //public new ReadOnlyCollection<byte> AsT0 => base.AsT0;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new Uri AsT1 => base.AsT1;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new string AsT2 => base.AsT2;

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool TryPickT0(out ReadOnlyCollection<byte> value, out OneOf<Uri, string> remainder) => base.TryPickT0(out value, out remainder);


    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool TryPickT1(out Uri value, out OneOf<ReadOnlyCollection<byte>, string> remainder) => base.TryPickT1(out value, out remainder);


    //[EditorBrowsable(EditorBrowsableState.Never)]
    //[Browsable(false)]
    //public new bool TryPickT2(out string value, out OneOf<ReadOnlyCollection<byte>, Uri> remainder) => base.TryPickT2(out value, out remainder);





}
