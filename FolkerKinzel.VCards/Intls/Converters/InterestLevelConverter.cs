using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class InterestLevelConverter
    {
        internal static class Values
        {
            internal const string High = "high";
            internal const string Medium = "medium";
            internal const string Low = "low";
        }

        internal static InterestLevel? Parse(string val)
        {
            Debug.Assert(val != null);

            return val.ToLowerInvariant() switch
            {
                Values.High => InterestLevel.High,
                Values.Medium => InterestLevel.Medium,
                Values.Low => InterestLevel.Low,
                _ => (InterestLevel?)null,
            };
        }


        internal static string? ToVCardString(this InterestLevel? interest)
        {
            return interest switch
            {
                InterestLevel.High => Values.High,
                InterestLevel.Medium => Values.Medium,
                InterestLevel.Low => Values.Low,
                _ => null,
            };
        }
    }
}

