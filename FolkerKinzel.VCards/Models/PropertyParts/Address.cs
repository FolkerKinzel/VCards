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
    /// Kapselt Informationen über die Postanschrift in vCards.
    /// </summary>
    public sealed class Address
    {
        private const int MAX_COUNT = 7;

        private const int POST_OFFICE_BOX = 0;
        private const int EXTENDED_ADDRESS = 1;
        private const int STREET = 2;
        private const int LOCALITY = 3;
        private const int REGION = 4;
        private const int POSTAL_CODE = 5;
        private const int COUNTRY = 6;

        private readonly ReadOnlyCollection<string> _postOfficeBox;
        private readonly ReadOnlyCollection<string> _extendedAddress;
        private readonly ReadOnlyCollection<string> _street;
        private readonly ReadOnlyCollection<string> _locality;
        private readonly ReadOnlyCollection<string> _region;
        private readonly ReadOnlyCollection<string> _postalCode;
        private readonly ReadOnlyCollection<string> _country;


        /// <summary>
        /// Initialisiert ein neues <see cref="Address"/>-Objekt.
        /// </summary>
        /// <param name="street">Straße</param>
        /// <param name="locality">Ort</param>
        /// <param name="postalCode">Postleitzahl</param>
        /// <param name="region">Bundesland</param>
        /// <param name="country">Land (Staat)</param>
        /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
        internal Address(ReadOnlyCollection<string> street,
                         ReadOnlyCollection<string> locality,
                         ReadOnlyCollection<string> postalCode,
                         ReadOnlyCollection<string> region,
                         ReadOnlyCollection<string> country,
                         ReadOnlyCollection<string> postOfficeBox,
                         ReadOnlyCollection<string> extendedAddress)
        {
            _postOfficeBox = postOfficeBox;
            _extendedAddress = extendedAddress;
            _street = street;
            _locality = locality;
            _region = region;
            _postalCode = postalCode;
            _country = country;
        }


        internal Address()
        {
            _postOfficeBox = 
            _extendedAddress = 
            _street = 
            _locality = 
            _region = 
            _postalCode =
            _country = ReadOnlyCollectionConverter.Empty();
        }


        internal Address(string vCardValue, StringBuilder builder, VCdVersion version)
        {
            Debug.Assert(vCardValue != null);

            List<List<string>?> listList = vCardValue
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
                    _ = builder.UnMask(version).Trim().RemoveQuotes();

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
                listList.Add(null);
            }

            _postOfficeBox = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[POST_OFFICE_BOX]);
            _extendedAddress = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[EXTENDED_ADDRESS]);
            _street = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[STREET]);
            _locality = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[LOCALITY]);
            _region = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[REGION]);
            _postalCode = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[POSTAL_CODE]);
            _country = ReadOnlyCollectionConverter.ToReadOnlyCollection(listList[COUNTRY]);
        }

        /// <summary>
        /// Postfach (nie <c>null</c>) (nicht verwenden)
        /// </summary>
        [Obsolete("Don't use this property.", false)]
        public ReadOnlyCollection<string> PostOfficeBox => _postOfficeBox;

        /// <summary>
        /// Adresszusatz (nie <c>null</c>) (nicht verwenden)
        /// </summary>
        [Obsolete("Don't use this property.", false)]
        public ReadOnlyCollection<string> ExtendedAddress => _extendedAddress;

        /// <summary>
        /// Straße (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Street => _street;

        /// <summary>
        /// Ort (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Locality => _locality;

        /// <summary>
        /// Bundesland (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Region => _region;

        /// <summary>
        /// Postleitzahl (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> PostalCode => _postalCode;

        /// <summary>
        /// Land (Staat) (nie <c>null</c>)
        /// </summary>
        public ReadOnlyCollection<string> Country => _country;


        /// <summary>
        /// <c>true</c>, wenn das <see cref="Address"/>-Objekt keine verwertbaren Daten enthält.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
        public bool IsEmpty
        {
            get
            {
                if (_locality.Count != 0)
                {
                    return false;
                }

                if (_street.Count != 0)
                {
                    return false;
                }

                if (_country.Count != 0)
                {
                    return false;
                }

                if (_region.Count != 0)
                {
                    return false;
                }

                if (_postalCode.Count != 0)
                {
                    return false;
                }

                if (_postOfficeBox.Count != 0)
                {
                    return false;
                }

                if (_extendedAddress.Count != 0)
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

            AppendProperty(_postOfficeBox);
            _ = builder.Append(';');

            AppendProperty(_extendedAddress);
            _ = builder.Append(';');

            AppendProperty(_street);
            _ = builder.Append(';');

            AppendProperty(_locality);
            _ = builder.Append(';');

            AppendProperty(_region);
            _ = builder.Append(';');

            AppendProperty(_postalCode);
            _ = builder.Append(';');

            AppendProperty(_country);


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
            if (_locality.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_street.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_country.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_region.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_postalCode.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_postOfficeBox.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            if (_extendedAddress.Any(x => x.NeedsToBeQpEncoded()))
            {
                return true;
            }

            return false;
        }





        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts.</returns>
        public override string ToString()
        {
            var worker = new StringBuilder();
            var dic = new List<Tuple<string, string>>();

            for (int i = 0; i < MAX_COUNT; i++)
            {
                switch (i)
                {
                    case POST_OFFICE_BOX:
                        {
                            string? s = BuildProperty(_postOfficeBox);

                            if (s is null)
                            {
                                continue;
                            }

#pragma warning disable CS0618 // Typ oder Element ist veraltet
                            dic.Add(new Tuple<string, string>(nameof(PostOfficeBox), s));
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                            break;
                        }
                    case EXTENDED_ADDRESS:
                        {
                            string? s = BuildProperty(_extendedAddress);

                            if (s is null)
                            {
                                continue;
                            }

#pragma warning disable CS0618 // Typ oder Element ist veraltet
                            dic.Add(new Tuple<string, string>(nameof(ExtendedAddress), s));
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                            break;
                        }
                    case STREET:
                        {
                            string? s = BuildProperty(_street);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Street), s));
                            break;
                        }
                    case LOCALITY:
                        {
                            string? s = BuildProperty(_locality);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Locality), s));
                            break;
                        }
                    case REGION:
                        {
                            string? s = BuildProperty(_region);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Region), s));
                            break;
                        }
                    case POSTAL_CODE:
                        {
                            string? s = BuildProperty(_postalCode);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(PostalCode), s));
                            break;
                        }
                    case COUNTRY:
                        {
                            string? s = BuildProperty(_country);

                            if (s is null)
                            {
                                continue;
                            }

                            dic.Add(new Tuple<string, string>(nameof(Country), s));
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

            ////////////////////////////////////////////

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
