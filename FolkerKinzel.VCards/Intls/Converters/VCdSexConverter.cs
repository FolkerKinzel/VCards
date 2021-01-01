using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class VCdSexConverter
    {
        private static class Values
        {
            internal const string Male = "M";
            internal const string Female = "F";
            internal const string Other = "O";
            internal const string NonOrNotApplicable = "N";
            internal const string Unknown = "U";
        }


        internal static VCdSex? Parse(string? value)
        {
            return value is null
                ? null
                : (value.ToUpperInvariant() switch
            {
                Values.Male => VCdSex.Male,
                Values.Female => VCdSex.Female,
                Values.Other => VCdSex.Other,
                Values.NonOrNotApplicable => VCdSex.NonOrNotApplicable,
                Values.Unknown => VCdSex.Unknown,
                _ => (VCdSex?)null
            });
        }


        internal static string? ToVCardString(this VCdSex? sex)
        {
            return sex switch
            {
                VCdSex.Male => Values.Male,
                VCdSex.Female => Values.Female,
                VCdSex.Other => Values.Other,
                VCdSex.NonOrNotApplicable => Values.NonOrNotApplicable,
                VCdSex.Unknown => Values.Unknown,
                _ => null
            };
        }
    }
}
