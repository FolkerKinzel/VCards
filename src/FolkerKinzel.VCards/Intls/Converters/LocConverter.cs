using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class LocConverter
{
    internal static class Values
    {
        internal const string INLINE = "INLINE";
        internal const string CONTENT_ID = "CONTENT-ID";
        internal const string CID = "CID";
        internal const string URL = "URL";
    }

    internal static Loc Parse(string? value)
    {
        Debug.Assert(value?.ToUpperInvariant() == value);

        return value switch
        {
            Values.CID => Loc.Cid,
            Values.CONTENT_ID => Loc.Cid,
            Values.URL => Loc.Url,
            _ => Loc.Inline
        };
    }

    internal static string ToVcfString(this Loc contentLocation)
    {
        return contentLocation switch
        {
            Loc.Inline => Values.INLINE,
            Loc.Cid => Values.CID,
            Loc.Url => Values.URL,
            _ => Values.INLINE
        };
    }
}
