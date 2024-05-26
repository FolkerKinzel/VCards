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
            internal const string MPEG = "image/mpeg";

        }
    }

    private const int SHORT_STRING = 128;

    internal static string? ImageTypeFromMimeType(string? mimeType)
        => mimeType switch
        {
            MimeTypeString.Image.PS => Const.ImageTypeValue.PS,
            MimeTypeString.Image.QTIME => Const.ImageTypeValue.QTIME,
            "image/jpg" => Const.ImageTypeValue.JPEG,
            _ => TypeValueFromMimeType(mimeType)
        };

    internal static string? MimeTypeFromImageType(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals("JPEG", comp) ? MimeTypeString.Image.JPEG
             : typeValue.Equals("JPG", comp) ? MimeTypeString.Image.JPEG
             : typeValue.Equals(Const.ImageTypeValue.MPEG, comp) ? MimeTypeString.Image.MPEG
             : typeValue.Equals(Const.ImageTypeValue.MPEG2, comp) ? MimeTypeString.Image.MPEG
             : typeValue.Equals(Const.ImageTypeValue.PICT, comp) ? MimeTypeString.Image.PICT
             : typeValue.Equals(Const.ImageTypeValue.PS, comp) ? MimeTypeString.Image.PS
             : typeValue.Equals(Const.ImageTypeValue.QTIME, comp) ? MimeTypeString.Image.QTIME
             : CreateMimeType("image", typeValue.ToString());
    }

    internal static string? KeyTypeFromMimeType(string? mimeType)
        => mimeType switch
        {
            MimeTypeString.EncryptionKey.X509 => Const.KeyTypeValue.X509,
            "application/x-x509-user-cert" => Const.KeyTypeValue.X509,
            MimeTypeString.EncryptionKey.PGP => Const.KeyTypeValue.PGP,
            _ => TypeValueFromMimeType(mimeType)
        };

    internal static string? MimeTypeFromKeyType(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(Const.KeyTypeValue.PGP, comp) ? MimeTypeString.EncryptionKey.PGP
             : typeValue.Equals(Const.KeyTypeValue.X509, comp) ? MimeTypeString.EncryptionKey.X509
             : CreateMimeType("application", typeValue.ToString());
    }


    internal static string? SoundTypeFromMimeType(string? mimeType)
        => mimeType switch
        {
            MimeTypeString.Audio.WAVE => Const.SoundTypeValue.WAVE,
            MimeTypeString.Audio.PCM => Const.SoundTypeValue.PCM,
            _ => TypeValueFromMimeType(mimeType)
        };

    internal static string? MimeTypeFromSoundType(ReadOnlySpan<char> typeValue)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return typeValue.Equals(Const.SoundTypeValue.PCM, comp) ? MimeTypeString.Audio.PCM
             : typeValue.Equals(Const.SoundTypeValue.WAVE, comp) ? MimeTypeString.Audio.WAVE
             : typeValue.Equals("MP3", comp) ? MimeTypeString.Audio.MPEG
             : typeValue.Equals(Const.SoundTypeValue.NonStandard.VORBIS, comp) ? MimeTypeString.Audio.VORBIS
             : CreateMimeType("audio", typeValue.ToString());
    }

    private static string? CreateMimeType(string mediaType, string subType)
    {
        if (MimeType.TryParse(subType, out MimeType? mimeType))
        {
            return mimeType.ToString();
        }

        try
        {
            return MimeType.Create(mediaType, subType).ToString();
        }
        catch
        {
            return null;
        }
    }

    private static string? TypeValueFromMimeType(string? mimeType)
    {
        if (mimeType is null)
        {
            return null;
        }

        if (MimeTypeInfo.TryParse(mimeType, out MimeTypeInfo info))
        {
            ReadOnlySpan<char> subType = info.SubType;
            subType = subType.StartsWith("x-", StringComparison.OrdinalIgnoreCase) ? subType.Slice(2) : subType;

            Span<char> span = subType.Length > SHORT_STRING ? new char[subType.Length].AsSpan() : stackalloc char[subType.Length];
            _ = subType.ToUpperInvariant(span);
            return span.ToString();
        }

        return null;
    }
}
