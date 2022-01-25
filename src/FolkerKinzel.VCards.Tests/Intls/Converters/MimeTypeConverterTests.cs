using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class MimeTypeConverterTests
{
    [DataTestMethod]
    [DataRow("image/jpg", "JPG")]
    [DataRow("image/JPG", "JPG")]
    [DataRow("IMAGE/JPG", "JPG")]
    [DataRow("image/", null)]
    [DataRow("IMAGE/", null)]
    [DataRow("jpg", "JPG")]
    [DataRow("JPG", "JPG")]
    [DataRow(null, null)]
    public void ImageTypeValueFromMimeTypeTest(string? input, string? expected)
    {
        string? result = MimeTypeConverter.ImageTypeValueFromMimeType(input);
        Assert.AreEqual(expected, result);
    }


    [DataTestMethod]
    [DataRow("audio/mp3", "MP3")]
    [DataRow("audio/MP3", "MP3")]
    [DataRow("AUDIO/MP3", "MP3")]
    [DataRow("audio/", null)]
    [DataRow("AUDIO/", null)]
    [DataRow("mp3", "MP3")]
    [DataRow("MP3", "MP3")]
    [DataRow(null, null)]
    public void SoundTypeValueFromMimeTypeTest(string? input, string? expected)
    {
        string? result = MimeTypeConverter.SoundTypeValueFromMimeType(input);
        Assert.AreEqual(expected, result);
    }


    [DataTestMethod]
    [DataRow(".pgp", "application/pgp-keys")]
    [DataRow(".crt", "application/x-x509-ca-cert")]
    [DataRow(".der", "application/x-x509-ca-cert")]
    [DataRow(".cer", "application/pkix-cert")]
    [DataRow(".pfx", "application/x-pkcs12")]
    [DataRow(".p12", "application/x-pkcs12")]
    [DataRow(".p7b", "application/x-pkcs7-certificates")]
    [DataRow(".spc", "application/x-pkcs7-certificates")]
    [DataRow(".p7c", "application/pkcs7-mime")]
    [DataRow(".pem", "application/x-pem-file")]
    [DataRow(".key", "application/pkcs8")]
    [DataRow(".p8", "application/pkcs8")]
    [DataRow(".csr", "application/pkcs10")]
    [DataRow(".p10", "application/pkcs10")]

    [DataRow(".bmp", "image/bmp")]
    [DataRow(".dib", "image/bmp")]
    [DataRow(".gif", "image/gif")]
    [DataRow(".ico", "image/x-icon")]
    [DataRow(".jpg", "image/jpeg")]
    [DataRow(".jpe", "image/jpeg")]
    [DataRow(".jpeg", "image/jpeg")]
    [DataRow(".pdf", "application/pdf")]
    [DataRow(".png", "image/png")]
    [DataRow(".pnz", "image/png")]
    [DataRow(".ps", "application/postscript")]
    [DataRow(".ai", "application/postscript")]
    [DataRow(".eps", "application/postscript")]
    [DataRow(".svg", "image/svg+xml")]
    [DataRow(".svgz", "image/svg+xml")]
    [DataRow(".tif", "image/tiff")]
    [DataRow(".tiff", "image/tiff")]
    [DataRow(".wmf", "image/x-wmf")]
    [DataRow(".hevc", "image/mpeg-h")]
    [DataRow(".xbm", "image/x-xbitmap")]
    [DataRow(".avi", "image/avi")]
    [DataRow(".cgm", "image/cgm")]
    [DataRow(".pic", "image/x-pict")]
    [DataRow(".pict", "image/x-pict")]
    [DataRow(".pct", "image/x-pict")]
    [DataRow(".mov", "image/mov")]

    [DataRow(".aac", "audio/aac")]
    [DataRow(".adts", "audio/aac")]
    [DataRow(".ac3", "audio/ac3")]
    [DataRow(".aif", "audio/x-aiff")]
    [DataRow(".aiff", "audio/x-aiff")]
    [DataRow(".aifc", "audio/x-aiff")]
    [DataRow(".snd", "audio/basic")]
    [DataRow(".au", "audio/basic")]
    [DataRow(".mp3", "audio/mpeg")]
    [DataRow(".mpga", "audio/mpeg")]
    [DataRow(".mp2", "audio/mpeg")]
    [DataRow(".mp2a", "audio/mpeg")]
    [DataRow(".m2a", "audio/mpeg")]
    [DataRow(".m3a", "audio/mpeg")]
    [DataRow(".m4a", "audio/mp4")]
    [DataRow(".mp4a", "audio/mp4")]
    [DataRow(".ogg", "audio/ogg")]
    [DataRow(".oga", "audio/ogg")]
    [DataRow(".spx", "audio/ogg")]
    [DataRow(".NixDa", "application/octet-stream")]
    public void GetMimeTypeFromFileExtensionTest(string? fileExtension, string mimeType)
    {
        string? fileName = fileExtension is null ? null : "file" + fileExtension;
        string result = MimeTypeConverter.GetMimeTypeFromFileExtension(fileName!);
        Assert.AreEqual(mimeType, result);
    }


    [DataTestMethod]
    [DataRow(".pgp", "application/pgp-keys")]
    [DataRow(".crt", "application/x-x509-ca-cert")]
    //[DataRow(".der", "application/x-x509-ca-cert")]
    [DataRow(".cer", "application/pkix-cert")]
    [DataRow(".pfx", "application/x-pkcs12")]
    //[DataRow(".p12", "application/x-pkcs12")]
    [DataRow(".p7b", "application/x-pkcs7-certificates")]
    //[DataRow(".spc", "application/x-pkcs7-certificates")]
    [DataRow(".p7c", "application/pkcs7-mime")]
    [DataRow(".pem", "application/x-pem-file")]
    [DataRow(".key", "application/pkcs8")]
    //[DataRow(".p8", "application/pkcs8")]
    [DataRow(".csr", "application/pkcs10")]
    //[DataRow(".p10", "application/pkcs10")]

    [DataRow(".bmp", "image/bmp")]
    //[DataRow(".dib", "image/bmp")]
    [DataRow(".gif", "image/gif")]
    [DataRow(".ico", "image/x-icon")]
    [DataRow(".jpg", "image/jpeg")]
    //[DataRow(".jpe", "image/jpeg")]
    //[DataRow(".jpeg", "image/jpeg")]
    [DataRow(".pdf", "application/pdf")]
    [DataRow(".png", "image/png")]
    //[DataRow(".pnz", "image/png")]
    [DataRow(".ps", "application/postscript")]
    //[DataRow(".ai", "application/postscript")]
    //[DataRow(".eps", "application/postscript")]
    [DataRow(".svg", "image/svg+xml")]
    //[DataRow(".svgz", "image/svg+xml")]
    [DataRow(".tif", "image/tiff")]
    //[DataRow(".tiff", "image/tiff")]
    [DataRow(".wmf", "image/x-wmf")]
    [DataRow(".hevc", "image/mpeg-h")]
    [DataRow(".xbm", "image/x-xbitmap")]
    [DataRow(".avi", "image/avi")]
    [DataRow(".cgm", "image/cgm")]
    [DataRow(".pic", "image/x-pict")]
    //[DataRow(".pict", "image/x-pict")]
    //[DataRow(".pct", "image/x-pict")]
    [DataRow(".mov", "image/mov")]

    [DataRow(".aac", "audio/aac")]
    //[DataRow(".adts", "audio/aac")]
    [DataRow(".ac3", "audio/ac3")]
    [DataRow(".aif", "audio/x-aiff")]
    //[DataRow(".aiff", "audio/x-aiff")]
    //[DataRow(".aifc", "audio/x-aiff")]
    [DataRow(".snd", "audio/basic")]
    //[DataRow(".au", "audio/basic")]
    [DataRow(".mp3", "audio/mpeg")]
    //[DataRow(".mpga", "audio/mpeg")]
    //[DataRow(".mp2", "audio/mpeg")]
    //[DataRow(".mp2a", "audio/mpeg")]
    //[DataRow(".m2a", "audio/mpeg")]
    //[DataRow(".m3a", "audio/mpeg")]
    [DataRow(".m4a", "audio/mp4")]
    //[DataRow(".mp4a", "audio/mp4")]
    [DataRow(".ogg", "audio/ogg")]
    //[DataRow(".oga", "audio/ogg")]
    //[DataRow(".spx", "audio/ogg")]
    [DataRow(".wav", "audio/wav")]
    [DataRow(".wav", "audio/x-wav")]

    [DataRow(".txt", "text/plain")]

    [DataRow(".octet-stream", "application/octet-stream")]
    public void GetFileExtensionTest(string extension, string mimeString)
    {
        var mime = new MimeType(mimeString);
        string result = MimeTypeConverter.GetFileExtension(mime);
        Assert.AreEqual(extension, result);
    }


    [DataTestMethod]
    [DataRow("PGP", "application/pgp-keys")]
    [DataRow("X509", "application/x-x509-ca-cert")]
    [DataRow("OTHER", "application/octet-stream")]
    public void MimeTypeFromEncryptionTypeValueTest(string typeValue, string mime)
    {
        string result = MimeTypeConverter.MimeTypeFromEncryptionTypeValue(typeValue);
        Assert.AreEqual(mime, result);
    }


    [DataTestMethod]
    [DataRow("PGP", "application/pgp-keys")]
    [DataRow("X509", "application/x-x509-ca-cert")]
    [DataRow("X509", "application/x-x509-user-cert")]
    [DataRow(null, "application/octet-stream")]
    public void KeyTypeValueFromMimeTypeTest(string? typeValue, string mime)
    {
        string? result = MimeTypeConverter.KeyTypeValueFromMimeType(mime);
        Assert.AreEqual(typeValue, result);
    }


    [DataTestMethod]
    [DataRow("BMP", "image/bmp")]
    [DataRow("DIB", "image/bmp")]
    [DataRow("GIF", "image/gif")]
    [DataRow("ICO", "image/x-icon")]
    [DataRow("JPG", "image/jpeg")]
    [DataRow("JPE", "image/jpeg")]
    [DataRow("JPEG", "image/jpeg")]
    [DataRow("PDF", "application/pdf")]
    [DataRow("PNG", "image/png")]
    //[DataRow("PNZ", "image/png")]
    [DataRow("PS", "application/postscript")]
    //[DataRow("AI", "application/postscript")]
    //[DataRow("EPS", "application/postscript")]
    [DataRow("SVG", "image/svg+xml")]
    //[DataRow("SVGZ", "image/svg+xml")]
    [DataRow("TIF", "image/tiff")]
    [DataRow("TIFF", "image/tiff")]
    [DataRow("WMF", "image/x-wmf")]
    //[DataRow("HEVC", "image/mpeg-h")]
    [DataRow("MPEG", "image/mpeg-h")]
    [DataRow("MPEG2", "image/mpeg-h")]
    [DataRow("XBM", "image/x-xbitmap")]
    [DataRow("AVI", "image/avi")]
    [DataRow("CGM", "image/cgm")]
    //[DataRow("PIC", "image/x-pict")]
    [DataRow("PICT", "image/x-pict")]
    //[DataRow("PCT", "image/x-pict")]
    [DataRow("MOV", "image/mov")]
    [DataRow("", "application/octet-stream")]
    public void MimeTypeFromImageTypeValueTest(string typeValue, string mime)
    {
        string result = MimeTypeConverter.MimeTypeFromImageTypeValue(typeValue);
        Assert.AreEqual(mime, result);
    }


    [DataTestMethod]
    [DataRow("AAC", "audio/aac")]
    //[DataRow("ADTS", "audio/aac")]
    [DataRow("AC3", "audio/ac3")]
    [DataRow("AIF", "audio/x-aiff")]
    [DataRow("AIFF", "audio/x-aiff")]
    //[DataRow("AIFC", "audio/x-aiff")]
    //[DataRow("SND", "audio/basic")]
    //[DataRow("AU", "audio/basic")]
    [DataRow("BASIC", "audio/basic")]
    [DataRow("MP3", "audio/mpeg")]
    //[DataRow("MPGA", "audio/mpeg")]
    //[DataRow("MP2", "audio/mpeg")]
    //[DataRow("MP2A", "audio/mpeg")]
    //[DataRow("M2A", "audio/mpeg")]
    //[DataRow("M3A", "audio/mpeg")]
    //[DataRow("M4A", "audio/mp4")]
    //[DataRow("MP4A", "audio/mp4")]
    [DataRow("MP4", "audio/mp4")]
    [DataRow("OGG", "audio/ogg")]
    //[DataRow("OGA", "audio/ogg")]
    //[DataRow("SPX", "audio/ogg")]

    [DataRow("WAV", "audio/x-wav")]
    [DataRow("WAVE", "audio/x-wav")]

    [DataRow("PCM", "audio/l16")]

    [DataRow("VORBIS", "audio/vorbis")]

    [DataRow("", "application/octet-stream")]
    public void MimeTypeFromSoundTypeValueTest(string typeValue, string mime)
    {
        string result = MimeTypeConverter.MimeTypeFromSoundTypeValue(typeValue);
        Assert.AreEqual(mime, result);
    }
}
