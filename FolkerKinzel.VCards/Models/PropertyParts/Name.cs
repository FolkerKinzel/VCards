using FolkerKinzel.VCards.Intls.Converters;
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
    public sealed class Name
    {
        private const int MAX_COUNT = 5;

        private const int LAST_NAME = 0;
        private const int FIRST_NAME = 1;
        private const int MIDDLE_NAME = 2;
        private const int PREFIX = 3;
        private const int SUFFIX = 4;
        private readonly ReadOnlyCollection<string> _lastName;
        private readonly ReadOnlyCollection<string> _firstName;
        private readonly ReadOnlyCollection<string> _middleName;
        private readonly ReadOnlyCollection<string> _prefix;
        private readonly ReadOnlyCollection<string> _suffix;

        /// <summary>
        /// Initialisiert ein neues <see cref="Name"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        internal Name(
            ReadOnlyCollection<string> lastName,
            ReadOnlyCollection<string> firstName,
            ReadOnlyCollection<string> middleName,
            ReadOnlyCollection<string> prefix,
            ReadOnlyCollection<string> suffix)
        {
            _lastName = lastName;
            _firstName = firstName;
            _middleName = middleName;
            _prefix = prefix;
            _suffix = suffix;
        }

        internal Name()
        {
            _lastName =
            _firstName =
            _middleName =
            _prefix =
            _suffix = ReadOnlyCollectionConverter.Empty();
        }


        internal Name(string vCardValue, StringBuilder builder, VCdVersion version)
        {
            Debug.Assert(vCardValue != null);

            List<List<string>?>? listList = vCardValue
                .SplitValueString(';')
                .Select(x => x.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList()!;

            for (int i = 0; i < listList.Count; i++)
            {
                List<string>? currentList = listList[i];

                Debug.Assert(currentList != null);

                for (int j = currentList.Count - 1; j >= 0; j--)
                {
                    _ = builder.Clear();
                    _ = builder.Append(currentList[j]);
                    _ = builder.UnMask(version); //.Trim().RemoveQuotes();

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

            _firstName = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[FIRST_NAME]);
            _lastName = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[LAST_NAME]);
            _middleName = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[MIDDLE_NAME]);
            _prefix = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[PREFIX]);
            _suffix = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[SUFFIX]);
        }

        /// <summary>
        /// Nachname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> LastName => _lastName;

        /// <summary>
        /// Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> FirstName => _firstName;

        /// <summary>
        /// zweiter Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> MiddleName => _middleName;

        /// <summary>
        /// Namenspräfix (z.B. "Prof. Dr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Prefix => _prefix;

        /// <summary>
        /// Namenssuffix (z.B. "jr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Suffix => _suffix;


        /// <summary>
        /// <c>true</c>, wenn das <see cref="Name"/>-Objekt keine verwertbaren Daten enthält.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
        public bool IsEmpty
        {
            get
            {
                if (_lastName.Count != 0)
                {
                    return false;
                }

                if (_firstName.Count != 0)
                {
                    return false;
                }

                if (_middleName.Count != 0)
                {
                    return false;
                }

                if (_prefix.Count != 0)
                {
                    return false;
                }

                if (_suffix.Count != 0)
                {
                    return false;
                }

                return true;
            }
        }


        internal void AppendVCardString(VcfSerializer serializer)
        {
            StringBuilder builder = serializer.Builder;
            StringBuilder worker = serializer.Worker;

            char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

            AppendProperty(_lastName);
            _ = builder.Append(';');

            AppendProperty(_firstName);
            _ = builder.Append(';');

            AppendProperty(_middleName);
            _ = builder.Append(';');

            AppendProperty(_prefix);
            _ = builder.Append(';');

            AppendProperty(_suffix);

            ///////////////////////////////////

            void AppendProperty(IList<string> strings)
            {
                if (strings.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < strings.Count - 1; i++)
                {
                    _ = worker.Clear().Append(strings[i]).Mask(serializer.Version);
                    _ = builder.Append(worker).Append(joinChar);
                }

                _ = worker.Clear().Append(strings[strings.Count - 1]).Mask(serializer.Version);
                _ = builder.Append(worker);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
        internal bool NeedsToBeQpEncoded()
        {
            if (_lastName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_firstName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_middleName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_prefix.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_suffix.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
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

            for (int i = 0; i < MAX_COUNT; i++)
            {
                switch (i)
                {
                    case LAST_NAME:
                        {
                            string? s = BuildProperty(_lastName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(LastName), s));
                            break;
                        }
                    case FIRST_NAME:
                        {
                            string? s = BuildProperty(_firstName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(FirstName), s));
                            break;
                        }
                    case MIDDLE_NAME:
                        {
                            string? s = BuildProperty(_middleName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(MiddleName), s));
                            break;
                        }
                    case PREFIX:
                        {
                            string? s = BuildProperty(_prefix);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Prefix), s));
                            break;
                        }
                    case SUFFIX:
                        {
                            string? s = BuildProperty(_suffix);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Suffix), s));
                            break;
                        }
                }
            }

            if (dic.Count == 0)
            {
                return string.Empty;
            }

            int maxLength = dic.Select(x => x.Item1.Length).Max();
            maxLength += 2;

            _ = worker.Clear();

            for (int i = 0; i < dic.Count; i++)
            {
                Tuple<string, string>? tpl = dic[i];
                string s = tpl.Item1 + ": ";
                _ = worker.Append(s.PadRight(maxLength)).Append(tpl.Item2).Append(Environment.NewLine);
            }

            worker.Length -= Environment.NewLine.Length;
            return worker.ToString();

            //////////////////////////////////////////////////

            string? BuildProperty(IList<string> strings)
            {
                _ = worker.Clear();

                if (strings.Count == 0)
                {
                    return null;
                }

                for (int i = 0; i < strings.Count - 1; i++)
                {
                    _ = worker.Append(strings[i]).Append(", ");
                }

                _ = worker.Append(strings[strings.Count - 1]);

                return worker.ToString();
            }
        }

    }
}
