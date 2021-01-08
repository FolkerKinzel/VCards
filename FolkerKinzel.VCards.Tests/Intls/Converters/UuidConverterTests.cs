using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class UuidConverterTest
    {
#if NET45
        [TestMethod]
        public void IsUuidUriTest1()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";
            guidString = "urn:uuid:" + guidString;


            Assert.IsTrue(guidString.IsUuidUri());
        }
#endif


#if !NET45
        [TestMethod]
        public void IsUuidUriTest1()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";
            guidString = "urn:uuid:" + guidString;


           Assert.IsTrue(guidString.AsSpan().IsUuidUri());
        }
#endif

        [TestMethod]
        public void IsUuidUriTest6()
        {
            string guidString = "550e8400e29b11d4a716446655440000";
            guidString = "   urn:uuid:" + guidString;

#if NET45
            Assert.IsTrue(guidString.IsUuidUri());
#else
            Assert.IsTrue(guidString.AsSpan().IsUuidUri());
#endif
        }

        [TestMethod]
        public void IsUuidUriTest2()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";
#if NET45
            Assert.IsFalse(guidString.IsUuidUri());
#else
            Assert.IsFalse(guidString.AsSpan().IsUuidUri());
#endif
        }

        [TestMethod]
        public void IsUuidUriTest3()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";
            guidString = "https://" + guidString;

#if NET45
            Assert.IsFalse(guidString.IsUuidUri());
#else
            Assert.IsFalse(guidString.AsSpan().IsUuidUri());
#endif
        }

        [TestMethod]
        public void IsUuidUriTest5()
        {
            string guidString = "550e8400";
#if NET45
            Assert.IsFalse(guidString.IsUuidUri());
#else
            Assert.IsFalse(guidString.AsSpan().IsUuidUri());
#endif
        }

        [TestMethod()]
        public void ToGuidTest1()
        {
            string guidString = "550e8400e29b11d4a716446655440000";

            var guid = UuidConverter.ToGuid(guidString);

            Assert.AreNotEqual(guid, Guid.Empty);

            guidString = "urn:uuid:" + guidString;

            var guid2 = UuidConverter.ToGuid(guidString);

            Assert.AreEqual(guid, guid2);
        }

        [TestMethod()]
        public void ToGuidTest6()
        {
            string guidString = "550e8400e29b11d4a716446655440000";

            var guid = UuidConverter.ToGuid(guidString);

            Assert.AreNotEqual(guid, Guid.Empty);

            guidString = "    urn:uuid:" + guidString;

            var guid2 = UuidConverter.ToGuid(guidString);

            Assert.AreEqual(guid, guid2);
        }


        [TestMethod()]
        public void ToGuidTest2()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";

            var guid = UuidConverter.ToGuid(guidString);

            Assert.AreNotEqual(guid, Guid.Empty);

            Assert.AreEqual(guidString, guid.ToString());

            guidString = "urn:uuid:" + guidString;

            var guid2 = UuidConverter.ToGuid(guidString);

            Assert.AreEqual(guid, guid2);

            var sb = new StringBuilder();

            sb.AppendUuid(guid2);

            Assert.AreEqual(sb.ToString(), guidString);

        }


        [TestMethod()]
        public void ToGuidTest3()
        {
            var guid3 = UuidConverter.ToGuid(null);

            Assert.AreEqual(Guid.Empty, guid3);
        }

        [TestMethod()]
        public void ToGuidTest4()
        {
            var guid4 = UuidConverter.ToGuid(string.Empty);

            Assert.AreEqual(Guid.Empty, guid4);
        }

        [TestMethod()]
        public void ToGuidTest5()
        {
            var guid4 = UuidConverter.ToGuid("               ");

            Assert.AreEqual(Guid.Empty, guid4);
        }

        

    }
}