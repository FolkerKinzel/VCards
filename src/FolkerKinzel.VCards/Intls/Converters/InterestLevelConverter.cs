using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class InterestLevelConverter
{
    internal static class Values
    {
        internal const string HIGH = "high";
        internal const string MEDIUM = "medium";
        internal const string LOW = "low";
    }

    internal static InterestLevel? Parse(string val)
    {
        Debug.Assert(val != null);
        Debug.Assert(StringComparer.Ordinal.Equals(val, val.ToLowerInvariant()));

        return val switch
        {
            Values.HIGH => InterestLevel.High,
            Values.MEDIUM => InterestLevel.Medium,
            Values.LOW => InterestLevel.Low,
            _ => null
        };
    }


    internal static string? ToVCardString(this InterestLevel? interest)
    {
        return interest switch
        {
            InterestLevel.High => Values.HIGH,
            InterestLevel.Medium => Values.MEDIUM,
            InterestLevel.Low => Values.LOW,
            _ => null
        };
    }
}

