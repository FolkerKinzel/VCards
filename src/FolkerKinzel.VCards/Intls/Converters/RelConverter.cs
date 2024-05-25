using FolkerKinzel.VCards.Enums;

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

    internal static Rel? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(RelationTypeValue.SPOUSE, comp) ? Rel.Spouse
            : typeValue.Equals(RelationTypeValue.SWEETHEART, comp) ? Rel.Sweetheart
            : typeValue.Equals(RelationTypeValue.AGENT, comp) ? Rel.Agent
            : typeValue.Equals(RelationTypeValue.CHILD, comp) ? Rel.Child
            : typeValue.Equals(RelationTypeValue.PARENT, comp) ? Rel.Parent
            : typeValue.Equals(RelationTypeValue.NEIGHBOR, comp) ? Rel.Neighbor
            : typeValue.Equals(RelationTypeValue.SIBLING, comp) ? Rel.Sibling
            : typeValue.Equals(RelationTypeValue.FRIEND, comp) ? Rel.Friend
            : typeValue.Equals(RelationTypeValue.CO_WORKER, comp) ? Rel.CoWorker
            : typeValue.Equals(RelationTypeValue.COLLEAGUE, comp) ? Rel.Colleague
            : typeValue.Equals(RelationTypeValue.CO_RESIDENT, comp) ? Rel.CoResident
            : typeValue.Equals(RelationTypeValue.ME, comp) ? Rel.Me
            : typeValue.Equals(RelationTypeValue.EMERGENCY, comp) ? Rel.Emergency
            : typeValue.Equals(RelationTypeValue.DATE, comp) ? Rel.Date
            : typeValue.Equals(RelationTypeValue.CONTACT, comp) ? Rel.Contact
            : typeValue.Equals(RelationTypeValue.ACQUAINTANCE, comp) ? Rel.Acquaintance
            : typeValue.Equals(RelationTypeValue.MET, comp) ? Rel.Met
            : typeValue.Equals(RelationTypeValue.KIN, comp) ? Rel.Kin
            : typeValue.Equals(RelationTypeValue.MUSE, comp) ? Rel.Muse
            : typeValue.Equals(RelationTypeValue.CRUSH, comp) ? Rel.Crush
            : null;
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
