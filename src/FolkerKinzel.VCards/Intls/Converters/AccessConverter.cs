using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AccessConverter
{
    private static class VCdAccessValue
    {
        internal const string PUBLIC = "PUBLIC";
        internal const string PRIVATE = "PRIVATE";
        internal const string CONFIDENTIAL = "CONFIDENTIAL";
    }

    internal static Acs Parse(string? value)
    {
        return (value?.ToUpperInvariant()) switch
        {
            VCdAccessValue.PUBLIC => Acs.Public,
            VCdAccessValue.PRIVATE => Acs.Private,
            VCdAccessValue.CONFIDENTIAL => Acs.Confidential,
            _ => Acs.Public
        };
    }

    internal static string ToVCardString(this Acs kind)
    {
        return kind switch
        {
            Acs.Public => VCdAccessValue.PUBLIC,
            Acs.Private => VCdAccessValue.PRIVATE,
            Acs.Confidential => VCdAccessValue.CONFIDENTIAL,
            _ => VCdAccessValue.PUBLIC
        };
    }
}
