using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class DataConverter
{
    private static class PropValue
    {
        internal const string BOOLEAN = "BOOLEAN";
        internal const string DATE = "DATE";
        internal const string DATE_AND_OR_TIME = "DATE-AND-OR-TIME";
        internal const string DATE_TIME = "DATE-TIME";
        internal const string FLOAT = "FLOAT";
        internal const string INTEGER = "INTEGER";
        internal const string LANGUAGE_TAG = "LANGUAGE-TAG";
        internal const string TEXT = "TEXT";
        internal const string TIME = "TIME";
        internal const string TIMESTAMP = "TIMESTAMP";
        internal const string URI = "URI";
        internal const string UTC_OFFSET = "UTC-OFFSET";

        internal static class V3_Specific
        {
            internal const string BINARY = "BINARY";
            internal const string VCARD = "VCARD";
            internal const string PHONE_NUMBER = "PHONE-NUMBER";
        }
    }

    internal static Data? Parse(ReadOnlySpan<char> span)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;
        return span.Equals(PropValue.URI, comp) ? Data.Uri
            : span.Equals(PropValue.TEXT, comp) ? Data.Text
            : span.Equals(PropValue.DATE, comp) ? Data.Date
            : span.Equals(PropValue.TIMESTAMP, comp) ? Data.TimeStamp
            : span.Equals(PropValue.DATE_TIME, comp) ? Data.DateTime
            : span.Equals(PropValue.UTC_OFFSET, comp) ? Data.UtcOffset
            : span.Equals(PropValue.V3_Specific.BINARY, comp) ? Data.Binary
            : span.Equals(PropValue.V3_Specific.VCARD, comp) ? Data.VCard
            : span.Equals(PropValue.LANGUAGE_TAG, comp) ? Data.LanguageTag
            : span.Equals(PropValue.DATE_AND_OR_TIME, comp) ? Data.DateAndOrTime
            : span.Equals(PropValue.TIME, comp) ? Data.Time
            : span.Equals(PropValue.V3_Specific.PHONE_NUMBER, comp) ? Data.PhoneNumber
            : span.Equals(PropValue.INTEGER, comp) ? Data.Integer
            : span.Equals(PropValue.FLOAT, comp) ? Data.Float
            : span.Equals(PropValue.BOOLEAN, comp) ? Data.Boolean
            : null;
            
    }

    internal static string? ToVcfString(this Data? s)
    {
        return s switch
        {
            Data.Boolean => PropValue.BOOLEAN,
            Data.Date => PropValue.DATE,
            Data.DateAndOrTime => PropValue.DATE_AND_OR_TIME,
            Data.DateTime => PropValue.DATE_TIME,
            Data.Float => PropValue.FLOAT,
            Data.Integer => PropValue.INTEGER,
            Data.LanguageTag => PropValue.LANGUAGE_TAG,
            Data.Text => PropValue.TEXT,
            Data.Time => PropValue.TIME,
            Data.TimeStamp => PropValue.TIMESTAMP,
            Data.Uri => PropValue.URI,
            Data.UtcOffset => PropValue.UTC_OFFSET,
            Data.Binary => PropValue.V3_Specific.BINARY,
            Data.PhoneNumber => PropValue.V3_Specific.PHONE_NUMBER,
            Data.VCard => PropValue.V3_Specific.VCARD,
            _ => null
        };
    }
}
