using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;

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

        return span.StartsWith(Values.BEGINNER, comp)
            ? Expertise.Beginner
            : span.StartsWith(Values.AVERAGE, comp)
              ? Expertise.Average
              : span.StartsWith(Values.EXPERT, comp)
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
