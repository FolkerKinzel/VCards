using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors
{
    internal sealed class ImppTypesCollector
    {
        private readonly
            ImppTypes[] definedEnumValues = new ImppTypes[] { ImppTypes.Business,
                                                              ImppTypes.Mobile,
                                                              ImppTypes.Personal
                                                             };


        /// <summary>
        /// Sammelt die Namen der in <paramref name="imppType"/> gesetzten Flags in
        /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
        /// geleert.
        /// </summary>
        /// <param name="imppType"><see cref="ImppTypes"/>-Objekt oder <c>null</c>.</param>
        /// <param name="list">Eine Liste zum sammeln.</param>
        internal void CollectValueStrings(ImppTypes? imppType, List<string> list)
        {
            Debug.Assert(list != null);


            for (int i = 0; i < definedEnumValues.Length; i++)
            {
                ImppTypes value = definedEnumValues[i];

                if ((imppType & value) == value)
                {
                    switch (value)
                    {
                        case ImppTypes.Business:
                            list.Add(ImppTypesConverter.TypeValue.Business);
                            break;
                        case ImppTypes.Mobile:
                            list.Add(ImppTypesConverter.TypeValue.Mobile);
                            break;
                        case ImppTypes.Personal:
                            list.Add(ImppTypesConverter.TypeValue.Personal);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
