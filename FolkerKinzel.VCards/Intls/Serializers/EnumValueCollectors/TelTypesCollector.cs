using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors
{
    internal class TelTypesCollector
    {
        private readonly
            TelTypes[] definedEnumValues
                = new TelTypes[] {  TelTypes.Voice,
                                    TelTypes.Fax,
                                    TelTypes.Msg,
                                    TelTypes.Cell,
                                    TelTypes.Pager,
                                    TelTypes.BBS,
                                    TelTypes.Modem,
                                    TelTypes.Car,
                                    TelTypes.ISDN,
                                    TelTypes.Video,
                                    TelTypes.PCS,
                                    TelTypes.TextPhone,
                                    TelTypes.Text };


        /// <summary>
        /// Sammelt die Namen der in <paramref name="telType"/> gesetzten Flags in
        /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
        /// geleert.
        /// </summary>
        /// <param name="telType"><see cref="TelTypes"/>-Objekt oder null.</param>
        /// <param name="list">Eine Liste zum sammeln.</param>
        internal void CollectValueStrings(TelTypes? telType, List<string> list)
        {
            Debug.Assert(list != null);


            for (int i = 0; i < definedEnumValues.Length; i++)
            {
                TelTypes value = definedEnumValues[i];

                if ((telType & value) == value)
                {
                    switch (value)
                    {
                        case TelTypes.Voice:
                            list.Add(TelTypesConverter.TelTypeValue.VOICE);
                            break;
                        case TelTypes.Fax:
                            list.Add(TelTypesConverter.TelTypeValue.FAX);
                            break;
                        case TelTypes.Msg:
                            list.Add(TelTypesConverter.TelTypeValue.MSG);
                            break;
                        case TelTypes.Cell:
                            list.Add(TelTypesConverter.TelTypeValue.CELL);
                            break;
                        case TelTypes.Pager:
                            list.Add(TelTypesConverter.TelTypeValue.PAGER);
                            break;
                        case TelTypes.BBS:
                            list.Add(TelTypesConverter.TelTypeValue.BBS);
                            break;
                        case TelTypes.Modem:
                            list.Add(TelTypesConverter.TelTypeValue.MODEM);
                            break;
                        case TelTypes.Car:
                            list.Add(TelTypesConverter.TelTypeValue.CAR);
                            break;
                        case TelTypes.ISDN:
                            list.Add(TelTypesConverter.TelTypeValue.ISDN);
                            break;
                        case TelTypes.Video:
                            list.Add(TelTypesConverter.TelTypeValue.VIDEO);
                            break;
                        case TelTypes.PCS:
                            list.Add(TelTypesConverter.TelTypeValue.PCS);
                            break;
                        case TelTypes.TextPhone:
                            list.Add(TelTypesConverter.TelTypeValue.TEXTPHONE);
                            break;
                        case TelTypes.Text:
                            list.Add(TelTypesConverter.TelTypeValue.TEXT);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
