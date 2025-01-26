using System.Collections;
using System.ComponentModel;
using FolkerKinzel.DataUrls;
using FolkerKinzel.MimeTypes;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates the information of vCard properties that provides
/// external data.</summary>
/// <seealso cref="RawData"/>
/// <seealso cref="VCard.Photos"/>
/// <seealso cref="VCard.Logos"/>
/// <seealso cref="VCard.Sounds"/>
/// <seealso cref="VCard.Keys"/>
public sealed class DataProperty : VCardProperty, IEnumerable<DataProperty>
{
    #region Remove with 8.0.1

    [Obsolete("Use RawData.GetFileTypeExtension instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string GetFileTypeExtension() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use RawData.FromFile instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DataProperty FromFile(string filePath,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                        string? mimeType = null,
                                        string? group = null)
               => throw new NotImplementedException();


    [Obsolete("Use RawData.FromBytes instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DataProperty FromBytes(byte[]? bytes,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                         string? mimeType = MimeString.OctetStream,
                                         string? group = null)
                => throw new NotImplementedException();


    [Obsolete("Use RawData.FromText instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DataProperty FromText(string? passWord,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                        string? mimeType = null,
                                        string? group = null)
            => throw new NotImplementedException();


    [Obsolete("Use RawData.FromUri instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static DataProperty FromUri(Uri? uri,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
                                       string? mimeType = null,
                                       string? group = null)
        => throw new NotImplementedException();

    #endregion

    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The<see cref="DataProperty" /> object to clone.</param>
    private DataProperty(DataProperty prop) : base(prop) => Value = prop.Value;

    /// <summary>
    /// Initializes a new <see cref="DataProperty"/> instance with a specified
    /// <see cref="RawData"/> object.
    /// </summary>
    /// <param name="value">The <see cref="RawData"/> instance to use as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public DataProperty(RawData value, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Parameters.MediaType = value.MediaType;
    }

    internal DataProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (DataUrl.TryParse(vcfRow.Value, out DataUrlInfo dataUrlInfo))
        {
            Value = DataUrlConverter.ToRawData(ref dataUrlInfo);
            Parameters.MediaType = Value.MediaType;
            return;
        }

        Enc? encoding = vcfRow.Parameters.Encoding;

        if (encoding == Enc.Base64)
        {
            Value = RawData.FromBytes(Base64Helper.GetBytesOrNull(vcfRow.Value.Span) ?? []);
            return;
        }

        Data? dataType = vcfRow.Parameters.DataType;

        if (dataType == Data.Uri)
        {
            Value = TryAsUri(vcfRow, version);
        }
        else if (dataType == Data.Text)
        {
            Value = RawData.FromText(StringDeserializer.Deserialize(vcfRow, version), 
                                     vcfRow.Parameters.MediaType);
        }
        // Quoted-Printable encoded binary data:
        else if (encoding == Enc.QuotedPrintable && Parameters.MediaType is not null)
        {
            ReadOnlySpan<char> valueSpan = vcfRow.Value.Span;

            Value = valueSpan.IsWhiteSpace()
                ? RawData.FromBytes([])
                : RawData.FromBytes(QuotedPrintable.DecodeData(valueSpan),
                                    Parameters.MediaType);
        }
        else // missing data type
        {
            Value = TryAsUri(vcfRow, version);
        }

        ///////////////////////////////////////////////////////////////

        static RawData TryAsUri(VcfRow vcfRow, VCdVersion version)
        {
            string val = StringDeserializer.Deserialize(vcfRow, version);

            return UriConverter.TryConvertToAbsoluteUri(val, out Uri? uri)
                       ? RawData.FromUri(uri, vcfRow.Parameters.MediaType)
                       : RawData.FromText(val, vcfRow.Parameters.MediaType);
        }
    }

    /// <summary> The data provided by the <see cref="DataProperty" />.</summary>
    public new RawData Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Value.ToString();

    /// <inheritdoc />
    IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    /// <inheritdoc/>
    public override object Clone() => new DataProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Value.Switch(
            (Parameters, serializer),
            static (bytes, tuple) => PrepareForBytes(tuple.Parameters),
            VCardPropertyPreparer.PrepareForUri,
            VCardPropertyPreparer.PrepareForText
            );

        static void PrepareForBytes(ParameterSection parameters)
        {
            parameters.ContentLocation = Loc.Inline;
            parameters.DataType = Data.Binary;
            parameters.Encoding = Enc.Base64;
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        // Empty properties must be written because vCard 2.1 needs an empty line after
        // base64 even if the data is empty
        Value.Switch(
            (serializer, Parameters),
            static (bytes, tuple) => tuple.serializer.AppendBase64EncodedData(bytes),
            static (uri, tuple) => _ = tuple.serializer.Builder.Append(uri.AbsoluteUri),
            static (text, tuple) => StringSerializer.AppendVcf(tuple.serializer.Builder, text, tuple.Parameters, tuple.serializer.Version)
            );
    }
}
