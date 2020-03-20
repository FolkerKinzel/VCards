using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class VCdKindConverter
    {
        private static class VCdKindValues
        {
            internal const string Individual = "individual";
            internal const string Group = "group";
            internal const string Organization = "organization";
            internal const string Location = "location";
            internal const string Application = "application";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Zeichenfolgen in Großbuchstaben normalisieren", Justification = "<Ausstehend>")]
        internal static VCdKind Parse(string? value)
        {
            return (value?.ToLowerInvariant()) switch
            {
                VCdKindValues.Individual => VCdKind.Individual,
                VCdKindValues.Group => VCdKind.Group,
                VCdKindValues.Organization => VCdKind.Organization,
                VCdKindValues.Location => VCdKind.Location,
                VCdKindValues.Application => VCdKind.Application,
                _ => VCdKind.Individual,
            };
        }


        internal static string ToVCardString(this VCdKind kind)
        {
            return (kind) switch
            {
                VCdKind.Individual => VCdKindValues.Individual,
                VCdKind.Group => VCdKindValues.Group,
                VCdKind.Organization => VCdKindValues.Organization,
                VCdKind.Location => VCdKindValues.Location,
                VCdKind.Application => VCdKindValues.Application,
                _ => VCdKindValues.Individual,
            };
        }
    }
}
