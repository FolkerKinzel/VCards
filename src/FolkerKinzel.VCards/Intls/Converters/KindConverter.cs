using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class KindConverter
{
    private static class VCdKindValues
    {
        internal const string INDIVIDUAL = "individual";
        internal const string GROUP = "group";
        internal const string ORGANIZATION = "organization";
        internal const string LOCATION = "location";
        internal const string APPLICATION = "application";
    }

    internal static Kind Parse(string? value)
    {
        return (value?.ToLowerInvariant()) switch
        {
            VCdKindValues.INDIVIDUAL => Kind.Individual,
            VCdKindValues.GROUP => Kind.Group,
            VCdKindValues.ORGANIZATION => Kind.Organization,
            VCdKindValues.LOCATION => Kind.Location,
            VCdKindValues.APPLICATION => Kind.Application,
            _ => Kind.Individual,
        };
    }

    internal static string ToVcfString(this Kind kind)
    {
        return kind switch
        {
            Kind.Individual => VCdKindValues.INDIVIDUAL,
            Kind.Group => VCdKindValues.GROUP,
            Kind.Organization => VCdKindValues.ORGANIZATION,
            Kind.Location => VCdKindValues.LOCATION,
            Kind.Application => VCdKindValues.APPLICATION,
            _ => VCdKindValues.INDIVIDUAL,
        };
    }
}
