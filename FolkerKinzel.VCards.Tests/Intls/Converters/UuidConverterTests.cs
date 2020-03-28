using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass()]
    public class UuidConverterTest
    {
        [TestMethod()]
        public void ToGuidTest()
        {
            string guidString = "550e8400-e29b-11d4-a716-446655440000";

            Guid guid = UuidConverter.ToGuid(guidString);

            Assert.AreNotEqual(guid, Guid.Empty);

            Assert.AreEqual(guidString, guid.ToString());

            guidString = "urn:uuid:" + guidString;

            Guid guid2 = UuidConverter.ToGuid(guidString);

            Assert.AreEqual(guid, guid2);

            StringBuilder sb = new StringBuilder();

            sb.AppendUuid(guid2);

            Assert.AreEqual(sb.ToString(), guidString);

            Guid guid3 = UuidConverter.ToGuid(null);

            Assert.AreEqual(Guid.Empty, guid3);

            Guid guid4 = UuidConverter.ToGuid(string.Empty);

            Assert.AreEqual(Guid.Empty, guid4);

        }

        
    }
}