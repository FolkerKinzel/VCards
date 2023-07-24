using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.MimeTypes;
using static FolkerKinzel.VCards.Intls.Converters.MimeTypeConverter;

namespace FolkerKinzel.VCards.Intls.Converters;
internal static class MimeTypeConverterNew
{
    private static class MimeTypeString
    {
        internal const string OCTET_STREAM = "application/octet-stream";

        internal static class EncryptionKey
        {
            internal const string X509 = "application/x-x509-ca-cert";
            internal const string PGP = "application/pgp-keys";
        }

        internal static class Audio
        {
            internal const string AIFF = "audio/x-aiff";
            internal const string PCM = "audio/l16";
            internal const string WAVE = "audio/x-wav";
            internal const string AAC = "audio/aac";
            internal const string AC3 = "audio/ac3";
            internal const string BASIC = "audio/basic";
            internal const string MP3 = "audio/mpeg";
            internal const string MP4 = "audio/mp4";
            internal const string OGG = "audio/ogg";
            internal const string VORBIS = "audio/vorbis";


        }

        internal static class Image
        {
            internal const string JPEG = "image/jpeg";
            internal const string TIFF = "image/tiff";
            internal const string BMP = "image/bmp";
            internal const string GIF = "image/gif";
            internal const string ICO = "image/x-icon";
            internal const string PNG = "image/png";
            internal const string SVG = "image/svg+xml";
            internal const string AVI = "image/avi";
            internal const string CGM = "image/cgm";
            internal const string MPEG = "image/mpeg-h"; // File-extension: ".hevc"
            internal const string PDF = "application/pdf";
            internal const string PICT = "image/x-pict";
            internal const string PS = "application/postscript";
            internal const string QTIME = "image/mov";
            internal const string WMF = "image/x-wmf";
            internal const string XBM = "image/x-xbitmap";
        }
    }

    

    private const string MP3 = "MP3";

    internal static string? ImageTypeValueFromMimeType(string? mimeType)
    {
        if (mimeType is null)
        {
            return null;
        }

        const string imageType = @"image/";

        return mimeType.StartsWith(imageType, StringComparison.OrdinalIgnoreCase)
            ? mimeType.Length == imageType.Length ? null : mimeType.Substring(imageType.Length).ToUpperInvariant()
            : mimeType.ToUpperInvariant();

    }

    internal static string MimeTypeFromImageTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        switch (typeValue)
        {
            //case ImageTypeValue.JPEG:
            //case ImageTypeValue.NonStandard.JPG:
            //case ImageTypeValue.NonStandard.JPE:
            //    return MimeTypeString.Image.JPEG;

            //case ImageTypeValue.TIFF:
            //case ImageTypeValue.NonStandard.TIF:
            //    return MimeTypeString.Image.TIFF;

            //case ImageTypeValue.BMP:
            case ImageTypeValue.DIB:
                return MimeTypeString.Image.BMP;

            //case ImageTypeValue.GIF:
            //    return MimeTypeString.Image.GIF;

            //case ImageTypeValue.NonStandard.ICO:
            //    return MimeTypeString.Image.ICO;

            //case ImageTypeValue.NonStandard.PNG:
            //    return MimeTypeString.Image.PNG;

            //case ImageTypeValue.NonStandard.SVG:
            //    return MimeTypeString.Image.SVG;

            //case ImageTypeValue.AVI:
            //    return MimeTypeString.Image.AVI;

            //case ImageTypeValue.CGM:
            //    return MimeTypeString.Image.CGM;

            ////case ImageTypeValue.MET:
            ////    return "";
            ////    
            //case ImageTypeValue.MPEG:
            //case ImageTypeValue.MPEG2:
            //    return MimeTypeString.Image.MPEG; // Standard?

            //case ImageTypeValue.PDF:
            //    return MimeTypeString.Image.PDF;

            case ImageTypeValue.PICT:
                return MimeTypeString.Image.PICT;

            ////case ImageTypeValue.PMB:
            ////    return "";
            ////
            //case ImageTypeValue.PS:
            //    return MimeTypeString.Image.PS;

            //case ImageTypeValue.QTIME:
            //case ImageTypeValue.NonStandard.QT:
            //case ImageTypeValue.NonStandard.MOV:
            //    return MimeTypeString.Image.QTIME;

            //case ImageTypeValue.WMF:
            //    return MimeTypeString.Image.WMF;

            //case ImageTypeValue.NonStandard.XBM:
            //    return MimeTypeString.Image.XBM;

            default:
                return MimeType.FromFileTypeExtension(typeValue).ToString();

        }//switch
    }



    internal static string MimeTypeFromEncryptionTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        return typeValue switch
        {
            KeyTypeValue.X509 => MimeTypeString.EncryptionKey.X509,
            KeyTypeValue.PGP => MimeTypeString.EncryptionKey.PGP,
            _ => MimeTypeString.OCTET_STREAM,
        };
    }

    internal static string? KeyTypeValueFromMimeType(string? mimeType)
    {
        return mimeType?.ToLowerInvariant() switch
        {
            MimeTypeString.EncryptionKey.X509 => KeyTypeValue.X509,
            "application/x-x509-user-cert" => KeyTypeValue.X509,
            MimeTypeString.EncryptionKey.PGP => KeyTypeValue.PGP,
            _ => GetFileTypeExtensionFromMimeType(mimeType)
        };
    }

    internal static string? SoundTypeValueFromMimeType(string? input)
    {
        if(input is null)
        {
            return null;
        }

        if(input.EndsWith(MP3, StringComparison.OrdinalIgnoreCase))
        {
            return MP3;
        }

        return GetFileTypeExtensionFromMimeType(input);
    }

    internal static string MimeTypeFromSoundTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        switch (typeValue)
        {
            //case SoundTypeValue.AIFF:
            //case SoundTypeValue.NonStandard.AIF:
            //    return MimeTypeString.Audio.AIFF;

            case SoundTypeValue.PCM:
                return MimeTypeString.Audio.PCM;

            case SoundTypeValue.WAVE:
            //case SoundTypeValue.NonStandard.WAV:
                return MimeTypeString.Audio.WAVE;

            //case SoundTypeValue.NonStandard.AAC:
            //    return MimeTypeString.Audio.AAC;

            //case SoundTypeValue.NonStandard.AC3:
            //    return MimeTypeString.Audio.AC3;

            case SoundTypeValue.NonStandard.BASIC:
                return MimeTypeString.Audio.BASIC;

            //case SoundTypeValue.NonStandard.MP3:
            //    return MimeTypeString.Audio.MP3;

            //case SoundTypeValue.NonStandard.MP4:
            //    return MimeTypeString.Audio.MP4;

            //case SoundTypeValue.NonStandard.OGG:
            //    return MimeTypeString.Audio.OGG;

            case SoundTypeValue.NonStandard.VORBIS:
                return MimeTypeString.Audio.VORBIS;

            default:
                return MimeType.FromFileTypeExtension(typeValue).ToString();

        }
    }



    private static string? GetFileTypeExtensionFromMimeType(string? mimeString)
    {
        string? extension = MimeType.GetFileTypeExtension(mimeString);
        return extension == ".bin" ? null : extension.Substring(1).ToUpperInvariant();
    }
}
