using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
#if NET8_0_OR_GREATER
    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters",
        Justification = "Not localizable")]
#endif
    private static string ParseAttributeKeyFromValue(ReadOnlySpan<char> value)
    {
        Debug.Assert(!value.IsWhiteSpace());

        const string ENCODING_PROPERTY = ParameterSection.ParameterKey.ENCODING;
        //const string CONTEXT_PROPERTY = ParameterSection.ParameterKey.CONTEXT;
        const string CHARSET_PROPERTY = ParameterSection.ParameterKey.CHARSET;
        const string TYPE_PROPERTY = ParameterSection.ParameterKey.TYPE;
        const string LANGUAGE_PROPERTY = ParameterSection.ParameterKey.LANGUAGE;
        const string VALUE_PROPERTY = ParameterSection.ParameterKey.VALUE;

        if (value.StartsWith("QUOTED".AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return ENCODING_PROPERTY; // "QUOTED-PRINTABLE"
        }

        if (value.StartsWith("UTF".AsSpan(), StringComparison.OrdinalIgnoreCase) || value.StartsWith("ISO".AsSpan(), StringComparison.OrdinalIgnoreCase))
        {
            return CHARSET_PROPERTY;
        }

        string valString = value.ToString();

        switch (valString.ToUpperInvariant())
        {
            case "BASE64":
            case "B":
            case "Q":
            case "8BIT":
            case "7BIT":
                return ENCODING_PROPERTY;

            //case "VCARD":
            //    return CONTEXT_PROPERTY;

            //Phone
            case ParameterSection.TypeValue.PREF:
            case ParameterSection.TypeValue.WORK:
            case ParameterSection.TypeValue.HOME:
            case TelConverter.PhoneTypesValue.VOICE:
            case TelConverter.PhoneTypesValue.FAX:
            case TelConverter.PhoneTypesValue.MSG:
            case TelConverter.PhoneTypesValue.CELL:
            case TelConverter.PhoneTypesValue.PAGER:
            case TelConverter.PhoneTypesValue.BBS:
            case TelConverter.PhoneTypesValue.MODEM:
            case TelConverter.PhoneTypesValue.CAR:
            case TelConverter.PhoneTypesValue.ISDN:
            case TelConverter.PhoneTypesValue.VIDEO:

            //Postal
            case AdrConverter.AddressTypesValue.DOM:
            case AdrConverter.AddressTypesValue.INTL:
            case AdrConverter.AddressTypesValue.POSTAL:
            case AdrConverter.AddressTypesValue.PARCEL:

            //E-Mail
            case "INTERNET":
            case "AOL":
            case "APPLELINK":
            case "ATTMAIL":
            case "CIS":
            case "EWORLD":
            case "IBMMAIL":
            case "MCIMAIL":
            case "POWERSHARE":
            case "PRODIGY":
            case "TLX":
            case "X400":

            //Images
            case Const.ImageTypeValue.NonStandard.PNG:
            case Const.ImageTypeValue.JPEG:
            case Const.ImageTypeValue.NonStandard.JPG:
            case Const.ImageTypeValue.NonStandard.JPE:
            case Const.ImageTypeValue.GIF:
            case Const.ImageTypeValue.NonStandard.ICO:
            case Const.ImageTypeValue.TIFF:
            case Const.ImageTypeValue.NonStandard.TIF:
            case Const.ImageTypeValue.BMP:
            case Const.ImageTypeValue.CGM:
            case Const.ImageTypeValue.WMF:
            case Const.ImageTypeValue.MET:
            case Const.ImageTypeValue.PMB:
            case Const.ImageTypeValue.DIB:
            case Const.ImageTypeValue.PICT:
            case Const.ImageTypeValue.NonStandard.PIC:
            case Const.ImageTypeValue.NonStandard.XBM:
            case Const.ImageTypeValue.NonStandard.SVG:

            case Const.ImageTypeValue.PS:
            case Const.ImageTypeValue.PDF:

            case Const.ImageTypeValue.MPEG:
            case Const.ImageTypeValue.MPEG2:
            case Const.ImageTypeValue.AVI:
            case Const.ImageTypeValue.QTIME:
            case Const.ImageTypeValue.NonStandard.MOV:
            case Const.ImageTypeValue.NonStandard.QT:

            //Sound
            case Const.SoundTypeValue.WAVE:
            case Const.SoundTypeValue.NonStandard.WAV:
            case Const.SoundTypeValue.PCM:
            case Const.SoundTypeValue.AIFF:
            case Const.SoundTypeValue.NonStandard.AIF:
            case Const.SoundTypeValue.NonStandard.MP3:
            case Const.SoundTypeValue.NonStandard.MP4:
            case Const.SoundTypeValue.NonStandard.OGG:
            case Const.SoundTypeValue.NonStandard.VORBIS:
            case Const.SoundTypeValue.NonStandard.BASIC:
            case Const.SoundTypeValue.NonStandard.AAC:
            case Const.SoundTypeValue.NonStandard.AC3:

            // Public Key
            case Const.KeyTypeValue.X509:
            case Const.KeyTypeValue.PGP:
                return TYPE_PROPERTY;

            case LocConverter.Values.INLINE:
            case LocConverter.Values.CID:
            case LocConverter.Values.CONTENT_ID:
            case LocConverter.Values.URL:
                return VALUE_PROPERTY;
            default:
                break;
        }

        return valString.IsIetfLanguageTag() ? LANGUAGE_PROPERTY : TYPE_PROPERTY;
    }
}
