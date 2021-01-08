using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert einen Data-URL nach RFC 2397.
    /// </summary>
    [Serializable]
    public class DataUrl : Uri, ISerializable
    {
        /// <summary>
        /// Gibt an, dass der URI ein Data-Url nach RFC 2397 ist. Dieses Feld ist schreibgeschützt.
        /// </summary>
        public const string UriSchemeData = "data";

        /// <summary>
        /// Gibt das Zeichen an, das das Schema des Kommunikationsprotokolls vom Adressteil des URIs trennt. Dieses Feld ist schreibgeschützt.
        /// </summary>
        public static readonly new string SchemeDelimiter = ":";


        /// <summary>
        /// Initialisiert ein neues <see cref="DataUrl"/>-Objekt.
        /// </summary>
        /// <param name="uriString">Der String, aus dem die <see cref="DataUrl"/> initialisiert wird.</param>
        /// <param name="mimeType">Der MIME-Typ der eingebetteten Daten. Wenn <c>null</c> übergeben wird, 
        /// wird der Standard-MIME-Typ "text/plain;charset=US-ASCII" erzeugt.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uriString"/> ist <c>null</c>.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil <paramref name="uriString"/> länger als 65519 Zeichen ist.</exception>
        private DataUrl(string uriString, MimeType? mimeType) : base(uriString)
        {
            this.MimeType = mimeType ?? new MimeType();
        }


        ///// <summary>
        ///// Füllt eine <see cref="SerializationInfo"/> mit den Daten auf, die zum Serialisieren des Zielobjekts erforderlich sind.
        ///// Die Methode wird während der Serialisierung des Objekts aufgerufen.
        ///// </summary>
        ///// <param name="info">Die mit Daten zu füllende <see cref="SerializationInfo"/>.</param>
        ///// <param name="context">Das Ziel dieser Serialisierung.</param>
        /// <inheritdoc/>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                // Use the AddValue method to specify serialized values.
                info?.AddValue("MimeType", this.MimeType, typeof(MimeType));
                info?.AddValue("Encoding", this.Encoding, typeof(DataEncoding));
            }
        }


        ///// <summary>
        ///// Konstruktor, der während der Deserialisierung des Objekts aufgerufen wird.
        ///// </summary>
        ///// <param name="serializationInfo">Die zu deserialisierenden Daten.</param>
        ///// <param name="streamingContext">Kontext der Deserialisierung.</param>
        /// <inheritdoc/>
        protected DataUrl(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            if (serializationInfo != null)
            {
                // Reset the property value using the GetValue method.
                this.MimeType = (MimeType)(serializationInfo.GetValue("MimeType", typeof(MimeType)) ?? new MimeType());
                this.Encoding = (DataEncoding)(serializationInfo.GetValue("Encoding", typeof(DataEncoding)) ?? DataEncoding.UrlEncoded);
            }
            else
            {
                this.MimeType = new MimeType();
            }
        }


        /// <summary>
        /// Initialisiert ein <see cref="Uri"/>-Objekt aus dem in einer <see cref="VcfRow"/> gespeicherten Text.
        /// Wenn der Text dies hergibt, handelt es sich um ein <see cref="DataUrl"/>-Objekt.
        /// </summary>
        /// <param name="vcfRow">Die zu parsende <see cref="VcfRow"/>.</param>
        /// <param name="info">Ein <see cref="VCardDeserializationInfo"/>-Objekt.</param>
        /// <param name="version">Version der VCF-Datei.</param>
        /// <returns>Ein <see cref="Uri"/>- oder <see cref="DataUrl"/>-Objekt.</returns>
        /// <exception cref="ArgumentNullException">Der in <paramref name="vcfRow"/> gespeicherte <see cref="string"/> ist <c>null</c>.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der in <paramref name="vcfRow"/> gespeicherte <see cref="string"/> länger als 65519 Zeichen ist.</exception>
        internal static Uri? FromVcfRow(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
        {
            Debug.Assert(vcfRow != null);

            if (string.IsNullOrWhiteSpace(vcfRow.Value))
            {
                return null;
            }

            string value = vcfRow.Value;

            switch (version)
            {
                case VCdVersion.V2_1:
                    {
                        if (vcfRow.Parameters.MediaType is null) // die KEY- und Sound-Property von vCard 2.1 enthält per Standard freien Text
                        {
                            vcfRow.DecodeQuotedPrintable();
                            vcfRow.Parameters.DataType = VCdDataType.Text;
                            //vcfRow.UnMask(info);
                            return DataUrl.FromText(value);
                        }
                        else
                        {
                            vcfRow.DecodeQuotedPrintableData();

                            if (vcfRow.Parameters.Encoding == VCdEncoding.Base64)
                            {
                                return BuildDataUri();
                            }

                            return BuildUri();
                        }
                    }
                case VCdVersion.V3_0:
                    {
                        if (vcfRow.Parameters.Encoding == VCdEncoding.Base64)
                        {
                            return BuildDataUri();
                        }
                        else if (vcfRow.Parameters.DataType == VCdDataType.Text)
                        {
                            vcfRow.UnMask(info, version);
                            return FromText(value);
                        }
                        else
                        {
                            return BuildUri();
                        }
                    }
                //case VCdVersion.V4_0:
                //    break;
                default:
                    break;
            }


            // ================================
            // vCard 4.0:

            vcfRow.UnMaskAndTrim(info, version);


            if (DataUrl.TryCreate(value, out DataUrl? dataUri)) // DataUri vCard 4.0
            {
                return dataUri;
            }
            else if (vcfRow.Parameters.DataType == VCdDataType.Text) // freier Text
            {
                return DataUrl.FromText(value);
            }
            else
            {
                return BuildUri();
            }


            DataUrl BuildDataUri()
            {
                var mType = new MimeType(vcfRow.Parameters.MediaType);
                return new DataUrl($"data:{mType};base64,{vcfRow.Value}", mType)
                {
                    Encoding = DataEncoding.Base64
                };
            }


            Uri BuildUri()
            {
                if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    return uri;
                }
                else
                {
                    return DataUrl.FromText(value);
                }
            }


        }


        /// <summary>
        /// Erstellt einen neuen <see cref="DataUrl"/>. Löst keine Ausnahme aus, wenn der <see cref="DataUrl"/> nicht erstellt werden kann.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/>, der dem Data-URL-Schema nach RFC 2397 entspricht.</param>
        /// <param name="dataUrl">Enthält nach dem Beenden der Methode einen aus <paramref name="value"/> erstellten <see cref="DataUrl"/>.
        /// Dieser Parameter wird nicht initialisiert übergeben.</param>
        /// <returns>Ein <see cref="bool"/>-Wert, der <c>true</c> ist, wenn der <see cref="DataUrl"/> erfolgreich erstellt wurde, andernfalls <c>false</c>.</returns>
        public static bool TryCreate(string? value, [NotNullWhen(true)] out DataUrl? dataUrl)
        {
            dataUrl = null;

            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            const int DATA_PROTOCOL_LENGTH = 5; // 5 == LengthOf "data:"
            const int BASE64_LENGTH = 7; // 7 = LengthOf ";base64"

            int endIndex = -1;
            DataEncoding enc = DataEncoding.UrlEncoded;


            for (int i = DATA_PROTOCOL_LENGTH; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsWhiteSpace(c))
                {
                    return false;
                }

                if (c == ',')
                {
                    endIndex = i;
                    break;
                }
            }

            if (endIndex == -1)
            {
                return false;
            }


            // dies ändert ggf. auch endIndex
            if (HasBase64Encoding(value))
            {
                enc = DataEncoding.Base64;
            }


            var mime = new MimeType(value.Substring(DATA_PROTOCOL_LENGTH, endIndex - DATA_PROTOCOL_LENGTH));


            try
            {
                dataUrl = new DataUrl(value, mime)
                {
                    Encoding = enc,
                };
            }
            catch
            {
                return false;
            }


            return true;


            bool HasBase64Encoding(string val)
            {
                //Suche ";base64"
                int start = endIndex - 1;
                int end = endIndex - BASE64_LENGTH;

                if (end > DATA_PROTOCOL_LENGTH)
                {
                    int index = BASE64_LENGTH - 1;

                    for (int i = start; i >= end; i--, index--)
                    {
                        char c = char.ToLowerInvariant(val[i]);

                        if (c != ";base64"[index])
                        {
                            return false;
                        }
                    }

                    endIndex = end;
                    return true;
                }

                return false;
            }


        }


        /// <summary>
        /// Erzeugt einen <see cref="DataUrl"/>, in den beliebiger Text eingebettet ist.
        /// </summary>
        /// <param name="text">Der in den <see cref="DataUrl"/> einzubettende Text.</param>
        /// <returns>Ein <see cref="DataUrl"/>, in den <paramref name="text"/> eingebettet ist.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> ist ein Leerstring oder
        /// enthält nur Whitespace.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der URI-String länger als 65519 Zeichen ist.</exception>
        public static DataUrl FromText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(Res.NoData, nameof(text));
            }

            var dataUri = new DataUrl($"data:,{Uri.EscapeDataString(text)}", null);


            return dataUri;
        }


        /// <summary>
        /// Erzeugt einen <see cref="DataUrl"/>, in den binäre Daten eingebettet sind.
        /// </summary>
        /// <param name="bytes">Die in den <see cref="DataUrl"/> einzubettenden Daten.</param>
        /// <param name="mimeType">Der MIME-Typ der in <paramref name="bytes"/> enthaltenen
        /// Daten oder <c>null</c> für "text/plain;charset=US-ASCII".</param>
        /// <returns>Ein <see cref="DataUrl"/>, in den die in <paramref name="bytes"/> enthaltenen 
        /// binären Daten eingebettet sind.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="bytes"/> ist ein leeres Array.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der URI-String länger als 65519 Zeichen ist.</exception>
        public static DataUrl FromBytes(byte[] bytes, string? mimeType)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentException(Res.NoData, nameof(bytes));
            }

            var mType = new MimeType(mimeType);
            var dataUri =
                new DataUrl($"data:{mType};base64,{Convert.ToBase64String(bytes)}", mType)
                {
                    Encoding = DataEncoding.Base64
                };

            return dataUri;
        }


        /// <summary>
        /// Erstellt einen <see cref="DataUrl"/> aus einer physisch vorhandenen Datei.
        /// </summary>
        /// <param name="path">Absoluter Pfad zu der einzubettenden Datei.</param>
        /// <param name="mimeType">MIME-Typ der einzubettenden Datei. Wenn <c>null</c> angegeben wird,
        /// wird versucht, den MIME-Typ aus der Dateiendung automatisch zu ermitteln.</param>
        /// <returns>Ein <see cref="DataUrl"/>, in den die Daten der mit <paramref name="path"/> referenzierten Datei
        /// eingebettet sind.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> oder <paramref name="mimeType"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> ist kein gültiger Dateipfad oder
        /// <paramref name="mimeType"/> hat kein gültiges Format.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der URI-String länger als 65519 Zeichen ist.</exception>
        /// <exception cref="IOException">E/A-Fehler.</exception>
        public static DataUrl FromFile(string path, string? mimeType = null)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);

                mimeType ??= MimeTypeConverter.GetMimeTypeFromFileExtension(path);

                return DataUrl.FromBytes(bytes, mimeType);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(path));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message, nameof(path), e);
            }
            catch (UriFormatException)
            {
                throw;
            }
            catch (UnauthorizedAccessException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (NotSupportedException e)
            {
                throw new ArgumentException(e.Message, nameof(path), e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (PathTooLongException e)
            {
                throw new ArgumentException(e.Message, nameof(path), e);
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
        }


        /// <summary>
        /// MIME-Type der eingebetteten Daten. (nie <c>null</c>)
        /// </summary>
        public MimeType MimeType { get; }

        /// <summary>
        /// Encodierung der eingebetteten Daten.
        /// </summary>
        public DataEncoding Encoding { get; private set; }

        /// <summary>
        /// Die eingebetteten encodierten Daten.
        /// </summary>
#if NET40
        public string EncodedData => ToString().Split(new char[] { ',' }, 2, StringSplitOptions.None)[1];
#else
        public string EncodedData => ToString().Split(',', 2, StringSplitOptions.None)[1];
#endif

        /// <summary>
        /// <c>true</c>, wenn der <see cref="DataUrl"/> eingebetteten Text enthält.
        /// </summary>
        public bool ContainsText => this.MimeType.MediaType.StartsWith("text", StringComparison.Ordinal);


        /// <summary>
        /// <c>true</c>, wenn der <see cref="DataUrl"/> eingebettete binäre Daten enthält.
        /// </summary>
        public bool ContainsBytes => !ContainsText;


        /// <summary>
        /// Gibt den im <see cref="DataUrl"/> eingebetteten Text zurück oder <c>null</c>,
        /// wenn der <see cref="DataUrl"/> eingebettete binäre Daten enthält.
        /// </summary>
        /// <returns>Der eingebettete Text oder <c>null</c>.</returns>
        public string? GetEmbeddedText()
        {
            if (!ContainsText)
            {
                return null;
            }

            if (Encoding == DataEncoding.Base64)
            {
                // als Base64 codierter Text:

                Encoding enc = TextEncodingConverter.GetEncoding(MimeType.Parameters.FirstOrDefault(x => x.Key == "charset").Value ?? "US-ASCII");
                try
                {
                    return enc.GetString(Convert.FromBase64String(EncodedData));
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                // Url-Codierter UTF-8-String:

                return Uri.UnescapeDataString(EncodedData ?? "");
            }
        }


        /// <summary>
        /// Gibt die im <see cref="DataUrl"/> eingebetteten binären Daten zurück oder <c>null</c>,
        /// wenn der <see cref="DataUrl"/> keine eingebetteten binäre Daten enthält oder wenn
        /// diese nicht dekodiert werden konnten.
        /// </summary>
        /// <returns>Die eingebetteten binären Daten oder <c>null</c>.</returns>
        public byte[]? GetEmbeddedBytes()
        {
            //if (!ContainsBytes) return null;
            try
            {
                return this.Encoding == DataEncoding.Base64 ? Convert.FromBase64String(EncodedData) : null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gibt eine geeignete Dateiendung für die in den den <see cref="DataUrl"/> eingebetteten Daten 
        /// zurück.
        /// </summary>
        /// <returns>Eine geeignete Dateiendung für die in den <see cref="DataUrl"/>
        /// eingebetteten Daten. Die Dateiendung enthält den Punkt "." als Trennzeichen.</returns>
        public string GetFileExtension() => MimeTypeConverter.GetFileExtension(this.MimeType);
    }
}
