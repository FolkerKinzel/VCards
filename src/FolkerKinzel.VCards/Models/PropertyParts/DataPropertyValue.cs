using System.Collections.ObjectModel;
using System.ComponentModel;
using OneOf;

namespace FolkerKinzel.VCards.Models.PropertyParts;

[GenerateOneOf]
public sealed partial class DataPropertyValue : OneOfBase<ReadOnlyCollection<byte>, Uri, string>
{
    public bool IsReadOnlyByteCollection => IsT0;

    public ReadOnlyCollection<byte> AsReadOnlyByteCollection => AsT0;

    public bool IsUri => IsT1;

    public Uri AsUri => AsT1;

    public bool IsString => IsT2;

    public string AsString => AsT2;

    public ReadOnlyCollection<byte>? ReadOnlyByteCollection => IsReadOnlyByteCollection ? AsReadOnlyByteCollection : null;

    public Uri? Uri => IsUri ? AsUri : null;

    public string? String => IsString ? AsString : null;

    public bool TryPickReadOnlyByteCollection(out ReadOnlyCollection<byte> value, out OneOf<Uri, string> remainder) => base.TryPickT0(out value, out remainder);

    public bool TryPickUri(out Uri value, out OneOf<ReadOnlyCollection<byte>, string> remainder) => base.TryPickT1(out value, out remainder);

    public bool TryPickString(out string value, out OneOf<ReadOnlyCollection<byte>, Uri> remainder) => base.TryPickT2(out value, out remainder);


    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new bool IsT0 => base.IsT0;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new bool IsT1 => base.IsT1;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new bool IsT2 => base.IsT2;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new ReadOnlyCollection<byte> AsT0 => base.AsT0;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new Uri AsT1 => base.AsT1;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new string AsT2 => base.AsT2;

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
