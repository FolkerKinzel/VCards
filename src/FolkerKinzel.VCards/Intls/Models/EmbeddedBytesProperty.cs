using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Models;
using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class EmbeddedBytesProperty : DataProperty
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    private EmbeddedBytesProperty(EmbeddedBytesProperty prop) : base(prop) => Value = prop.Value;


    internal EmbeddedBytesProperty(IEnumerable<byte>? value,
                                   string? mimeType,
                                   string? propertyGroup,
                                   ParameterSection parameterSection) :
        base(mimeType, parameterSection, propertyGroup) => Value = value is null ? null : new ReadOnlyCollection<byte>(value.ToArray());


    public new ReadOnlyCollection<byte>? Value { get; }


    public override string GetFileTypeExtension() => MimeString.ToFileTypeExtension(Parameters.MediaType);


    /// <inheritdoc/>
    public override object Clone() => new EmbeddedBytesProperty(this);

    protected override object? GetVCardPropertyValue() => Value;

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
            serializer.Builder.Append(DataUrl.FromBytes(Value?.ToArray(), Parameters.MediaType));
        }
    }

    public override string ToString() => Value != null ? $"{Value.Count} Bytes" : base.ToString();

}
