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

    internal static Expertise? Parse(string? value)
    {
        var span = value.AsSpan();

        return span.Contains(Values.BEGINNER, StringComparison.OrdinalIgnoreCase)
            ? Expertise.Beginner
            : span.Contains(Values.AVERAGE, StringComparison.OrdinalIgnoreCase)
              ? Expertise.Average
              : span.Contains(Values.EXPERT, StringComparison.OrdinalIgnoreCase)
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
