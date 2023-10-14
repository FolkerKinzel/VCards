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
    /// <param name="parameters"></param>
    /// <param name="propertyGroup"></param>
    internal DataProperty(ParameterSection parameters, string? propertyGroup)
        : base(parameters, propertyGroup) { }


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


    /// <inheritdoc/>
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    public abstract string GetFileTypeExtension();


    internal static DataProperty Parse(VcfRow vcfRow, VCdVersion version)
    {
        if (DataUrl.TryParse(vcfRow.Value, out DataUrlInfo info))
        {
            vcfRow.Parameters.MediaType =
                MimeTypeInfo.TryParse(info.MimeType, out MimeTypeInfo mimeTypeInfo)
                                  ? mimeTypeInfo.ToString()
                                  : MimeString.OctetStream;

            return info.TryGetEmbeddedData(out OneOf<string, byte[]> data)
                    ? data.Match<DataProperty>
                       (
                        s => new EmbeddedTextProperty(new TextProperty(vcfRow, version)),
                        b => new EmbeddedBytesProperty(b, vcfRow.Group, vcfRow.Parameters)
                        )
                    : FromText(vcfRow.Value, info.MimeType.ToString(), vcfRow.Group);
        }

        if (vcfRow.Parameters.Encoding == ValueEncoding.Base64)
        {
            return new EmbeddedBytesProperty
                       (
                        Base64Helper.GetBytesOrNull(vcfRow.Value),
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
                               new UriProperty(uri, vcfRow.Parameters, vcfRow.Group)
                               )
                       : new EmbeddedTextProperty(new TextProperty(vcfRow, version));
        }
    }


    public static DataProperty FromFile(string filePath,
                                        string? mimeType = null,
                                        string? propertyGroup = null)
       => mimeType is null
            ? new EmbeddedBytesProperty(LoadFile(filePath), propertyGroup, new ParameterSection() { MediaType = MimeString.FromFileName(filePath) })
            : MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
               ? new EmbeddedBytesProperty(LoadFile(filePath), propertyGroup, new ParameterSection() { MediaType = mimeInfo.ToString() })
               : FromFile(filePath, null, propertyGroup);


    public static DataProperty FromBytes(IEnumerable<byte>? bytes,
                                         string? mimeType = MimeString.OctetStream,
                                         string? propertyGroup = null)
        => new EmbeddedBytesProperty
           (
            bytes?.ToArray(),
            propertyGroup,
            new ParameterSection()
            {
                MediaType = MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                             ? mimeInfo.ToString()
                             : MimeString.OctetStream
            }
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
        => uri is null ? FromBytes(null, mimeType, propertyGroup)
                       : new ReferencedDataProperty
           (
             new UriProperty(uri.IsAbsoluteUri
                               ? uri
                               : throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)), nameof(uri)),
                 new ParameterSection() { MediaType = MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo) ? mimeInfo.ToString() : null },
                 propertyGroup)
           );


    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();


    private void InitializeValue()
    {
        _isValueInitialized = true;
        _value = this switch
        {
            EmbeddedBytesProperty bt => bt.IsEmpty ? null : new DataPropertyValue(bt.Value),
            EmbeddedTextProperty text => text.IsEmpty ? null : new DataPropertyValue(text.Value),
            ReferencedDataProperty uri => new DataPropertyValue(uri.Value),
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
