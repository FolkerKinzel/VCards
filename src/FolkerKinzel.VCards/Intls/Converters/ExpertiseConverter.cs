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

    internal static Expertise? Parse(string? value)
    {
        var span = value.AsSpan().TrimStart(VcfDeserializationInfo.TRIM_CHARS);

        return span.StartsWith(Values.BEGINNER, StringComparison.OrdinalIgnoreCase)
            ? Expertise.Beginner
            : span.StartsWith(Values.AVERAGE, StringComparison.OrdinalIgnoreCase)
              ? Expertise.Average
              : span.StartsWith(Values.EXPERT, StringComparison.OrdinalIgnoreCase)
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
