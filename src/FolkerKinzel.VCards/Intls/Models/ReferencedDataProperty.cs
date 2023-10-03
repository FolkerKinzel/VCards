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
            Debug.Assert(value.IsAbsoluteUri);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new ReferencedDataProperty(this);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        if (Value is null)
        {
            Parameters.ContentLocation = ContentLocation.Inline;
        }
        else if (serializer.Version == VCdVersion.V2_1)
        {
            if (UriConverter.IsContentId(Value))
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
            Parameters.DataType = VCdDataType.Uri;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.AbsoluteUri ?? base.ToString();
}
