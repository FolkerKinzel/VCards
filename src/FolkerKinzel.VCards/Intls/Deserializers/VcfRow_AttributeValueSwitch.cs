using FolkerKinzel.VCards.Intls.Converters;
using System.Globalization;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Deserializers
{
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
                return ENCODING_PROPERTY; // "QUOTED-PRINTABLE" oder "Q"
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
                case MimeTypeConverter.ImageTypeValue.NonStandard.PNG:
                case MimeTypeConverter.ImageTypeValue.JPEG:
                case MimeTypeConverter.ImageTypeValue.NonStandard.JPG:
                case MimeTypeConverter.ImageTypeValue.NonStandard.JPE:
                case MimeTypeConverter.ImageTypeValue.GIF:
                case MimeTypeConverter.ImageTypeValue.NonStandard.ICO:
                case MimeTypeConverter.ImageTypeValue.TIFF:
                case MimeTypeConverter.ImageTypeValue.NonStandard.TIF:
                case MimeTypeConverter.ImageTypeValue.BMP:
                case MimeTypeConverter.ImageTypeValue.CGM:
                case MimeTypeConverter.ImageTypeValue.WMF:
                case MimeTypeConverter.ImageTypeValue.MET:
                case MimeTypeConverter.ImageTypeValue.PMB:
                case MimeTypeConverter.ImageTypeValue.DIB:
                case MimeTypeConverter.ImageTypeValue.PICT:
                case MimeTypeConverter.ImageTypeValue.NonStandard.PIC:
                case MimeTypeConverter.ImageTypeValue.NonStandard.XBM:
                case MimeTypeConverter.ImageTypeValue.NonStandard.SVG:


                case MimeTypeConverter.ImageTypeValue.PS:
                case MimeTypeConverter.ImageTypeValue.PDF:

                case MimeTypeConverter.ImageTypeValue.MPEG:
                case MimeTypeConverter.ImageTypeValue.MPEG2:
                case MimeTypeConverter.ImageTypeValue.AVI:
                case MimeTypeConverter.ImageTypeValue.QTIME:
                case MimeTypeConverter.ImageTypeValue.NonStandard.MOV:
                case MimeTypeConverter.ImageTypeValue.NonStandard.QT:

                //Sound
                case MimeTypeConverter.SoundTypeValue.WAVE:
                case MimeTypeConverter.SoundTypeValue.NonStandard.WAV:
                case MimeTypeConverter.SoundTypeValue.PCM:
                case MimeTypeConverter.SoundTypeValue.AIFF:
                case MimeTypeConverter.SoundTypeValue.NonStandard.AIF:
                case MimeTypeConverter.SoundTypeValue.NonStandard.MP3:
                case MimeTypeConverter.SoundTypeValue.NonStandard.MP4:
                case MimeTypeConverter.SoundTypeValue.NonStandard.OGG:
                case MimeTypeConverter.SoundTypeValue.NonStandard.VORBIS:
                case MimeTypeConverter.SoundTypeValue.NonStandard.BASIC:
                case MimeTypeConverter.SoundTypeValue.NonStandard.AAC:
                case MimeTypeConverter.SoundTypeValue.NonStandard.AC3:


                // Public Key
                case MimeTypeConverter.KeyTypeValue.X509:
                case MimeTypeConverter.KeyTypeValue.PGP:
                    return TYPE_PROPERTY;


                case VCdContentLocationConverter.Values.INLINE:
                case VCdContentLocationConverter.Values.CID:
                case VCdContentLocationConverter.Values.CONTENT_ID:
                case VCdContentLocationConverter.Values.URL:
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
}
