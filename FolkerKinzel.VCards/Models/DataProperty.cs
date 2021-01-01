using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;

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
    public sealed class DataProperty : VCardProperty<Uri?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="DataProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="Uri"/>. Verwenden Sie die statischen Methoden der <see cref="DataUrl"/>-Klasse,
        /// um einzubettende Binärdaten oder freien Text als <see cref="Uri"/>zu übergeben.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public DataProperty(Uri? value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
            Parameters.DataType = VCdDataType.Uri;
        }

        internal DataProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            try
            {
                Value = DataUrl.FromVcfRow(vcfRow, info, version);
                Parameters.MediaType = vcfRow.Parameters.MediaType;
            }
            catch { } // Value = null : not readable
        }


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

            System.Text.StringBuilder? builder = serializer.Builder;
            System.Text.StringBuilder? worker = serializer.Worker;

            worker.Clear();

            switch (serializer.Version)
            {
                case VCdVersion.V2_1:
                    if (Value is DataUrl dataUrl)
                    {
                        if (dataUrl.ContainsText)
                        {
                            if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
                            {
                                builder.Append(QuotedPrintableConverter.Encode(dataUrl.GetEmbeddedText(), builder.Length));
                            }
                            else
                            {
                                builder.Append(dataUrl.GetEmbeddedText());
                            }
                        }
                        else // binary
                        {
                            builder.Append(dataUrl.EncodedData);
                            ((Vcf_2_1Serializer)serializer).WrapBase64Data();
                        }
                    }
                    else // Value is Uri
                    {
                        if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
                        {
                            builder.Append(QuotedPrintableConverter.Encode(Value.ToString(), builder.Length));
                        }
                        else
                        {
                            builder.Append(Value);
                        }
                    }
                    break;
                case VCdVersion.V3_0:
                    VCdDataType? dataType = Parameters.DataType;

                    if (dataType == VCdDataType.Binary)
                    {
                        Debug.Assert(Value as DataUrl != null);

                        builder.Append(((DataUrl)Value).EncodedData);
                        return;
                    }
                    else if (dataType == VCdDataType.Text)
                    {
                        Debug.Assert(Value as DataUrl != null);

                        worker.Append(((DataUrl)Value).GetEmbeddedText()).Mask(serializer.Version);
                        builder.Append(worker);
                    }
                    else
                    {
                        builder.Append(Value);
                        return;
                    }
                    break;

                default: // VCdVersion.V4_0

                    if (Parameters.DataType == VCdDataType.Text)
                    {
                        Debug.Assert(Value as DataUrl != null);

                        worker.Append(((DataUrl)Value).GetEmbeddedText()).Mask(serializer.Version);
                    }
                    else
                    {
                        worker.Append(Value).Mask(serializer.Version);
                    }
                    builder.Append(worker);
                    break;
            }
        }
    }
}
