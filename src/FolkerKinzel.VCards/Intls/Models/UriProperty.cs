using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Models;

internal sealed class UriProperty : VCardProperty
{
    private UriProperty(UriProperty prop)
        : base(prop) => Value = prop.Value;

    internal UriProperty(Uri value, ParameterSection parameters, string? group)
        : base(parameters, group)
    {
        Debug.Assert(value.IsAbsoluteUri);

        Value = value;
        Parameters.DataType = Data.Uri;
    }

    public new Uri Value { get; }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new UriProperty(this);

    /// <inheritdoc />
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        if (serializer.Version == VCdVersion.V2_1)
        {
            if (UriConverter.IsContentId(Value))
            {
                Parameters.ContentLocation = Loc.Cid;
            }
            else if (Parameters.ContentLocation != Loc.Cid)
            {
                Parameters.ContentLocation = Loc.Url;
            }
        }
        else
        {
            Parameters.DataType = Data.Uri;
        }
    }
    
    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        _ = serializer.Builder.Append(Value.AbsoluteUri);
    }
}
