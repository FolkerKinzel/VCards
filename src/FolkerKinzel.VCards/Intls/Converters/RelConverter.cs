using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class RelConverter
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

    internal const Rel DEFINED_RELATION_TYPES_VALUES = Rel.Contact
                                                               | Rel.Acquaintance
                                                               | Rel.Friend
                                                               | Rel.Met
                                                               | Rel.CoWorker
                                                               | Rel.Colleague
                                                               | Rel.CoResident
                                                               | Rel.Neighbor
                                                               | Rel.Child
                                                               | Rel.Parent
                                                               | Rel.Sibling
                                                               | Rel.Spouse
                                                               | Rel.Kin
                                                               | Rel.Muse
                                                               | Rel.Crush
                                                               | Rel.Date
                                                               | Rel.Sweetheart
                                                               | Rel.Me
                                                               | Rel.Agent
                                                               | Rel.Emergency;

    internal const int RELATION_TYPES_MIN_BIT = 0;
    internal const int RELATION_TYPES_MAX_BIT = 19;

    internal static Rel? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            RelationTypeValue.CONTACT => Rel.Contact,
            RelationTypeValue.ACQUAINTANCE => Rel.Acquaintance,
            RelationTypeValue.FRIEND => Rel.Friend,
            RelationTypeValue.MET => Rel.Met,
            RelationTypeValue.CO_WORKER => Rel.CoWorker,
            RelationTypeValue.COLLEAGUE => Rel.Colleague,
            RelationTypeValue.CO_RESIDENT => Rel.CoResident,
            RelationTypeValue.NEIGHBOR => Rel.Neighbor,
            RelationTypeValue.CHILD => Rel.Child,
            RelationTypeValue.PARENT => Rel.Parent,
            RelationTypeValue.SIBLING => Rel.Sibling,
            RelationTypeValue.SPOUSE => Rel.Spouse,
            RelationTypeValue.KIN => Rel.Kin,
            RelationTypeValue.MUSE => Rel.Muse,
            RelationTypeValue.CRUSH => Rel.Crush,
            RelationTypeValue.DATE => Rel.Date,
            RelationTypeValue.SWEETHEART => Rel.Sweetheart,
            RelationTypeValue.ME => Rel.Me,
            RelationTypeValue.AGENT => Rel.Agent,
            RelationTypeValue.EMERGENCY => Rel.Emergency,
            _ => null
        };
    }

    internal static string ToVcfString(this Rel value)
        => value switch
        {
            Rel.Contact => RelationTypeValue.CONTACT,
            Rel.Acquaintance => RelationTypeValue.ACQUAINTANCE,
            Rel.Friend => RelationTypeValue.FRIEND,
            Rel.Met => RelationTypeValue.MET,
            Rel.CoWorker => RelationTypeValue.CO_WORKER,
            Rel.Colleague => RelationTypeValue.COLLEAGUE,
            Rel.CoResident => RelationTypeValue.CO_RESIDENT,
            Rel.Neighbor => RelationTypeValue.NEIGHBOR,
            Rel.Child => RelationTypeValue.CHILD,
            Rel.Parent => RelationTypeValue.PARENT,
            Rel.Sibling => RelationTypeValue.SIBLING,
            Rel.Spouse => RelationTypeValue.SPOUSE,
            Rel.Kin => RelationTypeValue.KIN,
            Rel.Muse => RelationTypeValue.MUSE,
            Rel.Crush => RelationTypeValue.CRUSH,
            Rel.Date => RelationTypeValue.DATE,
            Rel.Sweetheart => RelationTypeValue.SWEETHEART,
            Rel.Me => RelationTypeValue.ME,
            Rel.Agent => RelationTypeValue.AGENT,
            Rel.Emergency => RelationTypeValue.EMERGENCY,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
