using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class VCdContentLocationConverter
{
    internal static class Values
    {
        internal const string INLINE = "INLINE";

        internal const string CONTENT_ID = "CONTENT-ID";

        internal const string CID = "CID";

        internal const string URL = "URL";
    }


    internal static VCdContentLocation Parse(string? value)
    {
        Debug.Assert(value?.ToUpperInvariant() == value);

        return value switch
        {
            Values.CID => VCdContentLocation.ContentID,
            Values.CONTENT_ID => VCdContentLocation.ContentID,
            Values.URL => VCdContentLocation.Url,
            _ => VCdContentLocation.Inline
        };
    }


    internal static string ToVcfString(this VCdContentLocation contentLocation)
    {
        return contentLocation switch
        {
            VCdContentLocation.Inline => Values.INLINE,
            VCdContentLocation.ContentID => Values.CID,
            VCdContentLocation.Url => Values.URL,
            _ => Values.INLINE
        };
    }
}
