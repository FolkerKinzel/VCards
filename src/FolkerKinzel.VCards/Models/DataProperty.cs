using System.Collections;
using System.Collections.ObjectModel;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using OneOf;

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
                        b => new EmbeddedBytesProperty
                             (
                              b,
                              MimeTypeInfo.TryParse(info.MimeType, out MimeTypeInfo mimeTypeInfo)
                                  ? mimeTypeInfo.ToString()
                                  : MimeString.OctetStream,
                              vcfRow.Group,
                              vcfRow.Parameters
                              )
                        )
                    : FromText(vcfRow.Value, info.MimeType.ToString(), vcfRow.Group);
        }

        if (vcfRow.Parameters.Encoding == ValueEncoding.Base64)
        {
            return new EmbeddedBytesProperty
                       (
                        Base64Helper.GetBytesOrNull(vcfRow.Value),
                        vcfRow.Parameters.MediaType,
                        vcfRow.Group,
                        vcfRow.Parameters
                        );
        }

        if (vcfRow.Parameters.DataType == VCdDataType.Uri ||
            vcfRow.Parameters.DataType == VCdDataType.Text)
        {
            return TryAsUri(vcfRow, version);
        }

        // Quoted-Printable encoded binary data:
        if (vcfRow.Parameters.Encoding == ValueEncoding.QuotedPrintable &&
            vcfRow.Parameters.MediaType != null)
        {
            return new EmbeddedBytesProperty
                   (
                     string.IsNullOrWhiteSpace(vcfRow.Value)
                            ? null
                            : QuotedPrintable.DecodeData(vcfRow.Value),
                     vcfRow.Parameters.MediaType,
                     vcfRow.Group,
                     vcfRow.Parameters
                     );
        }

        // Missing data type:
        return TryAsUri(vcfRow, version);

        ///////////////////////////////////////////////////////////////

        static DataProperty TryAsUri(VcfRow vcfRow, VCdVersion version)
        {
            vcfRow.UnMask(version);
            return Uri.TryCreate(vcfRow.Value.Trim(), UriKind.Absolute, out Uri? uri)
                       ? new ReferencedDataProperty
                              (
                               uri,
                               vcfRow.Parameters.MediaType,
                               vcfRow.Group,
                               vcfRow.Parameters
                               )
                       : new EmbeddedTextProperty(vcfRow, version);
        }
    }


    public static DataProperty FromFile(string filePath,
                                        string? mimeType = null,
                                        string? propertyGroup = null)
       => mimeType is null
            ? new EmbeddedBytesProperty(LoadFile(filePath), MimeString.FromFileName(filePath), propertyGroup)
            : MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
               ? new EmbeddedBytesProperty(LoadFile(filePath), mimeInfo.ToString(), propertyGroup)
               : FromFile(filePath, null, propertyGroup);


    public static DataProperty FromBytes(IEnumerable<byte>? bytes,
                                         string? mimeType = MimeString.OctetStream,
                                         string? propertyGroup = null)
        => new EmbeddedBytesProperty
           (
            bytes,
            MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                 ? mimeInfo.ToString()
                 : MimeString.OctetStream,
            propertyGroup
            );


    public static DataProperty FromText(string? text,
                                        string? mimeType = null,
                                        string? propertyGroup = null)
    {
        var textProp = new TextProperty(text, propertyGroup);
        textProp.Parameters.MediaType =
            MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                           ? mimeInfo.ToString()
                           : null;
        textProp.Parameters.DataType = VCdDataType.Text;
        return new EmbeddedTextProperty(textProp);
    }

    public static DataProperty FromUri(Uri? uri,
                                       string? mimeType = null,
                                       string? propertyGroup = null)
        => new ReferencedDataProperty
           (
             (uri is not null) && !uri.IsAbsoluteUri
                 ? throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)), nameof(uri))
                 : uri,
             MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo) ? mimeInfo.ToString() : null,
             propertyGroup,
             new ParameterSection()
            );


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
            ReadOnlyCollection<byte> bt => new DataPropertyValue(bt),
            string s => new DataPropertyValue(s),
            Uri uri => new DataPropertyValue(uri),
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
