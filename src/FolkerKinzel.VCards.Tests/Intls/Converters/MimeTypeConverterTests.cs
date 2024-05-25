namespace FolkerKinzel.VCards.Intls.Converters.Tests;

[TestClass]
public class MimeTypeConverterTests
{
    [DataTestMethod]
    [DataRow("image/jpg", "JPEG")]
    [DataRow("image/", null)]
    [DataRow("IMAGE/", null)]
    [DataRow("jpg", null)]
    [DataRow("JPG", null)]
    [DataRow(null, null)]
    public void ImageTypeValueFromMimeTypeTest(string? input, string? expected)
    {
        string? result = MimeTypeConverter.ImageTypeFromMimeType(input);
        Assert.AreEqual(expected, result);
    }


    [DataTestMethod]
    [DataRow("audio/mp3", "MP3")]
    [DataRow("audio/MP3", "MP3")]
    [DataRow("AUDIO/MP3", "MP3")]
    [DataRow("audio/", null)]
    [DataRow("AUDIO/", null)]
    [DataRow("mp3", null)]
    [DataRow("MP3", null)]
    [DataRow(null, null)]
    public void SoundTypeValueFromMimeTypeTest(string? input, string? expected)
    {
        string? result = MimeTypeConverter.SoundTypeFromMimeType(input);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void MimeTypeFromSoundTypeTest1()
    {
        const string mime = "AUDIO/X-AIFF";
        Assert.AreEqual(mime.ToLowerInvariant(), MimeTypeConverter.MimeTypeFromSoundType(mime.AsSpan()));
    }

    [TestMethod]
    public void MimeTypeFromSoundTypeTest2() => Assert.IsNull(MimeTypeConverter.MimeTypeFromSoundType("ÄÖÜ".AsSpan()));

    [DataTestMethod]
    [DataRow("PGP", "application/pgp-keys")]
    [DataRow("X509", "application/x-x509-ca-cert")]
    [DataRow("OTHER", "application/other")]
    public void MimeTypeFromEncryptionTypeValueTest(string typeValue, string mime)
    {
        string? result = MimeTypeConverter.MimeTypeFromKeyType(typeValue.AsSpan());
        Assert.AreEqual(mime, result);
    }


    [DataTestMethod]
    [DataRow("PGP", "application/pgp-keys")]
    [DataRow("X509", "application/x-x509-ca-cert")]
    [DataRow("X509", "application/x-x509-user-cert")]
    [DataRow("OCTET-STREAM", "application/octet-stream")]
    public void KeyTypeValueFromMimeTypeTest(string? typeValue, string mime)
    {
        string? result = MimeTypeConverter.KeyTypeFromMimeType(mime);
        Assert.AreEqual(typeValue, result);
    }


    [DataTestMethod]
    [DataRow("GIF")]
    [DataRow("CGM")]
    [DataRow("WMF")]
    [DataRow("BMP")]
    [DataRow("MET")]
    [DataRow("PMB")]
    [DataRow("DIB")]
    [DataRow("PICT")]
    [DataRow("TIFF")]
    [DataRow("PS")]
    [DataRow("PDF")]
    [DataRow("JPEG")]
    [DataRow("MPEG")]
    //[DataRow("MPEG2")]
    [DataRow("AVI")]
    [DataRow("QTIME")]
    [DataRow("SVG")]
    public void PictureRoundtripTest1(string typeValue)
    {
        string? mime = MimeTypeConverter.MimeTypeFromImageType(typeValue.AsSpan());
        string? type = MimeTypeConverter.ImageTypeFromMimeType(mime);
        Assert.IsNotNull(type);
        Assert.AreEqual(typeValue, type);
    }

    [TestMethod]
    public void PictureRoundtripTest2()
    {
        string? mime = MimeTypeConverter.MimeTypeFromImageType("MPEG2".AsSpan());
        string? type = MimeTypeConverter.ImageTypeFromMimeType(mime);
        Assert.AreEqual("MPEG", type);
    }

    [TestMethod]
    public void PictureRoundtripTest3()
    {
        string? mime = MimeTypeConverter.MimeTypeFromImageType("JPG".AsSpan());
        string? type = MimeTypeConverter.ImageTypeFromMimeType(mime);
        Assert.AreEqual("JPEG", type);
    }


    [DataTestMethod]
    [DataRow("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")]
    [DataRow("VORBIS")]
    [DataRow("WAVE")]
    [DataRow("PCM")]
    [DataRow("AIFF")]
    [DataRow("MPEG")]
    public void SoundRoundtripTest1(string typeValue)
    {
        string? mime = MimeTypeConverter.MimeTypeFromSoundType(typeValue.AsSpan());
        string? type = MimeTypeConverter.SoundTypeFromMimeType(mime);
        Assert.IsNotNull(type);
        Assert.AreEqual(typeValue, type);
    }

    [TestMethod]
    public void SoundRoundtripTest2()
    {
        string? mime = MimeTypeConverter.MimeTypeFromSoundType("MP3".AsSpan());
        string? type = MimeTypeConverter.SoundTypeFromMimeType(mime);
        Assert.IsNotNull(type);
        Assert.AreEqual("MPEG", type);
    }

    [DataTestMethod]
    [DataRow("X509")]
    [DataRow("PGP")]
    public void KeyRoundtripTest1(string typeValue)
    {
        string? mime = MimeTypeConverter.MimeTypeFromKeyType(typeValue.AsSpan());
        string? type = MimeTypeConverter.KeyTypeFromMimeType(mime);
        Assert.IsNotNull(type);
        Assert.AreEqual(typeValue, type);

        //var result = MimeString.ToFileTypeExtension("image/pmb");
        //var mimeString = MimeString.FromFileName(".pmb");
    }
}
