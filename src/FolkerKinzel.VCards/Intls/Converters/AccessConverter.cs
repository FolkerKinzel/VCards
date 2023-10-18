using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AccessConverter
{
    private static class VCdAccessValue
    {
        internal const string PUBLIC = "PUBLIC";
        internal const string PRIVATE = "PRIVATE";
        internal const string CONFIDENTIAL = "CONFIDENTIAL";
    }

    internal static Access Parse(string? value)
    {
        return (value?.ToUpperInvariant()) switch
        {
            VCdAccessValue.PUBLIC => Access.Public,
            VCdAccessValue.PRIVATE => Access.Private,
            VCdAccessValue.CONFIDENTIAL => Access.Confidential,
            _ => Access.Public
        };
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
