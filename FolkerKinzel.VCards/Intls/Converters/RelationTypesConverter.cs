using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.Helpers;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class RelationTypesConverter
    {
        internal static class RelationTypeValue
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


        internal static RelationTypes? Parse(string? typeValue, RelationTypes? relationType)
        {
            Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

            return typeValue switch
            {
                RelationTypeValue.CONTACT => relationType.Set(RelationTypes.Contact),
                RelationTypeValue.ACQUAINTANCE => relationType.Set(RelationTypes.Acquaintance),
                RelationTypeValue.FRIEND => relationType.Set(RelationTypes.Friend),
                RelationTypeValue.MET => relationType.Set(RelationTypes.Met),
                RelationTypeValue.CO_WORKER => relationType.Set(RelationTypes.CoWorker),
                RelationTypeValue.COLLEAGUE => relationType.Set(RelationTypes.Colleague),
                RelationTypeValue.CO_RESIDENT => relationType.Set(RelationTypes.CoResident),
                RelationTypeValue.NEIGHBOR => relationType.Set(RelationTypes.Neighbor),
                RelationTypeValue.CHILD => relationType.Set(RelationTypes.Child),
                RelationTypeValue.PARENT => relationType.Set(RelationTypes.Parent),
                RelationTypeValue.SIBLING => relationType.Set(RelationTypes.Sibling),
                RelationTypeValue.SPOUSE => relationType.Set(RelationTypes.Spouse),
                RelationTypeValue.KIN => relationType.Set(RelationTypes.Kin),
                RelationTypeValue.MUSE => relationType.Set(RelationTypes.Muse),
                RelationTypeValue.CRUSH => relationType.Set(RelationTypes.Crush),
                RelationTypeValue.DATE => relationType.Set(RelationTypes.Date),
                RelationTypeValue.SWEETHEART => relationType.Set(RelationTypes.Sweetheart),
                RelationTypeValue.ME => relationType.Set(RelationTypes.Me),
                RelationTypeValue.AGENT => relationType.Set(RelationTypes.Agent),
                RelationTypeValue.EMERGENCY => relationType.Set(RelationTypes.Emergency),
                _ => relationType,
            };
        }
    }
}
