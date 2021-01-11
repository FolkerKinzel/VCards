using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.Enums;



namespace FolkerKinzel.VCards.Intls.Converters.Tests
{
    [TestClass]
    public class DateAndOrTimeConverterTest
    {
        private readonly DateAndOrTimeConverter conv = new DateAndOrTimeConverter();

        [TestMethod]
        public void DateTest()
        {
            string s = "1963-08-17";

            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            var reference = new DateTime(1963, 8, 17);
            Assert.AreEqual(reference, dt.DateTime);

        }


        [TestMethod]
        public void DateTest2()
        {
            string s = "--08";
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            Assert.AreEqual(dt.Year, 4);
            Assert.AreEqual(dt.Month, 8);
        }

        [TestMethod]
        public void DateTest3()
        {
            string s = "--0803";
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            Assert.AreEqual(dt.Year, 4);
            Assert.AreEqual(dt.Month, 8);
            Assert.AreEqual(dt.Day, 3);
        }

        [TestMethod]
        public void DateTest4()
        {
            string s = "---03";
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            Assert.AreEqual(dt.Year, 4);
            Assert.AreEqual(dt.Month, 1);
            Assert.AreEqual(dt.Day, 3);
        }


        [TestMethod]
        public void RountripsTest()
        {
            Roundtrip("19720131");
            Roundtrip("19720131T15-07", false);
            Roundtrip("19720131T15+04", false);

            RoundtripTimestamp("19961022T140000", false);
            RoundtripTimestamp("19961022T140000Z", true);
            RoundtripTimestamp("19961022T140000-05", false);
            RoundtripTimestamp("19961022T140000-0500", false);
            RoundtripTimestamp("19961022T140000+0500", false);

            RoundtripTimestamp("19961022T140000", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000Z", false, VCdVersion.V2_1 );
            RoundtripTimestamp("19961022T140000-05", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000-0500", false, VCdVersion.V2_1);
            RoundtripTimestamp("19961022T140000+0500", false, VCdVersion.V2_1);
        }

        private void Roundtrip(
            string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
        {
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            string s2 = DateAndOrTimeConverter.ToDateTimeString(dt, version);

            if (stringRoundTrip)
            {
                Assert.AreEqual(s, s2);
            }

            Assert.IsTrue(conv.TryParse(s2, out DateTimeOffset dt2));

            Assert.AreEqual(dt, dt2);
        }

        private void RoundtripTimestamp(
            string s, bool stringRoundTrip = true, VCdVersion version = VCdVersion.V4_0)
        {
            Assert.IsTrue(conv.TryParse(s, out DateTimeOffset dt));

            string s2 = DateAndOrTimeConverter.ToTimestamp(dt, version);

            if (stringRoundTrip)
            {
                Assert.AreEqual(s, s2);
            }

            Assert.IsTrue(conv.TryParse(s2, out DateTimeOffset dt2));

            Assert.AreEqual(dt, dt2);
        }
    }
}
