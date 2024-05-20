using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class InterestConverter
{
    internal static class Values
    {
        internal const string HIGH = "high";
        internal const string MEDIUM = "medium";
        internal const string LOW = "low";
    }

    internal static Interest? Parse(ReadOnlySpan<char> span)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return span.Equals(Values.HIGH, comp)
            ? Interest.High
            : span.Equals(Values.MEDIUM, comp)
              ? Interest.Medium
              : span.Equals(Values.LOW, comp)
                ? Interest.Low
                : null;
    }

    internal static string? ToVCardString(this Interest? interest)
    {
        return interest switch
        {
            Interest.High => Values.HIGH,
            Interest.Medium => Values.MEDIUM,
            Interest.Low => Values.LOW,
            _ => null
        };
    }
}

