using System.Globalization;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal sealed partial class VcfRow
{
    private static string ParseAttributeKeyFromValue(string value)
    {
        const string ENCODING_PROPERTY = ParameterSection.ParameterKey.ENCODING;
        const string CONTEXT_PROPERTY = ParameterSection.ParameterKey.CONTEXT;
        const string CHARSET_PROPERTY = ParameterSection.ParameterKey.CHARSET;
        const string TYPE_PROPERTY = ParameterSection.ParameterKey.TYPE;
        const string LANGUAGE_PROPERTY = ParameterSection.ParameterKey.LANGUAGE;
        const string VALUE_PROPERTY = ParameterSection.ParameterKey.VALUE;

        if (value.StartsWith("QUOTED", true, CultureInfo.InvariantCulture))
        {
            return ENCODING_PROPERTY; // "QUOTED-PRINTABLE"
        }

        if (value.StartsWith("UTF", true, CultureInfo.InvariantCulture) || value.StartsWith("ISO", true, CultureInfo.InvariantCulture))
        {
            return CHARSET_PROPERTY;
        }

        switch (value.ToUpperInvariant())
        {
            case "BASE64":
            case "B":
            case "Q":
            case "8BIT":
            case "7BIT":
                return ENCODING_PROPERTY;

            case "VCARD":
                return CONTEXT_PROPERTY;
            //Telefon
            case ParameterSection.TypeValue.PREF:
            case ParameterSection.TypeValue.WORK:
            case ParameterSection.TypeValue.HOME:
            case TelTypesConverter.TelTypeValue.VOICE:
            case TelTypesConverter.TelTypeValue.FAX:
            case TelTypesConverter.TelTypeValue.MSG:
            case TelTypesConverter.TelTypeValue.CELL:
            case TelTypesConverter.TelTypeValue.PAGER:
            case TelTypesConverter.TelTypeValue.BBS:
            case TelTypesConverter.TelTypeValue.MODEM:
            case TelTypesConverter.TelTypeValue.CAR:
            case TelTypesConverter.TelTypeValue.ISDN:
            case TelTypesConverter.TelTypeValue.VIDEO:


            //Post
            case AddressTypesConverter.AdrTypeValue.DOM:
            case AddressTypesConverter.AdrTypeValue.INTL:
            case AddressTypesConverter.AdrTypeValue.POSTAL:
            case AddressTypesConverter.AdrTypeValue.PARCEL:

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


            //Bilder
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


            case ContentLocationConverter.Values.INLINE:
            case ContentLocationConverter.Values.CID:
            case ContentLocationConverter.Values.CONTENT_ID:
            case ContentLocationConverter.Values.URL:
                return VALUE_PROPERTY;
            default:
                break;
        }

        return value.IsIetfLanguageTag() ? LANGUAGE_PROPERTY : TYPE_PROPERTY;
    }
}
