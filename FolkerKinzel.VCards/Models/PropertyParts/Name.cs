using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
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
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            Prefix = prefix;
            Suffix = suffix;
        }

        internal Name()
        {
            LastName =
            FirstName =
            MiddleName =
            Prefix =
            Suffix = ReadOnlyCollectionConverter.Empty();
        }


        internal Name(string vCardValue, VCardDeserializationInfo info, VCdVersion version)
        {
            Debug.Assert(vCardValue != null);

            StringBuilder builder = info.Builder;
            ValueSplitter semicolonSplitter = info.SemiColonSplitter;
            ValueSplitter commaSplitter = info.CommaSplitter;

            semicolonSplitter.ValueString = vCardValue;
            int index = 0;
            foreach (var s in semicolonSplitter)
            {
                switch (index++)
                {
                    case LAST_NAME:
                        {
                            if (s.Length == 0)
                            {
                                LastName = ReadOnlyCollectionConverter.Empty();
                            }
                            else
                            {
                                var list = new List<string>();

                                commaSplitter.ValueString = s;
                                foreach (var item in commaSplitter)
                                {
                                    list.Add(item.UnMask(builder, version));
                                }

                                LastName = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                            }

                            break;
                        }
                    case FIRST_NAME:
                        {
                            if (s.Length == 0)
                            {
                                FirstName = ReadOnlyCollectionConverter.Empty();
                            }
                            else
                            {
                                var list = new List<string>();

                                commaSplitter.ValueString = s;
                                foreach (var item in commaSplitter)
                                {
                                    list.Add(item.UnMask(builder, version));
                                }

                                FirstName = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                            }

                            break;
                        }
                    case MIDDLE_NAME:
                        {
                            if (s.Length == 0)
                            {
                                MiddleName = ReadOnlyCollectionConverter.Empty();
                            }
                            else
                            {
                                var list = new List<string>();

                                commaSplitter.ValueString = s;
                                foreach (var item in commaSplitter)
                                {
                                    list.Add(item.UnMask(builder, version));
                                }

                                MiddleName = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                            }

                            break;
                        }
                    case PREFIX:
                        {
                            if (s.Length == 0)
                            {
                                Prefix = ReadOnlyCollectionConverter.Empty();
                            }
                            else
                            {
                                var list = new List<string>();

                                commaSplitter.ValueString = s;
                                foreach (var item in commaSplitter)
                                {
                                    list.Add(item.UnMask(builder, version));
                                }

                                Prefix = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                            }

                            break;
                        }
                    case SUFFIX:
                        {
                            if (s.Length == 0)
                            {
                                Suffix = ReadOnlyCollectionConverter.Empty();
                            }
                            else
                            {
                                var list = new List<string>();

                                commaSplitter.ValueString = s;
                                foreach (var item in commaSplitter)
                                {
                                    list.Add(item.UnMask(builder, version));
                                }

                                Suffix = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                            }

                            break;
                        }
                }//switch
            }//foreach

            // Wenn die VCF-Datei fehlerhaft ist, könnten Properties null sein:
            FirstName ??= ReadOnlyCollectionConverter.Empty();
            LastName ??= ReadOnlyCollectionConverter.Empty();
            MiddleName ??= ReadOnlyCollectionConverter.Empty();
            Prefix ??= ReadOnlyCollectionConverter.Empty();
            Suffix ??= ReadOnlyCollectionConverter.Empty();
        }

        /// <summary>
        /// Nachname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> LastName { get; }

        /// <summary>
        /// Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> FirstName { get; }

        /// <summary>
        /// zweiter Vorname (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> MiddleName { get; }

        /// <summary>
        /// Namenspräfix (z.B. "Prof. Dr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Prefix { get; }

        /// <summary>
        /// Namenssuffix (z.B. "jr.") (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Suffix { get; }


        /// <summary>
        /// <c>true</c>, wenn das <see cref="Name"/>-Objekt keine verwertbaren Daten enthält.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
        public bool IsEmpty
        {
            get
            {
                if (LastName.Count != 0)
                {
                    return false;
                }

                if (FirstName.Count != 0)
                {
                    return false;
                }

                if (MiddleName.Count != 0)
                {
                    return false;
                }

                if (Prefix.Count != 0)
                {
                    return false;
                }

                if (Suffix.Count != 0)
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

            AppendProperty(LastName);
            _ = builder.Append(';');

            AppendProperty(FirstName);
            _ = builder.Append(';');

            AppendProperty(MiddleName);
            _ = builder.Append(';');

            AppendProperty(Prefix);
            _ = builder.Append(';');

            AppendProperty(Suffix);

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
            if (LastName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (FirstName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (MiddleName.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (Prefix.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (Suffix.Any(x => x.NeedsToBeQpEncoded()))
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
                            string? s = BuildProperty(LastName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(LastName), s));
                            break;
                        }
                    case FIRST_NAME:
                        {
                            string? s = BuildProperty(FirstName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(FirstName), s));
                            break;
                        }
                    case MIDDLE_NAME:
                        {
                            string? s = BuildProperty(MiddleName);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(MiddleName), s));
                            break;
                        }
                    case PREFIX:
                        {
                            string? s = BuildProperty(Prefix);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Prefix), s));
                            break;
                        }
                    case SUFFIX:
                        {
                            string? s = BuildProperty(Suffix);

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
