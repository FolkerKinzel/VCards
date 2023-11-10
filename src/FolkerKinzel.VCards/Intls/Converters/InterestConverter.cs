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

    internal static Interest? Parse(string val)
    {
        Debug.Assert(val != null);
        Debug.Assert(StringComparer.Ordinal.Equals(val, val.ToLowerInvariant()));

        return val switch
        {
            Values.HIGH => Interest.High,
            Values.MEDIUM => Interest.Medium,
            Values.LOW => Interest.Low,
            _ => null
        };
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

