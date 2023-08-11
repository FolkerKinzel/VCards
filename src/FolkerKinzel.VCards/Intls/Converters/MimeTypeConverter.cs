using FolkerKinzel.MimeTypes;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class MimeTypeConverter
{
    private static class MimeTypeString
    {
        internal static class EncryptionKey
        {
            internal const string X509 = "application/x-x509-ca-cert";
            internal const string PGP = "application/pgp-keys";
        }

        internal static class Audio
        {
            internal const string PCM = "audio/l16";
            internal const string WAVE = "audio/x-wav";
            internal const string MPEG = "audio/mpeg";
            internal const string VORBIS = "audio/vorbis";
        }

        internal static class Image
        {
            internal const string JPEG = "image/jpeg";
            internal const string PICT = "image/x-pict";
            internal const string PS = "application/postscript";
            internal const string QTIME = "image/mov";
        }
    }

    private const int SHORT_STRING = 128;

    internal static string? ImageTypeFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.Image.PS =>   Const.ImageTypeValue.PS,
            MimeTypeString.Image.QTIME => Const.ImageTypeValue.QTIME,
            "image/jpg" => Const.ImageTypeValue.JPEG,
            _ => TypeValueFromMimeType(mimeType)
        };
    

    internal static string? MimeTypeFromImageType(string typeValue) =>
         typeValue switch
        {
            Const.ImageTypeValue.MPEG2 => MimeTypeFromImageType(Const.ImageTypeValue.MPEG),
            Const.ImageTypeValue.PICT => MimeTypeString.Image.PICT,
            Const.ImageTypeValue.PS => MimeTypeString.Image.PS,
            Const.ImageTypeValue.QTIME => MimeTypeString.Image.QTIME,
            "JPG" => MimeTypeString.Image.JPEG,
            _ => CreateMimeType("image", typeValue),
        };


    internal static string? KeyTypeFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.EncryptionKey.X509 => Const.KeyTypeValue.X509,
            "application/x-x509-user-cert" =>    Const.KeyTypeValue.X509,
            MimeTypeString.EncryptionKey.PGP =>  Const.KeyTypeValue.PGP,
            _ => TypeValueFromMimeType(mimeType)
        };


    internal static string? MimeTypeFromKeyType(string typeValue) =>
        typeValue switch
        {
            Const.KeyTypeValue.X509 => MimeTypeString.EncryptionKey.X509,
            Const.KeyTypeValue.PGP => MimeTypeString.EncryptionKey.PGP,
            _ => CreateMimeType("application", typeValue)
        };


    internal static string? SoundTypeFromMimeType(string? mimeType) =>
        mimeType switch
        {
            MimeTypeString.Audio.WAVE => Const.SoundTypeValue.WAVE,
            MimeTypeString.Audio.PCM =>  Const.SoundTypeValue.PCM,
            _ => TypeValueFromMimeType(mimeType)
        };

    internal static string? MimeTypeFromSoundType(string typeValue) =>
         typeValue switch
        {
            Const.SoundTypeValue.PCM => MimeTypeString.Audio.PCM,
            Const.SoundTypeValue.WAVE => MimeTypeString.Audio.WAVE,
            "MP3" => MimeTypeString.Audio.MPEG,
            Const.SoundTypeValue.NonStandard.VORBIS => MimeTypeString.Audio.VORBIS,
            _ => CreateMimeType("audio", typeValue)
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
