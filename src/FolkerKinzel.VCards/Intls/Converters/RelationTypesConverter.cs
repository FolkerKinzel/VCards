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

    internal static string ToVcfString(this RelationTypes value)
        => value switch
        {
            RelationTypes.Contact => RelationTypeValue.CONTACT,
            RelationTypes.Acquaintance => RelationTypeValue.ACQUAINTANCE,
            RelationTypes.Friend => RelationTypeValue.FRIEND,
            RelationTypes.Met => RelationTypeValue.MET,
            RelationTypes.CoWorker => RelationTypeValue.CO_WORKER,
            RelationTypes.Colleague => RelationTypeValue.COLLEAGUE,
            RelationTypes.CoResident => RelationTypeValue.CO_RESIDENT,
            RelationTypes.Neighbor => RelationTypeValue.NEIGHBOR,
            RelationTypes.Child => RelationTypeValue.CHILD,
            RelationTypes.Parent => RelationTypeValue.PARENT,
            RelationTypes.Sibling => RelationTypeValue.SIBLING,
            RelationTypes.Spouse => RelationTypeValue.SPOUSE,
            RelationTypes.Kin => RelationTypeValue.KIN,
            RelationTypes.Muse => RelationTypeValue.MUSE,
            RelationTypes.Crush => RelationTypeValue.CRUSH,
            RelationTypes.Date => RelationTypeValue.DATE,
            RelationTypes.Sweetheart => RelationTypeValue.SWEETHEART,
            RelationTypes.Me => RelationTypeValue.ME,
            RelationTypes.Agent => RelationTypeValue.AGENT,
            RelationTypes.Emergency => RelationTypeValue.EMERGENCY,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    
}
