using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Informationen über die Postanschrift in vCards
    /// </summary>
    public class Address : IDataContainer
    {
        private readonly ReadOnlyCollection<string>[] data;

        private const int MAX_COUNT = 7;

        private const int POST_OFFICE_BOX = 0;
        private const int EXTENDED_ADDRESS = 1;
        private const int STREET = 2;
        private const int LOCALITY = 3;
        private const int REGION = 4;
        private const int POSTAL_CODE = 5;
        private const int COUNTRY = 6;


        /// <summary>
        /// Initialisiert ein neues <see cref="Address"/>-Objekt.
        /// </summary>
        /// <param name="postOfficeBox">Postfach (sollte immer <c>null</c> sein).</param>
        /// <param name="extendedAddress">Adresszusatz (sollte immer <c>null</c> sein).</param>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="region">Bundesland</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="country">Land (Staat)</param>
        internal Address(
            IEnumerable<string?>? postOfficeBox = null,
            IEnumerable<string?>? extendedAddress = null,
            IEnumerable<string?>? street = null,
            IEnumerable<string?>? locality = null,
            IEnumerable<string?>? region = null,
            IEnumerable<string?>? postalCode = null,
            IEnumerable<string?>? country = null)
        {
#if NET40
            var empty = VCard.EmptyStringArray;
#else
            var empty = Array.Empty<string>();
#endif


            var arr = new ReadOnlyCollection<string>[]
            {
#nullable disable
                new ReadOnlyCollection<string>(postOfficeBox?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(extendedAddress?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(street?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(locality?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(region?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(postalCode?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(country?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty)
#nullable restore
            };

            data = arr;
        }


        internal Address(string vCardValue, StringBuilder builder, VCdVersion version)
        {
            Debug.Assert(vCardValue != null);

            var listList = vCardValue
                .SplitValueString(';')
                .Select(x => x.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            for (int i = 0; i < listList.Count; i++)
            {
                List<string>? currentList = listList[i];

                Debug.Assert(currentList != null);

                for (int j = currentList.Count - 1; j >= 0; j--)
                {
                    builder.Clear();
                    builder.Append(currentList[j]);
                    builder.UnMask(version).Trim().RemoveQuotes();

                    if (builder.Length != 0)
                    {
                        currentList[j] = builder.ToString();
                    }
                    else
                    {
                        currentList.RemoveAt(j); // wenn ein Eintrag nur Whitespace enthielt
                    }
                }
            }//for

            // wenn die vcf-Datei fehlerhaft ist, enthält listList zu wenige
            // Einträge, was zu Laufzeitfehlern führen kann:
            for (int i = listList.Count; i < MAX_COUNT; i++)
            {
                listList.Add(new List<string>());
            }

            data = listList.Select(x => new ReadOnlyCollection<string>(x)).ToArray();
        }

        /// <summary>
        /// Postfach (nie <c>null</c>) (nicht verwenden)
        /// </summary>
        [Obsolete("Don't use this property.", false)]
        public ReadOnlyCollection<string> PostOfficeBox => data[POST_OFFICE_BOX];

        /// <summary>
        /// Adresszusatz (nie <c>null</c>) (nicht verwenden)
        /// </summary>
        [Obsolete("Don't use this property.", false)]
        public ReadOnlyCollection<string> ExtendedAddress => data[EXTENDED_ADDRESS];

        /// <summary>
        /// Straße (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Street => data[STREET];

        /// <summary>
        /// Ort (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Locality => data[LOCALITY];

        /// <summary>
        /// Bundesland (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Region => data[REGION];

        /// <summary>
        /// Postleitzahl (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> PostalCode => data[POSTAL_CODE];

        /// <summary>
        /// Land (Staat) (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Country => data[COUNTRY];


        internal void AppendVCardString(VcfSerializer serializer)
        {
            StringBuilder builder = serializer.Builder;
            StringBuilder worker = serializer.Worker;

            char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

            for (int i = 0; i < data.Length - 1; i++)
            {
                AppendProperty(data[i]);
                builder.Append(';');
            }

            AppendProperty(data[data.Length - 1]);

            void AppendProperty(IList<string> strings)
            {
                if (strings.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < strings.Count - 1; i++)
                {
                    worker.Clear().Append(strings[i]).Mask(serializer.Version);
                    builder.Append(worker).Append(joinChar);
                }

                worker.Clear().Append(strings[strings.Count - 1]).Mask(serializer.Version);
                builder.Append(worker);
            }

        }


        internal bool NeedsToBeQpEncoded()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Any(s => s.NeedsToBeQpEncoded()))
                {
                    return true;
                }
            }

            return false;
        }

        ///// <summary>
        ///// True, wenn das <see cref="Address"/>-Objekt keine Daten enthält.
        ///// </summary>
        /// <inheritdoc/>
        public bool IsEmpty => !data.Any(x => x.Count != 0);

        object IDataContainer.Value => this;


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts.</returns>
        public override string ToString()
        {
            var worker = new StringBuilder();
            var dic = new List<Tuple<string, string>>();

            for (int i = 0; i < data.Length; i++)
            {
                string? s = BuildProperty(data[i]);

                if (s is null)
                {
                    continue;
                }

                dic.Add(new Tuple<string, string>(i switch
                {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                    POST_OFFICE_BOX => nameof(PostOfficeBox),
                    EXTENDED_ADDRESS => nameof(ExtendedAddress),
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                    STREET => nameof(Street),
                    LOCALITY => nameof(Locality),
                    REGION => nameof(Region),
                    POSTAL_CODE => nameof(PostalCode),
                    COUNTRY => nameof(Country),
                    _ => "Not Implemented"
                }, s));
            }

            if (dic.Count == 0)
            {
                return string.Empty;
            }

            int maxLength = dic.Select(x => x.Item1.Length).Max();
            maxLength += 2;

            worker.Clear();

            for (int i = 0; i < dic.Count; i++)
            {
                Tuple<string, string>? tpl = dic[i];
                string s = tpl.Item1 + ": ";
                worker.Append(s.PadRight(maxLength)).Append(tpl.Item2).Append(Environment.NewLine);
            }

            worker.Length -= Environment.NewLine.Length;
            return worker.ToString();

            string? BuildProperty(IList<string> strings)
            {
                worker.Clear();

                if (strings.Count == 0)
                {
                    return null;
                }

                for (int i = 0; i < strings.Count - 1; i++)
                {
                    worker.Append(strings[i]).Append(", ");
                }

                worker.Append(strings[strings.Count - 1]);

                return worker.ToString();
            }
        }


    }
}
