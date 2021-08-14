using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;
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
        [NonSerialized]
        private readonly MimeType _mimeType;

        [NonSerialized]
        private readonly DataEncoding _encoding;


        /// <summary>
        /// Gibt an, dass der <see cref="Uri"/> ein <see cref="DataUrl"/> nach RFC 2397 ist. Dieses Feld ist schreibgeschützt.
        /// </summary>
        public const string UriSchemeData = "data";

        /// <summary>
        /// Gibt das Zeichen an, das das Schema des Kommunikationsprotokolls vom Adressteil des URIs trennt. Dieses Feld ist schreibgeschützt.
        /// </summary>
        public static new readonly string SchemeDelimiter = ":";

        /// <summary>
        /// Initialisiert ein neues <see cref="DataUrl"/>-Objekt.
        /// </summary>
        /// <param name="uriString">Der <see cref="string"/>, aus dem der <see cref="DataUrl"/> initialisiert wird.</param>
        /// <param name="mimeType">Der MIME-Typ der eingebetteten Daten. Wenn <c>null</c> übergeben wird, 
        /// wird der Standard-MIME-Typ <c>text/plain;charset=US-ASCII</c> erzeugt.</param>
        /// <param name="encoding">Beschreibt, in welcher Form die Daten im  <see cref="DataUrl"/> kodiert sind.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uriString"/> ist <c>null</c>.</exception>
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil <paramref name="uriString"/> länger als 65519 Zeichen ist.</exception>
        internal DataUrl(string uriString, MimeType? mimeType, DataEncoding encoding) : base(uriString)
        {
            this._encoding = encoding;
            this._mimeType = mimeType ?? new MimeType();
        }


        /// <summary>
        /// MIME-Type der eingebetteten Daten. (Nie <c>null</c>.)
        /// </summary>
        public MimeType MimeType => _mimeType;

        /// <summary>
        /// Encodierung der eingebetteten Daten.
        /// </summary>
        public DataEncoding Encoding => _encoding;

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


        /// <inheritdoc/>
        protected DataUrl(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            if (TryCreate(OriginalString, out DataUrl? tmpUrl))
            {
                this._mimeType = tmpUrl.MimeType;
                this._encoding = tmpUrl.Encoding;
            }
            else
            {
                throw new UriFormatException(Res.NoDataUrl);
            }
        }


        /// <summary>
        /// Erstellt einen neuen <see cref="DataUrl"/>. Löst keine Ausnahme aus, wenn der <see cref="DataUrl"/> nicht erstellt werden kann.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/>, der dem Data-URL-Schema nach RFC 2397 entspricht.</param>
        /// <param name="dataUrl">Enthält nach dem Beenden der Methode einen aus <paramref name="value"/> erstellten <see cref="DataUrl"/>.
        /// Dieser Parameter wird nicht-initialisiert übergeben.</param>
        /// <returns>Ein <see cref="bool"/>-Wert, der <c>true</c> ist, wenn der <see cref="DataUrl"/> erfolgreich erstellt wurde, andernfalls <c>false</c>.</returns>
        public static bool TryCreate(string? value, [NotNullWhen(true)] out DataUrl? dataUrl)
        {
            dataUrl = null;

            if (value is null || !value.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
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
                dataUrl = new DataUrl(value, mime, enc);
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
        /// <exception cref="FormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der URI-String länger als 65519 Zeichen ist.</exception>
        public static DataUrl FromText(string? text)
        {
            string dataString = text is null ? "" : Uri.EscapeDataString(Uri.UnescapeDataString(text));
            var dataUri = new DataUrl($"data:,{dataString}", null, DataEncoding.UrlEncoded);

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
        /// <exception cref="UriFormatException">Es kann kein <see cref="DataUrl"/> initialisiert werden, z.B.
        /// weil der URI-String länger als 65519 Zeichen ist.</exception>
        public static DataUrl FromBytes(byte[]? bytes, string? mimeType)
        {
            var mType = new MimeType(mimeType);

#if NET40
            bytes ??= new byte[0];
#else
            bytes ??= Array.Empty<byte>();
#endif

            return new DataUrl($"data:{mType};base64,{Convert.ToBase64String(bytes)}", mType, DataEncoding.Base64);
        }


        /// <summary>
        /// Erstellt einen <see cref="DataUrl"/> aus einer physisch vorhandenen Datei.
        /// </summary>
        /// <param name="path">Absoluter Pfad zu der einzubettenden Datei.</param>
        /// <param name="mimeType">MIME-Typ der einzubettenden Datei. Wenn <c>null</c> angegeben wird,
        /// wird versucht, den MIME-Typ aus der Dateiendung automatisch zu ermitteln.</param>
        /// <returns>Ein <see cref="DataUrl"/>, in den die Daten der mit <paramref name="path"/> referenzierten Datei
        /// eingebettet sind.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> ist <c>null</c>.</exception>
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
                Encoding enc = TextEncodingConverter.GetEncoding(MimeType.Parameters?.FirstOrDefault(x => x.Key == "charset").Value ?? "US-ASCII");
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
            try
            {
                return this.Encoding == DataEncoding.Base64
                    ? Convert.FromBase64String(EncodedData)
                    : System.Text.Encoding.UTF8.GetBytes(Uri.UnescapeDataString(EncodedData ?? ""));
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
