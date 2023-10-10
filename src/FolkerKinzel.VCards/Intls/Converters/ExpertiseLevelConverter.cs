using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ExpertiseLevelConverter
{
    private static class Values
    {
        internal const string BEGINNER = "beginner";
        internal const string AVERAGE = "average";
        internal const string EXPERT = "expert";
    }

    internal static ExpertiseLevel? Parse(string val)
    {
        Debug.Assert(val != null);
        Debug.Assert(StringComparer.Ordinal.Equals(val, val.ToLowerInvariant()));

        return val switch
        {
            Values.BEGINNER => ExpertiseLevel.Beginner,
            Values.AVERAGE => ExpertiseLevel.Average,
            Values.EXPERT => ExpertiseLevel.Expert,
            _ => null
        };
    }

    internal static string? ToVcfString(this ExpertiseLevel? expertise)
    {
        return expertise switch
        {
            ExpertiseLevel.Beginner => Values.BEGINNER,
            ExpertiseLevel.Average => Values.AVERAGE,
            ExpertiseLevel.Expert => Values.EXPERT,
            _ => null
        };
    }
}
