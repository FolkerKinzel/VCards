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
    /// Kapselt Informationen über den Namen der Person, die die vCard repräsentiert.
    /// </summary>
    public class Name
    {
        private readonly ReadOnlyCollection<string>[] data;

        private const int MAX_COUNT = 5;
        private const int LAST_NAME = 0;
        private const int FIRST_NAME = 1;
        private const int MIDDLE_NAME = 2;
        private const int PREFIX = 3;
        private const int SUFFIX = 4;

        /// <summary>
        /// Initialisiert ein neues <see cref="Name"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        internal Name(
            IEnumerable<string?>? lastName = null,
            IEnumerable<string?>? firstName = null,
            IEnumerable<string?>? middleName = null,
            IEnumerable<string?>? prefix = null,
            IEnumerable<string?>? suffix = null)
        {
#if NET40
            var empty = VCard.EmptyStringArray;
#else       
            var empty = Array.Empty<string>();
#endif
            var arr = new ReadOnlyCollection<string>[]
            {
#nullable disable
                new ReadOnlyCollection<string>(lastName?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(firstName?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(middleName?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(prefix?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty),
                    new ReadOnlyCollection<string>(suffix?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? empty)
#nullable restore
            };

            data = arr;
        }

        internal Name(string vCardValue, StringBuilder builder, VCdVersion version)
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
        /// Nachname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> LastName => data[LAST_NAME];

        /// <summary>
        /// Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> FirstName => data[FIRST_NAME];

        /// <summary>
        /// zweiter Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> MiddleName => data[MIDDLE_NAME];

        /// <summary>
        /// Namenspräfix (z.B. "Prof. Dr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Prefix => data[PREFIX];

        /// <summary>
        /// Namenssuffix (z.B. "jr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Suffix => data[SUFFIX];

        /// <summary>
        /// True, wenn das <see cref="Name"/>-Objekt keine Daten enthält.
        /// </summary>
        public bool IsEmpty => !data.Any(x => x.Count != 0);


        internal void AppendVCardString(VcfSerializer serializer)
        {
            StringBuilder? builder = serializer.Builder;
            StringBuilder? worker = serializer.Worker;

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

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Name"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Name"/>-Objekts.</returns>
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
                    LAST_NAME => nameof(LastName),
                    FIRST_NAME => nameof(FirstName),
                    MIDDLE_NAME => nameof(MiddleName),
                    PREFIX => nameof(Prefix),
                    SUFFIX => nameof(Suffix),
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
