using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ContentLocationConverter
{
    internal static class Values
    {
        internal const string INLINE = "INLINE";

        internal const string CONTENT_ID = "CONTENT-ID";

        internal const string CID = "CID";

        internal const string URL = "URL";
    }


    internal static ContentLocation Parse(string? value)
    {
        Debug.Assert(value?.ToUpperInvariant() == value);

        return value switch
        {
            Values.CID => ContentLocation.ContentID,
            Values.CONTENT_ID => ContentLocation.ContentID,
            Values.URL => ContentLocation.Url,
            _ => ContentLocation.Inline
        };
    }


    internal static string ToVcfString(this ContentLocation contentLocation)
    {
        return contentLocation switch
        {
            ContentLocation.Inline => Values.INLINE,
            ContentLocation.ContentID => Values.CID,
            ContentLocation.Url => Values.URL,
            _ => Values.INLINE
        };
    }
}
