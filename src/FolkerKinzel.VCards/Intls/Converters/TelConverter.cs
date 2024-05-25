using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class TelConverter
{
    internal static class PhoneTypesValue
    {
        internal const string VOICE = "VOICE";
        internal const string FAX = "FAX";
        internal const string MSG = "MSG";
        internal const string CELL = "CELL";
        internal const string PAGER = "PAGER";
        internal const string BBS = "BBS";
        internal const string MODEM = "MODEM";
        internal const string CAR = "CAR";
        internal const string ISDN = "ISDN";
        internal const string VIDEO = "VIDEO";
        internal const string TEXTPHONE = "TEXTPHONE";
        internal const string TEXT = "TEXT";
        internal const string PCS = "PCS";
    }

    internal const Tel DEFINED_PHONE_TYPES_VALUES = Tel.Voice
                                                     | Tel.Fax
                                                     | Tel.Msg
                                                     | Tel.Cell
                                                     | Tel.Pager
                                                     | Tel.BBS
                                                     | Tel.Modem
                                                     | Tel.Car
                                                     | Tel.ISDN
                                                     | Tel.Video
                                                     | Tel.PCS
                                                     | Tel.TextPhone
                                                     | Tel.Text;

    internal const int PHONE_TYPES_MIN_BIT = 0;
    internal const int PHONE_TYPES_MAX_BIT = 12;

    internal static Tel? Parse(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(PhoneTypesValue.CELL, comp) ? Tel.Cell
            : typeValue.Equals(PhoneTypesValue.VOICE, comp) ? Tel.Voice
            : typeValue.Equals(PhoneTypesValue.TEXT, comp) ? Tel.Text
            : typeValue.Equals(PhoneTypesValue.FAX, comp) ? Tel.Fax
            : typeValue.Equals(PhoneTypesValue.VIDEO, comp) ? Tel.Video
            : typeValue.Equals(PhoneTypesValue.TEXTPHONE, comp) ? Tel.TextPhone
            : typeValue.Equals(PhoneTypesValue.MSG, comp) ? Tel.Msg
            : typeValue.Equals(PhoneTypesValue.PAGER, comp) ? Tel.Pager
            : typeValue.Equals(PhoneTypesValue.BBS, comp) ? Tel.BBS
            : typeValue.Equals(PhoneTypesValue.MODEM, comp) ? Tel.Modem
            : typeValue.Equals(PhoneTypesValue.CAR, comp) ? Tel.Car
            : typeValue.Equals(PhoneTypesValue.ISDN, comp) ? Tel.ISDN
            : typeValue.Equals(PhoneTypesValue.PCS, comp) ? Tel.PCS
            : null;
    }

    internal static string ToVcfString(this Tel value)
        => value switch
        {
            Tel.Voice => PhoneTypesValue.VOICE,
            Tel.Fax => PhoneTypesValue.FAX,
            Tel.Msg => PhoneTypesValue.MSG,
            Tel.Cell => PhoneTypesValue.CELL,
            Tel.Pager => PhoneTypesValue.PAGER,
            Tel.BBS => PhoneTypesValue.BBS,
            Tel.Modem => PhoneTypesValue.MODEM,
            Tel.Car => PhoneTypesValue.CAR,
            Tel.ISDN => PhoneTypesValue.ISDN,
            Tel.Video => PhoneTypesValue.VIDEO,
            Tel.PCS => PhoneTypesValue.PCS,
            Tel.TextPhone => PhoneTypesValue.TEXTPHONE,
            Tel.Text => PhoneTypesValue.TEXT,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
