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

    internal static Loc? Parse(ReadOnlySpan<char> value)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return value.Equals(Values.CID, comp) ? Loc.Cid
             : value.Equals(Values.CONTENT_ID, comp) ? Loc.Cid
             : value.Equals(Values.URL, comp) ? Loc.Url
             : value.Equals(Values.INLINE, comp) ? Loc.Inline
             : null;
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
