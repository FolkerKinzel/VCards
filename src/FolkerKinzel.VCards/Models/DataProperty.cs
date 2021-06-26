using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt eingebettete Binärdaten, Verweise auf Binärdaten (z.B. Urls) oder als freier Text vorliegende Information.
    /// </summary>
    /// <remarks>
    /// <para>Die Informationen könnnen in verschiedenen Formaten vorliegen:</para>
    /// <list type="bullet">
    /// <item>eingebettete Binärdaten</item>
    /// <item>Verweise auf Binärdaten (z.B. Urls)</item>
    /// <item>als freier Text vorliegende Information</item>
    /// </list>
    /// <para>Verwenden Sie die <see cref="DataUrl"/>-Klasse, um
    /// einzubettende Binärdaten oder freien Text zu übergeben oder diese Informationen aus der Property
    /// wieder auszulesen.</para>
    /// </remarks>
    public sealed class DataProperty : VCardProperty, IEnumerable<DataProperty>
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="DataProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="Uri"/>: Verwenden Sie die statischen Methoden der <see cref="DataUrl"/>-Klasse,
        /// um einzubettende Binärdaten oder freien Text als <see cref="Uri"/>zu übergeben.</param>
        /// <param name="propertyGroup">Bezeichner der Gruppe,
        /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
        /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
        public DataProperty(Uri? value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
        {
            Value = value;
            Parameters.DataType = VCdDataType.Uri;
        }

        internal DataProperty(VcfRow vcfRow, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.UnMask(version);

            if (vcfRow.Value is null)
            {
                return;
            }

            switch (version)
            {
                case VCdVersion.V2_1:
                    {
                        if (Parameters.MediaType is null) // die KEY- und Sound-Property von vCard 2.1 enthält per Standard freien Text
                        {
                            Parameters.DataType = VCdDataType.Text;

                            try
                            {
                                Value = DataUrl.FromText(vcfRow.Value);
                            }
                            catch { }
                            return;
                        }
                        else
                        {
                            string value = vcfRow.Value;

                            // vCard 2.1 ermöglicht noch andere Kodierungsarten als BASE64
                            // wandle diese in BASE64 um: (vCard 2.1 kennt keinen DataUrl)
                            if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
                            {
                                byte[] bytes = QuotedPrintableConverter.DecodeData(value);
                                value = Convert.ToBase64String(bytes);
                                Parameters.Encoding = VCdEncoding.Base64;
                            }

                            // Outlook verwendet falsches Line-Wrapping, weshalb Leerzeichen im Base64 und im Uri enthalten sein können
                            Value = Parameters.Encoding == VCdEncoding.Base64 ? BuildDataUri(Parameters.MediaType, value.Replace(" ", "")) : BuildUri(value.Replace(" ", ""));
                            return;
                        }
                    }
                case VCdVersion.V3_0:
                    {
                        if (Parameters.Encoding == VCdEncoding.Base64)
                        {
                            Value = BuildDataUri(Parameters.MediaType, vcfRow.Value);
                            return;
                        }

                        if (Parameters.DataType == VCdDataType.Text)
                        {
                            try
                            {
                                Value = DataUrl.FromText(vcfRow.Value);
                            }
                            catch { }
                            return;
                        }

                        Value = BuildUri(vcfRow.Value);
                        return;
                    }
                default:
                    {
                        // vCard 4.0:

                        if (Parameters.DataType == VCdDataType.Text)
                        {
                            try
                            {
                                Value = DataUrl.FromText(vcfRow.Value);
                            }
                            catch { }
                        }

                        Value = DataUrl.TryCreate(vcfRow.Value, out DataUrl? dataUri) ? dataUri : BuildUri(vcfRow.Value);

                        return;
                    }
            }


            // ================================

            static DataUrl? BuildDataUri(string? mediaType, string value)
            {
                var mType = new MimeType(mediaType);

                try
                {
                    return new DataUrl($"data:{mType};base64,{value}", mType, DataEncoding.Base64);
                }
                catch
                {
                    return null;
                }
            }

            static Uri? BuildUri(string value)
            {
                if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri? uri))
                {
                    return uri;
                }
                else
                {
                    try
                    {
                        return DataUrl.FromText(value);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Die von der <see cref="DataProperty"/> zur Verfügung gestellten Daten.
        /// </summary>
        public new Uri? Value
        {
            get;
        }


        /// <inheritdoc/>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected override object? GetVCardPropertyValue() => Value;


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            base.PrepareForVcfSerialization(serializer);


            switch (serializer.Version)
            {
                case VCdVersion.V2_1:
                    Prepare2_1();
                    break;
                case VCdVersion.V3_0:
                    Prepare3_0();
                    break;
                //case VCdVersion.V4_0:
                //    break;
                default:
                    Prepare4_0();
                    break;
            }


            void Prepare2_1()
            {
                switch (Value)
                {
                    case null:
                        Parameters.ContentLocation = VCdContentLocation.Inline;
                        break;
                    case DataUrl dataUri:
                        {
                            if (dataUri.ContainsBytes)
                            {
                                Parameters.ContentLocation = VCdContentLocation.Inline;
                                Parameters.Encoding = VCdEncoding.Base64;
                                Parameters.MediaType = dataUri.MimeType.ToString();

                            }
                            else // enthält Text
                            {
                                if (Parameters.ContentLocation != VCdContentLocation.ContentID)
                                {
                                    Parameters.ContentLocation = VCdContentLocation.Inline;
                                }

                                if (dataUri.GetEmbeddedText().NeedsToBeQpEncoded())
                                {
                                    Parameters.Encoding = VCdEncoding.QuotedPrintable;
                                    Parameters.Charset = VCard.DEFAULT_CHARSET;
                                }
                            }

                            break;
                        }
                    case Uri uri:
                        {
                            if (uri.IsAbsoluteUri && (uri.Scheme?.StartsWith("cid", StringComparison.Ordinal) ?? false))
                            {
                                Parameters.ContentLocation = VCdContentLocation.ContentID;
                            }
                            else if (Parameters.ContentLocation != VCdContentLocation.ContentID)
                            {
                                Parameters.ContentLocation = VCdContentLocation.Url;
                            }

                            if (uri.ToString().NeedsToBeQpEncoded())
                            {
                                Parameters.Encoding = VCdEncoding.QuotedPrintable;
                                Parameters.Charset = VCard.DEFAULT_CHARSET;
                            }
                            break;
                        }
                }
            }

            void Prepare3_0()
            {
                switch (Value)
                {
                    case null:
                        Parameters.DataType = VCdDataType.Binary;
                        Parameters.Encoding = VCdEncoding.Base64;
                        break;
                    case DataUrl dataUri:
                        {
                            if (dataUri.ContainsBytes)
                            {
                                Parameters.DataType = VCdDataType.Binary;
                                Parameters.Encoding = VCdEncoding.Base64;
                                Parameters.MediaType = dataUri.MimeType.ToString();
                            }
                            else // enthält Text
                            {
                                Parameters.DataType = VCdDataType.Text;
                            }

                            break;
                        }
                    case Uri _:

                        {
                            Parameters.DataType = VCdDataType.Uri;
                            break;
                        }
                }
            }

            void Prepare4_0()
            {
                switch (Value)
                {
                    case null:
                        Parameters.DataType = null;
                        break;
                    case DataUrl dataUri:
                        {
                            if (dataUri.ContainsText)
                            {
                                Parameters.DataType = VCdDataType.Text;
                            }
                            else
                            {
                                Parameters.DataType = null;
                                Parameters.MediaType = null;
                            }

                            break;
                        }
                    case Uri _:
                        {
                            Parameters.DataType = VCdDataType.Uri;
                            break;
                        }
                }
            }
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            if (Value is null)
            {
                return;
            }

            StringBuilder builder = serializer.Builder;
            StringBuilder worker = serializer.Worker;

            _ = worker.Clear();

            switch (serializer.Version)
            {
                case VCdVersion.V2_1:
                    if (Value is DataUrl dataUrl)
                    {
                        if (dataUrl.ContainsText)
                        {
                            _ = this.Parameters.Encoding == VCdEncoding.QuotedPrintable
                                ? builder.Append(QuotedPrintableConverter.Encode(dataUrl.GetEmbeddedText(), builder.Length))
                                : builder.Append(dataUrl.GetEmbeddedText());
                        }
                        else // binary
                        {
                            ((Vcf_2_1Serializer)serializer).AppendBase64Data(dataUrl.EncodedData);
                        }
                    }
                    else // Value is Uri
                    {
                        _ = this.Parameters.Encoding == VCdEncoding.QuotedPrintable
                            ? builder.Append(QuotedPrintableConverter.Encode(Value.ToString(), builder.Length))
                            : builder.Append(Value);
                    }
                    break;
                case VCdVersion.V3_0:
                    VCdDataType? dataType = Parameters.DataType;

                    if (dataType == VCdDataType.Binary)
                    {
                        Debug.Assert(Value is DataUrl);

                        _ = builder.Append(((DataUrl)Value).EncodedData);
                        return;
                    }
                    else if (dataType == VCdDataType.Text)
                    {
                        Debug.Assert(Value is DataUrl);

                        _ = worker.Append(((DataUrl)Value).GetEmbeddedText()).Mask(serializer.Version);
                        _ = builder.Append(worker);
                    }
                    else
                    {
                        _ = builder.Append(Value);
                        return;
                    }
                    break;

                default: // VCdVersion.V4_0

                    if (Parameters.DataType == VCdDataType.Text)
                    {
                        Debug.Assert(Value is DataUrl);

                        _ = worker.Append(((DataUrl)Value).GetEmbeddedText()).Mask(serializer.Version);
                    }
                    else
                    {
                        _ = worker.Append(Value).Mask(serializer.Version);
                    }
                    _ = builder.Append(worker);
                    break;
            }
        }

        IEnumerator<DataProperty> IEnumerable<DataProperty>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<DataProperty>)this).GetEnumerator();
    }
}
