using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
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
    }
}
