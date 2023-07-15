using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AccessConverter
{
    private static class VCdAccessValue
    {
        internal const string Public = "PUBLIC";
        internal const string Private = "PRIVATE";
        internal const string Confidential = "CONFIDENTIAL";
    }

    internal static Access Parse(string? value)
    {
        return (value?.ToUpperInvariant()) switch
        {
            VCdAccessValue.Public => Access.Public,
            VCdAccessValue.Private => Access.Private,
            VCdAccessValue.Confidential => Access.Confidential,
            _ => Access.Public
        };
    }


    internal static string ToVCardString(this Access kind)
    {
        return kind switch
        {
            Access.Public => VCdAccessValue.Public,
            Access.Private => VCdAccessValue.Private,
            Access.Confidential => VCdAccessValue.Confidential,
            _ => VCdAccessValue.Public
        };
    }

}
