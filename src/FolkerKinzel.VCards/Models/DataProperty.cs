using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

public sealed class ReferencedDataProperty : DataProperty, IEnumerable<ReferencedDataProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private ReferencedDataProperty(ReferencedDataProperty prop) : base(prop) => Value = prop.Value;

    public ReferencedDataProperty(Uri? value, string mimeType = "application/octet-stream", string? propertyGroup = null)
        : base(mimeType, propertyGroup)
    {
        Value = value;
        Parameters.DataType = VCdDataType.Uri;
    }

    public new Uri? Value { get; }

    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override object Clone() => new ReferencedDataProperty(this);

    public IEnumerator<ReferencedDataProperty> GetEnumerator()
    {
        yield return this;
    }
}

//public abstract class EmbeddedDataProperty : DataProperty
//{


//    /// <summary>
//    /// Copy ctor
//    /// </summary>
//    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
//    protected EmbeddedDataProperty(DataProperty prop) : base(prop) { }

//    protected EmbeddedDataProperty(object? value, string? mimeType, string? propertyGroup) : base(mimeType, propertyGroup)
//    {
//        if(!FitsSizeRestriction(value))
//        {
//            throw new ArgumentException(Res.ContentTooLarge, nameof(value));
//        }
//    }

//    private bool FitsSizeRestriction(object? value)
//    {
//        const int OVERHEAD = 64;
//        var limit = VCard.EmbeddedContentSizeLimit.SizeLimit;

//        return limit == SizeRestriction.None ||
//            (limit != SizeRestriction.Discard && 
//             value switch
//                {
//                    byte[] bytes => bytes.Length * 1.3 + OVERHEAD < (int)limit,
//                    string s => s.Length < (int)limit,
//                    _ => true
//                });
//    }
//}



public sealed class EmbeddedBytesProperty : DataProperty, IEnumerable<EmbeddedBytesProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    private EmbeddedBytesProperty(EmbeddedBytesProperty prop) : base(prop) => Value = prop.Value;

    public EmbeddedBytesProperty(byte[]? value, string mimeType = "application/octet-stream", string? propertyGroup = null) : base(mimeType, propertyGroup)
    {
        Value = value;
        Parameters.Encoding = ValueEncoding.Base64;
    }

    protected override bool ValidateMimeType(ref string? mimeType)
    {
        return mimeType is null
            ? throw new ArgumentNullException(nameof(mimeType))
            : string.IsNullOrWhiteSpace(mimeType) || !base.ValidateMimeType(ref mimeType)
                ? throw new ArgumentException(Res.InvalidMimeType, nameof(mimeType))
                : true;
    }

    public new byte[]? Value { get; }


    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override object Clone() => new EmbeddedBytesProperty(this);

    public IEnumerator<EmbeddedBytesProperty> GetEnumerator()
    { 
        yield return this;
    }
}

public sealed class EmbeddedTextProperty : DataProperty, IEnumerable<EmbeddedTextProperty>
{
    private readonly TextProperty _textProp;

    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private EmbeddedTextProperty(EmbeddedTextProperty prop) : base(prop)
    {
        Value = prop.Value;
        _textProp = new TextProperty(Value);
    }

    public EmbeddedTextProperty(string? value, string? propertyGroup = null) : base(null, propertyGroup)
    {
        Value = value;
        Parameters.DataType = VCdDataType.Text;
        _textProp = new TextProperty(value);
    }

    public new string? Value { get; }

    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);
        _textProp.PrepareForVcfSerialization(serializer);

        this.Parameters.Encoding = _textProp.Parameters.Encoding;
        this.Parameters.CharSet = _textProp.Parameters.CharSet;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        _textProp.AppendValue(serializer);
    }

    public override object Clone() => new EmbeddedTextProperty(this);

    public IEnumerator<EmbeddedTextProperty> GetEnumerator() { yield return this; }
}


public abstract class DataProperty : VCardProperty, IEnumerable<DataProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    protected DataProperty(DataProperty prop) : base(prop) { }


    protected DataProperty(string? mimeType, string? propertyGroup) : base(new ParameterSection(), propertyGroup)
    {
        if (ValidateMimeType(ref mimeType))
        {
            Parameters.MediaType = mimeType;
        }
    }

    protected virtual bool ValidateMimeType(ref string? mimeString)
    {
        if (string.IsNullOrWhiteSpace(mimeString))
        {
            return true;
        }

        if (MimeType.TryParse(mimeString, out MimeType? mimeType))
        {
            mimeString = mimeType.ToString();
            return true;
        }

        return false;
    }

    
    internal static DataProperty Create(VcfRow vcfRow, VCdVersion version) => throw new NotImplementedException();


    public static EmbeddedBytesProperty FromFile(string filePath) => throw new NotImplementedException();

    internal static EmbeddedBytesProperty FromBytes(byte[] bytes, string mimeType) => throw new NotImplementedException();

    internal static EmbeddedTextProperty FromText(string aSCIITEXT) => throw new NotImplementedException();

    internal static ReferencedDataProperty FromUri(Uri uri) => throw new NotImplementedException();


    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();


}
