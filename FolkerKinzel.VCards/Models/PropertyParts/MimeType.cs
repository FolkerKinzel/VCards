using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Informationen über einen MIME-Type. ("Multipurpose Internet Mail Extensions")
    /// </summary>
    [Serializable]
    public sealed class MimeType
    {
        private const string TEXT_PLAIN = "text/plain";
        private const string CHARSET_PARAMETER_NAME = "charset";
        private const string DEFAULT_CHARSET = "US-ASCII";

        internal MimeType(string? value = null)
        {
            this.MediaType = TEXT_PLAIN;

            if (string.IsNullOrWhiteSpace(value))
            {
                this.Parameters = new ReadOnlyCollection<KeyValuePair<string, string>>(
                    new KeyValuePair<string, string>[] { new KeyValuePair<string, string>(CHARSET_PARAMETER_NAME, DEFAULT_CHARSET) });
                return;
            }

#if NET40
            string[] arr = value.Replace(" ", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);;
#else
            string[] arr = value.Replace(" ", "", StringComparison.Ordinal).Split(';', StringSplitOptions.RemoveEmptyEntries); ;
#endif
            int start;

            // erlaubte Abkürzung in RFC 2397: nur der "charset" Parameter wird bei "text/plain" angegeben:
            if (value.StartsWith(";", StringComparison.Ordinal))
            {
                //this.MediaType = TEXT_PLAIN;
                start = 0;
            }
            else
            {
                start = 1;
                this.MediaType = arr[0].ToLowerInvariant();
            }



            if (arr.Length == 0)
            {
                // DEFAULT_CHARSET ergänzen
                if (MediaType == TEXT_PLAIN)
                {
                    this.Parameters = new ReadOnlyCollection<KeyValuePair<string, string>>(
                    new KeyValuePair<string, string>[] { new KeyValuePair<string, string>(CHARSET_PARAMETER_NAME, DEFAULT_CHARSET) });
                }
                return;
            }



            if (arr.Length > start)
            {
                var list = new List<KeyValuePair<string, string>>(arr.Length - start);

                for (int i = start; i < arr.Length; i++)
                {
#if NET40
                    string[] para = arr[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
#else
                    string[] para = arr[i].Split('=', StringSplitOptions.RemoveEmptyEntries);

#endif

                    if (para.Length < 2)
                    {
                        continue;
                    }

                    // repariere falsche Lowercase-Schreibweise:
                    if (para[1].StartsWith(DEFAULT_CHARSET, StringComparison.OrdinalIgnoreCase))
                    {
                        para[1] = DEFAULT_CHARSET;
                    }

                    list.Add(new KeyValuePair<string, string>(para[0].ToLowerInvariant(), para[1]));
                }

                if (list.Count != 0)
                {
                    Parameters = new ReadOnlyCollection<KeyValuePair<string, string>>(list);
                }
            }
        }

        /// <summary>
        /// Internet Media Type (Nie <c>null</c>.)
        /// </summary>
        public string MediaType { get; }

        /// <summary>
        /// Parameter
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>>? Parameters { get; }

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="MimeType"/>-Objekts. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="MimeType"/>-Objekts.</returns>
        public override string ToString()
        {
            if (MediaType == TEXT_PLAIN)
            {
                Debug.Assert(Parameters != null);
                Debug.Assert(Parameters.Count != 0);

                if (Parameters.Count == 1 && Parameters[0].Value == DEFAULT_CHARSET)
                {
                    return string.Empty;
                }
            }

            var sb = new StringBuilder();

            sb.Append(MediaType);

            if (Parameters is null)
            {
                return sb.ToString();
            }

            for (int i = 0; i < Parameters.Count; i++)
            {
                KeyValuePair<string, string> kvp = Parameters[i];
                sb.Append(';').Append(kvp.Key).Append('=').Append(kvp.Value);
            }

            return sb.ToString();
        }
    }
}
