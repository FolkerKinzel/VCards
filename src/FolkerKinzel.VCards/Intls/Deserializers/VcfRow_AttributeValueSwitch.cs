using System.Globalization;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Intls.Converters;
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
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.PNG:
            case MimeTypeConverterNew.ImageTypeValue.JPEG:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.JPG:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.JPE:
            case MimeTypeConverterNew.ImageTypeValue.GIF:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.ICO:
            case MimeTypeConverterNew.ImageTypeValue.TIFF:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.TIF:
            case MimeTypeConverterNew.ImageTypeValue.BMP:
            case MimeTypeConverterNew.ImageTypeValue.CGM:
            case MimeTypeConverterNew.ImageTypeValue.WMF:
            case MimeTypeConverterNew.ImageTypeValue.MET:
            case MimeTypeConverterNew.ImageTypeValue.PMB:
            case MimeTypeConverterNew.ImageTypeValue.DIB:
            case MimeTypeConverterNew.ImageTypeValue.PICT:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.PIC:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.XBM:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.SVG:


            case MimeTypeConverterNew.ImageTypeValue.PS:
            case MimeTypeConverterNew.ImageTypeValue.PDF:

            case MimeTypeConverterNew.ImageTypeValue.MPEG:
            case MimeTypeConverterNew.ImageTypeValue.MPEG2:
            case MimeTypeConverterNew.ImageTypeValue.AVI:
            case MimeTypeConverterNew.ImageTypeValue.QTIME:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.MOV:
            case MimeTypeConverterNew.ImageTypeValue.NonStandard.QT:

            //Sound
            case MimeTypeConverterNew.SoundTypeValue.WAVE:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.WAV:
            case MimeTypeConverterNew.SoundTypeValue.PCM:
            case MimeTypeConverterNew.SoundTypeValue.AIFF:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.AIF:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.MP3:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.MP4:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.OGG:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.VORBIS:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.BASIC:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.AAC:
            case MimeTypeConverterNew.SoundTypeValue.NonStandard.AC3:


            // Public Key
            case MimeTypeConverterNew.KeyTypeValue.X509:
            case MimeTypeConverterNew.KeyTypeValue.PGP:
                return TYPE_PROPERTY;


            case ContentLocationConverter.Values.INLINE:
            case ContentLocationConverter.Values.CID:
            case ContentLocationConverter.Values.CONTENT_ID:
            case ContentLocationConverter.Values.URL:
                return VALUE_PROPERTY;
            default:
                break;
        }

        // Das Regex ist nicht perfekt, findet aber die meisten IETF-Language-Tags:
        if (Regex.IsMatch(value, @"^[a-z]{2,3}-[A-Z]{2,3}$"))
        {
            return LANGUAGE_PROPERTY;
        }


        //CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures); //AllCultures

        //int i = 0;
        //while (i < cultures.Length && !cultures[i].Name.Equals(value, StringComparison.InvariantCultureIgnoreCase))
        //{
        //    i++;
        //}
        //if (i < cultures.Length) return LANGUAGE_PROPERTY;


        return TYPE_PROPERTY;

    }
}
