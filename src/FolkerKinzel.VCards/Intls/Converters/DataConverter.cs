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

    internal static Data? Parse(string? s)
    {
        Debug.Assert(s?.ToUpperInvariant() == s);

        return s switch
        {
            PropValue.BOOLEAN => Data.Boolean,
            PropValue.DATE => Data.Date,
            PropValue.DATE_AND_OR_TIME => Data.DateAndOrTime,
            PropValue.DATE_TIME => Data.DateTime,
            PropValue.FLOAT => Data.Float,
            PropValue.INTEGER => Data.Integer,
            PropValue.LANGUAGE_TAG => Data.LanguageTag,
            PropValue.TEXT => Data.Text,
            PropValue.TIME => Data.Time,
            PropValue.TIMESTAMP => Data.TimeStamp,
            PropValue.URI => Data.Uri,
            PropValue.UTC_OFFSET => Data.UtcOffset,
            PropValue.V3_Specific.BINARY => Data.Binary,
            PropValue.V3_Specific.PHONE_NUMBER => Data.PhoneNumber,
            PropValue.V3_Specific.VCARD => Data.VCard,
            _ => (Data?)null
        };
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
