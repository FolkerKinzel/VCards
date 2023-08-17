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
    private DataPropertyValue? _value;
    private bool _isValueInitialized;

    /// <summary>
    /// Kopierkonstruktor
    /// </summary>
    /// <param name="prop">Das zu klonende <see cref="DataProperty"/> Objekt.</param>
    protected DataProperty(DataProperty prop) : base(prop) { }


    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mimeType"></param>
    /// <param name="parameterSection"></param>
    /// <param name="propertyGroup"></param>
    /// <remarks>Must be internal, because <see cref="ParameterSection.MediaType"/> has strong
    /// restrictions.</remarks>
    internal DataProperty(string? mimeType,
                           ParameterSection parameterSection,
                           string? propertyGroup)
        : base(parameterSection, propertyGroup) => Parameters.MediaType = mimeType;


    public new DataPropertyValue? Value
    {
        get
        {
            if (!_isValueInitialized)
            {
                InitializeValue();
            }

            return _value;
        }
    }

    public abstract string GetFileTypeExtension();


    internal static DataProperty Create(VcfRow vcfRow, VCdVersion version)
    {
        if (DataUrl.TryParse(vcfRow.Value, out DataUrlInfo info))
        {
            return info.TryGetEmbeddedData(out OneOf<string, byte[]> data)
                    ? data.Match<DataProperty>
                       (
                        s => new EmbeddedTextProperty(vcfRow, version),
                        b => new EmbeddedBytesProperty(b,
                                                       MimeType.TryParse(info.MimeType,
                                                                         out MimeType? mimeType) ? mimeType.ToString()
                                                                                                 : MimeString.OctetStream,
                                                       vcfRow.Group,
                                                       vcfRow.Parameters)
                        )
                    : new EmbeddedTextProperty(vcfRow, version);
        }

        if(vcfRow.Parameters.DataType == VCdDataType.Text)
        {
            return new EmbeddedTextProperty(vcfRow, version);
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
            if (Uri.TryCreate(vcfRow.Value.Trim(), UriKind.Absolute, out Uri? uri))
            {
                return new ReferencedDataProperty(uri,
                                                  vcfRow.Parameters.MediaType,
                                                  vcfRow.Group,
                                                  vcfRow.Parameters);
            }
        } // Quoted-Printable encoded binary data:
        else if (vcfRow.Parameters.Encoding == ValueEncoding.QuotedPrintable &&
               vcfRow.Parameters.MediaType != null)
        {
            return new EmbeddedBytesProperty(string.IsNullOrWhiteSpace(vcfRow.Value) ? null : QuotedPrintable.DecodeData(vcfRow.Value),
                                             vcfRow.Parameters.MediaType,
                                             vcfRow.Group,
                                             vcfRow.Parameters);
        }

        // Text:
        return new EmbeddedTextProperty(vcfRow, version);
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
                                  propertyGroup);

    public static DataProperty FromText(string? text,
                                        string? mimeTypeString = null,
                                        string? propertyGroup = null) =>
        MimeType.TryParse(mimeTypeString, out MimeType? mimeType) ? FromText(text, mimeType, propertyGroup)
                                                                  : FromText(text, (MimeType?)null, propertyGroup);

    public static DataProperty FromText(string? text,
                                         MimeType? mimeType,
                                         string? propertyGroup = null)
    {
        var textProp = new TextProperty(text, propertyGroup);
        textProp.Parameters.MediaType = mimeType?.ToString();
        textProp.Parameters.DataType = VCdDataType.Text;
        return new EmbeddedTextProperty(textProp);
    }

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

    private void InitializeValue()
    {
        _isValueInitialized = true;
        _value = GetVCardPropertyValue() switch
        {
            ReadOnlyCollection<byte> bt => bt,
            string s => s,
            Uri uri => uri,
            _ => null
        };
    }

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
