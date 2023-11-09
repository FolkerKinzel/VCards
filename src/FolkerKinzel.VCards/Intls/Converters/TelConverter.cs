using FolkerKinzel.VCards.Models.Enums;

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

    internal static Tel? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            PhoneTypesValue.VOICE => Tel.Voice,
            PhoneTypesValue.FAX => Tel.Fax,
            PhoneTypesValue.MSG => Tel.Msg,
            PhoneTypesValue.CELL => Tel.Cell,
            PhoneTypesValue.PAGER => Tel.Pager,
            PhoneTypesValue.BBS => Tel.BBS,
            PhoneTypesValue.MODEM => Tel.Modem,
            PhoneTypesValue.CAR => Tel.Car,
            PhoneTypesValue.ISDN => Tel.ISDN,
            PhoneTypesValue.VIDEO => Tel.Video,
            PhoneTypesValue.PCS => Tel.PCS,
            PhoneTypesValue.TEXTPHONE => Tel.TextPhone,
            PhoneTypesValue.TEXT => Tel.Text,
            _ => null
        };
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
