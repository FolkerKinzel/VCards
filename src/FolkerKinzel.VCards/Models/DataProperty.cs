using System.Collections;
using System.Collections.ObjectModel;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.Uris;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using OneOf;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates the information of vCard properties that provides
/// external data.</summary>
/// <seealso cref="DataPropertyValue"/>
/// <seealso cref="VCard.Photos"/>
/// <seealso cref="VCard.Logos"/>
/// <seealso cref="VCard.Sounds"/>
/// <seealso cref="VCard.Keys"/>
public abstract class DataProperty : VCardProperty, IEnumerable<DataProperty>
{
    private DataPropertyValue? _value;
    private bool _isValueInitialized;

    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The<see cref="DataProperty" /> object to clone.</param>
    protected DataProperty(DataProperty prop) : base(prop) { }

    /// <summary>ctor</summary>
    /// <param name="parameters" />
    /// <param name="group" />
    internal DataProperty(ParameterSection parameters, string? group)
        : base(parameters, group) { }

    /// <summary> The data provided by the <see cref="DataProperty" />.</summary>
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

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;

    /// <summary>
    /// Gets an appropriate file type extension for <see cref="Value"/>.
    /// </summary>
    /// <returns>An appropriate file type extension for <see cref="Value"/>.
    /// </returns>
    /// <remarks>
    /// <para>If value is a <see cref="Uri"/>, the file type extension is for
    /// the data the <see cref="Uri"/> references.</para>
    /// </remarks>
    public abstract string GetFileTypeExtension();

    /// <summary>
    /// Creates a new <see cref="DataProperty"/> instance that embeds the binary content 
    /// of a file in a vCard.
    /// </summary>
    /// <param name="filePath">Path to the file whose content is to embed.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the file content
    /// or <c>null</c> to get the <paramref name="mimeType"/> automatically from the
    /// file type extension.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DataProperty"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
    public static DataProperty FromFile(string filePath,
                                        string? mimeType = null,
                                        string? group = null)
       => mimeType is null
            ? new EmbeddedBytesProperty(LoadFile(filePath),
                                        group,
                                        new ParameterSection() 
                                        { 
                                            MediaType = MimeString.FromFileName(filePath)
                                        })
            : MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
               ? new EmbeddedBytesProperty(LoadFile(filePath),
                                           group,
                                           new ParameterSection() { MediaType = mimeInfo.ToString() })
               : FromFile(filePath, null, group);

    /// <summary>
    /// Creates a new <see cref="DataProperty"/> instance that embeds an array of 
    /// <see cref="byte"/>s in a VCF file.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to embed or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the <paramref name="bytes"/>
    /// or <c>null</c> for <c>application/octet-stream</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DataProperty"/> instance.</returns>
    public static DataProperty FromBytes(byte[]? bytes,
                                         string? mimeType = MimeString.OctetStream,
                                         string? group = null)
        => new EmbeddedBytesProperty
           (
             bytes,
             group,
             new ParameterSection()
             {
                 MediaType = MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                              ? mimeInfo.ToString()
                              : MimeString.OctetStream
             }
           );

    /// <summary>
    /// Creates a new <see cref="DataProperty"/> instance that embeds text in a vCard.
    /// </summary>
    /// <param name="passWord">The text to embed or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the <paramref name="passWord"/>
    /// or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DataProperty"/> instance.</returns>
    /// <remarks>
    /// The vCard standard only allows to write a password as plain text to the <c>KEY</c> property.
    /// <see cref="VCard.Keys">(See VCard.Keys.)</see>
    /// </remarks>
    /// <seealso cref="VCard.Keys"/>
    public static DataProperty FromText(string? passWord,
                                        string? mimeType = null,
                                        string? group = null)
    {
        var textProp = new TextProperty(passWord, group);
        textProp.Parameters.MediaType =
            MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                           ? mimeInfo.ToString()
                           : null;
        textProp.Parameters.DataType = Data.Text;
        return new EmbeddedTextProperty(textProp);
    }

    /// <summary>
    /// Creates a new <see cref="DataProperty"/> instance from an absolute <see cref="Uri"/> 
    /// that references external data.
    /// </summary>
    /// <param name="uri">An absolute <see cref="Uri"/> or <c>null</c>.</param>
    /// <param name="mimeType">The Internet Media Type ("MIME type") of the 
    /// data the <paramref name="uri"/> points to, or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <returns>The newly created <see cref="DataProperty"/> instance.</returns>
    /// <exception cref="ArgumentException"><paramref name="uri"/> is neither <c>null</c> nor
    /// an absolute <see cref="Uri"/>.</exception>
    public static DataProperty FromUri(Uri? uri,
                                       string? mimeType = null,
                                       string? group = null)
        => uri is null ? FromBytes(null, mimeType, group)
                       : new ReferencedDataProperty
           (
             new UriProperty(uri.IsAbsoluteUri
                               ? uri
                               : throw new ArgumentException(string.Format(Res.RelativeUri, nameof(uri)),
                                                             nameof(uri)),
                             new ParameterSection()
                             {
                                 MediaType = MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo mimeInfo)
                                                                  ? mimeInfo.ToString() 
                                                                  : null 
                             },
                             group)
           );

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value?.ToString() ?? base.ToString();

    /// <inheritdoc />
    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

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

        if (vcfRow.Parameters.Encoding == Enc.Base64)
        {
            return new EmbeddedBytesProperty
                       (
                        Base64Helper.GetBytesOrNull(vcfRow.Value),
                        vcfRow.Group,
                        vcfRow.Parameters
                        );
        }

        if (vcfRow.Parameters.DataType == Data.Uri ||
            vcfRow.Parameters.DataType == Data.Text)
        {
            return TryAsUri(vcfRow, version);
        }

        // Quoted-Printable encoded binary data:
        if (vcfRow.Parameters.Encoding == Enc.QuotedPrintable &&
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
            return UriConverter.TryConvertToAbsoluteUri(vcfRow.Value, out Uri? uri)
                       ? new ReferencedDataProperty
                              (
                               new UriProperty(uri, vcfRow.Parameters, vcfRow.Group)
                               )
                       : new EmbeddedTextProperty(new TextProperty(vcfRow, version));
        }
    }


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

    /// <summary>
    /// Loads the file referenced by <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is not a valid file path.</exception>
    /// <exception cref="IOException">The file could not be loaded.</exception>
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
