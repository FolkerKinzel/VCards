using System;
using System.Diagnostics;
using System.IO;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class MimeTypeConverter
    {
        //Bilder
        internal static class ImageTypeValue
        {
            internal const string JPEG = "JPEG";
            internal const string GIF = "GIF";
            internal const string TIFF = "TIFF";
            internal const string BMP = "BMP";
            internal const string CGM = "CGM";
            internal const string WMF = "WMF";
            internal const string MET = "MET";
            internal const string PMB = "PMB";
            internal const string DIB = "DIB";
            internal const string PICT = "PICT";
            internal const string PS = "PS";
            internal const string PDF = "PDF";
            internal const string MPEG = "MPEG";
            internal const string MPEG2 = "MPEG2";
            internal const string AVI = "AVI";
            internal const string QTIME = "QTIME";


            internal static class NonStandard
            {
                internal const string PNG = "PNG";
                internal const string JPG = "JPG";
                internal const string JPE = "JPE";
                internal const string ICO = "ICO";
                internal const string TIF = "TIF";
                internal const string PIC = "PIC";
                internal const string XBM = "XBM";
                internal const string MOV = "MOV";
                internal const string QT = "QT";
                internal const string SVG = "SVG";
            }
        }


        // Public Key
        internal static class KeyTypeValue
        {
            internal const string X509 = "X509";
            internal const string PGP = "PGP";
        }

        //Sound
        internal static class SoundTypeValue
        {
            internal const string WAVE = "WAVE";
            internal const string PCM = "PCM";
            internal const string AIFF = "AIFF";


            internal static class NonStandard
            {
                internal const string AIF = "AIF";
                internal const string MP3 = "MP3";
                internal const string MP4 = "MP4";
                internal const string OGG = "OGG";
                internal const string VORBIS = "VORBIS";
                internal const string BASIC = "BASIC";
                internal const string AAC = "AAC";
                internal const string AC3 = "AC3";
                internal const string WAV = "WAV";
            }
        }

        private static class MimeTypeString
        {
            internal const string OCTET_STREAM = "application/octet-stream";

            internal static class EncryptionKey
            {
                internal const string X509 = "application/x-x509-ca-cert";
                internal const string PGP = "application/pgp-keys";
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
        }



        internal static string GetFileExtension(MimeType mimeType)
        {
            Debug.Assert(mimeType != null);

            string mime = mimeType.MediaType;
            switch (mime)
            {
                // Bilder
                case MimeTypeString.Image.BMP:
                    return ".bmp";
                case MimeTypeString.Image.GIF:
                    return ".gif";
                case MimeTypeString.Image.ICO:
                    return ".ico";
                case MimeTypeString.Image.JPEG:
                    return ".jpg";
                case MimeTypeString.Image.PDF:
                    return ".pdf";
                case MimeTypeString.Image.PNG:
                    return ".png";
                case MimeTypeString.Image.PS:
                    return ".ps";
                case MimeTypeString.Image.SVG:
                    return ".svg";
                case MimeTypeString.Image.TIFF:
                    return ".tif";
                case MimeTypeString.Image.WMF:
                case "application/x-msmetafile":
                case "image/wmf":
                    return ".wmf";
                case MimeTypeString.Image.MPEG:
                    return ".hevc";
                case MimeTypeString.Image.XBM:
                    return ".xbm";
                case MimeTypeString.Image.AVI:
                case "video/x-msvideo":
                    return ".avi";
                case MimeTypeString.Image.CGM:
                    return ".cgm";
                case MimeTypeString.Image.PICT:
                case "image/pict":
                    return "pic";
                case MimeTypeString.Image.QTIME:
                case "video/quicktime":
                    return ".mov";


                // Verschlüsselung
                case MimeTypeString.EncryptionKey.PGP:
                case "application/pgp-encrypted":
                    return ".pgp";
                case MimeTypeString.EncryptionKey.X509:
                case "application/x-x509-user-cert":
                    return ".crt";
                case "application/pkix-cert":
                    return ".cer";
                case "application/x-pkcs12":
                    return ".pfx";
                case "application/x-pkcs7-certificates":
                    return ".p7b";
                case "application/pkcs7-mime":
                    return ".p7c";
                case "application/x-pem-file":
                    return ".pem";
                case "application/pkcs8":
                    return ".key";
                case "application/pkcs10":
                    return ".csr";


                //Audio
                case MimeTypeString.Audio.AAC:
                case "audio/x-aac":
                    return ".aac";
                case MimeTypeString.Audio.AC3:
                    return ".ac3";
                case MimeTypeString.Audio.AIFF:
                    return ".aif";
                case MimeTypeString.Audio.BASIC:
                    return ".snd";
                case MimeTypeString.Audio.MP3:
                    return ".mp3";
                case MimeTypeString.Audio.MP4:
                    return ".m4a";
                case MimeTypeString.Audio.OGG:
                    return ".ogg";

                //case MimeType.Audio.PCM:

                //case MimeType.Audio.VORBIS:

                case MimeTypeString.Audio.WAVE:
                case "audio/wav":
                    return ".wav";

                case "text/plain":
                    return ".txt";

                default:
                    {
                        string[] arr = mime.Split(new char[] { '/', '+' }, StringSplitOptions.RemoveEmptyEntries);
                        return "." + arr[arr.Length - 1];
                    }
            }
        }

        internal static string GetMimeTypeFromFileExtension(string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                // Verchlüsselung
                case ".pgp":
                    return MimeTypeString.EncryptionKey.PGP;
                case ".crt":
                case ".der":
                    return MimeTypeString.EncryptionKey.X509;
                case ".cer":
                    return "application/pkix-cert";
                case ".pfx":
                case ".p12":
                    return "application/x-pkcs12";
                case ".p7b":
                case ".spc":
                    return "application/x-pkcs7-certificates";
                case ".p7c":
                    return "application/pkcs7-mime";
                case ".pem":
                    return "application/x-pem-file";
                case ".key":
                case ".p8":
                    return "application/pkcs8";
                case ".csr":
                case ".p10":
                    return "application/pkcs10";

                // Bilder
                case ".bmp":
                case ".dib":
                    return MimeTypeString.Image.BMP;
                case ".gif":
                    return MimeTypeString.Image.GIF;
                case ".ico":
                    return MimeTypeString.Image.ICO;
                case ".jpg":
                case ".jpe":
                case ".jpeg":
                    return MimeTypeString.Image.JPEG;
                case ".pdf":
                    return MimeTypeString.Image.PDF;
                case ".png":
                case ".pnz":
                    return MimeTypeString.Image.PNG;
                case ".ps":
                case ".ai":
                case ".eps":
                    return MimeTypeString.Image.PS;
                case ".svg":
                case ".svgz":
                    return MimeTypeString.Image.SVG;
                case ".tif":
                case ".tiff":
                    return MimeTypeString.Image.TIFF;
                case ".wmf":
                    return MimeTypeString.Image.WMF;
                case ".hevc":
                    return MimeTypeString.Image.MPEG;
                case ".xbm":
                    return MimeTypeString.Image.XBM;
                case ".avi":
                    return MimeTypeString.Image.AVI;
                case ".cgm":
                    return MimeTypeString.Image.CGM;
                case ".pic":
                case ".pict":
                case "pct":
                    return MimeTypeString.Image.PICT;
                case ".mov":
                    return MimeTypeString.Image.QTIME;

                // Audio
                case ".aac":
                case ".adts":
                    return MimeTypeString.Audio.AAC;
                case ".ac3":
                    return MimeTypeString.Audio.AC3;
                case ".aif":
                case ".aiff":
                case ".aifc":
                    return MimeTypeString.Audio.AIFF;
                case ".snd":
                case ".au":
                    return MimeTypeString.Audio.BASIC;
                case ".mp3":
                case ".mpga":
                case ".mp2":
                case ".mp2a":
                case ".m2a":
                case ".m3a":
                    return MimeTypeString.Audio.MP3;
                case ".m4a":
                case ".mp4a":
                    return MimeTypeString.Audio.MP4;
                case ".ogg":
                case ".oga":
                case ".spx":
                    return MimeTypeString.Audio.OGG;

                default:
                    return MimeTypeString.OCTET_STREAM;
            }
        }

        internal static string MimeTypeFromImageTypeValue(string typeValue)
        {
            switch (typeValue)
            {
                case ImageTypeValue.JPEG:
                case ImageTypeValue.NonStandard.JPG:
                case ImageTypeValue.NonStandard.JPE:
                    return MimeTypeString.Image.JPEG;

                case ImageTypeValue.TIFF:
                case ImageTypeValue.NonStandard.TIF:
                    return MimeTypeString.Image.TIFF;

                case ImageTypeValue.BMP:
                case ImageTypeValue.DIB:
                    return MimeTypeString.Image.BMP;

                case ImageTypeValue.GIF:
                    return MimeTypeString.Image.GIF;

                case ImageTypeValue.NonStandard.ICO:
                    return MimeTypeString.Image.ICO;

                case ImageTypeValue.NonStandard.PNG:
                    return MimeTypeString.Image.PNG;

                case ImageTypeValue.NonStandard.SVG:
                    return MimeTypeString.Image.SVG;

                case ImageTypeValue.AVI:
                    return MimeTypeString.Image.AVI;

                case ImageTypeValue.CGM:
                    return MimeTypeString.Image.CGM;

                //case ImageTypeValue.MET:
                //    return "";
                //    
                case ImageTypeValue.MPEG:
                case ImageTypeValue.MPEG2:
                    return MimeTypeString.Image.MPEG; // Standard?

                case ImageTypeValue.PDF:
                    return MimeTypeString.Image.PDF;

                case ImageTypeValue.PICT:
                    return MimeTypeString.Image.PICT;

                //case ImageTypeValue.PMB:
                //    return "";
                //
                case ImageTypeValue.PS:
                    return MimeTypeString.Image.PS;

                case ImageTypeValue.QTIME:
                case ImageTypeValue.NonStandard.QT:
                case ImageTypeValue.NonStandard.MOV:
                    return MimeTypeString.Image.QTIME;

                case ImageTypeValue.WMF:
                    return MimeTypeString.Image.WMF;

                case ImageTypeValue.NonStandard.XBM:
                    return MimeTypeString.Image.XBM;

                default:
                    return MimeTypeString.OCTET_STREAM;

            }//switch

        }

        internal static string? ImageTypeValueFromMimeType(string? mimeType)
        {
            if (mimeType is null)
            {
                return null;
            }

            const string imageType = @"image/";

            if (mimeType.StartsWith(imageType, StringComparison.OrdinalIgnoreCase))
            {
                string s = mimeType.Substring(imageType.Length).ToUpperInvariant();

                return s.Length == 0 ? null : s;
            }
            else
            {
                return mimeType;
            }
        }



        internal static string MimeTypeFromEncryptionTypeValue(string? typeValue)
        {
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
                _ => null,
            };
        }


        internal static string MimeTypeFromSoundTypeValue(string? typeValue)
        {
            switch (typeValue)
            {
                case SoundTypeValue.AIFF:
                case SoundTypeValue.NonStandard.AIF:
                    return MimeTypeString.Audio.AIFF;

                case SoundTypeValue.PCM:
                    return MimeTypeString.Audio.PCM;

                case SoundTypeValue.WAVE:
                case SoundTypeValue.NonStandard.WAV:
                    return MimeTypeString.Audio.WAVE;

                case SoundTypeValue.NonStandard.AAC:
                    return MimeTypeString.Audio.AAC;

                case SoundTypeValue.NonStandard.AC3:
                    return MimeTypeString.Audio.AC3;

                case SoundTypeValue.NonStandard.BASIC:
                    return MimeTypeString.Audio.BASIC;

                case SoundTypeValue.NonStandard.MP3:
                    return MimeTypeString.Audio.MP3;

                case SoundTypeValue.NonStandard.MP4:
                    return MimeTypeString.Audio.MP4;

                case SoundTypeValue.NonStandard.OGG:
                    return MimeTypeString.Audio.OGG;

                case SoundTypeValue.NonStandard.VORBIS:
                    return MimeTypeString.Audio.VORBIS;

                default:
                    return MimeTypeString.OCTET_STREAM;

            }
        }


        internal static string? SoundTypeValueFromMimeType(string? mimeType)
        {
            if (mimeType is null)
            {
                return null;
            }

            const string audioType = @"audio/";

            if (mimeType.StartsWith(audioType, StringComparison.OrdinalIgnoreCase))
            {
                string s = mimeType.Substring(audioType.Length).ToUpperInvariant();

                return s.Length == 0 ? null : s;
            }
            else
            {
                return mimeType;
            }
        }
    }
}
