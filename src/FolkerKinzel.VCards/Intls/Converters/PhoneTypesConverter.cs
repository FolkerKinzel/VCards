using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class PhoneTypesConverter
{
    internal static class TelTypeValue
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

    internal const PhoneTypes DEFINED_TEL_TYPES_VALUES = PhoneTypes.Voice
                                                     | PhoneTypes.Fax
                                                     | PhoneTypes.Msg
                                                     | PhoneTypes.Cell
                                                     | PhoneTypes.Pager
                                                     | PhoneTypes.BBS
                                                     | PhoneTypes.Modem
                                                     | PhoneTypes.Car
                                                     | PhoneTypes.ISDN
                                                     | PhoneTypes.Video
                                                     | PhoneTypes.PCS
                                                     | PhoneTypes.TextPhone
                                                     | PhoneTypes.Text;

    internal const int TEL_TYPES_MIN_BIT = 0;
    internal const int TEL_TYPES_MAX_BIT = 12;

    internal static PhoneTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            TelTypeValue.VOICE => PhoneTypes.Voice,
            TelTypeValue.FAX => PhoneTypes.Fax,
            TelTypeValue.MSG => PhoneTypes.Msg,
            TelTypeValue.CELL => PhoneTypes.Cell,
            TelTypeValue.PAGER => PhoneTypes.Pager,
            TelTypeValue.BBS => PhoneTypes.BBS,
            TelTypeValue.MODEM => PhoneTypes.Modem,
            TelTypeValue.CAR => PhoneTypes.Car,
            TelTypeValue.ISDN => PhoneTypes.ISDN,
            TelTypeValue.VIDEO => PhoneTypes.Video,
            TelTypeValue.PCS => PhoneTypes.PCS,
            TelTypeValue.TEXTPHONE => PhoneTypes.TextPhone,
            TelTypeValue.TEXT => PhoneTypes.Text,
            _ => null
        };
    }

    internal static string ToVcfString(this PhoneTypes value)
        => value switch
        {
            PhoneTypes.Voice => TelTypeValue.VOICE,
            PhoneTypes.Fax => TelTypeValue.FAX,
            PhoneTypes.Msg => TelTypeValue.MSG,
            PhoneTypes.Cell => TelTypeValue.CELL,
            PhoneTypes.Pager => TelTypeValue.PAGER,
            PhoneTypes.BBS => TelTypeValue.BBS,
            PhoneTypes.Modem => TelTypeValue.MODEM,
            PhoneTypes.Car => TelTypeValue.CAR,
            PhoneTypes.ISDN => TelTypeValue.ISDN,
            PhoneTypes.Video => TelTypeValue.VIDEO,
            PhoneTypes.PCS => TelTypeValue.PCS,
            PhoneTypes.TextPhone => TelTypeValue.TEXTPHONE,
            PhoneTypes.Text => TelTypeValue.TEXT,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
