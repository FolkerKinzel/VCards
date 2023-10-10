using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GenderConverter
{
    private static class Values
    {
        internal const string MALE = "M";
        internal const string FEMALE = "F";
        internal const string OTHER = "O";
        internal const string NON_OR_NOT_APPLICABLE = "N";
        internal const string UNKNOWN = "U";
    }

    internal static Gender? Parse(string? value)
    {
        return value is null
            ? null
            : (value.ToUpperInvariant() switch
            {
                Values.MALE => Gender.Male,
                Values.FEMALE => Gender.Female,
                Values.OTHER => Gender.Other,
                Values.NON_OR_NOT_APPLICABLE => Gender.NonOrNotApplicable,
                Values.UNKNOWN => Gender.Unknown,
                _ => (Gender?)null
            });
    }

    internal static string? ToVcfString(this Gender? sex)
    {
        return sex switch
        {
            Gender.Male => Values.MALE,
            Gender.Female => Values.FEMALE,
            Gender.Other => Values.OTHER,
            Gender.NonOrNotApplicable => Values.NON_OR_NOT_APPLICABLE,
            Gender.Unknown => Values.UNKNOWN,
            _ => null
        };
    }
}
