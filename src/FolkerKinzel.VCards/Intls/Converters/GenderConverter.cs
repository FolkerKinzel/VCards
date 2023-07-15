using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GenderConverter
{
    private static class Values
    {
        internal const string Male = "M";
        internal const string Female = "F";
        internal const string Other = "O";
        internal const string NonOrNotApplicable = "N";
        internal const string Unknown = "U";
    }

    internal static Gender? Parse(string? value)
    {
        return value is null
            ? null
            : (value.ToUpperInvariant() switch
            {
                Values.Male => Gender.Male,
                Values.Female => Gender.Female,
                Values.Other => Gender.Other,
                Values.NonOrNotApplicable => Gender.NonOrNotApplicable,
                Values.Unknown => Gender.Unknown,
                _ => (Gender?)null
            });
    }

    internal static string? ToVcfString(this Gender? sex)
    {
        return sex switch
        {
            Gender.Male => Values.Male,
            Gender.Female => Values.Female,
            Gender.Other => Values.Other,
            Gender.NonOrNotApplicable => Values.NonOrNotApplicable,
            Gender.Unknown => Values.Unknown,
            _ => null
        };
    }
}
