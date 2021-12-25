using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal sealed class PropertyClassTypesCollector
{
    private readonly
        PropertyClassTypes[] _definedEnumValues = new PropertyClassTypes[] { PropertyClassTypes.Home,
                                                                                 PropertyClassTypes.Work };


    /// <summary>
    /// Sammelt die Namen der in <paramref name="propertyClassType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="propertyClassType"><see cref="PropertyClassTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum sammeln.</param>
    internal void CollectValueStrings(PropertyClassTypes? propertyClassType, List<string> list)
    {
        Debug.Assert(list != null);


        for (int i = 0; i < _definedEnumValues.Length; i++)
        {
            PropertyClassTypes value = _definedEnumValues[i];

            if ((propertyClassType & value) == value)
            {
                switch (value)
                {
                    case PropertyClassTypes.Home:
                        list.Add(ParameterSection.TypeValue.HOME);
                        break;
                    case PropertyClassTypes.Work:
                        list.Add(ParameterSection.TypeValue.WORK);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
