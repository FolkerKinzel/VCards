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

    internal static Interest? Parse(string? value)
    {
        var val = value.AsSpan();

        return val.Contains(Values.HIGH, StringComparison.OrdinalIgnoreCase)
            ? Interest.High
            : val.Contains(Values.MEDIUM, StringComparison.OrdinalIgnoreCase)
              ? Interest.Medium
              : val.Contains(Values.LOW, StringComparison.OrdinalIgnoreCase)
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

