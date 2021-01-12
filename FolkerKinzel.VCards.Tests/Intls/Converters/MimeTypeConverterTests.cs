using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
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
        public void SoundTypeValueFromMimeTypeTest(string? input, string? expected)
        {
            string? result = MimeTypeConverter.SoundTypeValueFromMimeType(input);
            Assert.AreEqual(expected, result);
        }
    }
}
