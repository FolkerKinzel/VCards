using System.Collections.ObjectModel;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.DataUrls;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class EmbeddedBytesProperty : DataProperty
{
    /// <summary>Copy ctor</summary>
    /// <param name="prop">The <see cref="EmbeddedBytesProperty" /> object to clone.</param>
    private EmbeddedBytesProperty(EmbeddedBytesProperty prop)
        : base(prop) => Value = prop.Value;

    internal EmbeddedBytesProperty(byte[]? arr,
                                   string? group,
                                   ParameterSection parameters)
        : base(parameters, group)
    {
        if (arr != null && arr.Length != 0)
        {
            Value = arr;
        }
    }

    public new byte[]? Value { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => Value is null;


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
            serializer.AppendBase64EncodedData(Value);
        }
        else
        {
            serializer.Builder.Append(DataUrl.FromBytes(Value, Parameters.MediaType));
        }
    }
}
