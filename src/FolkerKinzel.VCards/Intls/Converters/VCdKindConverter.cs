using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class VCdKindConverter
{
    private static class VCdKindValues
    {
        internal const string INDIVIDUAL = "individual";
        internal const string GROUP = "group";
        internal const string ORGANIZATION = "organization";
        internal const string LOCATION = "location";
        internal const string APPLICATION = "application";
    }

    internal static VCdKind Parse(string? value)
    {
        return (value?.ToLowerInvariant()) switch
        {
            VCdKindValues.INDIVIDUAL => VCdKind.Individual,
            VCdKindValues.GROUP => VCdKind.Group,
            VCdKindValues.ORGANIZATION => VCdKind.Organization,
            VCdKindValues.LOCATION => VCdKind.Location,
            VCdKindValues.APPLICATION => VCdKind.Application,
            _ => VCdKind.Individual,
        };
    }


    internal static string ToVcfString(this VCdKind kind)
    {
        return kind switch
        {
            VCdKind.Individual => VCdKindValues.INDIVIDUAL,
            VCdKind.Group => VCdKindValues.GROUP,
            VCdKind.Organization => VCdKindValues.ORGANIZATION,
            VCdKind.Location => VCdKindValues.LOCATION,
            VCdKind.Application => VCdKindValues.APPLICATION,
            _ => VCdKindValues.INDIVIDUAL,
        };
    }
}
