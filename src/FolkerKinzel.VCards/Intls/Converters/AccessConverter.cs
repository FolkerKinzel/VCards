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

    internal static Access Parse(ReadOnlySpan<char> value)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return value.Equals(VCdAccessValue.PUBLIC, comp) ? Access.Public
             : value.Equals(VCdAccessValue.PRIVATE, comp) ? Access.Private
             : value.Equals(VCdAccessValue.CONFIDENTIAL, comp) ? Access.Confidential
             : Access.Public;
    }

    internal static string ToVCardString(this Access kind)
    {
        return kind switch
        {
            Access.Public => VCdAccessValue.PUBLIC,
            Access.Private => VCdAccessValue.PRIVATE,
            Access.Confidential => VCdAccessValue.CONFIDENTIAL,
            _ => VCdAccessValue.PUBLIC
        };
    }
}
