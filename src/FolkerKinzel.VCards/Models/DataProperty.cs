using System.Collections;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Models.Enums;
using OneOf;
using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Models;
using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Models;


public abstract class DataProperty : VCardProperty, IEnumerable<DataProperty>
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="DataProperty"/> object to clone.</param>
    protected DataProperty(DataProperty prop) : base(prop) { }


    protected DataProperty(string? mimeType,
                           ParameterSection parameterSection,
                           string? propertyGroup)
        : base(parameterSection, propertyGroup) => Parameters.MediaType = mimeType;


    public new DataPropertyValue? Value
    {
        get => base.Value switch
        {
            ReadOnlyCollection<byte> bt => bt,
            string s => s,
            Uri uri => uri,
            _ => null
        };
    }

    public abstract string GetFileTypeExtension();

    internal static DataProperty Create(VcfRow vcfRow, VCdVersion version)
    {
        if (DataUrl.TryParse(vcfRow.Value, out DataUrlInfo info))
        {
            return info.TryGetEmbeddedData(out OneOf<string, byte[]> data)
                    ? data.Match<DataProperty>(
                        s => new EmbeddedTextProperty(vcfRow, version),
                        b => new EmbeddedBytesProperty(b,
                                                       MimeType.TryParse(info.MimeType,
                                                                         out MimeType? mimeType) ? mimeType.ToString()
                                                                                                 : MimeString.OctetStream,
                                                       vcfRow.Group,
                                                       vcfRow.Parameters))
                    : new EmbeddedTextProperty(vcfRow, version);
        }

        if (vcfRow.Parameters.Encoding == ValueEncoding.Base64)
        {
            return new EmbeddedBytesProperty(Base64.Decode(vcfRow.Value),
                                             vcfRow.Parameters.MediaType,
                                             vcfRow.Group,
                                             vcfRow.Parameters);
        }

        if (vcfRow.Parameters.DataType == VCdDataType.Uri)
        {
            vcfRow.UnMask(version);
            return new ReferencedDataProperty(UriConverter.ToAbsoluteUri(vcfRow.Value),
                                              vcfRow.Parameters.MediaType,
                                              vcfRow.Group,
                                              vcfRow.Parameters);
        }

        // Quoted-Printable encoded binary data:
        return vcfRow.Parameters.Encoding == ValueEncoding.QuotedPrintable &&
               vcfRow.Parameters.MediaType != null &&
               vcfRow.Parameters.DataType != VCdDataType.Text
               ? new EmbeddedBytesProperty(vcfRow.Value is null ? null : QuotedPrintable.DecodeData(vcfRow.Value),
                                           vcfRow.Parameters.MediaType,
                                           vcfRow.Group,
                                           vcfRow.Parameters)
               : new EmbeddedTextProperty(vcfRow, version);
    }


    public static DataProperty FromFile(string filePath,
                                        string? mimeTypeString = null,
                                        string? propertyGroup = null) =>
        mimeTypeString is null ? FromFile(filePath, MimeType.Parse(MimeString.FromFileName(filePath)), propertyGroup)
                               : MimeType.TryParse(mimeTypeString, out MimeType? mimeType)
                                   ? FromFile(filePath, mimeType, propertyGroup)
                                   : FromFile(filePath, (string?)null, propertyGroup);

    public static DataProperty FromFile(string filePath,
                                        MimeType mimeType,
                                        string? propertyGroup = null) =>
        FromBytes(LoadFile(filePath), mimeType, propertyGroup);


    public static DataProperty FromBytes(IEnumerable<byte>? bytes,
                                         string? mimeTypeString = MimeString.OctetStream,
                                         string? propertyGroup = null) =>
        MimeType.TryParse(mimeTypeString, out MimeType? mimeType) ? FromBytes(bytes, mimeType, propertyGroup)
                                                                  : FromBytes(bytes, MimeString.OctetStream, propertyGroup);

    public static DataProperty FromBytes(IEnumerable<byte>? bytes,
                                         MimeType mimeType,
                                         string? propertyGroup = null) =>
        new EmbeddedBytesProperty(bytes,
                                  mimeType?.ToString() ?? throw new ArgumentNullException(nameof(mimeType)),
                                  propertyGroup,
                                  new ParameterSection() { Encoding = ValueEncoding.Base64 });


    public static DataProperty FromText(string? text, string? propertyGroup = null) =>
        new EmbeddedTextProperty(new TextProperty(text, propertyGroup));

    public static DataProperty FromUri(Uri? uri,
                                       string? mimeTypeString = null,
                                       string? propertyGroup = null) =>
        FromUri(uri,
                MimeType.TryParse(mimeTypeString, out MimeType? mimeType) ? mimeType : null,
                propertyGroup);

    public static DataProperty FromUri(Uri? uri,
                                      MimeType? mimeType,
                                      string? propertyGroup = null) =>
        new ReferencedDataProperty(uri, mimeType?.ToString(), propertyGroup, new ParameterSection());


    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();


    [ExcludeFromCodeCoverage]
    private static byte[] LoadFile(string filePath)
    {
        try
        {
            return File.ReadAllBytes(filePath);
        }
        catch (ArgumentNullException)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (NotSupportedException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (System.Security.SecurityException e)
        {
            throw new IOException(e.Message, e);
        }
        catch (PathTooLongException e)
        {
            throw new ArgumentException(e.Message, nameof(filePath), e);
        }
        catch (Exception e)
        {
            throw new IOException(e.Message, e);
        }
    }


}
