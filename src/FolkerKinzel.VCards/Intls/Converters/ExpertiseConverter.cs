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

    internal static Expertise? Parse(ReadOnlySpan<char> span)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return span.Equals(Values.BEGINNER, comp)
            ? Expertise.Beginner
            : span.Equals(Values.AVERAGE, comp)
              ? Expertise.Average
              : span.Equals(Values.EXPERT, comp)
                    ? Expertise.Expert
                    : null;
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
