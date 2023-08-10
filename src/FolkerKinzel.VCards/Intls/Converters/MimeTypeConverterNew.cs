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

    internal static string? ImageTypeFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.Image.MET => ImageTypeValue.MET,
            MimeTypeString.Image.PMB => ImageTypeValue.PMB,
            MimeTypeString.Image.DIB => ImageTypeValue.DIB,
            MimeTypeString.Image.PS => ImageTypeValue.PS,
            MimeTypeString.Image.QTIME => ImageTypeValue.QTIME,
            _ => TypeValueFromMimeType(mimeType)
        };
    

    internal static string? MimeTypeFromImageType(string typeValue) =>
         typeValue switch
        {
            ImageTypeValue.DIB => MimeTypeString.Image.DIB,
            ImageTypeValue.MET => MimeTypeString.Image.MET,
            ImageTypeValue.MPEG2 => MimeTypeFromImageType(ImageTypeValue.MPEG),
            ImageTypeValue.PICT => MimeTypeString.Image.PICT,
            ImageTypeValue.PMB => MimeTypeString.Image.PMB,
            ImageTypeValue.PS => MimeTypeString.Image.PS,
            ImageTypeValue.QTIME => MimeTypeString.Image.QTIME,
            _ => CreateMimeType("image", typeValue),
        };


    internal static string? KeyTypeFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.EncryptionKey.X509 => KeyTypeValue.X509,
            "application/x-x509-user-cert" => KeyTypeValue.X509,
            MimeTypeString.EncryptionKey.PGP => KeyTypeValue.PGP,
            _ => TypeValueFromMimeType(mimeType)
        };


    internal static string? MimeTypeFromKeyType(string typeValue) =>
        typeValue switch
        {
            KeyTypeValue.X509 => MimeTypeString.EncryptionKey.X509,
            KeyTypeValue.PGP => MimeTypeString.EncryptionKey.PGP,
            _ => CreateMimeType("application", typeValue)
        };


    internal static string? SoundTypeValueFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.Audio.WAVE => SoundTypeValue.WAVE,
            MimeTypeString.Audio.PCM => SoundTypeValue.PCM,
            _ => TypeValueFromMimeType(mimeType)
        };

    internal static string MimeTypeFromSoundTypeValue(string typeValue) =>
         typeValue switch
        {
            SoundTypeValue.PCM => MimeTypeString.Audio.PCM,
            SoundTypeValue.WAVE => MimeTypeString.Audio.WAVE,
            SoundTypeValue.NonStandard.BASIC => MimeTypeString.Audio.BASIC,
            "MPEG" => MimeTypeString.Audio.MP3,
            SoundTypeValue.NonStandard.VORBIS => MimeTypeString.Audio.VORBIS,
            _ => MimeString.FromFileName(typeValue)
        };


    private static string? CreateMimeType(string mediaType, string subType)
    {
        if(IsMimeType(subType))
        {
            return subType;
        }
        try
        {
            return MimeType.Create(mediaType, subType).ToString();
        }
        catch
        {
            return null;
        }

        static bool IsMimeType(string input) => input.Contains('/');
    }

    private static string? TypeValueFromMimeType(string? mimeType)
    {
        if (mimeType is null)
        {
            return null;
        }

        if (MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo info))
        {
            var subType = info.SubType;
            subType = subType.StartsWith("x-", StringComparison.OrdinalIgnoreCase) ? subType.Slice(2) : subType;

            var span = subType.Length > SHORT_STRING ? new char[subType.Length].AsSpan() : stackalloc char[subType.Length];
            _ = subType.ToUpperInvariant(span);
            return span.ToString();
        }
        return null;
    }

}
