using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class VCdDataTypeConverter
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



    internal static VCdDataType? Parse(string? s)
    {
        Debug.Assert(s?.ToUpperInvariant() == s);

        return s switch
        {
            PropValue.BOOLEAN => VCdDataType.Boolean,
            PropValue.DATE => VCdDataType.Date,
            PropValue.DATE_AND_OR_TIME => VCdDataType.DateAndOrTime,
            PropValue.DATE_TIME => VCdDataType.DateTime,
            PropValue.FLOAT => VCdDataType.Float,
            PropValue.INTEGER => VCdDataType.Integer,
            PropValue.LANGUAGE_TAG => VCdDataType.LanguageTag,
            PropValue.TEXT => VCdDataType.Text,
            PropValue.TIME => VCdDataType.Time,
            PropValue.TIMESTAMP => VCdDataType.TimeStamp,
            PropValue.URI => VCdDataType.Uri,
            PropValue.UTC_OFFSET => VCdDataType.UtcOffset,
            PropValue.V3_Specific.BINARY => VCdDataType.Binary,
            PropValue.V3_Specific.PHONE_NUMBER => VCdDataType.PhoneNumber,
            PropValue.V3_Specific.VCARD => VCdDataType.VCard,
            _ => (VCdDataType?)null
        };
    }

    internal static string? ToVcfString(this VCdDataType? s)
    {
        return s switch
        {
            VCdDataType.Boolean => PropValue.BOOLEAN,
            VCdDataType.Date => PropValue.DATE,
            VCdDataType.DateAndOrTime => PropValue.DATE_AND_OR_TIME,
            VCdDataType.DateTime => PropValue.DATE_TIME,
            VCdDataType.Float => PropValue.FLOAT,
            VCdDataType.Integer => PropValue.INTEGER,
            VCdDataType.LanguageTag => PropValue.LANGUAGE_TAG,
            VCdDataType.Text => PropValue.TEXT,
            VCdDataType.Time => PropValue.TIME,
            VCdDataType.TimeStamp => PropValue.TIMESTAMP,
            VCdDataType.Uri => PropValue.URI,
            VCdDataType.UtcOffset => PropValue.UTC_OFFSET,
            VCdDataType.Binary => PropValue.V3_Specific.BINARY,
            VCdDataType.PhoneNumber => PropValue.V3_Specific.PHONE_NUMBER,
            VCdDataType.VCard => PropValue.V3_Specific.VCARD,
            _ => null
        };
    }

}
