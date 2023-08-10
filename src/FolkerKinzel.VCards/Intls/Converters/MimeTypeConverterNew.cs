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
            internal const string MET = "IBM PM Metafile";
            internal const string PMB = "IBM PM Bitmap";
            internal const string DIB = "MS Windows DIB";
        }
    }

    private const int SHORT_STRING = 128;

    internal static string? ImageTypeValueFromMimeType(string? mimeType) =>
         mimeType?.ToLowerInvariant() switch
        {
            MimeTypeString.Image.MET => ImageTypeValue.MET,
            MimeTypeString.Image.PMB => ImageTypeValue.PMB,
            MimeTypeString.Image.DIB => ImageTypeValue.DIB,
            MimeTypeString.Image.PS => ImageTypeValue.PS,
            MimeTypeString.Image.QTIME => ImageTypeValue.QTIME,
            _ => TypeValueFromMimeType(mimeType),
        };


    private static string? TypeValueFromMimeType(string? mimeType)
    {
        if (mimeType is null)
        {
            return null;
        }

        Debug.Assert(mimeType.ToLowerInvariant() == mimeType);

        if (MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo info))
        {
            var subType = info.SubType;
            subType = subType.StartsWith("x-", StringComparison.Ordinal) ? subType.Slice(2) : subType;

            var span = subType.Length > SHORT_STRING ? new char[subType.Length].AsSpan() : stackalloc char[subType.Length];
            _ = subType.ToUpperInvariant(span);
            return span.ToString();
        }
        return mimeType.ToUpperInvariant();
    }


    internal static string MimeTypeFromImageTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        switch (typeValue)
        {
            case ImageTypeValue.DIB:
                return MimeTypeString.Image.DIB;
            case ImageTypeValue.MET:
                return MimeTypeString.Image.MET;
            case ImageTypeValue.MPEG2:
                return MimeTypeFromImageTypeValue(ImageTypeValue.MPEG);
            case ImageTypeValue.PICT:
                return MimeTypeString.Image.PICT;
            case ImageTypeValue.PMB:
                return MimeTypeString.Image.PMB;
            case ImageTypeValue.PS:
                return MimeTypeString.Image.PS;
            case ImageTypeValue.QTIME:
                return MimeTypeString.Image.QTIME;

            default:
                return MimeString.FromFileName(typeValue);

        }//switch
    }


    internal static string MimeTypeFromEncryptionTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        return typeValue switch
        {
            KeyTypeValue.X509 => MimeTypeString.EncryptionKey.X509,
            KeyTypeValue.PGP => MimeTypeString.EncryptionKey.PGP,
            _ => MimeString.FromFileName(typeValue)
        };
    }

    internal static string? KeyTypeValueFromMimeType(string? mimeType)
    {
        return mimeType?.ToLowerInvariant() switch
        {
            MimeTypeString.EncryptionKey.X509 => KeyTypeValue.X509,
            "application/x-x509-user-cert" => KeyTypeValue.X509,
            MimeTypeString.EncryptionKey.PGP => KeyTypeValue.PGP,
            _ => TypeValueFromMimeType(mimeType)
        };
    }

    internal static string? SoundTypeValueFromMimeType(string? mimeType)
    {

        switch (mimeType?.ToLowerInvariant())
        {
            case MimeTypeString.Audio.WAVE:
                return SoundTypeValue.WAVE;
            case MimeTypeString.Audio.PCM:
                return SoundTypeValue.PCM;
            default:
                return TypeValueFromMimeType(mimeType);
        }
    }

    internal static string MimeTypeFromSoundTypeValue(string typeValue)
    {
        Debug.Assert(StringComparer.Ordinal.Equals(typeValue, typeValue.ToUpperInvariant()));

        switch (typeValue)
        {
            case SoundTypeValue.PCM:
                return MimeTypeString.Audio.PCM;

            case SoundTypeValue.WAVE:
                return MimeTypeString.Audio.WAVE;

            case SoundTypeValue.NonStandard.BASIC:
                return MimeTypeString.Audio.BASIC;

            case "MPEG":
                return MimeTypeString.Audio.MP3;

            case SoundTypeValue.NonStandard.VORBIS:
                return MimeTypeString.Audio.VORBIS;

            default:
                return MimeString.FromFileName(typeValue);

        }
    }



    //private static string? GetFileTypeExtensionFromMimeType(string? mimeString)
    //{
    //    string? extension = MimeString.ToFileTypeExtension(mimeString, includePeriod: false);
    //    return extension == "bin" ? null : extension.ToUpperInvariant();
    //}
}
