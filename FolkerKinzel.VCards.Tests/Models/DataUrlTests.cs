using System;
using System.Text;
using System.Linq;
using FolkerKinzel.VCards.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using FolkerKinzel.VCards.Models.PropertyParts;
using System.Xml.Serialization;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass]
    public class DataUrlTests
    {
       // private const string DEFAULT_MIME_TYPE = "text/plain;charset=us-ascii";
        private const string DEFAULT_ENCODING = "UrlEncoding";
        private const string DATA_PROTOCOL = "data:";

        [TestMethod]
        public void TestDataUri()
        {
            string text = "http://www.fölkerchen.de";


            string test = DATA_PROTOCOL + "text/plain;charset=utf-8" + ";" + DEFAULT_ENCODING + "," + Uri.EscapeDataString(text);

            Assert.IsTrue(DataUrl.TryCreate(test, out DataUrl? dataUri));
            Assert.AreEqual(text, dataUri?.GetEmbeddedText());

            dataUri = DataUrl.FromText(text);

            Assert.IsNotNull(dataUri);

            dataUri = DataUrl.FromBytes(new byte[] { 1, 2, 3 }, "application/x-octet");

            Assert.IsNotNull(dataUri);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromTextOnNull()
        {
            var _ = DataUrl.FromText(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FromTextOnStringEmpty()
        {
            var _ = DataUrl.FromText("");
        }


        [TestMethod()]
        public void TryCreateTest()
        {
            const string TEXT = "In Märchenbüchern herumstöbern.";
            const string MIME_TYPE = "application/vnd.api+json";
            byte[] DATA = new byte[] { 1, 2, 3 };



            var dataUrl1 = DataUrl.FromText(TEXT);

            Assert.IsTrue(DataUrl.TryCreate(dataUrl1.ToString(), out DataUrl? dataUrl2));

            Assert.AreEqual(dataUrl2?.MimeType.MediaType, "text/plain");
            Assert.AreEqual(dataUrl2?.MimeType.Parameters?[0].Value, "US-ASCII");
            Assert.AreEqual(TEXT, dataUrl2?.GetEmbeddedText());


            dataUrl1 = DataUrl.FromBytes(DATA, MIME_TYPE);

            Assert.IsTrue(DataUrl.TryCreate(dataUrl1.ToString(), out dataUrl2));

            Assert.AreEqual(dataUrl2?.MimeType.MediaType, MIME_TYPE);
            Assert.AreEqual(dataUrl2?.GetFileExtension(), ".json");
            Assert.IsTrue(dataUrl2?.GetEmbeddedBytes()?.SequenceEqual(DATA) ?? false);


            dataUrl1 = DataUrl.FromBytes(Encoding.GetEncoding("iso-8859-1").GetBytes(TEXT), ";charset=ISO-8859-1");

            Assert.IsTrue(DataUrl.TryCreate(dataUrl1.ToString(), out dataUrl2));

            Assert.AreEqual(dataUrl2?.MimeType.MediaType, "text/plain");
            Assert.AreEqual(dataUrl2?.MimeType.Parameters?[0].Value, "ISO-8859-1");
            Assert.AreEqual(dataUrl2?.Encoding, DataEncoding.Base64);
            Assert.IsTrue(dataUrl2?.ContainsText ?? false);
            Assert.AreEqual(TEXT, dataUrl2?.GetEmbeddedText());

            string test = "data:;charset=UTF-8,Text";

            Assert.IsTrue(DataUrl.TryCreate(test, out dataUrl2));

            Assert.AreEqual(dataUrl2?.EncodedData, "Text");
            Assert.AreEqual(dataUrl2?.MimeType.MediaType, "text/plain");
            Assert.AreEqual(dataUrl2?.MimeType.Parameters?[0].Value, "UTF-8");
            Assert.AreEqual(dataUrl2?.Encoding, DataEncoding.UrlEncoded);
            Assert.AreEqual("Text", dataUrl2?.GetEmbeddedText());

            Assert.IsFalse(DataUrl.TryCreate(null, out _));
            Assert.IsFalse(DataUrl.TryCreate("", out _));
            Assert.IsFalse(DataUrl.TryCreate("http://wwww.folker-kinzel.de/index.htm", out _));
        }


        [TestMethod]
        public void SerializationTest1()
        {
            const string MEDIA_TYPE = "image/jpeg";
            byte[] data = new byte[] { 1, 2, 3 };

            var dataUri = DataUrl.FromBytes(data, MEDIA_TYPE);

            Assert.IsNotNull(dataUri);
            Assert.AreEqual(dataUri.MimeType.MediaType, MEDIA_TYPE);
            Assert.AreEqual(dataUri.Encoding, DataEncoding.Base64);
            Assert.IsTrue(data.SequenceEqual(dataUri.GetEmbeddedBytes()!));


            var formatter = new BinaryFormatter();

            using(var stream = new MemoryStream())
            {
                formatter.Serialize(stream, dataUri);

                stream.Position = 0;

                dataUri = (DataUrl)formatter.Deserialize(stream);
            }

            Assert.IsNotNull(dataUri);
            Assert.AreEqual(dataUri.MimeType.MediaType, MEDIA_TYPE);
            Assert.AreEqual(dataUri.Encoding, DataEncoding.Base64);
            Assert.IsTrue(data.SequenceEqual(dataUri.GetEmbeddedBytes()!));
        }


        // [TestMethod]
        //public void SerializationTest2()
        //{
        //    const string MEDIA_TYPE = "image/jpeg";
        //    byte[] data = new byte[] { 1, 2, 3 };

        //    var dataUri = DataUrl.FromBytes(data, MEDIA_TYPE);

        //    Assert.IsNotNull(dataUri);
        //    Assert.AreEqual(dataUri.MimeType.MediaType, MEDIA_TYPE);
        //    Assert.AreEqual(dataUri.Encoding, DataEncoding.Base64);
        //    Assert.IsTrue(data.SequenceEqual(dataUri.GetEmbeddedBytes()!));


        //    var formatter = new XmlSerializer(typeof(DataUrl));

        //    using(var stream = new MemoryStream())
        //    {
        //        formatter.Serialize(stream, dataUri);

        //        stream.Position = 0;

        //        using var reader = new StreamReader(stream);

        //        string s = reader.ReadToEnd();
        //        Assert.AreNotEqual(0, s.Length);

        //        stream.Position = 0;

        //        dataUri = (DataUrl?)formatter.Deserialize(stream);
        //    }

        //    Assert.IsNotNull(dataUri);
        //    Assert.AreEqual(dataUri!.MimeType.MediaType, MEDIA_TYPE);
        //    Assert.AreEqual(dataUri.Encoding, DataEncoding.Base64);
        //    Assert.IsTrue(data.SequenceEqual(dataUri.GetEmbeddedBytes()!));
        //}

        //[TestMethod]
        //public void GetObjectDataTest()
        //{
        //    const string MEDIA_TYPE = "image/jpeg";
        //    byte[] data = new byte[] { 1, 2, 3 };

        //    var dataUri = DataUrl.FromBytes(data, MEDIA_TYPE);

        //    ISerializable serializable = dataUri;

        //    var info = new SerializationInfo(typeof(DataUrl), new FormatterConverter());
        //    var context = new StreamingContext();

        //    serializable.GetObjectData(info, context);

        //    Assert.IsNotNull(info.GetValue("MimeType", typeof(MimeType)));
        //    Assert.IsNotNull(info.GetValue("Encoding", typeof(DataEncoding)));
        //}
    }
}
