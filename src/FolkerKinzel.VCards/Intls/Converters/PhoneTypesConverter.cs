using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class PhoneTypesConverter
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

    internal const PhoneTypes DEFINED_PHONE_TYPES_VALUES = PhoneTypes.Voice
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

    internal const int PHONE_TYPES_MIN_BIT = 0;
    internal const int PHONE_TYPES_MAX_BIT = 12;

    internal static PhoneTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            PhoneTypesValue.VOICE => PhoneTypes.Voice,
            PhoneTypesValue.FAX => PhoneTypes.Fax,
            PhoneTypesValue.MSG => PhoneTypes.Msg,
            PhoneTypesValue.CELL => PhoneTypes.Cell,
            PhoneTypesValue.PAGER => PhoneTypes.Pager,
            PhoneTypesValue.BBS => PhoneTypes.BBS,
            PhoneTypesValue.MODEM => PhoneTypes.Modem,
            PhoneTypesValue.CAR => PhoneTypes.Car,
            PhoneTypesValue.ISDN => PhoneTypes.ISDN,
            PhoneTypesValue.VIDEO => PhoneTypes.Video,
            PhoneTypesValue.PCS => PhoneTypes.PCS,
            PhoneTypesValue.TEXTPHONE => PhoneTypes.TextPhone,
            PhoneTypesValue.TEXT => PhoneTypes.Text,
            _ => null
        };
    }

    internal static string ToVcfString(this PhoneTypes value)
        => value switch
        {
            PhoneTypes.Voice => PhoneTypesValue.VOICE,
            PhoneTypes.Fax => PhoneTypesValue.FAX,
            PhoneTypes.Msg => PhoneTypesValue.MSG,
            PhoneTypes.Cell => PhoneTypesValue.CELL,
            PhoneTypes.Pager => PhoneTypesValue.PAGER,
            PhoneTypes.BBS => PhoneTypesValue.BBS,
            PhoneTypes.Modem => PhoneTypesValue.MODEM,
            PhoneTypes.Car => PhoneTypesValue.CAR,
            PhoneTypes.ISDN => PhoneTypesValue.ISDN,
            PhoneTypes.Video => PhoneTypesValue.VIDEO,
            PhoneTypes.PCS => PhoneTypesValue.PCS,
            PhoneTypes.TextPhone => PhoneTypesValue.TEXTPHONE,
            PhoneTypes.Text => PhoneTypesValue.TEXT,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}
