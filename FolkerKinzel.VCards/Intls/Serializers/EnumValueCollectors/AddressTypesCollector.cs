using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors
{
    internal sealed class AddressTypesCollector
    {
        private readonly
            AddressTypes[] _definedEnumValues = new AddressTypes[] { AddressTypes.Dom,
                                                                     AddressTypes.Intl,
                                                                     AddressTypes.Postal,
                                                                     AddressTypes.Parcel };


        /// <summary>
        /// Sammelt die Namen der in <paramref name="addressType"/> gesetzten Flags in
        /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
        /// geleert.
        /// </summary>
        /// <param name="addressType"><see cref="AddressTypes"/>-Objekt oder <c>null</c>.</param>
        /// <param name="list">Eine Liste zum sammeln.</param>
        internal void CollectValueStrings(AddressTypes? addressType, List<string> list)
        {
            Debug.Assert(list != null);


            for (int i = 0; i < _definedEnumValues.Length; i++)
            {
                AddressTypes value = _definedEnumValues[i];

                if ((addressType & value) == value)
                {
                    switch (value)
                    {
                        case AddressTypes.Dom:
                            list.Add(AddressTypesConverter.AdrTypeValue.DOM);
                            break;
                        case AddressTypes.Intl:
                            list.Add(AddressTypesConverter.AdrTypeValue.INTL);
                            break;
                        case AddressTypes.Postal:
                            list.Add(AddressTypesConverter.AdrTypeValue.POSTAL);
                            break;
                        case AddressTypes.Parcel:
                            list.Add(AddressTypesConverter.AdrTypeValue.PARCEL);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
