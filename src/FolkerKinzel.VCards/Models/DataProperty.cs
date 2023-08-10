using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

public sealed class ReferencedDataProperty : DataProperty, IEnumerable<ReferencedDataProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private ReferencedDataProperty(ReferencedDataProperty prop) : base(prop)
        => Value = prop.Value;

    public ReferencedDataProperty(Uri? value, string mimeType = "application/octet-stream", string? propertyGroup = null) 
        : base(mimeType, propertyGroup) => Value = value;

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

public abstract class EmbeddedDataProperty : DataProperty
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    protected EmbeddedDataProperty(DataProperty prop) : base(prop) { }

    protected EmbeddedDataProperty(string mimeType, string? propertyGroup) : base(mimeType, propertyGroup)
    {
    }
}

public sealed class EmbeddedBytesProperty : EmbeddedDataProperty, IEnumerable<EmbeddedBytesProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    private EmbeddedBytesProperty(EmbeddedBytesProperty prop) : base(prop) => Value = prop.Value;

    public EmbeddedBytesProperty(byte[]? value, string mimeType = "application/octet-stream", string? propertyGroup = null) : base(mimeType, propertyGroup)
    {
        Value = value;
    }

    public new byte[]? Value { get; }


    protected override object? GetVCardPropertyValue() => Value;
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override object Clone() => new EmbeddedBytesProperty(this);

    public IEnumerator<EmbeddedBytesProperty> GetEnumerator()
    { yield return this; }
}

public sealed class EmbeddedTextProperty : EmbeddedDataProperty, IEnumerable<EmbeddedTextProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop"></param>
    private EmbeddedTextProperty(EmbeddedTextProperty prop) : base(prop)
        => Value = prop.Value;

    public EmbeddedTextProperty(string? value, string mimeType = "text/plain", string? propertyGroup = null) : base(mimeType, propertyGroup)
    {
        Value = value;
    }

    public new string? Value { get; }

    public override object Clone() => new EmbeddedTextProperty (this);
    protected override object? GetVCardPropertyValue() => Value;
    internal override void AppendValue(VcfSerializer serializer) => throw new NotImplementedException();
    public IEnumerator<EmbeddedTextProperty> GetEnumerator() { yield return this; }
}


public abstract class DataProperty : VCardProperty, IEnumerable<DataProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    protected DataProperty(DataProperty prop) : base(prop) { }


    protected DataProperty(string mimeType, string? propertyGroup) : base(new ParameterSection(), propertyGroup)
    {
        throw new NotImplementedException();
    }

    public static EmbeddedDataProperty FromFile(string photoFilePath) => throw new NotImplementedException();
    internal static DataProperty Create(VcfRow vcfRow, VCdVersion version) => throw new NotImplementedException();
    internal static EmbeddedBytesProperty FromBytes(byte[] bytes, string v) => throw new NotImplementedException();
    internal static EmbeddedTextProperty FromText(string aSCIITEXT) => throw new NotImplementedException();
    internal static ReferencedDataProperty FromUri(Uri uri) => throw new NotImplementedException();

    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();


}
