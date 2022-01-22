using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class RelationTypesConverter
{
    private static class RelationTypeValue
    {
        internal const string CONTACT = "CONTACT";
        internal const string ACQUAINTANCE = "ACQUAINTANCE";
        internal const string FRIEND = "FRIEND";
        internal const string MET = "MET";
        internal const string CO_WORKER = "CO-WORKER";
        internal const string COLLEAGUE = "COLLEAGUE";
        internal const string CO_RESIDENT = "CO-RESIDENT";
        internal const string NEIGHBOR = "NEIGHBOR";
        internal const string CHILD = "CHILD";
        internal const string PARENT = "PARENT";
        internal const string SIBLING = "SIBLING";
        internal const string SPOUSE = "SPOUSE";
        internal const string KIN = "KIN";
        internal const string MUSE = "MUSE";
        internal const string CRUSH = "CRUSH";
        internal const string DATE = "DATE";
        internal const string SWEETHEART = "SWEETHEART";
        internal const string ME = "ME";
        internal const string AGENT = "AGENT";
        internal const string EMERGENCY = "EMERGENCY";
    }

    internal const RelationTypes DEFINED_RELATION_TYPES_VALUES = RelationTypes.Contact
                                                            | RelationTypes.Acquaintance
                                                            | RelationTypes.Friend
                                                            | RelationTypes.Met
                                                            | RelationTypes.CoWorker
                                                            | RelationTypes.Colleague
                                                            | RelationTypes.CoResident
                                                            | RelationTypes.Neighbor
                                                            | RelationTypes.Child
                                                            | RelationTypes.Parent
                                                            | RelationTypes.Sibling
                                                            | RelationTypes.Spouse
                                                            | RelationTypes.Kin
                                                            | RelationTypes.Muse
                                                            | RelationTypes.Crush
                                                            | RelationTypes.Date
                                                            | RelationTypes.Sweetheart
                                                            | RelationTypes.Me
                                                            | RelationTypes.Agent
                                                            | RelationTypes.Emergency;


    internal const int RelationTypesMinBit = 0;
    internal const int RelationTypesMaxBit = 19;

    internal static RelationTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            RelationTypeValue.CONTACT => RelationTypes.Contact,
            RelationTypeValue.ACQUAINTANCE => RelationTypes.Acquaintance,
            RelationTypeValue.FRIEND => RelationTypes.Friend,
            RelationTypeValue.MET => RelationTypes.Met,
            RelationTypeValue.CO_WORKER => RelationTypes.CoWorker,
            RelationTypeValue.COLLEAGUE => RelationTypes.Colleague,
            RelationTypeValue.CO_RESIDENT => RelationTypes.CoResident,
            RelationTypeValue.NEIGHBOR => RelationTypes.Neighbor,
            RelationTypeValue.CHILD => RelationTypes.Child,
            RelationTypeValue.PARENT => RelationTypes.Parent,
            RelationTypeValue.SIBLING => RelationTypes.Sibling,
            RelationTypeValue.SPOUSE => RelationTypes.Spouse,
            RelationTypeValue.KIN => RelationTypes.Kin,
            RelationTypeValue.MUSE => RelationTypes.Muse,
            RelationTypeValue.CRUSH => RelationTypes.Crush,
            RelationTypeValue.DATE => RelationTypes.Date,
            RelationTypeValue.SWEETHEART => RelationTypes.Sweetheart,
            RelationTypeValue.ME => RelationTypes.Me,
            RelationTypeValue.AGENT => RelationTypes.Agent,
            RelationTypeValue.EMERGENCY => RelationTypes.Emergency,
            _ => null
        };
    }

    internal static string ToVcfString(RelationTypes value)
    {
        switch (value)
        {
            case RelationTypes.Contact:
                return RelationTypeValue.CONTACT;
            case RelationTypes.Acquaintance:
                return RelationTypeValue.ACQUAINTANCE;
            case RelationTypes.Friend:
                return RelationTypeValue.FRIEND;
            case RelationTypes.Met:
                return RelationTypeValue.MET;
            case RelationTypes.CoWorker:
                return RelationTypeValue.CO_WORKER;
            case RelationTypes.Colleague:
                return RelationTypeValue.COLLEAGUE;
            case RelationTypes.CoResident:
                return RelationTypeValue.CO_RESIDENT;
            case RelationTypes.Neighbor:
                return RelationTypeValue.NEIGHBOR;
            case RelationTypes.Child:
                return RelationTypeValue.CHILD;
            case RelationTypes.Parent:
                return RelationTypeValue.PARENT;
            case RelationTypes.Sibling:
                return RelationTypeValue.SIBLING;
            case RelationTypes.Spouse:
                return RelationTypeValue.SPOUSE;
            case RelationTypes.Kin:
                return RelationTypeValue.KIN;
            case RelationTypes.Muse:
                return RelationTypeValue.MUSE;
            case RelationTypes.Crush:
                return RelationTypeValue.CRUSH;
            case RelationTypes.Date:
                return RelationTypeValue.DATE;
            case RelationTypes.Sweetheart:
                return RelationTypeValue.SWEETHEART;
            case RelationTypes.Me:
                return RelationTypeValue.ME;
            case RelationTypes.Agent:
                return RelationTypeValue.AGENT;
            case RelationTypes.Emergency:
                return RelationTypeValue.EMERGENCY;
            default:
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
