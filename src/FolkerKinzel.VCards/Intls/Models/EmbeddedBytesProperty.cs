using System.Collections.ObjectModel;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class EmbeddedBytesProperty : DataProperty
{
    private readonly byte[]? _bytes;
    private ReadOnlyCollection<byte>? _value;

    /// <summary />
    /// <param name="prop">The <see cref="DataProperty" /> object to clone.</param>
    private EmbeddedBytesProperty(EmbeddedBytesProperty prop) : base(prop)
    {
        _bytes = prop._bytes;
        _value = prop._value;
    }


    internal EmbeddedBytesProperty(byte[]? arr,
                                   string? propertyGroup,
                                   ParameterSection parameterSection)
        : base(parameterSection, propertyGroup)
    {
        if (arr != null && arr.Length != 0)
        {
            _bytes = arr;
        }
    }

    public new ReadOnlyCollection<byte>? Value
    {
        get
        {
            if (_value == null && _bytes != null)
            {
                _value = new ReadOnlyCollection<byte>(_bytes);
            }

            return _value;
        }
    }


    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => _bytes is null;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string GetFileTypeExtension() => MimeString.ToFileTypeExtension(Parameters.MediaType);


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new EmbeddedBytesProperty(this);


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.ContentLocation = ContentLocation.Inline;
        Parameters.DataType = VCdDataType.Binary;
        Parameters.Encoding = ValueEncoding.Base64;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        if (serializer.Version < VCdVersion.V4_0)
        {
            serializer.AppendBase64EncodedData(_bytes);
        }
        else
        {
            serializer.Builder.Append(DataUrl.FromBytes(_bytes, Parameters.MediaType));
        }
    }
}
