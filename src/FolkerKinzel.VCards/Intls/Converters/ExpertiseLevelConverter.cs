using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ExpertiseLevelConverter
{
    private static class Values
    {
        internal const string Beginner = "beginner";
        internal const string Average = "average";
        internal const string Expert = "expert";
    }

    internal static ExpertiseLevel? Parse(string val)
    {
        Debug.Assert(val != null);
        Debug.Assert(StringComparer.Ordinal.Equals(val, val.ToLowerInvariant()));

        return val switch
        {
            Values.Beginner => ExpertiseLevel.Beginner,
            Values.Average => ExpertiseLevel.Average,
            Values.Expert => ExpertiseLevel.Expert,
            _ => null
        };
    }

    internal static string? ToVCardString(this ExpertiseLevel? expertise)
    {
        return expertise switch
        {
            ExpertiseLevel.Beginner => Values.Beginner,
            ExpertiseLevel.Average => Values.Average,
            ExpertiseLevel.Expert => Values.Expert,
            _ => null
        };
    }
}
