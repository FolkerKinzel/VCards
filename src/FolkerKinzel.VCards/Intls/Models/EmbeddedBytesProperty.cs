using FolkerKinzel.DataUrls;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
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
        if (arr is not null && arr.Length != 0)
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
        Debug.Assert(serializer is not null);

        base.PrepareForVcfSerialization(serializer);

        Parameters.ContentLocation = Loc.Inline;
        Parameters.DataType = Data.Binary;
        Parameters.Encoding = Enc.Base64;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        serializer.AppendBase64EncodedData(Value);
    }
}
