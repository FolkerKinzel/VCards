using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class ReferencedDataProperty : DataProperty
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private ReferencedDataProperty(ReferencedDataProperty prop) : base(prop) => Value = prop.Value;

    internal ReferencedDataProperty(Uri? value, string? mimeType, string? propertyGroup, ParameterSection parameterSection)
        : base(mimeType, parameterSection, propertyGroup)
    {
        if (value != null)
        {
            if (!value.IsAbsoluteUri)
            {
                throw new ArgumentException(string.Format(Res.RelativeUri, nameof(value)), nameof(value));
            }

            Value = value;
            Parameters.DataType = VCdDataType.Uri;
        }
    }

    public new Uri? Value { get; }

    public override string GetFileTypeExtension()
    {
        string? mime = Parameters.MediaType;

        return mime != null ? MimeString.ToFileTypeExtension(mime)
                            : UriConverter.GetFileTypeExtensionFromUri(Value);
    }

    /// <inheritdoc/>
    public override object Clone() => new ReferencedDataProperty(this);

    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        if (serializer.Version == VCdVersion.V2_1)
        {
            if (Value is null)
            {
                Parameters.ContentLocation = ContentLocation.Inline;
            }
            else if (UriConverter.IsContentId(Value))
            {
                Parameters.ContentLocation = ContentLocation.ContentID;
            }
            else if (Parameters.ContentLocation != ContentLocation.ContentID)
            {
                Parameters.ContentLocation = ContentLocation.Url;
            }
        }
        else
        {
            if (Value != null) { Parameters.DataType = VCdDataType.Uri; }
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        if (Value is null)
        {
            return;
        }

        Debug.Assert(Value.IsAbsoluteUri);
        _ = serializer.Builder.Append(Value.AbsoluteUri);
    }

    public override string ToString() => Value?.AbsoluteUri ?? base.ToString();
}
