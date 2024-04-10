using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;

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
        var span = value.AsSpan().TrimStart(VcfDeserializationInfo.TRIM_CHARS);

        return span.StartsWith(Values.HIGH, StringComparison.OrdinalIgnoreCase)
            ? Interest.High
            : span.StartsWith(Values.MEDIUM, StringComparison.OrdinalIgnoreCase)
              ? Interest.Medium
              : span.StartsWith(Values.LOW, StringComparison.OrdinalIgnoreCase)
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

