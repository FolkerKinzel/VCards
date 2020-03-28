using FolkerKinzel.VCards.Models.PropertyParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.VCards.Models.Tests
{
    [TestClass()]
    public class MimeTypeTests
    {
        [TestMethod()]
        public void CtorTest()
        {
            //const string DEFAULT_MIMETYPE = "text/plain;charset=US-ASCII";
            const string TEXT_MEDIATYPE = "text/plain";
            const string IMAGE_MEDIATYPE = "image/jpeg";
            const string s1 = ";charset=US-ASCII";
            const string s2 = IMAGE_MEDIATYPE + ";para1=val1;para2=val2";

            var mime = new MimeType(s1);
            Assert.AreEqual(mime.MediaType, TEXT_MEDIATYPE);
            Assert.IsNotNull(mime.Parameters);
            Assert.AreEqual(1, mime.Parameters?.Count);
            Assert.AreEqual(mime.Parameters?[0].Key, "charset");
            Assert.AreEqual(mime.Parameters?[0].Value, "US-ASCII");


            mime = new MimeType(null);
            Assert.AreEqual(mime.MediaType, TEXT_MEDIATYPE);
            Assert.IsNotNull(mime.Parameters);
            Assert.AreEqual(1, mime.Parameters?.Count);
            Assert.AreEqual(mime.Parameters?[0].Key, "charset");
            Assert.AreEqual(mime.Parameters?[0].Value, "US-ASCII");

            mime = new MimeType(s2);
            Assert.AreEqual(mime.MediaType, IMAGE_MEDIATYPE);
            Assert.IsNotNull(mime.Parameters);
            Assert.AreEqual(2, mime.Parameters?.Count);
            Assert.AreEqual(mime.Parameters?[0].Key, "para1");
            Assert.AreEqual(mime.Parameters?[0].Value, "val1");
            Assert.AreEqual(mime.Parameters?[1].Key, "para2");
            Assert.AreEqual(mime.Parameters?[1].Value, "val2");


            mime = new MimeType(IMAGE_MEDIATYPE);
            Assert.AreEqual(mime.MediaType, IMAGE_MEDIATYPE);
            Assert.IsNull(mime.Parameters);
        }
    }
}


