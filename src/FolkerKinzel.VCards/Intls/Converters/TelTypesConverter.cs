using System.Collections.Generic;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class TelTypesConverter
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

    internal const TelTypes DEFINED_TEL_TYPES_VALUES = TelTypes.Voice
                                                     | TelTypes.Fax
                                                     | TelTypes.Msg
                                                     | TelTypes.Cell
                                                     | TelTypes.Pager
                                                     | TelTypes.BBS
                                                     | TelTypes.Modem
                                                     | TelTypes.Car
                                                     | TelTypes.ISDN
                                                     | TelTypes.Video
                                                     | TelTypes.PCS
                                                     | TelTypes.TextPhone
                                                     | TelTypes.Text;

    internal const int TelTypesMinBit = 0;
    internal const int TelTypesMaxBit = 12;


    internal static TelTypes? Parse(string? typeValue)
    {
        Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

        return typeValue switch
        {
            TelTypeValue.VOICE => TelTypes.Voice,
            TelTypeValue.FAX => TelTypes.Fax,
            TelTypeValue.MSG => TelTypes.Msg,
            TelTypeValue.CELL => TelTypes.Cell,
            TelTypeValue.PAGER => TelTypes.Pager,
            TelTypeValue.BBS => TelTypes.BBS,
            TelTypeValue.MODEM => TelTypes.Modem,
            TelTypeValue.CAR => TelTypes.Car,
            TelTypeValue.ISDN => TelTypes.ISDN,
            TelTypeValue.VIDEO => TelTypes.Video,
            TelTypeValue.PCS => TelTypes.PCS,
            TelTypeValue.TEXTPHONE => TelTypes.TextPhone,
            TelTypeValue.TEXT => TelTypes.Text,
            _ => null
        };
    }

    internal static string ToVcfString(TelTypes value)
        => value switch
        {
            TelTypes.Voice => TelTypeValue.VOICE,
            TelTypes.Fax => TelTypeValue.FAX,
            TelTypes.Msg => TelTypeValue.MSG,
            TelTypes.Cell => TelTypeValue.CELL,
            TelTypes.Pager => TelTypeValue.PAGER,
            TelTypes.BBS => TelTypeValue.BBS,
            TelTypes.Modem => TelTypeValue.MODEM,
            TelTypes.Car => TelTypeValue.CAR,
            TelTypes.ISDN => TelTypeValue.ISDN,
            TelTypes.Video => TelTypeValue.VIDEO,
            TelTypes.PCS => TelTypeValue.PCS,
            TelTypes.TextPhone => TelTypeValue.TEXTPHONE,
            TelTypes.Text => TelTypeValue.TEXT,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };

}
