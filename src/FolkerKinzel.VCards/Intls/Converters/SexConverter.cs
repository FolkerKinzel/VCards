using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class SexConverter
{
    private static class Values
    {
        internal const string MALE = "M";
        internal const string FEMALE = "F";
        internal const string OTHER = "O";
        internal const string NON_OR_NOT_APPLICABLE = "N";
        internal const string UNKNOWN = "U";
    }

    internal static Sex? Parse(string? value)
    {
        return value is null
            ? null
            : (value.ToUpperInvariant() switch
            {
                Values.MALE => Sex.Male,
                Values.FEMALE => Sex.Female,
                Values.OTHER => Sex.Other,
                Values.NON_OR_NOT_APPLICABLE => Sex.NonOrNotApplicable,
                Values.UNKNOWN => Sex.Unknown,
                _ => (Sex?)null
            });
    }

    internal static string? ToVcfString(this Sex? sex)
    {
        return sex switch
        {
            Sex.Male => Values.MALE,
            Sex.Female => Values.FEMALE,
            Sex.Other => Values.OTHER,
            Sex.NonOrNotApplicable => Values.NON_OR_NOT_APPLICABLE,
            Sex.Unknown => Values.UNKNOWN,
            _ => null
        };
    }
}
