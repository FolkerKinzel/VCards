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

    internal static bool TryParse(ReadOnlySpan<char> value, out Kind kind)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        Kind? result = value.Equals(VCdKindValues.INDIVIDUAL, comp) ? Kind.Individual
             : value.Equals(VCdKindValues.GROUP, comp) ? Kind.Group
             : value.Equals(VCdKindValues.ORGANIZATION, comp) ? Kind.Organization
             : value.Equals(VCdKindValues.LOCATION, comp) ? Kind.Location
             : value.Equals(VCdKindValues.APPLICATION, comp) ? Kind.Application
             : null;

        if (result.HasValue)
        {
            kind = result.Value;
            return true;
        }

        kind = default;
        return false;
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
