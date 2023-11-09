using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ExpertiseConverter
{
    private static class Values
    {
        internal const string BEGINNER = "beginner";
        internal const string AVERAGE = "average";
        internal const string EXPERT = "expert";
    }

    internal static Expertise? Parse(string val)
    {
        Debug.Assert(val != null);
        Debug.Assert(StringComparer.Ordinal.Equals(val, val.ToLowerInvariant()));

        return val switch
        {
            Values.BEGINNER => Expertise.Beginner,
            Values.AVERAGE => Expertise.Average,
            Values.EXPERT => Expertise.Expert,
            _ => null
        };
    }

    internal static string? ToVcfString(this Expertise? expertise)
    {
        return expertise switch
        {
            Expertise.Beginner => Values.BEGINNER,
            Expertise.Average => Values.AVERAGE,
            Expertise.Expert => Values.EXPERT,
            _ => null
        };
    }
}
