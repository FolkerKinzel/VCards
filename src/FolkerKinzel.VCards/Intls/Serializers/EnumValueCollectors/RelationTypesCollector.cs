using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;

internal sealed class RelationTypesCollector
{
    private readonly
        RelationTypes[] _definedEnumValues = new RelationTypes[]{ RelationTypes.Contact,
                                                                  RelationTypes.Acquaintance,
                                                                  RelationTypes.Friend,
                                                                  RelationTypes.Met,
                                                                  RelationTypes.CoWorker,
                                                                  RelationTypes.Colleague,
                                                                  RelationTypes.CoResident,
                                                                  RelationTypes.Neighbor,
                                                                  RelationTypes.Child,
                                                                  RelationTypes.Parent,
                                                                  RelationTypes.Sibling,
                                                                  RelationTypes.Spouse,
                                                                  RelationTypes.Kin,
                                                                  RelationTypes.Muse,
                                                                  RelationTypes.Crush,
                                                                  RelationTypes.Date,
                                                                  RelationTypes.Sweetheart,
                                                                  RelationTypes.Me,
                                                                  RelationTypes.Agent,
                                                                  RelationTypes.Emergency };

    /// <summary>
    /// Sammelt die Namen der in <paramref name="relationType"/> gesetzten Flags in
    /// <paramref name="list"/>. <paramref name="list"/> wird von der Methode nicht
    /// geleert.
    /// </summary>
    /// <param name="relationType"><see cref="RelationTypes"/>-Objekt oder <c>null</c>.</param>
    /// <param name="list">Eine Liste zum sammeln.</param>
    internal void CollectValueStrings(RelationTypes? relationType, List<string> list)
    {
        Debug.Assert(list != null);

        for (int i = 0; i < _definedEnumValues.Length; i++)
        {
            RelationTypes value = _definedEnumValues[i];

            if ((relationType & value) == value)
            {
                switch (value)
                {
                    case RelationTypes.Contact:
                        list.Add(RelationTypesConverter.RelationTypeValue.CONTACT);
                        break;
                    case RelationTypes.Acquaintance:
                        list.Add(RelationTypesConverter.RelationTypeValue.ACQUAINTANCE);
                        break;
                    case RelationTypes.Friend:
                        list.Add(RelationTypesConverter.RelationTypeValue.FRIEND);
                        break;
                    case RelationTypes.Met:
                        list.Add(RelationTypesConverter.RelationTypeValue.MET);
                        break;
                    case RelationTypes.CoWorker:
                        list.Add(RelationTypesConverter.RelationTypeValue.CO_WORKER);
                        break;
                    case RelationTypes.Colleague:
                        list.Add(RelationTypesConverter.RelationTypeValue.COLLEAGUE);
                        break;
                    case RelationTypes.CoResident:
                        list.Add(RelationTypesConverter.RelationTypeValue.CO_RESIDENT);
                        break;
                    case RelationTypes.Neighbor:
                        list.Add(RelationTypesConverter.RelationTypeValue.NEIGHBOR);
                        break;
                    case RelationTypes.Child:
                        list.Add(RelationTypesConverter.RelationTypeValue.CHILD);
                        break;
                    case RelationTypes.Parent:
                        list.Add(RelationTypesConverter.RelationTypeValue.PARENT);
                        break;
                    case RelationTypes.Sibling:
                        list.Add(RelationTypesConverter.RelationTypeValue.SIBLING);
                        break;
                    case RelationTypes.Spouse:
                        list.Add(RelationTypesConverter.RelationTypeValue.SPOUSE);
                        break;
                    case RelationTypes.Kin:
                        list.Add(RelationTypesConverter.RelationTypeValue.KIN);
                        break;
                    case RelationTypes.Muse:
                        list.Add(RelationTypesConverter.RelationTypeValue.MUSE);
                        break;
                    case RelationTypes.Crush:
                        list.Add(RelationTypesConverter.RelationTypeValue.CRUSH);
                        break;
                    case RelationTypes.Date:
                        list.Add(RelationTypesConverter.RelationTypeValue.DATE);
                        break;
                    case RelationTypes.Sweetheart:
                        list.Add(RelationTypesConverter.RelationTypeValue.SWEETHEART);
                        break;
                    case RelationTypes.Me:
                        list.Add(RelationTypesConverter.RelationTypeValue.ME);
                        break;
                    case RelationTypes.Agent:
                        list.Add(RelationTypesConverter.RelationTypeValue.AGENT);
                        break;
                    case RelationTypes.Emergency:
                        list.Add(RelationTypesConverter.RelationTypeValue.EMERGENCY);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
