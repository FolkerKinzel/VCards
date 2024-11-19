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

    internal static bool TryParse(ReadOnlySpan<char> value, out Access access)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        if (value.Equals(VCdAccessValue.PUBLIC, comp))
        {
            access = Access.Public;
            return true;
        }

        if (value.Equals(VCdAccessValue.PRIVATE, comp))
        {
            access = Access.Private;
            return true;
        }

        if (value.Equals(VCdAccessValue.CONFIDENTIAL, comp))
        {
            access = Access.Confidential;
            return true;
        }

        access = default;
        return false;
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
