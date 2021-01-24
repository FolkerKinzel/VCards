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

        internal static TelTypes? Parse(string? typeValue, TelTypes? telephoneType)
        {
            Debug.Assert(typeValue?.ToUpperInvariant() == typeValue);

            return typeValue switch
            {
                TelTypeValue.VOICE => telephoneType.Set(TelTypes.Voice),
                TelTypeValue.FAX => telephoneType.Set(TelTypes.Fax),
                TelTypeValue.MSG => telephoneType.Set(TelTypes.Msg),
                TelTypeValue.CELL => telephoneType.Set(TelTypes.Cell),
                TelTypeValue.PAGER => telephoneType.Set(TelTypes.Pager),
                TelTypeValue.BBS => telephoneType.Set(TelTypes.BBS),
                TelTypeValue.MODEM => telephoneType.Set(TelTypes.Modem),
                TelTypeValue.CAR => telephoneType.Set(TelTypes.Car),
                TelTypeValue.ISDN => telephoneType.Set(TelTypes.ISDN),
                TelTypeValue.VIDEO => telephoneType.Set(TelTypes.Video),
                TelTypeValue.PCS => telephoneType.Set(TelTypes.PCS),
                TelTypeValue.TEXTPHONE => telephoneType.Set(TelTypes.TextPhone),
                TelTypeValue.TEXT => telephoneType.Set(TelTypes.Text),
                _ => telephoneType,
            };
        }
    }
}
