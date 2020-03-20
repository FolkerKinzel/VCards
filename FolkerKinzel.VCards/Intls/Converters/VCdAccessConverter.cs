using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class VCdAccessConverter
    {
        private static class VCdAccessValue
        {
            internal const string Public = "PUBLIC";
            internal const string Private = "PRIVATE";
            internal const string Confidential = "CONFIDENTIAL";
        }

        internal static VCdAccess Parse(string? value)
        {
            return (value?.ToUpperInvariant()) switch
            {
                VCdAccessValue.Public => VCdAccess.Public,
                VCdAccessValue.Private => VCdAccess.Private,
                VCdAccessValue.Confidential => VCdAccess.Confidential,
                _ => VCdAccess.Public
            };
        }


        internal static string ToVCardString(this VCdAccess kind)
        {
            return (kind) switch
            {
                VCdAccess.Public => VCdAccessValue.Public,
                VCdAccess.Private => VCdAccessValue.Private,
                VCdAccess.Confidential => VCdAccessValue.Confidential,
                _ => VCdAccessValue.Public
            };
        }

    }
}
