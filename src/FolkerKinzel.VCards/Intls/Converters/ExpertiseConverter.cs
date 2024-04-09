using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ExpertiseConverter
{
    private static class Values
    {
        internal const string BEGINNER = "beginner";
        internal const string AVERAGE = "average";
        internal const string EXPERT = "expert";
    }

    internal static Expertise? Parse(ReadOnlySpan<char> val) =>
         val.Equals(Values.BEGINNER, StringComparison.OrdinalIgnoreCase)
            ? Expertise.Beginner
            : val.Equals(Values.AVERAGE, StringComparison.OrdinalIgnoreCase)
              ? Expertise.Average
              : val.Equals(Values.EXPERT, StringComparison.OrdinalIgnoreCase)
                    ? Expertise.Expert 
                    : null;
    

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
